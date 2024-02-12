using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Pet> Pets { get; set; }
    }
}
