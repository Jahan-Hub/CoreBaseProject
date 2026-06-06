using CoreBaseProject.Application.ApplicationLogic.MasterSettings.CompanyLogic.Queries;
using CoreBaseProject.Application.ApplicationShared.Queries;

namespace CoreBaseProject.Api.Controllers.MasterSettings
{
    public class CustomerController : BaseController
    {
        [HttpPost]
        [CheckAuthorize("Customer", "List")]
        public async Task<ActionResult<PaginatedResponse<CustomerGridModel>>> GetPaginatedAsync(GetPaginatedCustomerQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerViewModel>> GetByIdAsync(string id)
        {
            var customerViewModel = new CustomerViewModel
            {
                UpdateModel = await Mediator.Send(new GetCustomerDetailsQuery { Id = id })
            };

            // Select list         
            customerViewModel.OptionsDataSources.StatusSelectList = Mediator.Send(new GetEnumTypeCollectionQuery { EnumTypeId = BaseEnumConfiguration.CoreBaseProject.GlobalStatus }).Result;
            customerViewModel.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());

            return Ok(customerViewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [CheckAuthorize("Customer", "Create")]
        public async Task<ActionResult<int>> CreateAsync(CreateCustomerCommand customerCreateModel)
        {
            if (ModelState.IsValid)
            {
                var createCustomerId = await Mediator.Send(customerCreateModel);
                return Ok(createCustomerId);
            }

            return BadRequest(customerCreateModel);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomerUpdateModel), StatusCodes.Status200OK)]
        [CheckAuthorize("Customer", "Update")]
        public async Task<ActionResult<CustomerUpdateModel>> UpdateAsync(UpdateCustomerCommand customerUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var updateCustomer = await Mediator.Send(customerUpdateModel);
                return Ok(updateCustomer);
            }

            return BadRequest(customerUpdateModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("Customer", "Delete")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var deleteCustomer = await Mediator.Send(new DeleteCustomerCommand { Id = id });
            return Ok(deleteCustomer);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomerImageUpdateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerImageUpdateModel>> CustomerProfileImageUpdateAsync([FromForm] UpdateCustomerImageCommand customerImageUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var updateCustomerImage = await Mediator.Send(customerImageUpdateModel);
                return Ok(updateCustomerImage);
            }

            return BadRequest(customerImageUpdateModel);
        }
    }
}