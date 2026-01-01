namespace ProjectManagement.Domain.Exceptions;

public class TeamNotFoundException : NotFoundException
{
    public TeamNotFoundException(string teamId)
        : base($"Team with ID '{teamId}' was not found.") { }
}