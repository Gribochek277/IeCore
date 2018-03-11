using Irrational.Core.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using Irrational.Core.Shaders;

namespace Irrational.Core.Entities.SceneObjectComponents
{
    public class SkyboxSceneObjectComponent : ISceneObjectComponent
    {
        private string _skyboxLocation;
        private int _texId = -1;
        private ShaderProg _shader;

        public string SkyboxLocation { get { return _skyboxLocation; } }        
        public int TexId { get { return _texId; } }
        public ShaderProg Shader { get { return _shader ?? null; } }

        public SkyboxSceneObjectComponent(string location, ShaderProg Shader)
        {
            _skyboxLocation = location;
            _shader = Shader;
        }

        public void OnLoad()
        {
            LoadSkybox();
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        private void LoadSkybox()
        {
            Dictionary<string, int> skyboxTexturesOrder = new Dictionary<string, int>();
            if (_skyboxLocation == string.Empty)
            {
                throw new FileNotFoundException("Provided location is empty");
            }

            string[] skyboxTextures;

            skyboxTextures = Directory.GetFiles(_skyboxLocation);

            if (skyboxTextures.Length > 6)
            {
                throw new Exception("Directory should contain only 6 textures");
            }

            for (int i = 0; i < skyboxTextures.Length; i++)
            {
               string filename = Path.GetFileName(skyboxTextures[i]).Split('.')[0];
               string texturePostfix = filename.Substring(Math.Max(0, filename.Length - 2));
               switch (texturePostfix)
               {
                   case "rt":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 0);
                           break;
                       }
                   case "lf":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 1);
                           break;
                       }
                   case "up":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 2);
                           break;
                       }
                   case "dn":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 3);
                           break;
                       }
                   case "bk":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 4);
                           break;
                       }
                   case "ft":
                       {
                           skyboxTexturesOrder.Add(skyboxTextures[i], 5);
                           break;
                       }

               }
            }

            _texId = LoadImage(skyboxTexturesOrder, PixelInternalFormat.Srgb8);
        }


        // Similiar realisation exists in Material scene object maybe need to be merged in future
        int LoadImage(Dictionary<int, Bitmap> loadedImages, PixelInternalFormat textureColorspace)
        {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, texID);
            foreach(var image in loadedImages)
            { 
            BitmapData data = image.Value.LockBits(new Rectangle(0, 0, image.Value.Width, image.Value.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX+image.Key, 0, textureColorspace, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.Value.UnlockBits(data);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return texID;
        }

        int LoadImage(Dictionary<string, int> textures, PixelInternalFormat textureColorspace)
        {
            Dictionary<int, Bitmap> loadedImages = new Dictionary<int, Bitmap>();
            try
            {
                foreach (var value in textures)
                { 
                    Bitmap file = new Bitmap(value.Key);
                    loadedImages.Add(value.Value, file);
                }
                return LoadImage(loadedImages, textureColorspace);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
    }
}
