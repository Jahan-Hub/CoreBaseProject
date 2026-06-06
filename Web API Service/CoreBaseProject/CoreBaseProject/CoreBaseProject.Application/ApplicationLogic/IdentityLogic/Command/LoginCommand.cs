using CoreBaseProject.Application.ApplicationLogic.IdentityLogic.Model;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.AccessControl.UserAccessMapping.Model;
using CoreBaseProject.Application.Configurations;
using CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CoreBaseProject.Application.ApplicationLogic.IdentityLogic.Command
{
    public class LoginCommand : LoginModel, IRequest<UserModel>
    {
        public class Handler : IRequestHandler<LoginCommand, UserModel>
        {
            private readonly UserManager<User> _userManager;
            private readonly IOptions<AppSettings> _appSeeting;
            private readonly IMapper _mapper;
            private readonly IUserAccessMappingRepository _userAccessMappingRepository;
            private readonly ICustomerRepository _customerRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserLoginHistoryRepository _userLoginHistoryRepository;

            public Handler(UserManager<User> userManager, IOptions<AppSettings> appSeeting, IMapper mapper, IUserAccessMappingRepository userAccessMappingRepository,
                ICustomerRepository customerRepository, IHttpContextAccessor httpContextAccessor, IUserLoginHistoryRepository userLoginHistoryRepository)
            {
                _userManager = userManager;
                _userAccessMappingRepository = userAccessMappingRepository;
                _appSeeting = appSeeting;
                _mapper = mapper;
                _customerRepository = customerRepository;
                _httpContextAccessor = httpContextAccessor;
                _userLoginHistoryRepository = userLoginHistoryRepository;
            }

            public async Task<UserModel> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var existUser = await _userManager.FindByEmailAsync(request.Email);

                // Check disable login access for this customer
                if (existUser is not null && existUser.LockoutEnd is not null)
                    throw new Exception("Login access is currently disabled. Please reach out to the administrator for support.");

                if (existUser is not null && await _userManager.CheckPasswordAsync(existUser, request.Password))
                {
                    var mapExistUser = _mapper.Map<UserModel>(existUser);

                    // Check customer id exist or not
                    if (mapExistUser.CustomerId is not null && mapExistUser.CustomerId > 0)
                    {
                        // Get selected customer info
                        var selectedCustomerInfo = await _customerRepository.GetByIdAsync((int)mapExistUser.CustomerId);

                        // Get selected customer gander id
                        mapExistUser.CustomerEncryptedId = EncryptionService.Encrypt(mapExistUser.CustomerId.ToString());

                        // Check, customer have image
                        if (selectedCustomerInfo.Image is not null)
                            mapExistUser.Image = selectedCustomerInfo.Image;
                    }
                    else
                        mapExistUser.CompanyId = null;

                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSeeting.Value.JWTSecret));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, existUser.Id.ToString()),
                            new Claim("UserName", existUser.UserName!.ToString()),
                            new Claim("FullName", existUser.FullName!.ToString()),
                            new Claim("CustomerId",(existUser.CustomerId == null) ? 0.ToString() : existUser.CustomerId.ToString()!),
                            new Claim("CompanyId",(mapExistUser.CompanyId == null) ? 0.ToString() : mapExistUser.CompanyId.ToString()!),
                            new Claim("ForcePasswordChanged",(existUser.ForcePasswordChanged == null) ? "false": "true")
                        }),

                        Expires = DateTime.UtcNow.AddHours(10),
                        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);

                    // Set login user token
                    mapExistUser.Token = token;

                    // Get login user permission
                    var userPermissions = await _userAccessMappingRepository.GetUserWiseAccessAsync(existUser.Id);
                    var mapUserPermissions = _mapper.Map<ICollection<UserAccessMappingDetails>>(userPermissions);

                    if (mapUserPermissions is not null)
                        mapExistUser.UserAccessMappingDetails = mapUserPermissions;

                    //Get User IP Address
                    var clientLoginIp = IPHelper.GetIpAddress(_httpContextAccessor.HttpContext!);

                    //Save User IP Address in DB
                    var createUserLoginHistoryModel = new UserLoginHistory
                    {
                        UserId = existUser.Id,
                        LoginIp = clientLoginIp,
                        LoginDateTime = DateTime.UtcNow
                    };

                    var createUserLoginHistory =
                        await _userLoginHistoryRepository.CreateAsync(createUserLoginHistoryModel);

                    return mapExistUser;
                }

                throw new Exception("Email and password cannot matched! Please, try again.");
            }
        }
    }
}