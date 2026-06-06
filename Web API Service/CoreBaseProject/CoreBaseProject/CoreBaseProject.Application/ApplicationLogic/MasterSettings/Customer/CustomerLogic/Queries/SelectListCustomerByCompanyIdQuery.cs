namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Queries
{
    public class SelectListCustomerByCompanyIdQuery : IRequest<ICollection<SelectModel>>
    {
        public int CompanyId { get; set; }

        public class Handler : IRequestHandler<SelectListCustomerByCompanyIdQuery, ICollection<SelectModel>>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ICustomerRepository customerRepository, IHttpContextAccessor httpContextAccessors)
            {
                _customerRepository = customerRepository;
                _httpContextAccessor = httpContextAccessors;
            }

            public async Task<ICollection<SelectModel>> Handle(SelectListCustomerByCompanyIdQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var customerList = await _customerRepository.CustomerSelectListByCompanyIdAsync(request.CompanyId, cancellationToken);
                return customerList;
            }
        }
    }
}