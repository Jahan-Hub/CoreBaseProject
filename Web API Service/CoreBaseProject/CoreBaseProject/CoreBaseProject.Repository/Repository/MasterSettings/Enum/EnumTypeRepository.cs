using CoreBaseProject.Database.DatabaseContexts;
using CoreBaseProject.Model.Models.MasterSettings.Enum;
using CoreBaseProject.Repository.Base;
using CoreBaseProject.Repository.Contracts.MasterSettings.Enum;
using Microsoft.EntityFrameworkCore;

namespace CoreBaseProject.Repository.Repository.MasterSettings.Enum
{
    public class EnumTypeRepository : BaseRepository<EnumType>, IEnumTypeRepository
    {
        public EnumTypeRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<EnumType>> GetAllAsync()
        {
            var getEnymTypes = await dbContext.EnumTypes
                .AsNoTracking()
                .Where(et => !et.IsDeleted)
                .ToListAsync();

            return getEnymTypes;
        }

        public override async Task<EnumType?> GetByIdAsync(int id)
        {
            var getEnumType = await dbContext.EnumTypes
                .Where(et => et.Id == id && !et.IsDeleted)
                .FirstOrDefaultAsync();

            return getEnumType;
        }
    }
}