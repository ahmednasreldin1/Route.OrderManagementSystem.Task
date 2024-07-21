namespace Route.OrderManagementSystem.APIs.DTOs
{
	public class CustomerDto
	{
		public int Id { get; set; }
		public string DisplayName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
	}
}
