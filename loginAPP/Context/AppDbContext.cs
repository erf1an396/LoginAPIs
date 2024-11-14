﻿using Microsoft.EntityFrameworkCore;
using loginAPP.Model;



namespace loginAPP.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Expense> Expenses { get; set; }



    }

}
