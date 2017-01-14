using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OpenGLTUT
{
    static class ModelLoader
    {
        public static List<Vector3> LoadVertexes(string pathToModel)
        {
            List<Vector3> objVertexies = new List<Vector3>();
        
                string line;
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
                
           return objVertexies;
        }
        public static List<Vector3> LoadNormals(string pathToModel)
        {
            List<Vector3> objNormals = new List<Vector3>();

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

            while ((line = file.ReadLine()) != null)
            {
                string[] coords = line.Replace("  "," ").Split();
                if (coords[0] == "vn")
                {
                    Vector3 vertex = new Vector3(float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(coords[3], CultureInfo.InvariantCulture.NumberFormat));
                    objNormals.Add(vertex);
          //          Console.WriteLine(objNormals[objNormals.Count-1]);
                }
            }

            return objNormals;
        }
        public static List<Vector2> LoadUV(string pathToModel)
        {
            List<Vector2> objUV = new List<Vector2>();

            string line;
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

            return objUV;
        }
        public static WavefrontModel LoadModel(string pathToModel)
        {

            WavefrontModel _model = new WavefrontModel();
            List<int> objFaces = new List<int>();

            Vector3[] tempVertices = LoadVertexes(pathToModel).ToArray();
            Vector3[] tempnormals = LoadNormals(pathToModel).ToArray();
            Vector2[] tempUvCoords = LoadUV(pathToModel).ToArray();



            List<Vector3> objVertexies = new List<Vector3>();
            List<Vector3> objNormals = new List<Vector3>();
            List<Vector2> objUV = new List<Vector2>();

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(pathToModel);

            while ((line = file.ReadLine()) != null)
            {
                string[] splitedLine = line.Split(' ');
                if (splitedLine[0] == "f")
                {
                  //  Match row = Regex.Match(line, "(\\d*)\\/(\\d*)\\/(\\d*) (\\d*)\\/(\\d*)\\/(\\d*) (\\d*)\\/(\\d*)\\/(\\d*)");
                    MatchCollection rows = Regex.Matches(line, "(\\d*)\\/(\\d*)\\/(\\d*)");
                    for (int i=0;i<rows.Count;i++)
                    {
                        objVertexies.Add(tempVertices[Int32.Parse(rows[i].Groups[1].Value) - 1]);
                        objUV.Add(tempUvCoords[Int32.Parse(rows[i].Groups[2].Value) - 1]);
                        objNormals.Add(tempnormals[Int32.Parse(rows[i].Groups[3].Value) - 1]);
                    }
                    //objVertexies.Add(tempVertices[Int32.Parse(row.Groups[1].Value) - 1]);
                    //objVertexies.Add(tempVertices[Int32.Parse(row.Groups[4].Value) - 1]);
                    //objVertexies.Add(tempVertices[Int32.Parse(row.Groups[7].Value) - 1]);

                    //objUV.Add(tempUvCoords[Int32.Parse(row.Groups[2].Value) - 1]);
                    //objUV.Add(tempUvCoords[Int32.Parse(row.Groups[5].Value) - 1]);
                    //objUV.Add(tempUvCoords[Int32.Parse(row.Groups[8].Value) - 1]);

                    //objNormals.Add(tempnormals[Int32.Parse(row.Groups[3].Value) - 1]);
                    //objNormals.Add(tempnormals[Int32.Parse(row.Groups[6].Value) - 1]);
                    //objNormals.Add(tempnormals[Int32.Parse(row.Groups[9].Value) - 1]);
                }
            }
            _model.vertices = objVertexies.ToArray();
            _model.normals = objNormals.ToArray();
            _model.uvCoords = objUV.ToArray();

            return _model;
        }
    }
}
