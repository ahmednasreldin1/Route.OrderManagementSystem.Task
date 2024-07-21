using Microsoft.AspNetCore.Identity;

namespace Route.OrderManagementSystem.Core.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
    }
}
