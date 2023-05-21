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
				.ForMember(dest => dest.OffsetMatrix, opt => opt.MapFrom(src => src.OffsetMatrix.ToNumericMatrix()));
		}
	}
}
