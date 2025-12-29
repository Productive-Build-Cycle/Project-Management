using MediatR;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.Features.Queries.GetProjectById;

public class GetAllProjectsQuery : IRequest<PagedList<ProjectDto>>
{
    public int Page { get; set; }

    [Range(1, 1000)]
    public int PageSize { get; set; }

    public IList<Sort> SortColumns { get; set; } = new List<Sort>();

    public IList<FilterColumn> FilterColumn { get; set; } = new List<FilterColumn>();
} 