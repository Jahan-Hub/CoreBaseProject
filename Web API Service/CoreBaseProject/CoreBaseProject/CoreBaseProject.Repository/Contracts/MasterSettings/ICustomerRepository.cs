using CoreBaseProject.Model.Models.MasterSettings;
using CoreBaseProject.Shared.Models;
using CoreBaseProject.Shared.Pagination;

namespace CoreBaseProject.Repository.Contracts.MasterSettings
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<PaginatedResponse<Customer>> GetCustomersFilterAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<IQueryable<Customer>> GetAllCustomerFilterByIdQuery(int? companyId, int? statusId);
        Task<string> GetCustomerNameByCustomerIdsAsync(int[] customerIds);
        Task<ICollection<SelectModel>> CustomerSelectListAsync(CancellationToken cancellationToken);
        Task<ICollection<SelectModel>> CustomerSelectListByCompanyIdAsync(int companyId, CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email);
    }
}