﻿using Assimp;
using AutoMapper;

namespace IeWin.MappingProfiles
{
	/// <summary>
	/// Mapping profile for animation
	/// </summary>
	public class AnimationProfile : Profile
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public AnimationProfile()
		{
			CreateMap<Animation, IeCoreEntities.Animation.Animation>()
				.ForMember(dest => dest.FrameRate, opt => opt.MapFrom(src => src.TicksPerSecond))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Ticks, opt => opt.MapFrom(src => src.DurationInTicks));
		}
	}
}
