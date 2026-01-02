namespace ProjectManagement.Api.Models.Requests
{
    public class StartProjectDto
    {
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
