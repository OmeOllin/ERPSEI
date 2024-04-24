using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
		//Tablas de trabajo Empleados
        public DbSet<ArchivoEmpleado> ArchivosEmpleado { get; set; }
		public DbSet<Empleado> Empleados { get; set; }
		public DbSet<ContactoEmergencia> ContactosEmergencia { get; set; }

		//Catálogos Administrables Empleados
		public DbSet<Puesto> Puestos { get; set; }
		public DbSet<Area> Areas { get; set; }
		public DbSet<Oficina> Oficinas { get; set; }
		public DbSet<Subarea> Subareas { get; set; }

		//Catálogos no Administrables Empleados
		public DbSet<EstadoCivil> EstadosCiviles { get; set; }
		public DbSet<Genero> Generos { get; set; }


		//Tablas de trabajo Empresas
		public DbSet<BancoEmpresa> BancosEmpresa { get; set; }
		public DbSet<ArchivoEmpresa> ArchivosEmpresa { get; set; }
		public DbSet<Empresa> Empresas { get; set; }
		public DbSet<ActividadEconomicaEmpresa> ActividadesEconomicasEmpresa { get; set; }
		public DbSet<ProductoServicioPerfil> ProductosServiciosPerfil { get; set; }

		//Catálogos Administrables Empresas
		public DbSet<Origen> Origenes { get; set; }
		public DbSet<Nivel> Niveles { get; set; }
		public DbSet<Perfil> Perfiles { get; set; }
		public DbSet<ProductoServicio> ProductosServicios { get; set; }

		//Catálogos no Administrables Empresas
		public DbSet<ActividadEconomica> ActividadesEconomicas { get; set; }

		//Tablas de trabajo SAT
		public DbSet<Prefactura> Prefacturas { get; set; }
		public DbSet<Concepto> Conceptos { get; set; }

		//Catálogos no Administrables SAT
		public DbSet<Exportacion> Exportaciones { get; set; }
		public DbSet<FormaPago> FormasPago { get; set; }
		public DbSet<Impuesto> Impuestos { get; set; }
		public DbSet<Mes> Meses { get; set; }
		public DbSet<MetodoPago> MetodosPago { get; set; }
		public DbSet<Moneda> Monedas { get; set; }
		public DbSet<ObjetoImpuesto> ObjetosImpuesto { get; set; }
		public DbSet<Periodicidad> Periodicidades { get; set; }
		public DbSet<RegimenFiscal> RegimenesFiscales { get; set; }
		public DbSet<TasaOCuota> TasasOCuotas { get; set; }
		public DbSet<TipoComprobante> TiposComprobante { get; set; }
		public DbSet<TipoFactor> TiposFactor { get; set; }
		public DbSet<TipoRelacion> TiposRelacion { get; set; }
		public DbSet<UnidadMedida> UnidadesMedida { get; set; }
		public DbSet<UsoCFDI> UsosCFDI { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			//Empresas
			buildEmpresas(modelBuilder);

			//Empleados
			buildEmpleados(modelBuilder);

			//SAT
			buildSAT(modelBuilder);
		}

		private void buildEmpresas(ModelBuilder b) 
		{
			b.Entity<Empresa>().HasOne(e => e.Perfil).WithMany(p => p.Empresas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empresa>().HasOne(e => e.Origen).WithMany(o => o.Empresas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Empresa>().HasOne(e => e.Nivel).WithMany(o => o.Empresas).OnDelete(DeleteBehavior.NoAction);;
			b.Entity<Empresa>().HasMany(e => e.BancosEmpresa).WithOne(b => b.Empresa).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Empresa>().HasMany(e => e.ArchivosEmpresa).WithOne(a => a.Empresa).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empresa>().HasMany(e => e.ActividadesEconomicasEmpresa).WithOne(a => a.Empresa).OnDelete(DeleteBehavior.NoAction);

			b.Entity<ActividadEconomica>().HasMany(a => a.ActividadesEconomicasEmpresa).WithOne(a => a.ActividadEconomica).OnDelete(DeleteBehavior.NoAction);

			b.Entity<ArchivoEmpresa>().HasOne(a => a.TipoArchivo).WithMany(ta => ta.ArchivosEmpresa).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Perfil>().HasMany(p => p.ProductosServiciosPerfil).WithOne(p => p.Perfil).OnDelete(DeleteBehavior.NoAction);

			b.Entity<ProductoServicio>().HasMany(p => p.ProductosServiciosPerfil).WithOne(p => p.ProductoServicio).OnDelete(DeleteBehavior.NoAction);

			b.Entity<TipoArchivoEmpresa>()
				.HasData(
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.CSF, "CSF"),
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.INE, "INE"),
                    new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.RFC, "RFC"),
                    new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.ComprobanteDomicilio, "ComprobanteDomicilio"),
                    new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.Otro, "Otro")
                );
		}

		private void buildEmpleados(ModelBuilder b)
		{
			b.Entity<ArchivoEmpleado>().HasOne(ae => ae.TipoArchivo).WithMany(ta => ta.ArchivosEmpleado).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Empleado>().HasOne(e => e.EstadoCivil).WithMany(ec => ec.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasOne(e => e.Genero).WithMany(g => g.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasOne(e => e.Puesto).WithMany(p => p.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasOne(e => e.Area).WithMany(a => a.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasOne(e => e.Subarea).WithMany(sa => sa.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasOne(e => e.Oficina).WithMany(o => o.Empleados).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasMany(e => e.ContactosEmergencia).WithOne(ce => ce.Empleado).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empleado>().HasMany(e => e.ArchivosEmpleado).WithOne(ae => ae.Empleado).OnDelete(DeleteBehavior.NoAction);


			b.Entity<TipoArchivo>()
				.HasData(
					new TipoArchivo((int)Entities.Empleados.FileTypes.ImagenPerfil, "Imagen de perfil"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.ActaNacimiento, "Acta de nacimiento"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.CURP, "CURP"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.CLABE, "CLABE"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.ComprobanteDomicilio, "Comprobante de domicilio"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.CSF, "CSF"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.INE, "INE"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.RFC, "RFC"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.ComprobanteEstudios, "Comprobante de estudios"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.NSS, "NSS"),
					new TipoArchivo((int)Entities.Empleados.FileTypes.Otro, "Otro")
				);

			b.Entity<EstadoCivil>()
				.HasData(
					new EstadoCivil() { Id = 1, Nombre = "Soltero" },
					new EstadoCivil() { Id = 2, Nombre = "Casado" }
				);

			b.Entity<Genero>()
				.HasData(
					new Genero() { Id = 1, Nombre = "Masculino" },
					new Genero() { Id = 2, Nombre = "Femenino" },
					new Genero() { Id = 3, Nombre = "Otro" }
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
				"Socio Director",
				"Encargado",
				"Gerente",
				"Mantenimiento y Limpieza",
				"Recepcionista",
				"Recepcionista Coordinadora",
				"Seguridad Privada",
				"Socio",
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
			b.Entity<Puesto>().HasData(dataPuestos);

			List<KeyValuePair<string, List<string>>> areas = new List<KeyValuePair<string, List<string>>>()
			{
				new KeyValuePair<string, List<string>>("Administración", new List<string>(){"Sistemas"}),
				new KeyValuePair<string, List<string>>("Auditoría", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Bancos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Contabilidad", new List<string>(){"Interna", "Externa", "Impuestos"}),
				new KeyValuePair<string, List<string>>("Dirección General", new List<string>(){"Control Vehicular"}),
				new KeyValuePair<string, List<string>>("Expedientes", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Family Office", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Fiscal", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Impuestos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Legal", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Nóminas", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Operaciones", new List<string>(){"IMSS", "Internas", "Facturación", "Nóminas"}),
				new KeyValuePair<string, List<string>>("Recursos Humanos", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Tesorería", new List<string>(){ }),
				new KeyValuePair<string, List<string>>("Socio", new List<string>() { })
			};

			Area[] dataAreas = new Area[areas.Count];
			List<Subarea> dataSubareas = new List<Subarea>();
			int j = 0;
			int k = 0;
			foreach (KeyValuePair<string, List<string>> area in areas)
			{
				dataAreas[j] = new Area() { Id = j + 1, Nombre = area.Key };
				//Se agregan las subareas
				foreach (string subarea in area.Value)
				{
					dataSubareas.Add(new Subarea() { Id = k + 1, Nombre = subarea, AreaId = dataAreas[j].Id });
					k++;
				}
				j++;
			}
			b.Entity<Area>().HasData(dataAreas);
			b.Entity<Area>().HasMany(a => a.Subareas).WithOne(sa => sa.Area).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Subarea>().HasData(dataSubareas.ToArray());

			List<string> oficinas = new List<string>()
			{
				"Austria 1",
				"Austria 6",
				"Big Ben",
				"Cancún",
				"Capri",
				"Centro Urbano",
				"Cóndor",
				"Izaguirre",
				"Lago de Guadalupe",
				"León",
				"Lomas Verdes",
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
			b.Entity<Oficina>().HasData(dataOficinas);
		}

		private void buildSAT(ModelBuilder b)
		{
			b.Entity<TasaOCuota>().HasOne(t => t.Factor).WithMany(f => f.TasasOCuotas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<TasaOCuota>().HasOne(t => t.Impuesto).WithMany(i => i.TasasOCuotas).OnDelete(DeleteBehavior.NoAction);

			b.Entity<TipoComprobante>().Property(t => t.ValorMaximo).HasPrecision(24, 6);

			b.Entity<Prefactura>().HasMany(p => p.Conceptos).WithOne(c => c.Prefactura).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Emisor).WithMany(e => e.PrefacturasEmitidas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Receptor).WithMany(e => e.PrefacturasRecibidas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.TipoComprobante).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Moneda).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.FormaPago).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.MetodoPago).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.UsoCFDI).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Exportacion).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.UsuarioCreador).WithMany(e => e.PrefacturasCreadas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Prefactura>().HasOne(p => p.UsuarioAutorizador).WithMany(e => e.PrefacturasAutorizadas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Prefactura>().HasOne(p => p.UsuarioFinalizador).WithMany(e => e.PrefacturasFinalizadas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Estatus).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Prefactura>().Property(c => c.TipoCambio).HasPrecision(18, 6);

			b.Entity<Concepto>().HasOne(c => c.UnidadMedida).WithMany(e => e.Conceptos).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Concepto>().HasOne(c => c.ObjetoImpuesto).WithMany(e => e.Conceptos).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Concepto>().Property(c => c.PrecioUnitario).HasPrecision(18, 6);
			b.Entity<Concepto>().Property(c => c.Descuento).HasPrecision(18, 6);
			b.Entity<Concepto>().Property(c => c.TasaTraslado).HasPrecision(18, 6);
			b.Entity<Concepto>().Property(c => c.TasaRetencion).HasPrecision(18, 6);

            b.Entity<EstatusPrefactura>()
                .HasData(
                    new EstatusPrefactura() { Id = 1, Descripcion = "Solicitada" },
                    new EstatusPrefactura() { Id = 2, Descripcion = "Autorizada" },
                    new EstatusPrefactura() { Id = 3, Descripcion = "Finalizada" }
                );
        }
	}
}