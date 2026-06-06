namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Queries
{
    public class GetPaginatedCustomerQuery : PaginationRequest, IRequest<PaginatedResponse<CustomerGridModel>>
    {
        public class Handler : IRequestHandler<GetPaginatedCustomerQuery, PaginatedResponse<CustomerGridModel>>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<User> _userManager;

            public Handler(ICustomerRepository customerRepository, IMapper mapper,IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _userManager = userManager;
            }

            public async Task<PaginatedResponse<CustomerGridModel>> Handle(GetPaginatedCustomerQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);
                
                // Get customers and map 
                var getCustomers = await _customerRepository.GetCustomersFilterAsync(request, cancellationToken);
                var mapGetCustomer = _mapper.Map<ICollection<CustomerGridModel>>(getCustomers.Data);

                // Get users and customer ids
                var customerIdSet = (await _userManager.Users
                        .AsNoTracking()
                        .Where(u => u.CustomerId != null)
                        .Select(u => u.CustomerId!.Value)
                        .Distinct()
                        .ToListAsync(cancellationToken))
                    .ToHashSet();

                var result = new PaginatedResponse<CustomerGridModel>
                {
                    Data = mapGetCustomer,
                    CurrentPage = getCustomers.CurrentPage,
                    TotalPages = getCustomers.TotalPages,
                    TotalRecords = getCustomers.TotalRecords,
                    PageSize = getCustomers.PageSize
                };

                foreach (var customer in result.Data)
                {
                    if (customerIdSet.Contains(customer.Id))
                    {
                        customer.HasUserId = true;
                    }
                }

                return result;
            }
        }
    }
}