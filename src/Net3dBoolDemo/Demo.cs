using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Net3dBoolDemo
{

    public class TDemoWindow : ExtendedGameWindow
    {

        public Net3dBool.Solid Mesh;

        public override void CreateMesh()
        {
            var start = DateTime.UtcNow;
            Console.Write("Generate mesh...");

            var box = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_BOX_COORDINATES);
            var sphere = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_COORDINATES);
            sphere.Scale(0.68, 0.68, 0.68);

            var cylinder1 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder1.Scale(0.38, 1, 0.38);

            var cylinder2 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder2.Scale(0.38, 1, 0.38);
            cylinder2.Rotate(Math.PI / 2, 0);

            var cylinder3 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder3.Scale(0.38, 1, 0.38);
            cylinder3.Rotate(Math.PI / 2, 0);
            cylinder3.Rotate(0, Math.PI / 2);

            var modeller = new Net3dBool.BooleanModeller(box, sphere);
            var tmp = modeller.GetIntersection();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder1);
            tmp = modeller.GetDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder2);
            tmp = modeller.GetDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder3);
            tmp = modeller.GetDifference();

            Mesh = tmp;

            var elapsed = DateTime.UtcNow - start;
            Console.WriteLine("done.");
            Console.WriteLine("Consumed time: {0}", elapsed);
        }

        public override void RenderMesh()
        {
            GL.Begin(PrimitiveType.Triangles);

            var verts = Mesh.GetVertices();
            int[] ind = Mesh.GetIndices();

            for (var i = 0; i < ind.Length; i = i + 3)
            {
                GL.Normal3(new Vector3(1, 1, 1));
                var p = verts[ind[i]];
                GL.Color3(Color.Red);
                GL.Vertex3(new Vector3d(p.X, p.Y, p.Z));

                p = verts[ind[i + 1]];
                GL.Color3(Color.Blue);
                GL.Vertex3(new Vector3d(p.X, p.Y, p.Z));

                p = verts[ind[i + 2]];
                GL.Color3(Color.Green);
                GL.Vertex3(new Vector3d(p.X, p.Y, p.Z));
            }

            GL.End();
        }

    }
}

