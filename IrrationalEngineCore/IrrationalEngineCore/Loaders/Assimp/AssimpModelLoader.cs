using Assimp;
using Irrational.Loaders.Interfaces;
using IrrationalEngineCore.Loaders.Assimp.Extentions;
using System;
using System.Collections.Generic;

namespace IrrationalEngineCore.Loaders.Assimp
{
    public class AssimpModelLoader : IModelLoader
    {
        List<Mesh> meshes = new List<Mesh>();
        public Irrational.Core.Entities.Mesh LoadFromFile(string path)
        {
            AssimpContext context = new AssimpContext();
            Scene loadedAssimpScene = context.ImportFile(path, PostProcessSteps.OptimizeMeshes | PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);
            if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
            {
                Console.WriteLine("Scene error");
            }

            processNode(loadedAssimpScene.RootNode, loadedAssimpScene);

            return convertAssimpToEngineFormat(meshes[0]);
        }

        private void processNode(Node node, Scene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                Mesh mesh = scene.Meshes[i];
                meshes.Add(mesh);
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }

        private Irrational.Core.Entities.Mesh convertAssimpToEngineFormat(Mesh assimpMesh)
        {
            Irrational.Core.Entities.Mesh mesh = new Irrational.Core.Entities.Mesh();

            mesh.Vertices = assimpMesh.Vertices.AssimpListToOpentkVector().ToArray();
            mesh.Normals = assimpMesh.Normals.AssimpListToOpentkVector().ToArray();
            mesh.UvCoords = assimpMesh.TextureCoordinateChannels[0].GetUVCoordChanel().ToArray();
            mesh.Indeces = assimpMesh.GetIndices();

            return mesh;
        }
    }
}
