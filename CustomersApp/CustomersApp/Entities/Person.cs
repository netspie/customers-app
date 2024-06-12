namespace CustomersApp.Entities;

public class Person(string firstName, string lastName, IEnumerable<Email>? emails = null)
{
    public int Id { get; private set; }
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string? Description { get; private set; }

    public List<Email>? Emails { get; private set; } = emails?.ToList();
}
