using CustomersApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomersApp.Services;

public record GetPersonsQuery(DbSet<Person> DbSet)
{
    public async Task<PersonsDTO> Execute(RangeArg? range = null)
    {
        range ??= new(0, RangeArg.MaxLimit);

        var persons = await DbSet
            .Include(p => p.Emails)
            .Select(person => new
            {
                person.Id,
                person.FirstName,
                person.LastName,
                Email = person.Emails.FirstOrDefault().Content
            })
            .Skip(range.Offset)
            .Take(range.Limit)
        .ToArrayAsync();

        var count = await DbSet.CountAsync();

        return new PersonsDTO(
            persons.Select(p => new PersonDTO(p.Id, p.FirstName, p.LastName, p.Email)).ToArray(),
            new RangeDTO(count, range.Offset, range.Limit));
    }
}

public record PersonsDTO(
    PersonDTO[] Persons,
    RangeDTO Range);

public record PersonDTO(
    int Id,
    string FirstName,
    string LastName,
    string? Email);

public record RangeDTO(
    int TotalCount,
    int Offset = 0,
    int Limit = int.MaxValue);

public record RangeArg
{
    public const int MaxLimit = 25;

    public int Offset { get; }
    public int Limit { get; }

    public RangeArg(int offset, int limit)
    {
        Offset = Math.Max(0, offset);
        Limit = Math.Min(MaxLimit, limit);
    }
}
