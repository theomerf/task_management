using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace TaskManagementAPI.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDtoForRegistration, Account>();   
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
            CreateMap<Label, LabelDto>();
            CreateMap<LabelDtoForCreation, Label>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LabelDtoForUpdate, Label>();
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForMember(dest => dest.AccountEmail, opt => opt.MapFrom(src => src.Account!.Email))
                .ForMember(dest => dest.AccountFirstName, opt => opt.MapFrom(src => src.Account!.FirstName))
                .ForMember(dest => dest.AccountLastName, opt => opt.MapFrom(src => src.Account!.LastName));
            CreateMap<ProjectMemberDtoForCreation, ProjectMember>();
            CreateMap<ProjectMemberDtoForUpdate, ProjectMember>();
            CreateMap<Entities.Models.Task, TaskDetailsDto>()
                .ForMember(dest => dest.CreatedByEmail, opt => opt.MapFrom(src => src.CreatedBy!.Email))
                .ForMember(dest => dest.CreatedByFirstName, opt => opt.MapFrom(src => src.CreatedBy!.FirstName))
                .ForMember(dest => dest.CreatedByLastName, opt => opt.MapFrom(src => src.CreatedBy!.LastName))
                .ForMember(dest => dest.AssignedToEmail, opt => opt.MapFrom(src => src.AssignedTo!.Email))
                .ForMember(dest => dest.AssignedToFirstName, opt => opt.MapFrom(src => src.AssignedTo!.FirstName))
                .ForMember(dest => dest.AssignedToLastName, opt => opt.MapFrom(src => src.AssignedTo!.LastName))
                .ForMember(dest => dest.LabelName, opt => opt.MapFrom(src => src.Label!.Name))
                .ForMember(dest => dest.LabelColor, opt => opt.MapFrom(src => src.Label!.Color));
            var taskCreationMap = CreateMap<TaskDtoForCreation, Entities.Models.Task>();
                taskCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                taskCreationMap.ForMember(dest => dest.LabelId, opt => opt.Ignore());
            CreateMap<TaskDtoForUpdate, Entities.Models.Task>()
                .ForMember(dest => dest.LabelId, opt => opt.Ignore());
            CreateMap<TaskDtoForStatusUpdate, Entities.Models.Task>();
            CreateMap<TaskDtoForPriorityUpdate, Entities.Models.Task>();
            CreateMap<Attachment, AttachmentDetailsDto>()
                .ForMember(dest => dest.UploadedByEmail, opt => opt.MapFrom(src => src.UploadedBy!.Email))
                .ForMember(dest => dest.UploadedByFirstName, opt => opt.MapFrom(src => src.UploadedBy!.FirstName))
                .ForMember(dest => dest.UploadedByLastName, opt => opt.MapFrom(src => src.UploadedBy!.LastName));
            var taCreationMap = CreateMap<AttachmentDtoForCreation, Attachment>();
                taCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                taCreationMap.ForMember(dest => dest.TaskId, opt => opt.Ignore())
                    .ForMember(dest => dest.CommentId, opt => opt.Ignore());
            CreateMap<AttachmentDtoForUpdate, Attachment>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore());
            CreateMap<TimeLog, TimeLogDto>()
                .ForMember(dest => dest.LoggedByEmail, opt => opt.MapFrom(src => src.LoggedBy!.Email))
                .ForMember(dest => dest.TimeLogCategoryName, opt => opt.MapFrom(src => src.TimeLogCategory!.Name))
                .ForMember(dest => dest.TimeLogCategoryColor, opt => opt.MapFrom(src => src.TimeLogCategory!.Color));
            CreateMap<TimeLog, TimeLogDetailsDto>()
                .ForMember(dest => dest.LoggedByEmail, opt => opt.MapFrom(src => src.LoggedBy!.Email))
                .ForMember(dest => dest.LoggedByFirstName, opt => opt.MapFrom(src => src.LoggedBy!.FirstName))
                .ForMember(dest => dest.LoggedByLastName, opt => opt.MapFrom(src => src.LoggedBy!.LastName))
                .ForMember(dest => dest.TimeLogCategory, opt => opt.MapFrom(src => src.TimeLogCategory));
            var tlCreationMap = CreateMap<TimeLogDtoForCreation, TimeLog>();
                tlCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                tlCreationMap.ForMember(dest => dest.TaskId, opt => opt.Ignore())
                    .ForMember(dest => dest.LoggedById, opt => opt.Ignore());
            CreateMap<TimeLogDtoForUpdate, TimeLog>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.LoggedById, opt => opt.Ignore());
            CreateMap<TimeLogCategory, TimeLogCategoryDto>();
            CreateMap<TimeLogCategory, TimeLogCategoryDetailsDto>()
                .ForMember(dest => dest.TimeLogs, opt => opt.MapFrom(src => src.TimeLogs));
            CreateMap<TimeLogCategoryDtoForCreation, TimeLogCategory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TimeLogCategoryDtoForUpdate, TimeLogCategory>();
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.Author!.Email))
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => src.Author!.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author!.LastName));
            var commentCreationMap = CreateMap<CommentDtoForCreation, Comment>();
                commentCreationMap.ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore());
                commentCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CommentDtoForUpdate, Comment>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore());
            CreateMap<Mention, MentionDto>()
                .ForMember(dest => dest.MentionedUserEmail, opt => opt.MapFrom(src => src.MentionedUser!.Email))
                .ForMember(dest => dest.MentionedUserFirstName, opt => opt.MapFrom(src => src.MentionedUser!.FirstName))
                .ForMember(dest => dest.MentionedUserLastName, opt => opt.MapFrom(src => src.MentionedUser!.LastName));
            var mentionCreationMap = CreateMap<MentionDtoForCreation, Mention>();
                mentionCreationMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                mentionCreationMap.ForMember(dest => dest.CommentId, opt => opt.Ignore());
            CreateMap<ActivityLog, ActivityLogDto>()
                .ForMember(dest => dest.PerformedByEmail, opt => opt.MapFrom(src => src.PerformedBy!.Email))
                .ForMember(dest => dest.PerformedByFirstName, opt => opt.MapFrom(src => src.PerformedBy!.FirstName))
                .ForMember(dest => dest.PerformedByLastName, opt => opt.MapFrom(src => src.PerformedBy!.LastName));
        }
    }
}
