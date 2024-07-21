using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Models.Invoice;
using Route.OrderManagementSystem.Core.Models.Order;
using Route.OrderManagementSystem.Core.Models.Product;
using Route.OrderManagementSystem.Core.Specifications.Order_Specs;

namespace Route.OrderManagementSystem.Application.Services.OrderService
{
	public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IEmailService emailService
            )
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task<Order?> CreateOrderAsync(Order order)
        {

            if (order is not null)
            {
                // Validate order items and get total
                if (order.Items?.Count > 0)
                {
                    var productsRepo = _unitOfWork.Repository<Product>();
                    foreach (var item in order.Items)
                    {
                        var product = await productsRepo.GetAsync(item.ProductId);
                        if (product == null)
                        {
                            throw new Exception($"Product with ID {item.ProductId} not found.");
                        }
                        if (product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product.Name}");
                        }
                        else
                        {
                            item.Price = product.Price;

                            // Update product stock
                            product.Stock -= item.Quantity;
                            productsRepo.Update(product);
                        }
                    }
                    order.TotalAmount = order.Items.Sum(item => item.Price * item.Quantity);
                }

                // Apply Discounts
                ApplyDiscounts(order);


                // Generate invoice
                GenerateInvoice(order);

                // Save Order to Database
                _unitOfWork.Repository<Order>().Add(order);

                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    // Send email notification
                    await SendOrderNotificationAsync(order, false);

                    return order;
                }
            }

            return null;
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, int status)
        {
            var order = await _unitOfWork.Repository<Order>().GetAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.Status = (OrderStatus)status;
            _unitOfWork.Repository<Order>().Update(order);

            // Send status update notification
            await SendOrderNotificationAsync(order, true);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            var spec = new OrderSpecifications();

            var orderRepo = _unitOfWork.Repository<Order>();

            var orders = await orderRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string customerEmail, int orderId)
		{
            var spec = new OrderSpecifications(O => O.Customer.User.Email == customerEmail && O.Id == orderId);

            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            return order;
        }

        #region Helper Methods

        private void ApplyDiscounts(Order order)
        {
            if (order.TotalAmount > 200)
            {
                order.TotalAmount *= 0.9m; // 10% discount
            }
            else if (order.TotalAmount > 100)
            {
                order.TotalAmount *= 0.95m; // 5% discount
            }
        }

        private Invoice GenerateInvoice(Order order)
        {
            var invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = order.TotalAmount
            };
            _unitOfWork.Repository<Invoice>().Add(invoice);
            return invoice;
        }

        private async Task SendOrderNotificationAsync(Order order, bool isOrderUpdated)
        {
            var subject = !isOrderUpdated ? "Order Confirmation" : "Order Status Update";
            var message = !isOrderUpdated ? $"Dear {order.Customer.User.DisplayName},\n\nThank you for your order. Your order number is {order.Id}."
                                        : $"Dear {order.Customer.User.DisplayName}, your order with ID {order.Id} status has been updated to {order.Status.ToString()}.";
            await _emailService.SendEmailAsync(order.Customer.User.Email ?? "", subject, message);
        }

		#endregion

	}
}
