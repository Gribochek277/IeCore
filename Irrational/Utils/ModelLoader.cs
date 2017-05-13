using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;


namespace IrrationalSpace
{
    static class ModelLoader
    {
        public static List<Vector3> LoadVertexes(string pathToModel)
        {
            List<Vector3> objVertexies = new List<Vector3>();
        
                string line;
			if (File.Exists(pathToModel))
			{
				System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

				while ((line = file.ReadLine()) != null)
				{
					string[] coords = line.Replace("  ", " ").Split();
					if (coords[0] == "v")
					{
						Vector3 vertex = new Vector3(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat));
						objVertexies.Add(vertex);
						//  Console.WriteLine(objVertexies[objVertexies.Count-1]);
					}
				}
			}
			else
			{
				foreach (var substring in Cube.cube.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{ 
					string[] coords = substring.Replace("  ", " ").Split();
					if (coords[0] == "v")
					{
						Vector3 vertex = new Vector3(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat));
						objVertexies.Add(vertex);
						//  Console.WriteLine(objVertexies[objVertexies.Count-1]);
					}
				}
			}
                
           return objVertexies;
        }
        public static List<Vector3> LoadNormals(string pathToModel)
        {
            List<Vector3> objNormals = new List<Vector3>();

            string line;
			if (File.Exists(pathToModel))
			{
				System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

				while ((line = file.ReadLine()) != null)
				{
					string[] coords = line.Replace("  ", " ").Split();
					if (coords[0] == "vn")
					{
						Vector3 vertex = new Vector3(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat));
						objNormals.Add(vertex);
						//          Console.WriteLine(objNormals[objNormals.Count-1]);
					}
				}
			}
			else
			{
				foreach (var substring in Cube.cube.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] coords = substring.Replace("  ", " ").Split();
					if (coords[0] == "vn")
					{
						Vector3 vertex = new Vector3(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat));
						objNormals.Add(vertex);
						//          Console.WriteLine(objNormals[objNormals.Count-1]);
					}
				}
			}

            return objNormals;
        }
        public static List<Vector2> LoadUV(string pathToModel)
        {
            List<Vector2> objUV = new List<Vector2>();

            string line;
			if (File.Exists(pathToModel))
			{
				System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

				while ((line = file.ReadLine()) != null)
				{
					string[] coords = line.Replace("  ", " ").Split();
					if (coords[0] == "vt")
					{
						Vector2 vertex = new Vector2(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat));
						objUV.Add(vertex);
						// Console.WriteLine(objUV[objUV.Count-1]);
					}
				}
			}
			else
			{
				foreach (var substring in Cube.cube.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] coords = substring.Replace("  ", " ").Split();
					if (coords[0] == "vt")
					{
						Vector2 vertex = new Vector2(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat));
						objUV.Add(vertex);
						// Console.WriteLine(objUV[objUV.Count-1]);
					}
				}
			}
            return objUV;
        }
        public static Mesh LoadModel(string pathToModel)
        {

            List<int> objFaces = new List<int>();

            Vector3[] tempVertices = LoadVertexes(pathToModel).ToArray();
            Vector3[] tempnormals = LoadNormals(pathToModel).ToArray();
            Vector2[] tempUvCoords = LoadUV(pathToModel).ToArray();



            List<Vector3> objVertexies = new List<Vector3>();
            List<Vector3> objNormals = new List<Vector3>();
            List<Vector2> objUV = new List<Vector2>();

            string line;
			if (File.Exists(pathToModel))
			{
				System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

				while ((line = file.ReadLine()) != null)
				{
					string[] splitedLine = line.Split(' ');
					if (splitedLine[0] == "f")
					{
						MatchCollection rows = Regex.Matches(line, "(\\d*)\\/(\\d*)\\/(\\d*)");
						for (int i = 0; i < rows.Count; i++)
						{
							objVertexies.Add(tempVertices[Int32.Parse(rows[i].Groups[1].Value) - 1]);
							objUV.Add(tempUvCoords[Int32.Parse(rows[i].Groups[2].Value) - 1]);
							objNormals.Add(tempnormals[Int32.Parse(rows[i].Groups[3].Value) - 1]);
						}
					}
				}
			}
			else
			{
				foreach (var substring in Cube.cube.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] splitedLine = substring.Split(' ');
					if (splitedLine[0] == "f")
					{
						MatchCollection rows = Regex.Matches(substring, "(\\d*)\\/(\\d*)\\/(\\d*)");
						for (int i = 0; i < rows.Count; i++)
						{
							objVertexies.Add(tempVertices[Int32.Parse(rows[i].Groups[1].Value) - 1]);
							objUV.Add(tempUvCoords[Int32.Parse(rows[i].Groups[2].Value) - 1]);
							objNormals.Add(tempnormals[Int32.Parse(rows[i].Groups[3].Value) - 1]);
						}
					}
				}
			}

            Mesh model = new Mesh();

			model.modelVertex = new VBO<Vector3>(objVertexies.ToArray());//right


			List<int> elements = new List<int>();
            for (int i = 0; i<objVertexies.ToArray().Length; i++)

			{
	            elements.Add(i);
            }
            model.modelElements = new VBO<int>(elements.ToArray(), BufferTarget.ElementArrayBuffer);
            model.modelTangents = new VBO<Vector3>(ModelLoader.CalculateTangents(objVertexies.ToArray(), objNormals.ToArray(), elements.ToArray(), objUV.ToArray()));
            model.modelUV = new VBO<Vector2>(objUV.ToArray());
            model.modelNormals = new VBO<Vector3>(objNormals.ToArray());

            return model;
        }


        /// <summary>
        /// Calculate the Tangent array based on the Vertex, Face, Normal and UV data.
        /// </summary>
        public static Vector3[] CalculateTangents(Vector3[] vertices, Vector3[] normals, int[] triangles, Vector2[] uvs)
        {
            Vector3[] tangents = new Vector3[vertices.Length];
            Vector3[] tangentData = new Vector3[vertices.Length];

            for (int i = 0; i < triangles.Length / 3; i++)
            {
                Vector3 v1 = vertices[triangles[i * 3]];
                Vector3 v2 = vertices[triangles[i * 3 + 1]];
                Vector3 v3 = vertices[triangles[i * 3 + 2]];

                Vector2 w1 = uvs[triangles[i * 3]];
                Vector2 w2 = uvs[triangles[i * 3 + 1]];
                Vector2 w3 = uvs[triangles[i * 3 + 2]];

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;
                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);

                tangents[triangles[i * 3]] += sdir;
                tangents[triangles[i * 3 + 1]] += sdir;
                tangents[triangles[i * 3 + 2]] += sdir;
            }

            for (int i = 0; i < vertices.Length; i++)
                tangentData[i] = (tangents[i] - normals[i] * Vector3.Dot(normals[i], tangents[i])).Normalize();

            return tangentData;
        }
    }
}
