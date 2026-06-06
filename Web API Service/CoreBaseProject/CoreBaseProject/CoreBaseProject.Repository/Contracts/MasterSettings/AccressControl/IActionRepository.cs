using CoreBaseProject.Shared.Pagination;

namespace CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl
{
    public interface IActionRepository : IBaseRepository<Model.Models.MasterSettings.AccessControl.Action>
    {
        Task<Model.Models.MasterSettings.AccessControl.Action?> GetActionByName(string name);
        Task<PaginatedResponse<Model.Models.MasterSettings.AccessControl.Action>> GetActionsFilterAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}