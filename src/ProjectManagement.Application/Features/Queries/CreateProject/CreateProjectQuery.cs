using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.Features.Commands.CreateProject;

public class CreateProjectQuery : IRequest<Guid>
{
    [Required] [MaxLength(200)] public string Name { get; set; } = string.Empty;

    [Required] public Guid TeamId { get; set; }

    [Required] public DateTime DeadlineTime { get; set; }

    public string? Description { get; set; }

    [Required]
    [RegularExpression("Draft|InProgress|Completed", ErrorMessage = "Status must be Draft, InProgress, or Completed.")]
    public string Status { get; set; } = "Draft";
}