using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Net3dBoolDemo
{

    public class TGameWindow : GameWindow
    {

        public static Matrix4 cameraMatrix;
        private float[] mouseSpeed = new float[3];
        private Vector2 mouseDelta;
        private float upDownDelta;
        private Vector3 location;
        private Vector3 up = Vector3.UnitZ;
        private float pitch = -0.3f;
        private float facing = (float)Math.PI / 2 + 0.15f;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            VSync = VSyncMode.On;
            this.Title = "Net3dBool Demo with OpenTK";

            cameraMatrix = Matrix4.Identity;
            location = new Vector3(1f, -5f, 2f);
            mouseDelta = new Vector2();

            //System.Windows.Forms.Cursor.Position = new Point(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);

            Mouse.Move += new EventHandler<MouseMoveEventArgs>(OnMouseMove);

            CreateMesh();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.W])
            {
                location.X += (float)Math.Cos(facing) * 0.1f;
                location.Y += (float)Math.Sin(facing) * 0.1f;
            }

            if (Keyboard[Key.S])
            {
                location.X -= (float)Math.Cos(facing) * 0.1f;
                location.Y -= (float)Math.Sin(facing) * 0.1f;
            }

            if (Keyboard[Key.A])
            {
                location.X += (float)Math.Cos(facing + Math.PI / 2) * 0.1f;
                location.Y += (float)Math.Sin(facing + Math.PI / 2) * 0.1f;
            }

            if (Keyboard[Key.D])
            {
                location.X -= (float)Math.Cos(facing + Math.PI / 2) * 0.1f;
                location.Y -= (float)Math.Sin(facing + Math.PI / 2) * 0.1f;
            }

            if (Keyboard[Key.Left])
            {
                //facing += 0.04f;
                mouseDelta.X = -2;
            }

            if (Keyboard[Key.Right])
            {
                //facing -= 0.04f;
                mouseDelta.X = 2;

            }

            if (Keyboard[Key.Up])
            {
                //pitch += 0.02f;
                mouseDelta.Y = -1;
            }

            if (Keyboard[Key.Down])
            {
                //pitch -= 0.02f;
                mouseDelta.Y = 1;
            }

            if (Keyboard[Key.PageUp])
            {
                //pitch += 0.02f;
                upDownDelta = -3;
            }

            if (Keyboard[Key.PageDown])
            {
                //pitch -= 0.02f;
                upDownDelta = 3;
            }

            //mouseSpeed[0] *= 0.9f;
            //mouseSpeed[1] *= 0.9f;
            mouseSpeed[0] *= 0.9f;
            mouseSpeed[1] *= 0.9f;
            mouseSpeed[2] *= 0.9f;
            mouseSpeed[0] -= mouseDelta.X / 1000f;
            mouseSpeed[1] -= mouseDelta.Y / 1000f;
            mouseSpeed[2] -= upDownDelta / 1000f;
            //mouseSpeed[2] -= mouseDelta.Y / 1000f;
            mouseDelta = new Vector2();
            upDownDelta = 0;

            facing += mouseSpeed[0] * 2;
            pitch += mouseSpeed[1] * 2;
            location.Z += mouseSpeed[2] * 2;

            var loc = new Vector3(location.X, location.Y, location.Z);
            Vector3 lookatPoint = new Vector3((float)Math.Cos(facing), (float)Math.Sin(facing), (float)Math.Sin(pitch));
            cameraMatrix = Matrix4.LookAt(loc, loc + lookatPoint, up);

            if (Keyboard[Key.Escape])
                Exit();
        }

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            //mouseDelta = new Vector2(e.XDelta, e.YDelta);
            if (e.Mouse.LeftButton == ButtonState.Pressed)
                mouseDelta = new Vector2(e.XDelta, e.YDelta);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
//            GL.Enable(EnableCap.Lighting);
//            GL.Enable(EnableCap.Light0);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


//            GL.ShadeModel(ShadingModel.Smooth);
//            GL.Enable(EnableCap.ColorMaterial);
//            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
//            float[] mat_shininess = { 50.0f };
//            GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
//            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref cameraMatrix);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

//            GL.Light(LightName.Light1, LightParameter.Ambient, OpenTK.Graphics.Color4.Gray);
//            GL.Light(LightName.Light1, LightParameter.Diffuse, OpenTK.Graphics.Color4.White);
//            GL.Light(LightName.Light1, LightParameter.Position, (new Vector4(0f, 0f, 0f, -5f)));
//            GL.Light(LightName.Light1, LightParameter.SpotDirection, (new Vector4(0f, 0f, 1f, 0f)));
//            GL.Enable(EnableCap.Light1);

            RenderMesh();
            RenderLines();

            SwapBuffers();
        }

        public Net3dBool.Color3f[] getColorArray(int length, Color c)
        {
            var ar = new Net3dBool.Color3f[length];
            for (var i = 0; i < length; i++)
                ar[i] = new Net3dBool.Color3f(1 / 255d * c.R, 1 / 255d * c.G, 1 / 255d * c.B);
            return ar;
        }

        public virtual void CreateMesh()
        {
        }

        public virtual void RenderMesh()
        {

        }

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
            var min = -0.01f;
            GL.Begin(PrimitiveType.Quads);

            GL.Color4(0, 0, 0.5, 0.35);
            GL.Vertex3(-dist, -dist, min);
            GL.Vertex3(-dist, dist, min);
            GL.Vertex3(dist, dist, min);
            GL.Vertex3(dist, -dist, min);
            GL.End();

        }

    }

}

