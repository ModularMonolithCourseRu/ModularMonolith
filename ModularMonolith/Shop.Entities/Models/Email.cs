namespace Shop.Entities.Models;

public class Email : Aggregate
{
    public string Address { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public int Attempts { get; set; }
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}