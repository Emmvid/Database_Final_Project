using Database_Final_Project.Data;
using Database_Final_Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace Database_Final_Project.Services;

internal class ComplaintService
{
    private readonly DataContext _context = new();
    private readonly CustomerService _customerService = new CustomerService();
    private readonly StatusService _statusService = new StatusService();

    /*public async Task<ComplaintEntity> CreateAsync(ComplaintEntity complaintEntity)
    {
        if(await _customerService.GetAsync(X => X.id  == complaintEntity.Customerid) != null) && 
           await 


    }*/
    public async Task<IEnumerable<ComplaintEntity>> GetAllAsyn()
    {
        return await _context.Complaints
             .Include(x => x.Customer)
            .Include(x => x.Status)
            .Include(x => x.Comments)
            .ToListAsync();
    }
    public async Task<ComplaintEntity> GetAsync(Expression<Func<ComplaintEntity, bool>> predicate)
    {
        var _caseEntity = await _context.Complaints
            .Include(x => x.Customer)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(predicate);
        return _caseEntity!;
    }
}
 