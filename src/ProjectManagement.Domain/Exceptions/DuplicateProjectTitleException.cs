namespace ProjectManagement.Domain.Exceptions;

public class DuplicateProjectTitleException : ProjectException
{
    public DuplicateProjectTitleException(string projectTitle)
        : base($"Project with Title: '{projectTitle}' already exists.") { }
}