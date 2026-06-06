using CoreBaseProject.Model.Models.MasterSettings;
using CoreBaseProject.Shared.Models;

namespace CoreBaseProject.Repository.Contracts.MasterSettings
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<ICollection<SelectModel>> GetCompanySelectListAsync();
        Task<Company?> GetFirstOrDefaultCompanyAsync();
    }
}