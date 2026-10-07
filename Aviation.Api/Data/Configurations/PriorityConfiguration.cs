using Aviation.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviation.Api.Data.Configurations;

public class PriorityConfiguration : BaseEntityConfiguration<Priority>
{
    public override void Configure(EntityTypeBuilder<Priority> builder)
    {
        base.Configure(builder);

        builder.Property(priority => priority.Severity)
            .IsRequired();

        builder.HasIndex(priority => priority.Severity)
            .IsUnique();
    }
}
