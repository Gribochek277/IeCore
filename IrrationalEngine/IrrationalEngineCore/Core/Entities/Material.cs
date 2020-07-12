using System;
using OpenTK;

namespace IrrationalEngineCore.Core.Entities {
    public class Material {
        public string MaterialName = string.Empty;
        public Vector3 AmbientColor = new Vector3 ();
        public Vector3 DiffuseColor = new Vector3 ();
        public Vector3 SpecularColor = new Vector3 ();
        public float SpecularExponent = 1;
        public float Opacity = 1.0f;

        public string AmbientMap = "";
        public string DiffuseMap = "";
        public string SpecularMap = "";
        public string OpacityMap = "";
        public string NormalMap = "";
        public string MetallicRoughness = "";
        public string Metallic = "";
        public string Roughness = "";

        public float NormalScale = 1;

        public Material () { }

        public Material (Vector3 ambient, Vector3 diffuse, Vector3 specular, float specexponent = 1.0f, float opacity = 1.0f) {
            AmbientColor = ambient;
            DiffuseColor = diffuse;
            SpecularColor = specular;
            SpecularExponent = specexponent;
            Opacity = opacity;
        }
    }
}