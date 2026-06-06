using CoreBaseProject.Application.ApplicationLogic.MasterSettings.CompanyLogic.Queries;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Model;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Queries;

namespace CoreBaseProject.Api.Controllers.MasterSettings
{
    public class UserController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(PaginatedResponse<UserGridModel>), StatusCodes.Status200OK)]
        [CheckAuthorize("User", "List")]
        public async Task<ActionResult<PaginatedResponse<UserGridModel>>> GetAllAsync(GetAllUserQuery getAllUserQuery)
        {
            var getUsers = await Mediator.Send(getAllUserQuery);
            return Ok(getUsers);
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserByCustomerIdAsync(int? customerId)
        {
            var getUser = await Mediator.Send(new UserByCustomerIdQuery());
            return Ok(getUser);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserViewModel>> GetByIdAsync(string? id)
        {
            var userViewModel = new UserViewModel
            {
                UserUpdateModel = await Mediator.Send(new GetUserDetailsQuery { Id = id })
            };

            // Select list
            userViewModel.OptionsDataSources.CompanySelectList = Mediator.Send(new SelectListCompanyQuery()).Result;
            return Ok(userViewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("User", "Create")]
        public async Task<ActionResult<bool>> CreateAsync(CreateUserCommand userCreateModel)
        {
            if (ModelState.IsValid)
            {
                var createUser = await Mediator.Send(userCreateModel);
                return Ok(createUser);
            }
            return BadRequest(userCreateModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserCreateFromCustomerModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserCreateFromCustomerModel>> UserCreateFromCustomerAsync(CreateUserFromCustomerCommand request)
        {
            var createUser = await Mediator.Send(request);
            return Ok(createUser);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserUpdateModel), StatusCodes.Status200OK)]
        [CheckAuthorize("User", "Update")]
        public async Task<ActionResult<UserUpdateModel>> UpdateAsync(UpdateUserCommand userUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var updateUser = await Mediator.Send(userUpdateModel);
                return Ok(updateUser);
            }

            return BadRequest(userUpdateModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserViewModel>> GetSelectListUserAsync()
        {
            var userViewModel = new UserViewModel();
            userViewModel.OptionsDataSources.UserSelectList = await Mediator.Send(new SelectListUserQuery());
            return Ok(userViewModel);
        }
    }
}