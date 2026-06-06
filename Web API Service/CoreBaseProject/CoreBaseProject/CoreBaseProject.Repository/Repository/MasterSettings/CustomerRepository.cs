using CoreBaseProject.Database.DatabaseContexts;
using CoreBaseProject.Model.Models.MasterSettings;
using CoreBaseProject.Repository.Base;
using CoreBaseProject.Repository.Contracts.MasterSettings;
using CoreBaseProject.Shared.Models;
using CoreBaseProject.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreBaseProject.Repository.Repository.MasterSettings
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<Customer>> GetAllAsync()
        {
            var getEmployess = await dbContext.Customers
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Status)
                .ToListAsync();

            return getEmployess;
        }

        public override async Task<Customer?> GetByIdAsync(int id)
        {
            var getCustomer = await dbContext.Customers
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();

            return getCustomer!;
        }

        public async Task<IQueryable<Customer>> GetAllCustomerFilterByIdQuery(int? companyId, int? statusId)
        {
            // Get customers
            var customers = dbContext.Customers
                .Where(e => !e.IsDeleted)
                .Include(e => e.Status)
                .AsNoTracking()
                .AsQueryable();

            // Apply company id filter 
            if (companyId.HasValue && companyId.Value != -1)
                customers = customers.Where(e => e.CompanyId == companyId);           

            // Apply status id filter
            if (statusId.HasValue && statusId.Value != -1)
                customers = customers.Where(e => e.StatusId == statusId);

            return customers;
        }

        public async Task<string> GetCustomerNameByCustomerIdsAsync(int[] customerIds)
        {
            var customerNames = await dbContext.Customers
                .Where(e => customerIds.Contains(e.Id) && !e.IsDeleted)
                .Select(e => e.FullName)
                .ToListAsync();

            return string.Join(", ", customerNames);
        }

        public async Task<ICollection<SelectModel>> CustomerSelectListAsync(CancellationToken cancellationToken)
        {
            var customers = await dbContext.Customers
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Select(e => new SelectModel
                {
                    Id = e.Id,
                    Name = e.FullName
                })
                .ToListAsync(cancellationToken);

            return customers;
        }

        public async Task<ICollection<SelectModel>> CustomerSelectListByCompanyIdAsync(int companyId, CancellationToken cancellationToken)
        {
            var customers = await dbContext.Customers
                .AsNoTracking()
                .Where(e => !e.IsDeleted && e.CompanyId == companyId)
                .Select(e => new SelectModel
                {
                    Id = e.Id,
                    Name = e.FullName
                })
                .ToListAsync(cancellationToken);

            return customers;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            var isExist = await dbContext.Customers
                .AsNoTracking()
                .AnyAsync(e => e.Email.ToLower() == email.ToLower() && !e.IsDeleted);

            return isExist;
        }

        public async Task<PaginatedResponse<Customer>> GetCustomersFilterAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            // Get customer
            var getCustomer = dbContext.Customers
                .AsNoTracking()
                .Include(e => e.Company)
                .Include(e => e.Status)
                .Where(e => !e.IsDeleted)
                .AsNoTracking();

            // Apply filtering
            var filterValue = paginationRequest.SearchTerm?.Trim();
            if (!string.IsNullOrWhiteSpace(filterValue) && !string.IsNullOrEmpty(filterValue))
            {
                var searchPattern = $"%{filterValue}%";
                getCustomer = getCustomer.Where(e => 
                (e.FullName != null && EF.Functions.Like(e.FullName, searchPattern))
                || (e.Email != null && EF.Functions.Like(e.Email, searchPattern))
                || (e.PhoneNumber != null && EF.Functions.Like(e.PhoneNumber, searchPattern)));
            }

            // Apply sorting
            var sortableColumns = new Dictionary<string, Expression<Func<Customer, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["fullName"] = u => u.FullName,
                ["email"] = u => u.Email,
                ["companyName"] = u => u.Company.Name,
                ["id"] = u => u.Id
            };

            var sortColumn = paginationRequest.SortField?.Trim();
            var sortOrder = paginationRequest.SortOrder?.Trim();

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortableColumns.TryGetValue(sortColumn, out var sortExpression))
                getCustomer = string.Equals(sortOrder, "descend", StringComparison.OrdinalIgnoreCase) ? getCustomer.OrderByDescending(sortExpression) : getCustomer.OrderBy(sortExpression);
            else
                getCustomer = getCustomer.OrderBy(u => u.Id);

            // Get total count before paging
            var totalCount = await getCustomer.CountAsync(cancellationToken);

            // Apply paging
            var pageSize = paginationRequest.PageSize <= 0 ? 10 : paginationRequest.PageSize;
            var pageIndex = paginationRequest.Page < 0 ? 0 : paginationRequest.Page;

            // Retrieve paged result
            var result = await getCustomer
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Prepare paginated response
            var paginatedResponse = new PaginatedResponse<Customer>
            {
                Data = result,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalRecords = totalCount,
            };

            return paginatedResponse;
        }
    }
}