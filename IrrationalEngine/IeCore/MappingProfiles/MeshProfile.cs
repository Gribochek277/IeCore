using AutoMapper;
using IeCoreEntites.Model;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IeCore.MappingProfiles
{
    public class MeshProfile : Profile
    {
        public MeshProfile() 
        {
            CreateMap<Assimp.Mesh, Mesh>()
                .ForMember(dest => dest.Vertices, opt => opt.MapFrom(src => MapVertices(src)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Elements, opt => opt.MapFrom(src => src.Faces.SelectMany(face => face.Indices)));
        }

        private List<Vertex> MapVertices(Assimp.Mesh mesh)
        {
            List<Vertex> resultVertices = new List<Vertex>();
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex();
                vertex.Position = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                if (mesh.HasTextureCoords(0) == true)
                    vertex.TextureCoordinates = new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                if (mesh.HasNormals == true)
                    vertex.Normal = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);

                resultVertices.Add(vertex);
            }
            return resultVertices;
        }
    }
}
