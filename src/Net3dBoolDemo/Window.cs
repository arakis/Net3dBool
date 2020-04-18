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
        private float[] _MouseSpeed = new float[3];
        private Vector2 _MouseDelta;
        private float _UpDownDelta;
        private Vector3 CameraLocation;
        private Vector3 Up = Vector3.UnitZ;
        private float Pitch = -0.3f;
        private float Facing = (float)Math.PI / 2 + 0.15f;

        public ExtendedGameWindow() : base(GameWindowSettings.Default, new NativeWindowSettings { APIVersion = new Version(3, 0), Profile = ContextProfile.Any })
        {
        }

        protected override void OnLoad()
        {
            GL.LoadBindings(new OpenToolkit.Windowing.GraphicsLibraryFramework.GLFWBindingsContext());

            VSync = VSyncMode.On;
            Title = "Net3dBool Demo with OpenTK";

            CameraMatrix = Matrix4.Identity;
            CameraLocation = new Vector3(1f, -5f, 2f);
            _MouseDelta = new Vector2();

            MouseMove += ProcessMouseMove;

            CreateMesh();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);

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
                _MouseDelta.X = -2;

            if (kbState[Key.Right])
                _MouseDelta.X = 2;

            if (kbState[Key.Up])
                _MouseDelta.Y = -1;

            if (kbState[Key.Down])
                _MouseDelta.Y = 1;

            if (kbState[Key.PageUp])
                _UpDownDelta = -3;

            if (kbState[Key.PageDown])
                _UpDownDelta = 3;

            _MouseSpeed[0] *= 0.9f;
            _MouseSpeed[1] *= 0.9f;
            _MouseSpeed[2] *= 0.9f;
            _MouseSpeed[0] -= _MouseDelta.X / 1000f;
            _MouseSpeed[1] -= _MouseDelta.Y / 1000f;
            _MouseSpeed[2] -= _UpDownDelta / 1000f;
            _MouseDelta = new Vector2();
            _UpDownDelta = 0;

            Facing += _MouseSpeed[0] * 2;
            Pitch += _MouseSpeed[1] * 2;
            CameraLocation.Z += _MouseSpeed[2] * 2;

            var loc = new Vector3(CameraLocation.X, CameraLocation.Y, CameraLocation.Z);
            Vector3 lookatPoint = new Vector3((float)Math.Cos(Facing), (float)Math.Sin(Facing), (float)Math.Sin(Pitch));
            CameraMatrix = Matrix4.LookAt(loc, loc + lookatPoint, Up);

            if (kbState[Key.Escape])
            {
                Close();
                Environment.Exit(0);
            }
        }

        private void ProcessMouseMove(MouseMoveEventArgs e)
        {
            if (MouseState[MouseButton.Left])
                _MouseDelta = new Vector2(e.DeltaX, e.DeltaY);
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

