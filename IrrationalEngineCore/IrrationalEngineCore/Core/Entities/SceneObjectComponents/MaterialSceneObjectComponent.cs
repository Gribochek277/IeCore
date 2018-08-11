using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Shaders;
using Irrational.Utils.Interfaces;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Irrational.Core.Entities.SceneObjectComponents
{
    public class MaterialSceneObjectComponent : ISceneObjectComponent
    {
        private string _matSource;
        private ShaderProg _shader;
        private Dictionary<string, int> _textures = new Dictionary<string, int>();
        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        public Dictionary<string, int> Textures { get { return _textures; } set { _textures = value; } }
        public Dictionary<string, Material> Materials { get { return _materials; } set { _materials = value; } }
        private IMaterialLoader _materialLoader;

        public MaterialSceneObjectComponent(ShaderProg Shader, string MaterialSource, IMaterialLoader materialLoader)
        {
            _shader = Shader;
            _matSource = MaterialSource;
            _materialLoader = materialLoader;
        }

        public string MaterialSource
        {
            //TODO: create default material
            get { return _matSource != null ? _matSource : throw new NullReferenceException(); }
            set { _matSource = value; }
        }

        public ShaderProg Shader
        {
            get { return _shader ?? null; }
            set {_shader = value; }
        }

        public void OnLoad()
        {
            LoadMaterials(MaterialSource);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        private void LoadMaterials(String filename)
        {
            foreach (var mat in _materialLoader.LoadFromFile(filename))
            {
                if (!Materials.ContainsKey(mat.Key))
                {
                    Materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in Materials.Values)
            {
                if (File.Exists(mat.DiffuseMap) && !Textures.ContainsKey(mat.DiffuseMap))
                {
                    Textures.Add(mat.DiffuseMap, LoadImage(mat.DiffuseMap, PixelInternalFormat.Srgb8Alpha8));
                }

                if (File.Exists(mat.NormalMap) && !Textures.ContainsKey(mat.NormalMap))
                {
                    Textures.Add(mat.NormalMap, LoadImage(mat.NormalMap, PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.OpacityMap) && !Textures.ContainsKey(mat.OpacityMap))
                {
                    Textures.Add(mat.OpacityMap, LoadImage(mat.OpacityMap,PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.AmbientMap) && !Textures.ContainsKey(mat.AmbientMap))
                {
                    Textures.Add(mat.AmbientMap, LoadImage(mat.AmbientMap,PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.SpecularMap) && !Textures.ContainsKey(mat.SpecularMap))
                {
                    Textures.Add(mat.SpecularMap, LoadImage(mat.SpecularMap,PixelInternalFormat.Rgba));
                }


                if (File.Exists(mat.MetallicRoughness) && !Textures.ContainsKey(mat.MetallicRoughness))
                {
                    Textures.Add(mat.MetallicRoughness, LoadImage(mat.MetallicRoughness,PixelInternalFormat.Rgb));
                }
            }
        }
        // Similiar realisation exists in SkyboxComponent maybe need to be merged in future
        int LoadImage(Bitmap image, PixelInternalFormat textureColorspace)
        {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, textureColorspace, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        int LoadImage(string filename, PixelInternalFormat textureColorspace)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return LoadImage(file, textureColorspace);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
    }
}
