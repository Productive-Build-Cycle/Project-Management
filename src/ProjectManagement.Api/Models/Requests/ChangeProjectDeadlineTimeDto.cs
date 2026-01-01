namespace ProjectManagement.Api.Models.Requests;

using System.ComponentModel.DataAnnotations;

public class ChangeProjectDeadlineTimeDto
{
    [Required] public Guid Id { get; set; }

    [Required] public DateTime DeadlineTime { get; set; }
}