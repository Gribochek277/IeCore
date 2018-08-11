using OpenTK;
using System;

namespace Irrational.Core.Entities
{
    public class Material
    {
        public string MaterialName = String.Empty;
        public Vector3 AmbientColor = new Vector3();
        public Vector3 DiffuseColor = new Vector3();
        public Vector3 SpecularColor = new Vector3();
        public float SpecularExponent = 1;
        public float Opacity = 1.0f;

        public String AmbientMap = "";
        public String DiffuseMap = "";
        public String SpecularMap = "";
        public String OpacityMap = "";
        public String NormalMap = "";
        public String MetallicRoughness = "";

        public Material()
        {
        }

        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specexponent = 1.0f, float opacity = 1.0f)
        {
            AmbientColor = ambient;
            DiffuseColor = diffuse;
            SpecularColor = specular;
            SpecularExponent = specexponent;
            Opacity = opacity;
        }
    }
}
