using Microsoft.EntityFrameworkCore;
using loginAPP.Model;



namespace loginAPP.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Expense> Expenses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>()
        .HasOne(sl => sl.User)
        .WithMany(up => up.Expenses)
        .HasForeignKey(sl => sl.UserId)
        .OnDelete(DeleteBehavior.Cascade);
        }

    }

    

}
