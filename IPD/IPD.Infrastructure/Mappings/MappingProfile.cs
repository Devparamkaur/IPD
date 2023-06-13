using AutoMapper;
using IPD.Application.DTOs.Identity;
using IPD.Infrastructure.Identity;

namespace IPD.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, ApplicationUser>()
               .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
