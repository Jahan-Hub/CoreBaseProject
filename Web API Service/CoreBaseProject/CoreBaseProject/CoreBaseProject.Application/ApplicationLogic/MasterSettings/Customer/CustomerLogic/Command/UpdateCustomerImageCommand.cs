using CoreBaseProject.Application.Configurations.UploadImages;

namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Command
{
    public class UpdateCustomerImageCommand : CustomerImageUpdateModel, IRequest<CustomerImageUpdateModel>
    {
        public class Handler : IRequestHandler<UpdateCustomerImageCommand, CustomerImageUpdateModel>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public Handler(ICustomerRepository customerRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _webHostEnvironment = webHostEnvironment;
            }

            public async Task<CustomerImageUpdateModel> Handle(UpdateCustomerImageCommand request, CancellationToken cancellationToken)
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

                var customerImagePath = UploadImageProvider.UploadImageFile(_webHostEnvironment, request.ImageFile!, "Upload", "CustomerImage");

                getExistCustomer.Image = customerImagePath;
                getExistCustomer.UpdatedById = userId;
                getExistCustomer.UpdatedDateTime = DateTime.UtcNow;

                getExistCustomer = await _customerRepository.UpdateAsync(getExistCustomer);
                return request;
            }
        }
    }
}