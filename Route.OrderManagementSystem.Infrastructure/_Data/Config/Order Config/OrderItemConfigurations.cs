using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.OrderManagementSystem.Core.Models.Order;

namespace Route.OrderManagementSystem.Infrastructure._Data.Config.Order_Config
{
	internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{

			builder.Property(orderItem => orderItem.Price)
				.HasColumnType("decimal(12,2)");
		}
	}
}
