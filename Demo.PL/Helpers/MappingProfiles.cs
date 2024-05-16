using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap()/*.ForMember(d => d.Name, o => o.MapFrom(s => s.EmpName))*/;

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

            CreateMap<RoleViewModel, IdentityRole>()
                     .ForMember(d => d.Name, o => o.MapFrom(s => s.RoleName)).ReverseMap();

            //CreateMap<IdentityRole, RoleViewModel>()
            //         .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Name)).ReverseMap();

        }
    }
}
