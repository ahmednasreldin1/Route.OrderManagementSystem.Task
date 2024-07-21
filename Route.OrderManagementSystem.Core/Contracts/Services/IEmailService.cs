using Route.OrderManagementSystem.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(string toEmail, string subject, string message);
	}
}
