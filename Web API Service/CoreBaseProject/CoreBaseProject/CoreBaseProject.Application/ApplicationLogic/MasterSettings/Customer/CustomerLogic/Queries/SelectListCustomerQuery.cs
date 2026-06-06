public class SelectListCustomerQuery : IRequest<ICollection<SelectModel>>
{
    public class Handler : IRequestHandler<SelectListCustomerQuery, ICollection<SelectModel>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ICustomerRepository customerRepository, IHttpContextAccessor httpContextAccessors)
        {
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessors;
        }

        public async Task<ICollection<SelectModel>> Handle(SelectListCustomerQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the user's Id from the current HTTP context
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

            // Check if the user Id is null or not
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

            var customerList = await _customerRepository.CustomerSelectListAsync(cancellationToken);
            return customerList;
        }
    }
}