using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.OrderManagementSystem.Core.Models.Product;

namespace Route.OrderManagementSystem.Infrastructure._Data.Config.Product_Config
{
	internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name)
                .HasMaxLength(100)
                .IsRequired();


            builder.Property(P => P.Price)
                .HasColumnType("decimal(12, 2)");

           

        }
    }
}
