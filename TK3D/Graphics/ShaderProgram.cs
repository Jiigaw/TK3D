﻿using System;
using System.Linq.Expressions;
using OpenTK.Graphics.OpenGL4;

namespace TK3D.Graphics
{
    internal class ShaderProgram
    {
        public int ID;
        public ShaderProgram(string vertexShaderFilepath, string fragmentShaderFilepath)
        {
            ID = GL.CreateProgram();
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource(vertexShaderFilepath));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource(fragmentShaderFilepath));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(ID, fragmentShader);
            GL.AttachShader(ID, vertexShader);

            GL.LinkProgram(ID);

            //Clean up
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        public void Bind() { GL.UseProgram(ID); }
        public void Unbind () { GL.UseProgram(0); }
        public void Delete () { GL.DeleteProgram(ID); }
        public static string LoadShaderSource(string Filepath) {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + Filepath))
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