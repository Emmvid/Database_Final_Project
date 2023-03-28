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

    //skapa ärende, men jag måste kolla för nu behöver det finnas en kund först??
    public async Task CreateAsync(ComplaintEntity complaintEntity)
    {
        if (await _customerService.GetAsync(customerEntity => customerEntity.Id == complaintEntity.CustomerId) != null
           && await _statusService.GetAsync(statusEntity => statusEntity.Id == complaintEntity.StatusId) != null)
        {
            _context.Add(complaintEntity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<ComplaintEntity>> GetAllAsync()
    {
        return await _context.Complaints
            .Include(x => x.Customer)
            .Include(x => x.Status)
            .Include(x => x.Comments)
            .OrderByDescending(x => x.Created)
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

    public async Task<ComplaintEntity> UpdateStatusAsync(Expression<Func<ComplaintEntity, bool>> predicate)
    {
        var _complaintEntity = await _context.Complaints.FirstOrDefaultAsync(predicate);
        if (_complaintEntity != null)
        {
            switch (_complaintEntity.StatusId)
            {
                case 1:
                    _complaintEntity.StatusId = 2;
                    _complaintEntity.Modified = DateTime.Now;
                    break;
                case 2:
                    _complaintEntity.StatusId = 3;
                    _complaintEntity.Modified = DateTime.Now;
                    break;
                case 3:
                    _complaintEntity.StatusId = 2;
                    _complaintEntity.Modified = DateTime.Now;
                    break;
            }
            _context.Update(_complaintEntity);
            await _context.SaveChangesAsync();
        }

        return _complaintEntity!;
    }
}
