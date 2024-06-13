namespace CustomersApp.Entities;

public class Person(string firstName, string lastName, IEnumerable<Email>? emails = null)
{
    public int Id { get; private set; }
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string? Description { get; set; }

    public ICollection<Email>? Emails { get; private set; } = emails?.ToList();

    public Person() : this("", "") {}
}
