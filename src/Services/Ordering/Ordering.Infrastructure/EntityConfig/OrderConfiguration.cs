using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.EntityConfig;

public class OrderConfiguration  : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.TotalPrice).IsRequired();
        
        builder.Property(x => x.FirstName).IsRequired(false);
        builder.Property(x => x.LastName).IsRequired(false);
        builder.Property(x => x.EmailAddress).IsRequired(false);
        builder.Property(x => x.AddressLine).IsRequired(false);
        builder.Property(x => x.Country).IsRequired(false);
        builder.Property(x => x.State).IsRequired(false);
        builder.Property(x => x.ZipCode).IsRequired(false);
        builder.Property(x => x.CardName).IsRequired(false);
        builder.Property(x => x.CardNumber).IsRequired(false);
        builder.Property(x => x.Expiration).IsRequired(false);
        builder.Property(x => x.CVV).IsRequired(false);
        builder.Property(x => x.PaymentMethod).IsRequired(false);
    }
}