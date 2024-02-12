using StbImageSharp;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TK3D
{
    internal class Game : GameWindow
    {
        //float[] vertices =
        //{
        //    -0.5f, 0.5f, 0f, //top left vertex - 0
        //    0.5f, 0.5f, 0f, //top right vertex - 1
        //    0.5f, -0.5f, 0f, //bottom right vertex - 2
        //    -0.5f, -0.5f, 0f //bottom left vertex - 3
        //};

        List<Vector3> vertices = new List<Vector3>() 
        { 

            //front face
            new Vector3(-0.5f, 0.5f, 0.5f), //topleft
            new Vector3(0.5f, 0.5f, 0.5f), //topright
            new Vector3(0.5f, -0.5f, 0.5f), //bottomright
            new Vector3(-0.5f, -0.5f, 0.5f), //bottomleft
            //right face
            new Vector3(0.5f, 0.5f, 0.5f), //topleft
            new Vector3(0.5f, 0.5f, -0.5f), //topright
            new Vector3(0.5f, -0.5f, -0.5f), //bottomright
            new Vector3(0.5f, -0.5f, 0.5f), //bottomleft
            //back face
            new Vector3(0.5f, 0.5f, -0.5f), //topleft
            new Vector3(-0.5f, 0.5f, -0.5f), //topright
            new Vector3(-0.5f, -0.5f, -0.5f), //bottomright
            new Vector3(0.5f, -0.5f, -0.5f), //bottomleft
            //left face
            new Vector3(-0.5f, 0.5f, -0.5f), //topleft
            new Vector3(-0.5f, 0.5f, 0.5f), //topright
            new Vector3(-0.5f, -0.5f, 0.5f), //bottomright
            new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft
            //top face
            new Vector3(-0.5f, 0.5f, -0.5f), //topleft
            new Vector3(0.5f, 0.5f, -0.5f), //topright
            new Vector3(0.5f, 0.5f, 0.5f), //bottomright
            new Vector3(-0.5f, 0.5f, 0.5f), //bottomleft
            //bottom face
            new Vector3(-0.5f, -0.5f, 0.5f), //topleft
            new Vector3(0.5f, -0.5f, 0.5f), //topright
            new Vector3(0.5f, -0.5f, -0.5f), //bottomright
            new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft

        };

        //float[] texCoords =
        //{
        //    0f, 1f,
        //    1f, 1f,
        //    1f, 0f,
        //    0f, 0f
        //};

        List<Vector2> texCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] indices =
        {
            //first face
            //top triangle
            0, 1, 2,
            //bottom triangle
            2, 3, 0,
            //
            4, 5, 6,
            6, 7, 4,
            //
            8,9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };

        //Render Pip vars
        int vao;
        int shaderProgram;
        int vbo;
        int textureVBO;
        int ebo;
        int textureID;

        //Transformation variables
        float yRot = 0f;
        Camera camera;


        int width, height;
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(width, height));
            this.width = width;
            this.height = height;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();

            //bind vao
            GL.BindVertexArray(vao);

            // --Vert vbo--

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            // put vert VBO in slot 0 of VAO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // --Texture vbo--
            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector2.SizeInBytes, texCoords.ToArray(), BufferUsageHint.StaticDraw);

            // put tex VBO in slot 1 of VAO
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Unbind
            GL.BindVertexArray(0);

            ebo = GL.GenBuffer();   
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length*sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //create shader prog
            shaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderProgram,vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            //del shader
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // --textures--
            textureID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //load
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/Tex.PNG"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Texture.Width, Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Texture.Data);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteTexture(textureID);
            GL.DeleteProgram(shaderProgram);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //draw trig
            GL.UseProgram(shaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            //transformation matricies
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.getViewMatirx();
            Matrix4 projection = camera.getProjectionMatrix();

            model = Matrix4.CreateRotationY(yRot);
            yRot += 0.001f;


            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);

            camera.Update(input, mouse, args);
        }

        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }

    }
}