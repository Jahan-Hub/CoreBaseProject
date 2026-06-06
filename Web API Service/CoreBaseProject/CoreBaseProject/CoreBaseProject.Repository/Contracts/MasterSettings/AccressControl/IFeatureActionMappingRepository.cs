using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Repository.Contracts;

namespace CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl
{
    public interface IFeatureActionMappingRepository : IBaseRepository<FeatureActionMapping>
    {
        Task<ICollection<FeatureActionMapping>> GetFeatureWiseActionsAsync(int id);
        Task<ICollection<FeatureActionMapping>> GetAllFeatureWiseActionsAsync();
        Task<FeatureActionMapping?> GetFeatureActionMappingByFeatureAndActionId(int featureId, int actionId);
    }
}