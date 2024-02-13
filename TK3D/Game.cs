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
using TK3D.Graphics;

namespace TK3D
{
    internal class Game : GameWindow
    {

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

        List<uint> indices = new List<uint>
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
        VAO vao;
        IBO ibo;
        ShaderProgram program;
        Texture texture;

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

            vao = new VAO();

            VBO vbo = new VBO(vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoords);
            vao.LinkToVAO(1, 2, uvVBO);
            ibo = new IBO(indices);
            program = new ShaderProgram("Default.vert", "Default.frag");
            texture = new Texture("Tex.PNG");
            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            vao.Delete();
            ibo.Delete();
            texture.Delete();
            program.Delete();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            program.Bind();
            vao.Bind();
            texture.Bind();
            ibo.Bind();

            //transformation matricies
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.getViewMatirx();
            Matrix4 projection = camera.getProjectionMatrix();

            //model = Matrix4.CreateRotationY(yRot);
            //yRot += 0.001f;


            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(program.ID, "model");
            int viewLocation = GL.GetUniformLocation(program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            model += Matrix4.CreateTranslation(new Vector3(20f, 0f, 0f));
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
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
    }
}