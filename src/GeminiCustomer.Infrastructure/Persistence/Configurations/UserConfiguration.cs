using GeminiCustomer.Domain.Customers;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users;
using GeminiCustomer.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeminiCustomer.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(u => u.CustomerId)
            .IsRequired()
            .HasConversion(
                customerId => customerId.Value,
                value => CustomerId.Create(value));

        // Configure the one-to-one relationship with Customer
        builder.HasOne(u => u.Customer)
            .WithOne() // This makes it truly one-to-one
            .HasForeignKey<User>(u => u.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(); // Customer navigation is required

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username_Unique");

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.Token);
        builder.Property(u => u.RefreshToken);
        builder.Property(u => u.TokenExpiry);

    }
}