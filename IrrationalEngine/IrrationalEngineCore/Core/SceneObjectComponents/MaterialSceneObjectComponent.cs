using ImageMagick;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Loaders.Interfaces;
using IrrationalEngineCore.Core.Shaders.Abstractions;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace IrrationalEngineCore.Core.SceneObjectComponents
{
    public class MaterialSceneObjectComponent : ISceneObjectComponent
    {
        private string _matSource;
       
        private IMaterialLoader _materialLoader;

        public IShaderImplementation shaderImplementation {get; private set;}

        public MaterialSceneObjectComponent(IShaderImplementation ShaderImplementation, string MaterialSource, IMaterialLoader materialLoader)
        {
            _matSource = MaterialSource;
            _materialLoader = materialLoader;
            shaderImplementation = ShaderImplementation;
        }

        public string MaterialSource
        {
            //TODO: create default material
            get { return _matSource != null ? _matSource : throw new NullReferenceException(); }
            set { _matSource = value; }
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
                if (!shaderImplementation.Materials.ContainsKey(mat.Key))
                {
                    shaderImplementation.Materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in shaderImplementation.Materials.Values) //TODO; probably required textures should be retrieved from shaderImplementation
            {
                if (File.Exists(mat.DiffuseMap) && !shaderImplementation.Textures.ContainsKey(mat.DiffuseMap))
                {
                    shaderImplementation.Textures.Add(mat.DiffuseMap, LoadImage(mat.DiffuseMap, PixelInternalFormat.Srgb8));
                }

                if (File.Exists(mat.NormalMap) && !shaderImplementation.Textures.ContainsKey(mat.NormalMap))
                {
                    shaderImplementation.Textures.Add(mat.NormalMap, LoadNormalsImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !shaderImplementation.Textures.ContainsKey(mat.OpacityMap))
                {
                    shaderImplementation.Textures.Add(mat.OpacityMap, LoadImage(mat.OpacityMap,PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.AmbientMap) && !shaderImplementation.Textures.ContainsKey(mat.AmbientMap))
                {
                    shaderImplementation.Textures.Add(mat.AmbientMap, LoadImage(mat.AmbientMap,PixelInternalFormat.Rgb));
                }

                if (File.Exists(mat.SpecularMap) && !shaderImplementation.Textures.ContainsKey(mat.SpecularMap))
                {
                    shaderImplementation.Textures.Add(mat.SpecularMap, LoadImage(mat.SpecularMap,PixelInternalFormat.Rgba));
                }


                if (File.Exists(mat.MetallicRoughness) && !shaderImplementation.Textures.ContainsKey(mat.MetallicRoughness))
                {
                    shaderImplementation.Textures.Add(mat.MetallicRoughness, LoadImage(mat.MetallicRoughness,PixelInternalFormat.Rgb));
                }
            }
        }
        // Similar realization exists in SkyboxComponent maybe need to be merged in future
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

        int LoadNormalsImage(string filename)
        {
           
           MagickImage img = new MagickImage(filename);

            byte[] data = img.GetPixels().ToByteArray(0, 0, img.Width, img.Height, "RGB");
           
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, img.Width, img.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data);



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
