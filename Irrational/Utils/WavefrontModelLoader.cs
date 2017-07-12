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
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<TempVertex, TempVertex, TempVertex>> faces = new List<Tuple<TempVertex, TempVertex, TempVertex>>();

            // Base values
            verts.Add(new Vector3());
            texs.Add(new Vector2());

            int currentindice = 0;

            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.X);
                        success |= float.TryParse(vertparts[1],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Y);
                        success |= float.TryParse(vertparts[2],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing vertex: {0}", line);
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("vt ")) // Texture coordinate
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector2 vec = new Vector2();

                    if (temp.Trim().Count((char c) => c == ' ') > 0) // Check if there's enough elements for a vertex
                    {
                        String[] texcoordparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(texcoordparts[0],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.X);
                        success |= float.TryParse(texcoordparts[1],NumberStyles.Any, CultureInfo.InvariantCulture, out vec.Y);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing texture coordinate: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing texture coordinate: {0}", line);
                    }

                    texs.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<TempVertex, TempVertex, TempVertex> face = new Tuple<TempVertex, TempVertex, TempVertex>(new TempVertex(), new TempVertex(), new TempVertex());

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        int i1, i2, i3;
                        int t1, t2, t3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0].Split('/')[0], out i1);
                        success |= int.TryParse(faceparts[1].Split('/')[0], out i2);
                        success |= int.TryParse(faceparts[2].Split('/')[0], out i3);

                        if (faceparts[0].Count((char c) => c == '/') == 2)
                        {
                            success |= int.TryParse(faceparts[0].Split('/')[1], out t1);
                            success |= int.TryParse(faceparts[1].Split('/')[1], out t2);
                            success |= int.TryParse(faceparts[2].Split('/')[1], out t3);
                        }
                        else
                        {
                            t1 = i1;
                            t2 = i2;
                            t3 = i3;
                        }

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            TempVertex v1 = new TempVertex(i1, 0, t1);
                            TempVertex v2 = new TempVertex(i2, 0, t2);
                            TempVertex v3 = new TempVertex(i3, 0, t3);

                            if (texs.Count < t1)
                            {
                                texs.Add(new Vector2());
                            }

                            if (texs.Count < t2)
                            {
                                texs.Add(new Vector2());
                            }

                            if (texs.Count < t3)
                            {
                                texs.Add(new Vector2());
                            }

                            face = new Tuple<TempVertex, TempVertex, TempVertex>(v1, v2, v3);
                            faces.Add(face);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing face: {0}", line);
                    }
                }
            }

            // Create the ObjVolume
            Mesh loadedModel = new Mesh();
            texs.Add(new Vector2());
            texs.Add(new Vector2());
            texs.Add(new Vector2());

            foreach (var face in faces)
            {
                FaceVertex v1 = new FaceVertex(verts[face.Item1.Vertex], new Vector3(), texs[face.Item1.Texcoord]);
                FaceVertex v2 = new FaceVertex(verts[face.Item2.Vertex], new Vector3(), texs[face.Item2.Texcoord]);
                FaceVertex v3 = new FaceVertex(verts[face.Item3.Vertex], new Vector3(), texs[face.Item3.Texcoord]);

                loadedModel.faces.Add(new Tuple<FaceVertex, FaceVertex, FaceVertex>(v1, v2, v3));
            }

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

        private class TempVertex
        {
            public int Vertex;
            public int Normal;
            public int Texcoord;

            public TempVertex(int vert = 0, int norm = 0, int tex = 0)
            {
                Vertex = vert;
                Normal = norm;
                Texcoord = tex;
            }
        }
    }
}
