namespace ProjectManagement.Domain.Exceptions;

public class ProjectException : Exception
{
    public ProjectException(string exceptionMessage)
        : base(exceptionMessage) { }
}

