using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK3D
{
    internal class Camera
    {
        //Constans
        private float speed = 8f;
        private float screenwidth;
        private float screenheight;
        private float sensetivity = 100f;
        const float ViewHeight = 2;

        //Position vars
        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;

        //View rotations
        private float pitch;
        private float yaw = -90.0f;

        private bool firstMove = true;
        public Vector2 lastPos;
        public Camera(float width, float height, Vector3 position) { 
            screenheight = height;
            screenwidth = width;
            this.position = position;
        }
        public Matrix4 getViewMatirx() {
            return Matrix4.LookAt(position, position + front, up);
        }
        public Matrix4 getProjectionMatrix() {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), screenwidth / screenheight, 0.1f, 100.0f);
        }

        private void UpdateVectors()
        {
            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }

            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                position -= right * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position += right * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                position.Y -= speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Space))
            {
                position.Y += speed * (float)e.Time;
            }
            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            } else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                yaw += deltaX * sensetivity * (float)e.Time;
                pitch -= deltaY * sensetivity * (float)e.Time;
            }
            UpdateVectors();

        }
        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) 
        { 
            InputController(input, mouse, e);
            Console.Clear();
            Console.WriteLine(position);
        }
    }
}
