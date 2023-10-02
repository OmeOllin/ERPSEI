using ERPSEI.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace ERPSEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<UserFile> UserFile { get; set; }  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFile>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserFiles)
                .HasForeignKey( uf => uf.UserId );
        }

    }
}