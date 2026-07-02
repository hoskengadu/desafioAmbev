using DeveloperStore.Sales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeveloperStore.Sales.Infrastructure.Persistence.Configurations;

public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.SaleNumber, b =>
        {
            b.Property(x => x.Value).HasColumnName("SaleNumber").HasMaxLength(50).IsRequired();
        });
        builder.OwnsOne(x => x.Customer, b =>
        {
            b.Property(x => x.Id).HasColumnName("CustomerId");
            b.Property(x => x.Name).HasColumnName("CustomerName").HasMaxLength(200).IsRequired();
        });
        builder.OwnsOne(x => x.Branch, b =>
        {
            b.Property(x => x.Id).HasColumnName("BranchId");
            b.Property(x => x.Name).HasColumnName("BranchName").HasMaxLength(200).IsRequired();
        });
        builder.Property(x => x.TotalAmount).HasConversion(v => v.Amount, v => new DeveloperStore.Sales.Domain.ValueObjects.Money(v)).HasColumnType("decimal(18,2)");
        builder.OwnsMany(x => x.SaleItems, items =>
        {
            items.ToTable("SaleItems");
            items.WithOwner().HasForeignKey("SaleId");
            items.HasKey(x => x.Id);
            items.Property(x => x.ProductId).IsRequired();
            items.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
            items.Property(x => x.Quantity).IsRequired();
            items.Property(x => x.UnitPrice).HasConversion(v => v.Amount, v => new DeveloperStore.Sales.Domain.ValueObjects.Money(v)).HasColumnType("decimal(18,2)");
            items.OwnsOne(x => x.DiscountPercentage, p => p.Property(v => v.Value).HasColumnName("DiscountPercentage").HasColumnType("decimal(5,2)"));
            items.Property(x => x.DiscountAmount).HasConversion(v => v.Amount, v => new DeveloperStore.Sales.Domain.ValueObjects.Money(v)).HasColumnType("decimal(18,2)");
            items.Property(x => x.Total).HasConversion(v => v.Amount, v => new DeveloperStore.Sales.Domain.ValueObjects.Money(v)).HasColumnType("decimal(18,2)");
        });
        builder.Navigation(x => x.SaleItems).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
