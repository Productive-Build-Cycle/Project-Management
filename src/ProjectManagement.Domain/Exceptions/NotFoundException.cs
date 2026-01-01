namespace ProjectManagement.Domain.Exceptions;

public class NotFoundException : ProjectException
{
    public NotFoundException(string exceptionMessage)
        : base(exceptionMessage) { }
}

