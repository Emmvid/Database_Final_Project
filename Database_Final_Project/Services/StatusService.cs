using Database_Final_Project.Data;
using Database_Final_Project.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database_Final_Project.Services;

internal class StatusService
{
    private readonly DataContext _context = new();

    public async Task CreateStatusIfNotExistsAsync()
    {
        if (!await _context.Statuses.AnyAsync())
        {
            string[] _statuses = new string[] { "Ej påbörjad", "Pågående", "Avslutad" };

            foreach (var status in _statuses)
            {
                await _context.AddAsync(new StatusEntity { StatusName = status });
                await _context.SaveChangesAsync();
            }

        }
    }

    public async Task<StatusEntity> GetAsync(Expression<Func<StatusEntity, bool>> predicate)
    {
        var _statusEntity = await _context.Statuses.FirstOrDefaultAsync(predicate);
        return _statusEntity!;
    }


}
