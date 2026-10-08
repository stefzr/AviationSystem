using Aviation.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviation.Api.Data.Configurations;

public class StatusConfiguration : BaseEntityConfiguration<Status>
{
    public override void Configure(EntityTypeBuilder<Status> builder)
    {
        base.Configure(builder);
    }
}
