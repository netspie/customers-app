using Bogus;
using CustomersApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomersApp.Data;

using Person = Entities.Person;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Email> Emails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var personsCount = 10001;

        modelBuilder.Entity<Person>().HasData(
            new Faker<Person>()
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                .RuleFor(x => x.LastName, x => x.Person.LastName)
                .RuleFor(x => x.Description, x => x.Lorem.Paragraph())
                .Generate(personsCount)
                .Skip(1));

        var skipValue = 10;
        modelBuilder.Entity<Email>().HasData(
            new Faker<Email>()
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Content, x => x.Person.Email)
                .RuleFor(x => x.PersonId, x => x.IndexFaker + skipValue)
                .Generate(personsCount - skipValue)
                .Skip(1)
                .Concat(new Faker<Email>()
                    .RuleFor(x => x.Id, x => x.IndexFaker + personsCount)
                    .RuleFor(x => x.Content, x => x.Person.Email)
                    .RuleFor(x => x.PersonId, x => x.IndexFaker * 2)
                    .Generate(personsCount / 4 - skipValue)
                    .Skip(1)));
    }
}
