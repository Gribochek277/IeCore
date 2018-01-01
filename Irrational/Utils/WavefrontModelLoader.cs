using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Irrational.Utils.Interfaces;
using Irrational.Core.Entities;

namespace Irrational
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
        catch (FileNotFoundException)
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
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();

            List<Tuple<TempVertex, TempVertex, TempVertex>> faces = new List<Tuple<TempVertex, TempVertex, TempVertex>>();

            // Base values
            verts.Add(new Vector3());
            texs.Add(new Vector2());
            normals.Add(new Vector3());

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
                else if (line.StartsWith("vn ")) // Normal vector
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a normal
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing normal: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing normal: {0}", line);
                    }

                    normals.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<TempVertex, TempVertex, TempVertex> face = new Tuple<TempVertex, TempVertex, TempVertex>(new TempVertex(), new TempVertex(), new TempVertex());

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        int v1, v2, v3;
                        int t1, t2, t3;
                        int n1, n2, n3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0].Split('/')[0], out v1);
                        success |= int.TryParse(faceparts[1].Split('/')[0], out v2);
                        success |= int.TryParse(faceparts[2].Split('/')[0], out v3);

                        if (faceparts[0].Count((char c) => c == '/') >= 2)
                        {
                            success |= int.TryParse(faceparts[0].Split('/')[1], out t1);
                            success |= int.TryParse(faceparts[1].Split('/')[1], out t2);
                            success |= int.TryParse(faceparts[2].Split('/')[1], out t3);
                            success |= int.TryParse(faceparts[0].Split('/')[2], out n1);
                            success |= int.TryParse(faceparts[1].Split('/')[2], out n2);
                            success |= int.TryParse(faceparts[2].Split('/')[2], out n3);
                        }
                        else
                        {
                            if (texs.Count > v1 && texs.Count > v2 && texs.Count > v3)
                            {
                                t1 = v1;
                                t2 = v2;
                                t3 = v3;
                            }
                            else
                            {
                                t1 = 0;
                                t2 = 0;
                                t3 = 0;
                            }


                            if (normals.Count > v1 && normals.Count > v2 && normals.Count > v3)
                            {
                                n1 = v1;
                                n2 = v2;
                                n3 = v3;
                            }
                            else
                            {
                                n1 = 0;
                                n2 = 0;
                                n3 = 0;
                            }
                        }


                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            TempVertex tv1 = new TempVertex(v1, n1, t1);
                            TempVertex tv2 = new TempVertex(v2, n2, t2);
                            TempVertex tv3 = new TempVertex(v3, n3, t3);
                            face = new Tuple<TempVertex, TempVertex, TempVertex>(tv1, tv2, tv3);
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

            foreach (var face in faces)
            {
                FaceVertex v1 = new FaceVertex(verts[face.Item1.Vertex], normals[face.Item1.Normal], texs[face.Item1.Texcoord]);
                FaceVertex v2 = new FaceVertex(verts[face.Item2.Vertex], normals[face.Item2.Normal], texs[face.Item2.Texcoord]);
                FaceVertex v3 = new FaceVertex(verts[face.Item3.Vertex], normals[face.Item3.Normal], texs[face.Item3.Texcoord]);

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
