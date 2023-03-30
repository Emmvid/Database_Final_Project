namespace Database_Final_Project.Entities;

internal class CommentEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public string Comment { get; set; } = null!;

    //En complaint kan ha många kommentarer --> kopplad till ett ärende.
    public int ComplaintId { get; set; }
    public ComplaintEntity Complaint { get; set; } = null!;


}
