using CoreBaseProject.Database.DatabaseContexts;
using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Repository.Base;
using CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl;

namespace CoreBaseProject.Repository.Repository.MasterSettings.AccessControl
{
    public class RoleRepository : BaseRepository<Role>,IRoleRepository
    {
        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}