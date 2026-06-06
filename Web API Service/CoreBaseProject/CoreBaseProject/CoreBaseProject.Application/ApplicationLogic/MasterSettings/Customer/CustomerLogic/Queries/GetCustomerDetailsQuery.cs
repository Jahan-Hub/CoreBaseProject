namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Queries
{
    public class GetCustomerDetailsQuery : IRequest<CustomerUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetCustomerDetailsQuery, CustomerUpdateModel>
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

            public async Task<CustomerUpdateModel> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if customer id is null
                if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrEmpty(request.Id) || request.Id == "-1")
                    return new CustomerUpdateModel();

                // Decrypt customer id
                var decryptCustomerId = EncryptionService.Decrypt(request.Id);

                // Check if customer decrypt id is null
                if (string.IsNullOrWhiteSpace(decryptCustomerId) || string.IsNullOrEmpty(decryptCustomerId))
                    return new CustomerUpdateModel();

                // Convert decrypt customer id
                var convertCustomerId = Convert.ToInt32(decryptCustomerId);

                var getExistCustomer = await _customerRepository.GetByIdAsync(convertCustomerId);

                if (getExistCustomer is null)
                    return new CustomerUpdateModel();

                var mapExitCustomer = _mapper.Map<CustomerUpdateModel>(getExistCustomer);
                return mapExitCustomer;
            }
        }
    }
}