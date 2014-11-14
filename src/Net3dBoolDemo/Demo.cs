using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Net3dBoolDemo
{

    public class TDemoWindow : TGameWindow
    {

        public Net3dBool.Solid mesh;

        public override void CreateMesh()
        {
            //            var a = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_BOX_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES.Length, Color.Red));
//            a.scale(1.5, 1.5, 1.5);
            //            var b = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES.Length, Color.Green));
//            var c = new Net3dBool.BooleanModeller(a, b);
//            mesh = c.getDifference();

            var b = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_BOX_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES.Length, Color.Red));
            var s = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES.Length, Color.Red));
            s.scale(0.68, 0.68, 0.68);

            var c1 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES.Length, Color.Green));
            c1.scale(0.38, 1, 0.38);

            var c2 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES.Length, Color.Green));
            c2.scale(0.38, 1, 0.38);
            c2.rotate(Math.PI / 2, 0);
//
            var c3 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES, getColorArray(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES.Length, Color.Green));
            c3.scale(0.38, 1, 0.38);
            c3.rotate(Math.PI / 2, 0);
            c3.rotate(0, Math.PI / 2);

            var modeller = new Net3dBool.BooleanModeller(b, s);
            var tmp = modeller.getIntersection();

            modeller = new Net3dBool.BooleanModeller(tmp, c1);
            tmp = modeller.getDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, c2);
            tmp = modeller.getDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, c3);
            tmp = modeller.getDifference();

            mesh = tmp;
        }

        public override void RenderMesh()
        {
            GL.Begin(PrimitiveType.Triangles);

            var verts = mesh.getVertices();
            int[] ind = mesh.getIndices();
            Net3dBool.Color3f[] colors = mesh.getColors();

            for (var i = 0; i < (int)(ind.Length / 3); i++)
            {
                GL.Normal3(new Vector3(1, 1, 1));
                var p = verts[ind[i * 3]];
                var c = colors[ind[i * 3]];
                //GL.Color3(c.r, c.g, c.b);
                GL.Color3(Color.Red);
                GL.Vertex3(new Vector3d(p.x, p.y, p.z));

                p = verts[ind[i * 3 + 1]];
                c = colors[ind[i * 3 + 1]];
                //GL.Color3(c.r, c.g, c.b);
                GL.Color3(Color.Blue);
                GL.Vertex3(new Vector3d(p.x, p.y, p.z));

                p = verts[ind[i * 3 + 2]];
                c = colors[ind[i * 3 + 2]];
                //GL.Color3(c.r, c.g, c.b);
                GL.Color3(Color.Green);
                GL.Vertex3(new Vector3d(p.x, p.y, p.z));
            }

            GL.End();
        }

    }
}

