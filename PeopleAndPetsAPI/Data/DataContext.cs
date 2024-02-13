using Microsoft.EntityFrameworkCore;
using PeopleAndPetsAPI.Entities;

namespace PeopleAndPetsAPI.Data
{
    /// <summary>
    /// The context has access to the database tables within the database, the table names are specified in the class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Provides access to the people database table.
        /// </summary>
        public DbSet<Person> People { get; set; }

        /// <summary>
        /// Provides access to the pets database table.
        /// </summary>
        public DbSet<Pet> Pets { get; set; }
    }
}
