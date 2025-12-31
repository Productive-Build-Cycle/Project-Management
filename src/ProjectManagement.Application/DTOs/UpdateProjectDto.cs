namespace ProjectManagement.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateProjectDto
{
    [Required] public Guid Id { get; set; }

    [Required] [MaxLength(200)] public string Title { get; set; }

    [MaxLength(1000)] public string Description { get; set; }

    public string TeamId { get; set; }

    [Required] public DateTime DeadlineTime { get; set; }
}