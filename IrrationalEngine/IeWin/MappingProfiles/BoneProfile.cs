using Assimp;
using AutoMapper;
using IeWin.Extensions;

namespace IeWin.MappingProfiles
{
	/// <summary>
	/// Mapping profile for bones
	/// </summary>
	public class BoneProfile : Profile
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public BoneProfile()
		{
			CreateMap<Bone, IeCoreEntities.Animation.Bone>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.OffsetMatrix, opt => opt.MapFrom(src => src.OffsetMatrix.ToNumericMatrix()))
				.ForMember(dest => dest.VertexWeights, opt => opt.MapFrom(src => MapVertexWeights(src.VertexWeights)));
		}

		private List<Tuple<int, float>> MapVertexWeights(List<VertexWeight> assimpVertexWeights)
		{
			List<Tuple<int, float>> mappedVertexWeights = new List<Tuple<int, float>>();
			foreach (VertexWeight vertexWeight in assimpVertexWeights)
			{
				mappedVertexWeights.Add(new Tuple<int, float>(vertexWeight.VertexID, vertexWeight.Weight));
			}

			return mappedVertexWeights;
		}
	}
}
