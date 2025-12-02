using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace TaskManagementAPI.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDetailsDto>();
            var projectCreationMap = CreateMap<ProjectDtoForCreation, Project>();
                projectCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                projectCreationMap.ForMember(dest => dest.Settings, opt =>
                {
                    opt.Condition(src => src.ProjectSetting != null);
                    opt.MapFrom(src => src.ProjectSetting);
                });
            CreateMap<ProjectDtoForUpdate, Project>();
            CreateMap<ProjectSetting, ProjectSettingDto>();
            var settingCreationMap = CreateMap<ProjectSettingDtoForCreation, ProjectSetting>();
                settingCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProjectSettingDtoForUpdate, ProjectSetting>();
            CreateMap<LabelDto, Label>();
            CreateMap<TaskDto, Entities.Models.Task>();
        }
    }
}
