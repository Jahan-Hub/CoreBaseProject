using BlazorApp1.Core.Entities;
using BlazorApp1.Core.Interfaces;
using BlazorApp1.Infrastructure.Data;
using System.Data.Entity;

namespace BlazorApp1.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _db.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _db.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _db.Customers.FindAsync(customer.Id);
            if (existingCustomer is null) return;

            // Update properties
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.DateOfBirth = customer.DateOfBirth;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return;
            _db.Customers.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
