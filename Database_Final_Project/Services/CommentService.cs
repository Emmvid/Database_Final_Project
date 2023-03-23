using Database_Final_Project.Data;
using Database_Final_Project.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Database_Final_Project.Services;

internal class CommentService
{
    private readonly DataContext _context = new();

    public async Task CreateAsync(CommentEntity commentEntity)
    {
        if (await _context.Complaints.AnyAsync(x => x.Id == commentEntity.ComplaintId))
        {
            _context.Add(commentEntity);
            await _context.SaveChangesAsync();
        }
    }

  

}

