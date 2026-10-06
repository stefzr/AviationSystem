using Aviation.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviation.Api.Data.Configurations;

public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        // Ορίζουμε το Id ως Primary Key για ΟΛΑ τα entities
        builder.HasKey(e => e.Id);
        
        // Ορίζουμε κανόνες για τα κοινά πεδία ελέγχου (audit)
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.LastUpdatedBy).HasMaxLength(100);
    }
}