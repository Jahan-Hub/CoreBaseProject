using CoreBaseProject.Model.Models.MasterSettings.Enum;
using CoreBaseProject.Repository.Contracts;

namespace CoreBaseProject.Repository.Contracts.MasterSettings.Enum
{
    public interface IEnumTypeCollectionRepository : IBaseRepository<EnumTypeCollection>
    {
        Task<List<string>> GetEnumTypeCollectionByIds(List<int> ids);
    }
}