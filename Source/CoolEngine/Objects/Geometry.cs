using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace OpenGL_Game.Objects
{
    public class Geometry
    {
        List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
        int numberOfTriangles;
        VertexBuffer vertexBuffer;

        public Geometry()
        {

        }

        public void LoadObject(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fin);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (values[0].StartsWith("NUM_OF_TRIANGLES"))
                    {
                        numberOfTriangles = int.Parse(values[0].Remove(0, "NUM_OF_TRIANGLES".Length));
                        continue;
                    }
                    if (values[0].StartsWith("//") || values.Length < 5) continue;

                    var format = CultureInfo.InvariantCulture.NumberFormat;
                    vertices.Add(new VertexPositionNormalTexture(new Vector3(float.Parse(values[0], format), float.Parse(values[1], format), float.Parse(values[2], format)),
                                 new Vector3(float.Parse(values[5], format), float.Parse(values[6], format), float.Parse(values[7], format)),
                                 new Vector2(float.Parse(values[3], format), float.Parse(values[4], format))));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            vertexBuffer = new VertexBuffer(CoolGameBase.gameInstance.GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData(vertices.ToArray());
        }

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public int NumberOfTriangles
        {
            get { return numberOfTriangles; }
        }

        public VertexPositionNormalTexture[] Vertices
        {
            get { return vertices.ToArray(); }
        }
    }
}
