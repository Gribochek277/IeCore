using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Windows;
using Irrational.Utils;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using Irrational.Shaders;

namespace Irrational.Core.Entities
{
    public class SceneObject : ISceneObject
    {
         Volume _modelMesh;
        string _mdlSource;
        string _matSource;
		       

        public Dictionary<string, int> textures = new Dictionary<string, int>();
        public Dictionary<String, Material> materials = new Dictionary<string, Material>();

        public Vector3 Position {
            get {
                if (_modelMesh != null)
                {
                    return _modelMesh.Position;
                }
                else
                {
                    return Vector3.Zero;
                }
               } 
            set {
                if (_modelMesh != null)
                {
                    _modelMesh.Position = value;
                }
                else
                {
                    //TODO: handle input to null position somehow;
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                if (_modelMesh != null)
                {
                    return _modelMesh.Scale;
                }
                else
                {
                    return Vector3.Zero;
                }
            }
            set
            {
                if (_modelMesh != null)
                {
                    _modelMesh.Scale = value;
                }
                else
                {
                    //TODO: handle input to null position somehow;
                }
            }
        }

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
        
        public string ModelSource
        {
            get
            {
                if (_mdlSource != null)
                    return _mdlSource;
                else
                    throw new NullReferenceException();//TODO: create default model source
            }
            set { _mdlSource = value; }
        }
        
        public Volume ModelMesh {
            get
            {
                if (_modelMesh != null)
                    return _modelMesh;
                else
                    throw new NullReferenceException();//TODO: Remove get for realisation of incapsulation
            }
            set { _modelMesh = value; }
        }

        public Vector3 Rotation
        {
            get
            {
                if (_modelMesh != null)
                {
                    return _modelMesh.Rotation;
                }
                else
                {
                    return Vector3.Zero;
                }
            }
            set
            {
                if (_modelMesh != null)
                {
                    _modelMesh.Rotation = value;
                }
                else
                {
                    //TODO: handle input to null position somehow;
                }
            }
        }

		ShaderProg _shader = null;
		public ShaderProg shader
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

        public void OnRendered()
        {
	
        }

        public void OnResized()
        {
            throw new NotImplementedException();
        }

        public void OnTransform()
        {
            throw new NotImplementedException();
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        public void OnUpdated(double deltatime)
        {
            throw new NotImplementedException();
        }

        private void loadMaterials(String filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!materials.ContainsKey(mat.Key))
                {
                    materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in materials.Values)
            {
                if (File.Exists(mat.AmbientMap) && !textures.ContainsKey(mat.AmbientMap))
                {
                    textures.Add(mat.AmbientMap, loadImage(mat.AmbientMap));
                }

                if (File.Exists(mat.DiffuseMap) && !textures.ContainsKey(mat.DiffuseMap))
                {
                    textures.Add(mat.DiffuseMap, loadImage(mat.DiffuseMap));
                }

                if (File.Exists(mat.SpecularMap) && !textures.ContainsKey(mat.SpecularMap))
                {
                    textures.Add(mat.SpecularMap, loadImage(mat.SpecularMap));
                }

                if (File.Exists(mat.NormalMap) && !textures.ContainsKey(mat.NormalMap))
                {
                    textures.Add(mat.NormalMap, loadImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !textures.ContainsKey(mat.OpacityMap))
                {
                    textures.Add(mat.OpacityMap, loadImage(mat.OpacityMap));
                }
            }
        }

        int loadImage(Bitmap image)
        {
            int texID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        int loadImage(string filename)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return loadImage(file);
            }
            catch (FileNotFoundException e)
            {
                return -1;
            }
        }
    }
}
