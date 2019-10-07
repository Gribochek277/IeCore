using Assimp;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrrationalEngineCore.Loaders.Assimp.Extentions
{
    public static class AssimpExtentions
    {
        public static Vector3 AssimpToOpentkVector(this Vector3D assimpVector)
        {
            return new Vector3(assimpVector.X, assimpVector.Y, assimpVector.Z);
        }

        public static Vector2 AssimpToOpentkVector(this Vector2D assimpVector)
        {
            return new Vector2(assimpVector.X, assimpVector.Y);
        }

        public static List<Vector3> AssimpListToOpentkVector(this List<Vector3D> assimpListVector)
        {
            List<Vector3> resultList = new List<Vector3>();
            foreach (Vector3D assimpVector in assimpListVector)
            {
                resultList.Add(assimpVector.AssimpToOpentkVector());
            }
            return resultList;
        }

        public static List<Vector2> AssimpListToOpentkVector(this List<Vector2D> assimpListVector)
        {
            List<Vector2> resultList = new List<Vector2>();
            foreach (Vector2D assimpVector in assimpListVector)
            {
                resultList.Add(assimpVector.AssimpToOpentkVector());
            }
            return resultList;
        }

        public static List<Vector2> GetUVCoordChanel(this List<Vector3D> assimpListVector)
        {
            List<Vector2> resultList = new List<Vector2>();
            foreach (Vector3D assimpVector in assimpListVector)
            {
                Vector3 vector3 = assimpVector.AssimpToOpentkVector();
                resultList.Add(new Vector2(vector3.X, vector3.Y));
            }
            return resultList;
        }
    }
}
