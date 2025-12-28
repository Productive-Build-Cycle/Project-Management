namespace ProjectManagement.Domain.Exceptions;

public class ProjectNotFoundException : ProjectException
{
    public ProjectNotFoundException(Guid projectId)
        : base($"Project with ID '{projectId}' was not found.") { }
}