using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.OrderManagementSystem.Core.Models.Order;
using System.Reflection.Emit;

namespace Route.OrderManagementSystem.Infrastructure._Data.Config.Order_Config
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{

			builder.Property(order => order.Status)
				.HasConversion(
				(OStatus) => OStatus.ToString(),
				(OStatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)
				);

			builder.Property(order => order.TotalAmount)
				.HasColumnType("decimal(12,2)");

			builder.HasOne(o => o.Customer)
			.WithMany(c => c.Orders)
			.HasForeignKey("CustomerId");

			builder.HasMany(order => order.Items)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade);
		
		}
	}
}
