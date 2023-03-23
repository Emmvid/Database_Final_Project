using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Final_Project.Entities;

internal class CustomerEntity
{
    [Key]
    //skapar unique identifier
    public int Id { get; set; } 
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    [Column(TypeName = "char(13)")] // Ställer så det blir av typen char med max 13 tecken. ? gör det valbart--> kan vara null
    public string? Phone { get; set; }

    // Skapar koppling mellan kund och klagomål, kunden kan ju ha flera klagomål. Hashset gör att det skapas en samling av unika element, som är oordnad.
    public ICollection<ComplaintEntity> Complaints { get; set; } = new HashSet<ComplaintEntity>(); 
}
