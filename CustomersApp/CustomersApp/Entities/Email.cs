namespace CustomersApp.Entities;

public class Email(string content)
{
    public int Id { get; private set; }
    public int PersonId { get; private set; }
    public string Content { get; private set; } = content;
}
