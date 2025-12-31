namespace ProjectManagement.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateProjectDto
{
    [Required] [MaxLength(200)] public string Title { get; set; }

    [MaxLength(1000)] public string Description { get; set; }

    [Required] public string TeamId { get; set; }

    [Required] public DateTime DeadlineTime { get; set; }
}