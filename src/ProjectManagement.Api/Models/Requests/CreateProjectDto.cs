namespace ProjectManagement.Api.Models.Requests;

using System.ComponentModel.DataAnnotations;

public class CreateProjectDto
{
    [Required] [MaxLength(200)] public string Title { get; set; }

    [MaxLength(1000)] public string Description { get; set; }

    public string? TeamId { get; set; }

    [Required] public DateTime DeadlineTime { get; set; }
}