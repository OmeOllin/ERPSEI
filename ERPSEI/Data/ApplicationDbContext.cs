using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
		//Tablas de trabajo
        public DbSet<ArchivoEmpleado> ArchivosEmpleado { get; set; }
		public DbSet<Empleado> Empleados { get; set; }
		public DbSet<ContactoEmergencia> ContactosEmergencia { get; set; }

		//Catálogos Administrables
		public DbSet<Puesto> Puestos { get; set; }
		public DbSet<Area> Areas { get; set; }
		public DbSet<Oficina> Oficinas { get; set; }
		public DbSet<Subarea> Subareas { get; set; }

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

            modelBuilder.Entity<ArchivoEmpleado>().HasOne(ae => ae.TipoArchivo).WithMany(ta => ta.ArchivosEmpleado).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Empleado>().HasOne(e => e.User).WithOne(u => u.Empleado).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.EstadoCivil).WithMany(ec => ec.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Genero).WithMany(g => g.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Puesto).WithMany(p => p.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Area).WithMany(a => a.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Subarea).WithMany(sa => sa.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Oficina).WithMany(o => o.Empleados).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasMany(e => e.ContactosEmergencia).WithOne(ce => ce.Empleado).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasMany(e => e.ArchivosEmpleado).WithOne(ae => ae.Empleado).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Empleado>().HasOne(e => e.Jefe).WithMany(j => j.Empleados).OnDelete(DeleteBehavior.NoAction);


			modelBuilder.Entity<TipoArchivo>()
				.HasMany(ta => ta.ArchivosEmpleado)
				.WithOne(ae => ae.TipoArchivo)
				.OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<TipoArchivo>()
				.HasData(
					new TipoArchivo((int)FileTypes.ActaNacimiento, "Acta de nacimiento"),
					new TipoArchivo((int)FileTypes.CURP, "CURP"),
					new TipoArchivo((int)FileTypes.CLABE, "CLABE"),
					new TipoArchivo((int)FileTypes.ComprobanteDomicilio, "Comprobante de domicilio"),
					new TipoArchivo((int)FileTypes.ContactosEmergencia, "Contactos de emergencia"),
					new TipoArchivo((int)FileTypes.CSF, "CSF"),
					new TipoArchivo((int)FileTypes.INE, "INE"),
					new TipoArchivo((int)FileTypes.RFC, "RFC"),
					new TipoArchivo((int)FileTypes.ComprobanteEstudios, "Comprobante de estudios"),
					new TipoArchivo((int)FileTypes.NSS, "NSS"),
					new TipoArchivo((int)FileTypes.Otro, "Otro")
				);

			modelBuilder.Entity<EstadoCivil>().HasMany(ec => ec.Empleados).WithOne(e => e.EstadoCivil).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<EstadoCivil>()
				.HasData(
					new EstadoCivil() { Id = 1, Nombre = "Soltero" },
					new EstadoCivil() { Id = 2, Nombre = "Casado" }
				);

			modelBuilder.Entity<Genero>().HasMany(g => g.Empleados).WithOne(e => e.Genero).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Genero>()
				.HasData(
					new Genero() { Id = 1, Nombre = "Masculino" },
					new Genero() { Id = 2, Nombre = "Femenino" }
				);

			List<string> puestos = new List<string>()
			{
				"Analista",
				"Asistente",
				"Auditor",
				"Auxiliar",
				"Chofer",
				"Desarrollador",
				"Director",
				"Encargado",
				"Gerente",
				"Mantenimiento y Limpieza",
				"Recepcionista",
				"Seguridad Privada",
				"Socio Director",
				"Subencargado",
				"Subgerente",
				"Supervisor",
				"Técnico",
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

			List<KeyValuePair<string, List<string>>> areas = new List<KeyValuePair<string, List<string>>>()
			{
				new KeyValuePair<string, List<string>>("Administración", new List<string>(){"Sistemas"}),
				new KeyValuePair<string, List<string>>("Auditoría", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Bancos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Contabilidad", new List<string>(){"Interna", "Externa"}),
				new KeyValuePair<string, List<string>>("Dirección General", new List<string>(){"Control Vehicular"}),
				new KeyValuePair<string, List<string>>("Expedientes", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Family Office", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Fiscal", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Impuestos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Legal", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Nóminas", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Operaciones", new List<string>(){"IMSS", "Internas", "Facturación", "Nóminas"}),
				new KeyValuePair<string, List<string>>("Recursos Humanos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Tesorería", new List<string>(){ })
			};

			Area[] dataAreas = new Area[areas.Count];
			List<Subarea> dataSubareas = new List<Subarea>();
			int j = 0;
			int k = 0;
			foreach (KeyValuePair<string, List<string>> area in areas)
			{
				dataAreas[j] = new Area() { Id = j + 1, Nombre = area.Key };
				//Se agregan las subareas
				foreach(string subarea in area.Value)
				{
					dataSubareas.Add(new Subarea() { Id = k + 1, Nombre = subarea, AreaId = dataAreas[j].Id });
					k++;
				}
				j++;
			}
			modelBuilder.Entity<Area>().HasData(dataAreas);
			modelBuilder.Entity<Area>().HasMany(a => a.Empleados).WithOne(e => e.Area).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Area>().HasMany(a => a.Subareas).WithOne(sa => sa.Area).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Subarea>().HasData(dataSubareas.ToArray());
			modelBuilder.Entity<Subarea>().HasMany(sa => sa.Empleados).WithOne(e => e.Subarea).OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Subarea>().HasOne(sa => sa.Area).WithMany(a => a.Subareas).OnDelete(DeleteBehavior.SetNull);

			List<string> oficinas = new List<string>()
			{
				"Austria 1",
				"Austria 6",
				"Big Ben",
				"Cancún",
				"Capri",
				"Cóndor",
				"Izaguirre",
				"Lago de Guadalupe",
				"Los Reyes La Paz",
				"Pafnuncio",
				"Pirules",
				"Polanco",
				"Santa Mónica",
				"Torre Esmeralda"
			};
			Oficina[] dataOficinas = new Oficina[oficinas.Count];
			int l = 0;
            foreach (string oficina in oficinas)
            {
				dataOficinas[l] = new Oficina() { Id = l + 1, Nombre = oficina };
				l++;
            }
			modelBuilder.Entity<Oficina>().HasData(dataOficinas);
			modelBuilder.Entity<Oficina>().HasMany(o => o.Empleados).WithOne(e => e.Oficina).OnDelete(DeleteBehavior.SetNull);
		}

    }
}