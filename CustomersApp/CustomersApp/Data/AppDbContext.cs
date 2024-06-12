using CustomersApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomersApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Email> Emails { get; set; }

    private DbContextOptions<AppDbContext> _options = options;
}
