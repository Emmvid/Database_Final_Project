using Database_Final_Project.Data;
using Database_Final_Project.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database_Final_Project.Services;

internal class StatusService
{
    private readonly DataContext _context = new();

    //Om det inte finns några statusar inlagt, skapas statusar och läggs till, i en array
    // Med hjälp av foreach loopen så kontrolleras hur många statusar som finns i _statuses, 
    // och för varje status läggs dessa till i StatusEntity med den status som loopen befinner sig
    // på som namn. Kommer alltid få "Ej påbörjad 
    public async Task CreateStatusIfNotExistsAsync()
    {
        if(!await _context.Statuses.AnyAsync())
        {
            string[] _statuses = new string[] {"Ej påbörjad", "Pågående", "Avslutad"};

            foreach(var status in _statuses)
            {
                await _context.AddAsync(new StatusEntity { StatusName = status });
                await _context.SaveChangesAsync();
            }
          
        }
    }

    //För att hämta ut alla statusärenden
    public async Task<IEnumerable<StatusEntity>> GetAllAsync()
    {
        return await _context.Statuses.ToListAsync();
    }

    public async Task<StatusEntity> GetAsync(Expression<Func<StatusEntity, bool>> predicate)
    {
        var _statusEntity = await _context.Statuses.FirstOrDefaultAsync(predicate);
        return _statusEntity!;
    }

    
}
