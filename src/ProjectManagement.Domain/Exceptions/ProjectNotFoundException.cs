namespace ProjectManagement.Domain.Exceptions;

public class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException(Guid projectId)
        : base($"Project with ID '{projectId}' was not found.") { }
}