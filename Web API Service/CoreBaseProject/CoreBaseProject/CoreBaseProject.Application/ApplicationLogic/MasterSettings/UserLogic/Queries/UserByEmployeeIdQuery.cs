namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Queries
{
    public class UserByCustomerIdQuery : IRequest<bool>
    {
        public int? CustomerId { get; set; }

        public class Handler : IRequestHandler<UserByCustomerIdQuery, bool>
        {
            private readonly UserManager<User> _userManager;
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper, ICustomerRepository customerRepository)
            {
                _userManager = userManager;
                _customerRepository = customerRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UserByCustomerIdQuery request, CancellationToken cancellationToken)
            {
                if (request.CustomerId is null)
                    return false;

                var users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);
                return users.Any(u => u.CustomerId == request.CustomerId);
            }
        }
    }
}