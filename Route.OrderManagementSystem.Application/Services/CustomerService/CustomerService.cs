using Microsoft.AspNetCore.Identity;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Models.Customer;
using Route.OrderManagementSystem.Core.Models.Identity;
using Route.OrderManagementSystem.Core.Models.Order;
using Route.OrderManagementSystem.Core.Specifications.Order_Specs;

namespace Route.OrderManagementSystem.Application.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;


		public CustomerService(
            IUnitOfWork unitOfWork,
			UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
			_userManager = userManager;

		}
		public async Task<Customer> CreateCustomerAsync(Customer customer, string password)
        {
            if ((_userManager.FindByEmailAsync(customer.User.Email!)).Result is not null)
                throw new Exception("This Email is already in use.");

			var result = await _userManager.CreateAsync(customer.User, password);

			if (!result.Succeeded) throw new Exception("Can not create this Customer.");

            await _userManager.AddToRoleAsync(customer.User, "Customer");

            customer.UserId = _userManager.FindByEmailAsync(customer.User.Email!).Result?.Id!;

			_unitOfWork.Repository<Customer>().Add(customer);
            await _unitOfWork.CompleteAsync();  
            return customer;        
        }

        public async Task<IReadOnlyList<Order>> GetCustomerOrdersAsync(string customerEmail)
        {
            var spec = new OrderSpecifications(O => O.Customer.User.Email == customerEmail && O.CustomerEmail == customerEmail);

            var orderRepo = _unitOfWork.Repository<Order>();

            var orders = await orderRepo.GetAllWithSpecAsync(spec);

            return orders;
        }


    }
}
