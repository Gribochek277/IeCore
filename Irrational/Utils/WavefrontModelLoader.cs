using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using IrrationalSpace.Utils.Interfaces;

namespace IrrationalSpace
{
    class WavefrontModelLoader : IModelLoader
    {
        public Mesh LoadFromFile(string path)
        {
            Mesh loadedModel = new Mesh();

            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    loadedModel = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: {0}", path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: {0} error {1}", path, e.Message);
            }


            return loadedModel;
        }

        public Mesh LoadFromString(string objModel)
        {
           
            // Seperate lines from the file
            List<String> lines = new List<string>(objModel.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            


            verts = LoadVertexes(lines);

            foreach(var vec in verts) { 
                  // Dummy color/texture coordinates for now
                  colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                 texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
            }



            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success |= int.TryParse(faceparts[1], out i2);
                        success |= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Tuple<int, int, int>(i1 - 1, i2 - 1, i3 - 1);
                            faces.Add(face);
                        }
                    }
                }
            }

            // Create the Mesh
            Mesh loadedModel = new Mesh();
            loadedModel.vertices = verts.ToArray();
            loadedModel.faces = new List<Tuple<int, int, int>>(faces);
            loadedModel.colors = colors.ToArray();
            loadedModel.texturecoords = texs.ToArray();







            return loadedModel;
        }


        public static List<Vector3> LoadVertexes(List<string> lines)
        {
            List<Vector3> objVertexies = new List<Vector3>();
            // Read file line by line
            foreach (String line in lines)
            {
                string[] coords = line.Replace("  ", " ").Split();
                if (coords[0] == "v")
                {
                    Vector3 vertex = new Vector3(
                        float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat)
                        );
                    objVertexies.Add(vertex);
                }
            }
            return objVertexies;
        }
    }
}
