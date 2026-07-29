using BlazorApp1.Core.Entities;
using BlazorApp1.Core.Interfaces;

namespace BlazorApp1.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;
        public CustomerService(ICustomerRepository repo) => _repo = repo;

        public Task<IEnumerable<Customer>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Customer?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Customer c) => _repo.AddAsync(c);
        public Task UpdateAsync(Customer c) => _repo.UpdateAsync(c);
        public Task DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    }
}
