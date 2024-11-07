using AutoMapper;

namespace HospitalMS.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName));
            CreateMap<Doctor, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.ToString()));
            CreateMap<Nurse, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.ToString()));
            CreateMap<Admin, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.ToString()))
                .ForMember(dest => dest.Image, src => src.MapFrom(x => x.Imag))
                ;
            CreateMap<SuperAdmin, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.ToString()))
                .ForMember(dest => dest.Image, src => src.MapFrom(x => x.Imag))
                ;
            CreateMap<Doctor, AdminNurseDoctorViewModel>();
            CreateMap<AdminNurseDoctorViewModel,Doctor>();
            CreateMap<AdminNurseDoctorViewModel, Nurse>();
            CreateMap<Nurse, AdminNurseDoctorViewModel>();
            CreateMap<ApplicationUser, Patient>()
                .ForMember(dest => dest.Password, src => src.MapFrom(x => x.PasswordHash))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.Username, src => src.MapFrom(x => x.UserName));
                

        }
    }
}
