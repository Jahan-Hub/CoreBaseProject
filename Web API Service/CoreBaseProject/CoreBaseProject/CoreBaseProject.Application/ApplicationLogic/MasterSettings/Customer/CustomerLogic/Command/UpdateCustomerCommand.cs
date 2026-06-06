namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Command
{
    public class UpdateCustomerCommand : CustomerUpdateModel, IRequest<CustomerUpdateModel>
    {
        public class Handler : IRequestHandler<UpdateCustomerCommand, CustomerUpdateModel>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ICustomerRepository customerRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<CustomerUpdateModel> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get exist customer
                var getExistCustomer = await _customerRepository.GetByIdAsync(request.Id);

                if (getExistCustomer is null)
                    throw new BadRequestException(ProvideErrorMessage.CustomerIdNotFound);

                getExistCustomer = _mapper.Map((CustomerUpdateModel)request, getExistCustomer);
                getExistCustomer.UpdatedById = userId;
                getExistCustomer.UpdatedDateTime = DateTime.UtcNow;

                getExistCustomer = await _customerRepository.UpdateAsync(getExistCustomer);
                return request;
            }
        }
    }
}