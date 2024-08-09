using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiPOC.DataBaseModel;

public class transactionDBContext : DbContext
{
    public transactionDBContext(DbContextOptions<transactionDBContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRole {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasMany(c => c.States).WithOne(s => s.Country);
        modelBuilder.Entity<State>().HasMany(s => s.Cities).WithOne(c => c.State);
    }
}

