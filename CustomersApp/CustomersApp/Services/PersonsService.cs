using CustomersApp.Data;
using CustomersApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomersApp.Services;

public class PersonsService : IPersonsService
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Person> _dbSet;

    public PersonsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Persons;
    }

    public async Task Create(Person person)
    {
        await _dbSet.AddAsync(person);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Person person)
    {
        var entry = _dbSet.Entry(person);

        _dbSet.Attach(person);
        entry.State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(string personId)
    {
        var person = _dbSet.Find(personId);
        if (person is null)
            return;

        if (_dbContext.Entry(person)?.State == EntityState.Detached)
            _dbSet.Attach(person);
        
        _dbSet.Remove(person);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<PersonsDTO> GetRange(RangeArg? range = null)
    {
        range ??= new(0, RangeArg.MaxLimit);

        var persons = await _dbSet
            .Skip(range.Offset)
            .Take(range.Limit)
            .ToArrayAsync();

        var count = await _dbSet.CountAsync();

        return new PersonsDTO(
            persons,
            new RangeDTO(count, range.Offset, range.Limit));
    }
}

public interface IPersonsService
{
    Task Create(Person person);
    Task Update(Person person);
    Task Delete(string personId);

    Task<PersonsDTO> GetRange(RangeArg range);
}

public record PersonsDTO(
    Person[] Persons,
    RangeDTO Range);

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
