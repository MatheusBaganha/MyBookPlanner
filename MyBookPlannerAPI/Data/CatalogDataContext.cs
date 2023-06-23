using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Models;
using System;

namespace MyBookPlannerAPI.Data
{
    //  DataContext is a in memory database.
    //  Each DbSet is one of the tables in the database.
    public class CatalogDataContext : DbContext
    {
        public CatalogDataContext(DbContextOptions<CatalogDataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
      
    }

}
