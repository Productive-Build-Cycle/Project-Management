namespace ProjectManagement.Api.Models.Requests;

using System.ComponentModel.DataAnnotations;

public class AssignTeamToProjectDto
{
    [Required] public Guid Id { get; set; }

    [Required] public string TeamId { get; set; }
}