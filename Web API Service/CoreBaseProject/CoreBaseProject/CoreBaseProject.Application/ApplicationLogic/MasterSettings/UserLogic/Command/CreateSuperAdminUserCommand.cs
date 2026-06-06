namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Command
{
    public class CreateSuperAdminUserCommand
    {
        private readonly UserManager<User> _userManager;
        private readonly ICustomerRepository _customerRepository;

        public CreateSuperAdminUserCommand(UserManager<User> userManager, ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _customerRepository = customerRepository;
        }

        public async Task SeedAsync()
        {
            // Fix id for supper admin
            var id = "a6923057-af80-4dd0-b3b2-3ff979f69b6d";

            // Fix email for supper admin
            var isEmailExist = await _customerRepository.IsEmailExistAsync("super_admin@gmail.com");

            // Create Super Admin Customer
            if (!isEmailExist)
            {
                var superAdminCustomer = new CoreBaseProject.Model.Models.MasterSettings.Customer
                {
                    FullName = "Super Admin",
                    CompanyId = 1,
                    Email = "super_admin@gmail.com",
                    PhoneNumber = string.Empty,
                    Image = string.Empty,
                    Address = string.Empty,
                    StatusId = GlobalStatus.Active,
                    CreatedById = "System",
                    CreatedDateTime = DateTime.UtcNow,
                };

                // Save Customer
                var createdCustomer = await _customerRepository.CreateAsync(superAdminCustomer);

                var registerUser = new User();
                registerUser.Id = id;
                registerUser.UserName = "super_admin@gmail.com";
                registerUser.Email = "super_admin@gmail.com";
                registerUser.FullName = "Super Admin";
                registerUser.CustomerId = createdCustomer.Id;
                registerUser.LockoutEnabled = false;
                registerUser.ForcePasswordChanged = true;
                registerUser.ApplicationUserTypeId = ApplicationUserType.Admin;

                // Save user
                await _userManager.CreateAsync(registerUser, "9qDH121P7%yF7@*");
            }
        }
    }
}