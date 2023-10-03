using ERPSEI.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace ERPSEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<UserFile> UserFiles { get; set; }  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFile>();

            modelBuilder.Entity<FileType>()
                .HasData(
                    new FileType((int)FileTypes.ActaNacimiento, "Acta de nacimiento"),
                    new FileType((int)FileTypes.CURP, "CURP"),
                    new FileType((int)FileTypes.CLABE, "CLABE"),
                    new FileType((int)FileTypes.ComprobanteDomicilio, "Comprobante de domicilio"),
                    new FileType((int)FileTypes.ContactosEmergencia, "Contactos de emergencia"),
                    new FileType((int)FileTypes.CSF, "CSF"),
                    new FileType((int)FileTypes.INE, "INE"),
                    new FileType((int)FileTypes.RFC, "RFC"),
                    new FileType((int)FileTypes.ComprobanteEstudios, "Comprobante de estudios"),
                    new FileType((int)FileTypes.NSS, "NSS")
                );
        }

    }
}