using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Infrastructure.DataAccess.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired(false);

        builder.Property(x => x.TeamId)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.StartTime)
            .IsRequired(false);

        builder.Property(x => x.EndTime)
            .IsRequired(false);

        builder.Property(x => x.DeadlineTime)
            .IsRequired();

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.TeamId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DeadlineTime);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
