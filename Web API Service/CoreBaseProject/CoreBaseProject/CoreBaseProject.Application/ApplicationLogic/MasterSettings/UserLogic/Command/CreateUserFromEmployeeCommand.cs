using CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Model;

namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Command
{
    public class CreateUserFromCustomerCommand : UserCreateFromCustomerModel,IRequest<UserCreateFromCustomerModel>
    {
        public class Handler : IRequestHandler<CreateUserFromCustomerCommand, UserCreateFromCustomerModel>
        {
            private readonly UserManager<User> _userManager;
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, ICustomerRepository customerRepository, IMapper mapper)
            {
                _userManager = userManager;
                _customerRepository = customerRepository;
                _mapper = mapper;
            }

            public async Task<UserCreateFromCustomerModel> Handle(CreateUserFromCustomerCommand request, CancellationToken cancellationToken)
            {
                // Get customer details by id
                var customerDetails = await _customerRepository.GetByIdAsync(request.CustomerId);

                var requestUserModel = new UserCreateModel
                {
                    Email = customerDetails.Email!,
                    FullName = customerDetails.FullName,
                    Password = "Admin@123456" // password should be auto generated and send to customer office email
                };

                var existUser = await _userManager.Users.Where(u => u.Email == requestUserModel.Email).FirstOrDefaultAsync();

                if (existUser is not null)
                    throw new Exception("Email already exist! Try new one.");

                var registerUser = _mapper.Map<User>(requestUserModel);
                registerUser.UserName = requestUserModel.Email;
                registerUser.Email = requestUserModel.Email;
                registerUser.FullName = requestUserModel.FullName;
                registerUser.CustomerId = request.CustomerId;
                registerUser.LockoutEnabled = false;
                registerUser.ForcePasswordChanged = false;
                registerUser.ApplicationUserTypeId = ApplicationUserType.Customer;

                var result = _userManager.CreateAsync(registerUser, requestUserModel.Password);
                var registerCompleteUser = _mapper.Map<UserCreateModel>(registerUser);

                if (result.Result.Succeeded)
                    return request;
                else
                    throw new Exception(result.Result.Errors.Select(s => s.Description).FirstOrDefault());
            }
        }
    }
}