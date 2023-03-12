using Assimp;
using AutoMapper;
using IeCore.Extensions;

namespace IeCore.MappingProfiles
{
	public class BoneProfile : Profile
	{
		public BoneProfile()
		{
			CreateMap<Bone, IeCoreEntities.Animation.Bone>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.OffsetMatrix, opt => opt.MapFrom(src => src.OffsetMatrix.ToNumericMatrix()));
		}
	}
}
