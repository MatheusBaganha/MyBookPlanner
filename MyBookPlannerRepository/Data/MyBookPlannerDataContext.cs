using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using System;

namespace MyBookPlanner.Repository.Data
{
    public class MyBookPlannerDataContext : DbContext
    {
        public MyBookPlannerDataContext(DbContextOptions<MyBookPlannerDataContext> options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
      
    }

}
