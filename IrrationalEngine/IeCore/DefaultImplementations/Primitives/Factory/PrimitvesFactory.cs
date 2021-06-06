using System.Numerics;
using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreEntities.Materials;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Primitives;
using IeCoreInterfaces.Shaders;

namespace IeCore.DefaultImplementations.Primitives.Factory
{
    public class PrimitvesFactory : IPrimitvesFactory
    {
        private const string CheckerboardTextureName = "CheckerboardTexture_resolution_2048x2048";
        private const string CubeName = "Cube";
        private const string CubeFileName = "defaultCubeFile";
        private const string MaterialName = "CubeDiffuse";
        private const string MaterialFileName = "cubeFile";
        private IAssetManager _assetManager;
        private IShaderProgram _shaderProgram;
        public PrimitvesFactory(IAssetManager assetManager, IShaderProgram shaderProgram)
        {
            _assetManager = assetManager;
            _shaderProgram = shaderProgram;
        }

        public ISceneObject CreateCube()
        {
            var registeredCubeModel = _assetManager.Retrieve<CubeModel>(CubeName);
            if (registeredCubeModel == null)
            { 
                CubeModel cube = new CubeModel(CubeName, CubeFileName);
                _assetManager.Register(cube);
                registeredCubeModel = cube;
            }
            SceneObject sceneObject = new SceneObject();

            ModelComponent modelSceneObject = new ModelComponent(registeredCubeModel/*"knight.fbx"*/);
            MaterialComponent materiaComponent = new MaterialComponent(_shaderProgram);

            var registereadMaterial = _assetManager.Retrieve<Material>(MaterialName);
            if(registereadMaterial == null)
            { 
                Material material = new Material(MaterialName, MaterialFileName);
                _assetManager.Register(material);

                material.DiffuseColor = new Vector4(0, 2, 0, 1);
                material.DiffuseTexture = _assetManager.Retrieve<Texture>(CheckerboardTextureName);

                registereadMaterial = material;
            }

            materiaComponent.Materials.Add(registereadMaterial.Name, registereadMaterial);
            sceneObject.AddComponent(modelSceneObject);
            sceneObject.AddComponent(materiaComponent);

            return sceneObject;
        }

        public ISceneObject CreateRectangle()
        {
            RectangleModel rectangleModel = new RectangleModel("Rectangle", "defaultRectangleFile");
            _assetManager.Register(rectangleModel);
            SceneObject sceneObject = new SceneObject();

            ModelComponent modelSceneObject = new ModelComponent(rectangleModel/*"knight.fbx"*/);
            MaterialComponent materiaComponent = new MaterialComponent(_shaderProgram);
            Material material = new Material("RectangleDiffuse", "reactangleFile");
            _assetManager.Register(material);

            material.DiffuseColor = new Vector4(0, 2, 0, 1);
            material.DiffuseTexture = _assetManager.Retrieve<Texture>(CheckerboardTextureName);
            materiaComponent.Materials.Add(material.Name, material);
            sceneObject.AddComponent(modelSceneObject);
            sceneObject.AddComponent(materiaComponent);

            return sceneObject;
        }

        public ISceneObject CreateTriangle()
        {
            TriangleModel triangleModel = new TriangleModel("Triangle", "defaultTriangleFile");
            _assetManager.Register(triangleModel);
            SceneObject sceneObject = new SceneObject();
            ModelComponent modelSceneObject = new ModelComponent(triangleModel);
            MaterialComponent materiaComponent = new MaterialComponent(_shaderProgram);
            Material material = new Material("TriangleDiffuse", "reactangleFile");
            _assetManager.Register(material);

            material.DiffuseColor = new Vector4(2, 0, 0, 1);
            material.DiffuseTexture = _assetManager.Retrieve<Texture>(CheckerboardTextureName);
            materiaComponent.Materials.Add(material.Name, material);
            sceneObject.AddComponent(modelSceneObject);
            sceneObject.AddComponent(materiaComponent);
            return sceneObject;
        }

    }
}
