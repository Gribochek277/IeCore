using IeCoreEntites.Model;
using System;
using System.Numerics;
using IeCoreInterfaces.SceneObjectComponents;
using System.Linq;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
    public class ModelComponent : IModelComponent
    {

        public string Name => "ModelSceneObjectComponent";
        private readonly string _modelName;

        public Model Model { get; private set; }
        public Vector3 Position { get ; set ; }

        public ModelComponent(string modelName)
        {
            _modelName = modelName;
        }

        public void OnLoad()
        {
            Model = Context.Assetmanager.Retrieve<Model>(_modelName);
        }

        public void OnUnload()
        {
            Model = null;
        }

        public void OnTransform()
        {
            Console.WriteLine("Implement Transform of Models " + Name);
        }

        public float[] GetVBODataOfModel()
        {
            return Model.Meshes.SelectMany(meshes => meshes.Vertices).SelectMany(vertice => vertice.FloatArray()).ToArray();
        }
    }
}
