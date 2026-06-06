using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Repository.Contracts;

namespace CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl
{
    public interface IRoleActionMappingRepository : IBaseRepository<RoleActionMapping>
    {
        Task<ICollection<RoleActionMapping>> GetRoleMappingById(int id);
    }
}