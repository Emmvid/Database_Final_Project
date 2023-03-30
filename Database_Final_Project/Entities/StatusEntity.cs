namespace Database_Final_Project.Entities;

internal class StatusEntity
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;

    //varje status kan kopplas till olika complaints
    public ICollection<ComplaintEntity> Complaints { get; set; } = new List<ComplaintEntity>();
}
