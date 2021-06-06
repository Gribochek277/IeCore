using Assimp;
using AutoMapper;

namespace IeCore.MappingProfiles
{
    public class AnimationProfile : Profile
    {
        public AnimationProfile() 
        {
            CreateMap<Animation, IeCoreEntities.Animation.Animation>()
                .ForMember(dest => dest.FrameRate, opt => opt.MapFrom(src => src.TicksPerSecond))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Ticks, opt => opt.MapFrom(src => src.DurationInTicks));
        }
    }
}
