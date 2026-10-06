using Aviation.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviation.Api.Data.Configurations;

public class MaintenanceTaskConfiguration : BaseEntityConfiguration<MaintenanceTask>
{
    public override void Configure(EntityTypeBuilder<MaintenanceTask> builder)
    {
        // Εφαρμόζουμε τους βασικούς κανόνες (το Id ως Primary Key)
        base.Configure(builder);

        // Προσθέτουμε κανόνες ειδικά για το MaintenanceTask
        builder.Property(e => e.AircraftTailNumber)
               .IsRequired()
               .HasMaxLength(20);
    }
}