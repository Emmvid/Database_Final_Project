using System.ComponentModel.DataAnnotations;

namespace Database_Final_Project.Entities;

internal class ComplaintEntity
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;

    // Dessa tillsammans skapar kopplingen till Customer table.
    public int CustomerId { get; set; } 
    public CustomerEntity Customer { get; set; } = null!;

    //Skapar kopplingen till Status table. Måste ha en status och får automatiskt "ej påbörjad" när ärendet skapas eftersom den har "1" i arrayen
    public int StatusId { get; set; } = 1;

    public StatusEntity Status { get; set; } = null!;

    public ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();    
}
