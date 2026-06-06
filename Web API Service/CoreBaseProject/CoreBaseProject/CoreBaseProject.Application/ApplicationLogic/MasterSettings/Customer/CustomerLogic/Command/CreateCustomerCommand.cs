
namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Command
{
    public class CreateCustomerCommand : CustomerCreateModel, IRequest<int>
    {
        public class Handler : IRequestHandler<CreateCustomerCommand, int>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ICustomerRepository customerRepository, UserManager<User> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _customerRepository = customerRepository;
                _userManager = userManager; 
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);
              
                var createdCustomer = _mapper.Map<CoreBaseProject.Model.Models.MasterSettings.Customer>(request);
                createdCustomer.CreatedById = userId;
                createdCustomer.CreatedDateTime = DateTime.UtcNow;
                createdCustomer.StatusId = GlobalStatus.Active;

                createdCustomer = await _customerRepository.CreateAsync(createdCustomer);

                // Created customer assign in the user table
                var registerUser = new User();
                registerUser.UserName = request.Email;
                registerUser.Email = request.Email;
                registerUser.FullName = request.FullName;
                registerUser.CustomerId = createdCustomer.Id;
                registerUser.ForcePasswordChanged = true;
                registerUser.ApplicationUserTypeId = ApplicationUserType.Customer;

                // Register customer as user
                await _userManager.CreateAsync(registerUser, "Customer@123");
                return createdCustomer.Id;
            }
        }
    }
}