using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Models;
using System;

namespace MyBookPlannerAPI.Data
{
    //  DataContext é o banco em memoria.
    //  Cada DbSet é uma das tabelas do banco.
    public class CatalogDataContext : DbContext
    {
        public CatalogDataContext(DbContextOptions<CatalogDataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        //public DbSet<UserBook> UserBooks { get; set; }
      
    }

}
