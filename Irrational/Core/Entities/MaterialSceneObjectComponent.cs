using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Shaders;
using Irrational.Utils;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Irrational.Core.Entities
{
    public class MaterialSceneObjectComponent : ISceneObjectComponent
    {
        private string _matSource;
        private ShaderProg _shader;
        public Dictionary<string, int> textures = new Dictionary<string, int>();
        public Dictionary<String, Material> materials = new Dictionary<string, Material>();

        public string MaterialSource
        {
            get
            {
                if (_matSource != null)
                    return _matSource;
                else
                    throw new NullReferenceException();//TODO: create default material
            }
            set { _matSource = value; }
        }

        public ShaderProg Shader
        {
            get
            {
                if (_shader != null)
                {
                    return _shader;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                _shader = value;
            }
        }

        public void OnLoad()
        {
            loadMaterials(MaterialSource);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        private void loadMaterials(String filename)
        {
            foreach (var mat in MaterialLoader.LoadFromFile(filename))
            {
                if (!materials.ContainsKey(mat.Key))
                {
                    materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in materials.Values)
            {
                if (File.Exists(mat.DiffuseMap) && !textures.ContainsKey(mat.DiffuseMap))
                {
                    textures.Add(mat.DiffuseMap, loadImage(mat.DiffuseMap, PixelInternalFormat.Srgb8Alpha8));
                }

                if (File.Exists(mat.NormalMap) && !textures.ContainsKey(mat.NormalMap))
                {
                    textures.Add(mat.NormalMap, loadImage(mat.NormalMap, PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.OpacityMap) && !textures.ContainsKey(mat.OpacityMap))
                {
                    textures.Add(mat.OpacityMap, loadImage(mat.OpacityMap,PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.AmbientMap) && !textures.ContainsKey(mat.AmbientMap))
                {
                    textures.Add(mat.AmbientMap, loadImage(mat.AmbientMap,PixelInternalFormat.Rgba));
                }

                if (File.Exists(mat.SpecularMap) && !textures.ContainsKey(mat.SpecularMap))
                {
                    textures.Add(mat.SpecularMap, loadImage(mat.SpecularMap,PixelInternalFormat.Rgba));
                }
            }
        }

        int loadImage(Bitmap image, PixelInternalFormat textureColorspace)
        {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, textureColorspace, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        int loadImage(string filename, PixelInternalFormat textureColorspace)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return loadImage(file, textureColorspace);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
    }
}
