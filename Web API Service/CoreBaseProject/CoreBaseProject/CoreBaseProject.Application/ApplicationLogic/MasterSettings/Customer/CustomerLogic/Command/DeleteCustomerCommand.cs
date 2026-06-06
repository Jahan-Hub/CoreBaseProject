namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Command
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCustomerCommand, bool>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly UserManager<User> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ICustomerRepository customerRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _customerRepository = customerRepository;
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt customer id
                var decryptCustomerId = EncryptionService.Decrypt(request.Id);

                // Convert decrypt customer id
                var convertCustomerId = Convert.ToInt32(decryptCustomerId);

                var isDeleteCustomer = false;
                var existCustomer = await _customerRepository.GetByIdAsync(convertCustomerId);

                if (existCustomer is null)
                    throw new BadRequestException(ProvideErrorMessage.CustomerIdNotFound);

                if (existCustomer is not null)
                {
                    existCustomer.IsDeleted = true;
                    existCustomer.DeletedDateTime = DateTime.UtcNow;
                    var updatedCustomer = await _customerRepository.UpdateAsync(existCustomer);

                    // Remove customer user table also
                    var getCustomerAsUser = await _userManager.FindByEmailAsync(existCustomer.Email!);
                    await _userManager.DeleteAsync(getCustomerAsUser);

                    isDeleteCustomer = true;
                }

                return isDeleteCustomer;
            }
        }
    }
}