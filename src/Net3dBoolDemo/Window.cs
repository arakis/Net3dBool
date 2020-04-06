using System;
using System.Drawing;

using OpenToolkit;
using OpenToolkit.Graphics;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Input;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.Desktop;

namespace Net3dBoolDemo
{

    public abstract class ExtendedGameWindow : GameWindow
    {

        public static Matrix4 CameraMatrix;
        private float[] MouseSpeed = new float[3];
        private Vector2 MouseDelta;
        private float UpDownDelta;
        private Vector3 CameraLocation;
        private Vector3 Up = Vector3.UnitZ;
        private float Pitch = -0.3f;
        private float Facing = (float)Math.PI / 2 + 0.15f;

        public ExtendedGameWindow() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
        }

        protected override void OnLoad()
        {
            GL.LoadBindings(new OpenToolkit.Windowing.GraphicsLibraryFramework.GLFWBindingsContext());

            VSync = VSyncMode.On;
            Title = "Net3dBool Demo with OpenTK";

            CameraMatrix = Matrix4.Identity;
            CameraLocation = new Vector3(1f, -5f, 2f);
            MouseDelta = new Vector2();

            MouseMove += OnMouseMove;

            CreateMesh();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var kbState = KeyboardState;
            if (kbState[Key.W])
            {
                CameraLocation.X += (float)Math.Cos(Facing) * 0.1f;
                CameraLocation.Y += (float)Math.Sin(Facing) * 0.1f;
            }

            if (kbState[Key.S])
            {
                CameraLocation.X -= (float)Math.Cos(Facing) * 0.1f;
                CameraLocation.Y -= (float)Math.Sin(Facing) * 0.1f;
            }

            if (kbState[Key.A])
            {
                CameraLocation.X += (float)Math.Cos(Facing + Math.PI / 2) * 0.1f;
                CameraLocation.Y += (float)Math.Sin(Facing + Math.PI / 2) * 0.1f;
            }

            if (kbState[Key.D])
            {
                CameraLocation.X -= (float)Math.Cos(Facing + Math.PI / 2) * 0.1f;
                CameraLocation.Y -= (float)Math.Sin(Facing + Math.PI / 2) * 0.1f;
            }

            if (kbState[Key.Left])
                MouseDelta.X = -2;

            if (kbState[Key.Right])
                MouseDelta.X = 2;

            if (kbState[Key.Up])
                MouseDelta.Y = -1;

            if (kbState[Key.Down])
                MouseDelta.Y = 1;

            if (kbState[Key.PageUp])
                UpDownDelta = -3;

            if (kbState[Key.PageDown])
                UpDownDelta = 3;

            MouseSpeed[0] *= 0.9f;
            MouseSpeed[1] *= 0.9f;
            MouseSpeed[2] *= 0.9f;
            MouseSpeed[0] -= MouseDelta.X / 1000f;
            MouseSpeed[1] -= MouseDelta.Y / 1000f;
            MouseSpeed[2] -= UpDownDelta / 1000f;
            MouseDelta = new Vector2();
            UpDownDelta = 0;

            Facing += MouseSpeed[0] * 2;
            Pitch += MouseSpeed[1] * 2;
            CameraLocation.Z += MouseSpeed[2] * 2;

            var loc = new Vector3(CameraLocation.X, CameraLocation.Y, CameraLocation.Z);
            Vector3 lookatPoint = new Vector3((float)Math.Cos(Facing), (float)Math.Sin(Facing), (float)Math.Sin(Pitch));
            CameraMatrix = Matrix4.LookAt(loc, loc + lookatPoint, Up);

            if (kbState[Key.Escape])
            {
                Close();
                Environment.Exit(0);
            }
        }

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (MouseState[MouseButton.Left])
                MouseDelta = new Vector2(e.DeltaX, e.DeltaY);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.Min.X, ClientRectangle.Min.Y, ClientRectangle.Size.X, ClientRectangle.Size.Y);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Size.X / (float)Size.Y, 0.1f, 150.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref CameraMatrix);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            GL.Enable(EnableCap.ColorMaterial);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Light(LightName.Light1, LightParameter.Ambient, Color4.Gray);
            GL.Light(LightName.Light1, LightParameter.Diffuse, Color4.White);
            GL.Light(LightName.Light1, LightParameter.Position, (new Vector4(0f, 0f, 0f, 1f)));
            GL.Enable(EnableCap.Light1);

            RenderMesh();
            RenderLines();

            SwapBuffers();
        }

        public abstract void CreateMesh();

        public abstract void RenderMesh();

        public void RenderLines()
        {
            GL.Begin(PrimitiveType.Lines);
            float dist = 10f;
            for (float i = -dist; i <= dist; i += dist / 10)
            {
                GL.Color3(0.2, 0.2, 0.2);

                if (i == 0)
                    GL.Color3(Color.DarkBlue);

                GL.Vertex3(i, dist, 0);
                GL.Vertex3(i, -dist, 0);

                if (i == 0)
                    GL.Color3(Color.DarkRed);

                GL.Vertex3(dist, i, 0);
                GL.Vertex3(-dist, i, 0);
            }

            GL.Color3(Color.DarkGreen);
            GL.Vertex3(0, 0, dist);
            GL.Vertex3(0, 0, -dist);

            GL.End();

            return;
        }

    }

}

