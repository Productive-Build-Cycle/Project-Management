namespace ProjectManagement.Api.Models.Requests;

using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using System.ComponentModel.DataAnnotations;

public class ChangeProjectStatusDto
{
    [Required] public Guid Id { get; set; }

    [Required]
    [EnumDataType(typeof(ProjectStatus), ErrorMessage = "Invalid Project Status.")]
    public ProjectStatus Status { get; set; }
}