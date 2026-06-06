using CoreBaseProject.Model.Models.MasterSettings.ReportAccessControl;
using CoreBaseProject.Repository.Contracts;

namespace CoreBaseProject.Repository.Contracts.MasterSettings.ReportAccessControl
{
    public interface IReportUserAccessRepository : IBaseRepository<ReportUserAccess>
    {
        Task<ICollection<ReportUserAccess>> GetReportUserAccessesByUserAsync(string userId);
    }
}