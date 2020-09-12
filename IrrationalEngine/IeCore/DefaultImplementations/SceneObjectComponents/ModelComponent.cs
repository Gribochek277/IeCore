using IeCoreEntites.Model;
using System.Numerics;
using System.Linq;
using IeCoreInterfaces.SceneObjectComponents;

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

        public float[] GetVBODataOfModel() //TODO: Add caching;
        {
            return Model.Meshes.SelectMany(meshes => meshes.Vertices).SelectMany(vertice => vertice.FloatArray()).ToArray();
        }

        public uint[] GetIndicesOfModel() //TODO: Add caching;
        {
            return Model.Meshes.SelectMany(meshes => meshes.Elements).ToArray();
        }
    }
}
