using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
		//Tablas de trabajo
        public DbSet<UserFile> UserFiles { get; set; }
		public DbSet<Empleado> Empleados { get; set; }
		public DbSet<ContactoEmergencia> ContactosEmergencia { get; set; }

		//Catálogos Administrables
		public DbSet<Puesto> Puestos { get; set; }
		public DbSet<Area> Areas { get; set; }

		//Catálogos no Administrables
		public DbSet<EstadoCivil> EstadosCiviles { get; set; }
		public DbSet<Genero> Generos { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFile>().HasOne(uf => uf.FileType).WithMany(f => f.UserFiles).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Empleado>().HasOne(e => e.User).WithOne(u => u.Empleado).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.EstadoCivil).WithMany(ec => ec.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Genero).WithMany(g => g.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Puesto).WithMany(p => p.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Area).WithMany(a => a.Empleados).OnDelete(DeleteBehavior.SetNull);


			modelBuilder.Entity<FileType>()
				.HasMany(ft => ft.UserFiles)
				.WithOne(uf => uf.FileType)
				.OnDelete(DeleteBehavior.SetNull);

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

			modelBuilder.Entity<EstadoCivil>().HasMany(ec => ec.Empleados).WithOne(e => e.EstadoCivil).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<EstadoCivil>()
				.HasData(
					new EstadoCivil() { Id = 1, Nombre = "Soltero(a)" },
					new EstadoCivil() { Id = 2, Nombre = "Casado(a)" }
				);

			modelBuilder.Entity<Genero>().HasMany(g => g.Empleados).WithOne(e => e.Genero).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Genero>()
				.HasData(
					new Genero() { Id = 1, Nombre = "Masculino" },
					new Genero() { Id = 2, Nombre = "Femenino" }
				);

			List<string> puestos = new List<string>()
			{
				"Analista de IMSS",
				"Analista de Nómina",
				"Asistente AC",
				"Asistente de Dirección",
				"Asistente de Dirección / Julio",
				"Asistente de Dirección / Paco",
				"Asistente de Dirección / Alex",
				"Auditoría",
				"Auxiliar de Tesorería",
				"Auxiliar Administrativo",
				"Auxiliar Contable",
				"Auxiliar Contable Austria 6",
				"Auxiliar Contable Julio",
				"Auxiliar Contable Nómina Julio",
				"Auxiliar de Área Legal",
				"Auxiliar de Auditoría",
				"Auxiliar de Bancos",
				"Auxiliar de Entregables",
				"Auxiliar de Impuestos",
				"Auxiliar de Nómina",
				"Auxiliar de Nómina Cancún",
				"Auxiliar de Recursos Humanos",
				"Auxiliar de Sistemas",
				"Auxiliar de Tesorería",
				"Auxiliar IMSS",
				"Chofer",
				"Director Administrativo",
				"Director General",
				"Encargado de Impuestos",
				"Encargado de Contabilidad",
				"Encargado de IMSS",
				"Encargado de Nómina",
				"Facturación",
				"Gerente de Área Legal",
				"Gerente de Auditoría",
				"Gerente de Bancos",
				"Gerente de Contabilidad",
				"Gerente de Entregables",
				"Gerente de Impuestos",
				"Gerente de Nómina",
				"Gerente de Operaciones",
				"Gerente de Operaciones Internas",
				"Gerente de Recursos Humanos",
				"Inplant Nóminas/Asimilados",
				"Mantenimiento y Limpieza",
				"Nóminas Brame/Asimilados",
				"Pasante del Área Legal Clarisa",
				"Recepción Entregables Pafnuncio piso 5",
				"Recepción Lago de Guadalupe",
				"Recepción Los Reyes la Paz",
				"Recepcionista Austria 6",
				"Recepcionista Big 407",
				"Recepcionista Big 510",
				"Recepcionista Izaguirre",
				"Recepcionista Pirules",
				"Recepcionista Polanco",
				"Recepcionista Sta Mónica",
				"Recepcionista Torres Esmeralda",
				"Recepcionista y Apoyo Facturación Big 303",
				"Recepcionista Condor",
				"Senior de Soporte - Desarrollador de Software Izcalli 1",
				"Sistemas",
				"Subencargado de Nómina",
				"Supervisor Contable",
				"Supervisor de Contabilidad",
				"Supervisor Entregables",
				"Supervisor / Encargado de Nómina",
				"Tesorero"
			};
			Puesto[] dataPuestos = new Puesto[puestos.Count];
			int i = 0;
			foreach (string puesto in puestos)
			{
				dataPuestos[i] = new Puesto() { Id = i + 1, Nombre = puesto };
				i++;
			}
			modelBuilder.Entity<Puesto>().HasData(dataPuestos);
			modelBuilder.Entity<Puesto>().HasMany(p => p.Empleados).WithOne(e => e.Puesto).OnDelete(DeleteBehavior.SetNull);

			List<string> areas = new List<string>()
			{
				"Asistente Dirección",
				"Auditoría",
				"Bancos",
				"Contabilidad",
				"Director Administrativo",
				"Entregables",
				"Facturación",
				"Fiscal",
				"Impuestos",
				"IMSS",
				"Legal",
				"Nómina",
				"Nómina Externo",
				"Operaciones",
				"Operaciones Internas",
				"Operaciones Nómina",
				"Recursos Humanos",
				"Sistemas",
				"Soporte Dirección",
				"Tesorería"
			};
			Area[] dataAreas = new Area[areas.Count];
			int j = 0;
			foreach (string area in areas)
			{
				dataAreas[j] = new Area() { Id = j + 1, Nombre = area };
				j++;
			}
			modelBuilder.Entity<Area>().HasData(dataAreas);
			modelBuilder.Entity<Area>().HasMany(a => a.Empleados).WithOne(e => e.Area).OnDelete(DeleteBehavior.SetNull);
		}

    }
}