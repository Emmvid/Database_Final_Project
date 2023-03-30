using Database_Final_Project.Data;
using Database_Final_Project.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database_Final_Project.Services;

internal class CustomerService
{
    private readonly DataContext _context = new();

    public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customerEntity)
    {
        var _customerEntity = await GetAsync(x => x.Email == customerEntity.Email);

        if (_customerEntity == null)
        {
            _customerEntity = customerEntity;
            _context.Add(_customerEntity);
            await _context.SaveChangesAsync();
        }
        return _customerEntity;
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<CustomerEntity> GetAsync(Expression<Func<CustomerEntity, bool>> predicate)
    {
        var _customerEntity = await _context.Customers.FirstOrDefaultAsync(predicate);
        // ! gör att den returnerar det den hittade och annars null.
        return _customerEntity!;
    }
}
