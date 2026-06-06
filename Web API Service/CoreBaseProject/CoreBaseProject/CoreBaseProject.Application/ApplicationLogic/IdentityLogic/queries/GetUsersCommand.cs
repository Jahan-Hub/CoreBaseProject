using Microsoft.Extensions.Options;
using CoreBaseProject.Application.ApplicationLogic.IdentityLogic.Model;

namespace CoreBaseProject.Application.ApplicationLogic.IdentityLogic.queries
{
    public class GetUsersCommand : IRequest<ICollection<UserModel>>
    {
        public int CompanyId { get;set; }

        public class Handler : IRequestHandler<GetUsersCommand, ICollection<UserModel>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IOptions<AppSettings> _appSeeting;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICustomerRepository _customerRepository;

            public Handler(UserManager<User> userManager, IOptions<AppSettings> appSeeting, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICustomerRepository customerRepository)
            {
                _userManager = userManager;
                _appSeeting = appSeeting;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _customerRepository = customerRepository;
        }

            public async Task<ICollection<UserModel>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Step 1: Get all users who need password reset and are linked with customers
                var existUsers = await _userManager.Users
                    .AsNoTracking()
                    .Where(x => x.ForcePasswordChanged && x.CustomerId != null)
                    .ToListAsync(cancellationToken);

                // Step 2: Apply filter only if necessary
                if (request.CompanyId != -1)
                {
                    // Collect customer IDs
                    var customerIds = existUsers
                        .Select(u => (int)u.CustomerId!)
                        .Distinct()
                        .ToList();

                    // Bulk fetch customers instead of calling one-by-one
                    var customers = await _customerRepository.GetAllAsync();

                    // Build dictionary for quick lookup
                    var customerDict = customers.ToDictionary(e => e.Id);

                    // Filter users in-memory
                    existUsers = existUsers.Where(user =>
                    {
                        if (user.CustomerId == null) return false;

                        var customer = customerDict[user.CustomerId.Value];

                        // Company filter
                        if (request.CompanyId != null && request.CompanyId != -1 && customer.CompanyId != request.CompanyId)
                        {
                            return false;
                        }

                        return true;
                    }).ToList();
                }

                var mapUser = _mapper.Map<ICollection<UserModel>>(existUsers);
                return mapUser;
            }
        }
    }
}