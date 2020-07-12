using Assimp;
using IrrationalEngineCore.Loaders.Interfaces;
using IrrationalEngineCore.Loaders.Assimp.Extentions;
using System;
using System.Collections.Generic;
using IrrationalEngineCore.Core.Entities;
using Mesh = Assimp.Mesh;
using Scene = Assimp.Scene;

namespace IrrationalEngineCore.Loaders.Assimp
{
    public class AssimpModelLoader : IModelLoader
    {
        List<Mesh> meshes = new List<Mesh>();
        public Core.Entities.Mesh LoadFromFile(string path)
        {
            AssimpContext context = new AssimpContext();
            const PostProcessSteps ASSIMP_LOAD_FLAGS = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
            Scene loadedAssimpScene = context.ImportFile(path, ASSIMP_LOAD_FLAGS);
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

        private Core.Entities.Mesh convertAssimpToEngineFormat(Mesh assimpMesh)
        {
            Core.Entities.Mesh mesh = new Core.Entities.Mesh();

            mesh.Vertices = assimpMesh.Vertices.AssimpListToOpentkVector().ToArray();
            mesh.Normals = assimpMesh.Normals.AssimpListToOpentkVector().ToArray();
            mesh.UvCoords = assimpMesh.TextureCoordinateChannels[0].GetUVCoordChanel().ToArray();
            mesh.Indeces = assimpMesh.GetIndices();

            return mesh;
        }
    }
}
