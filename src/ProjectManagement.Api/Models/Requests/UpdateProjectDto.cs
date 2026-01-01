namespace ProjectManagement.Api.Models.Requests;

using System.ComponentModel.DataAnnotations;

public class UpdateProjectDto
{
    [Required] public Guid Id { get; set; }

    [Required] [MaxLength(200)] public string Title { get; set; }

    [MaxLength(1000)] public string Description { get; set; }
}