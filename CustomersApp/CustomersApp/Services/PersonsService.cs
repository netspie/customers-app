using CustomersApp.Data;
using CustomersApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomersApp.Services;

public class PersonsService : IPersonsService
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Person> _dbSet;

    private GetPersonsQuery _getPersonsQuery;

    public PersonsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Persons;

        _getPersonsQuery = new(_dbSet);
    }

    public async Task Create(Person person)
    {
        await _dbSet.AddAsync(person);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(int personId, string firstName, string lastName)
    {
        var person = await _dbSet.FindAsync(personId);
        if (person is null)
            return;

        person.FirstName = firstName;
        person.LastName = lastName;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int personId)
    {
        var person = _dbSet.Find(personId);
        if (person is null)
            return;

        if (_dbContext.Entry(person)?.State == EntityState.Detached)
            _dbSet.Attach(person);
        
        _dbSet.Remove(person);

        await _dbContext.SaveChangesAsync();
    }

    public Task<PersonsDTO> GetRange(RangeArg? range = null) =>
        _getPersonsQuery.Execute(range);
}

public interface IPersonsService
{
    Task Create(Person person);
    Task Update(int personId, string firstName, string lastName);
    Task Delete(int personId);

    Task<PersonsDTO> GetRange(RangeArg range);
}
