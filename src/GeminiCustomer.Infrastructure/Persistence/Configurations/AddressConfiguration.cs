using GeminiCustomer.Domain.Customers.Entities;
using GeminiCustomer.Domain.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiCustomer.Infrastructure.Persistence.Configurations;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => AddressId.Create(value));

        builder.Property(a => a.CustomerId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.Property(a => a.AddressLine1)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.AddressLine2)
            .HasMaxLength(100);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.PostCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Country)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(2); // ISO country codes are 2 characters

        builder.Property(a => a.IsDefault)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt);
    }
}