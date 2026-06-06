using CoreBaseProject.Database.DatabaseContexts;
using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Repository.Base;
using CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl;
using Microsoft.EntityFrameworkCore;

namespace CoreBaseProject.Repository.Repository.MasterSettings.AccessControl
{
    public class RoleActionMappingRepository : BaseRepository<RoleActionMapping>, IRoleActionMappingRepository
    {
        public RoleActionMappingRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<ICollection<RoleActionMapping>> GetRoleMappingById(int id)
        {
            var RoleMappings = await dbContext.RoleActionMappings
                .AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.Feature)
                .Include(x => x.Action)
                .Include(x => x.Role)
                .ToListAsync();

            return RoleMappings;
        }
    }
}