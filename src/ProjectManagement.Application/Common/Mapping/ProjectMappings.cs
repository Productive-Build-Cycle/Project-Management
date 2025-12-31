using Mapster;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Commands;

namespace ProjectManagement.Application.Common.Mapping;

public class ProjectMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectDto, CreateProjectCommand>();
    }
}