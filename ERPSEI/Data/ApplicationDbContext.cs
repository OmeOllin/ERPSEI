﻿using ERPSEI.Data.Entities.Conciliaciones;
using ERPSEI.Data.Entities.Clientes;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.Reportes;
using ERPSEI.Data.Entities.SAT;
using ERPSEI.Data.Entities.SAT.Catalogos;
using ERPSEI.Data.Entities.SAT.cfdiv40;
using ERPSEI.Data.Entities.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
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
		public DbSet<AutorizacionesPrefactura> AutorizacionesPrefacturas { get; set; }
		public DbSet<Prefactura> Prefacturas { get; set; }
		public DbSet<Concepto> Conceptos { get; set; }
		//public DbSet<ComprobanteAddenda> ComprobantesAddendas { get; set; }
		public DbSet<ComprobanteCfdiRelacionados> ComprobantesCfdisRelacionados { get; set; }
		//public DbSet<ComprobanteComplemento> ComprobantesComplementos { get; set; }
		public DbSet<ComprobanteConcepto> ComprobantesConceptos { get; set; }
		public DbSet<ComprobanteConceptoACuentaTerceros> ComprobantesConceptosACuentaTerceros { get; set; }
		//public DbSet<ComprobanteConceptoComplementoConcepto> ComprobantesConceptosComplementosConceptos { get; set; }
		public DbSet<ComprobanteConceptoCuentaPredial> ComprobantesConceptosCuentasPrediales { get; set; }
		public DbSet<ComprobanteConceptoImpuestos> ComprobantesConceptosImpuestos { get; set; }
		public DbSet<ComprobanteConceptoImpuestosRetencion> ComprobantesConceptosImpuestosRetenciones { get; set; }
		public DbSet<ComprobanteConceptoImpuestosTraslado> ComprobantesConceptosImpuestosTraslados { get; set; }
		public DbSet<ComprobanteConceptoInformacionAduanera> ComprobantesConceptosInformacionesAduaneras { get; set; }
		public DbSet<ComprobanteConceptoParte> ComprobantesConceptosPartes { get; set; }
		public DbSet<ComprobanteConceptoParteInformacionAduanera> ComprobantesConceptosPartesInformacionesAduaneras { get; set; }
		public DbSet<ComprobanteEmisor> ComprobantesEmisores { get; set; }
		public DbSet<ComprobanteImpuestos> ComprobantesImpuestos { get; set; }
		public DbSet<ComprobanteImpuestosRetencion> ComprobantesImpuestosRetenciones { get; set; }
		public DbSet<ComprobanteImpuestosTraslado> ComprobantesImpuestosTraslados { get; set; }
		public DbSet<ComprobanteInformacionGlobal> ComprobantesInformacionesGlobales { get; set; }
		public DbSet<ComprobanteReceptor> ComprobantesReceptores { get; set; }


		public DbSet<Comprobante> Comprobantes { get; set; }

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

		//Reporte asistencias
		public DbSet<Asistencia> Asistencias { get; set; }
		public DbSet<Horario> Horarios { get; set; }
		public DbSet<HorarioDetalle> HorariosDetalles { get; set; }

		//Conciliaciones
		public DbSet<Conciliacion> Conciliaciones { get; set; }
		public DbSet<ConciliacionDetalle> ConciliacionesDetalles { get; set; }
		public DbSet<Banco> Bancos { get; set; }
		public DbSet<MovimientoBancario> MovimientosBancarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ConciliacionDetalleComprobante> ConciliacionesDetallesComprobantes { get; set; }
        public DbSet<ConciliacionDetalleMovimiento> ConciliacionesDetallesMovimientos { get; set; }


        //Catálogos no administrables Usuarios
        public DbSet<AccesoModulo> AccesosModulos { get; set; }
		public DbSet<Modulo> Modulos { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			//Empresas
			BuildEmpresas(modelBuilder);

			//Empleados
			BuildEmpleados(modelBuilder);

			//SAT
			BuildSAT(modelBuilder);

			//Accesos
			BuildAccesos(modelBuilder);

			//Asistencias
			BuildAsistencias(modelBuilder);

			//Conciliaciones
            BuildConciliaciones(modelBuilder);
        }

		private static void BuildAsistencias(ModelBuilder b) 
		{
			b.Entity<Asistencia>().HasOne(e => e.Empleado).WithMany(a => a.Asistencias).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Empleado>().HasOne(e => e.Horario).WithMany(h => h.Empleados).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Horario>().HasMany(h => h.HorarioDetalles).WithOne(hd => hd.Horario).OnDelete(DeleteBehavior.NoAction);
		}

		private static void BuildConciliaciones(ModelBuilder b) 
		{
            b.Entity<Conciliacion>().HasOne(e => e.Banco).WithMany(a => a.Conciliaciones).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Conciliacion>().HasOne(e => e.AppUserC).WithMany(a => a.ConciliacionesCreadas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Conciliacion>().HasOne(e => e.AppUserM).WithMany(a => a.ConciliacionesModificadas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Conciliacion>().HasOne(e => e.Empresa).WithMany(a => a.Conciliaciones).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Conciliacion>().HasMany(e => e.DetallesConciliacion).WithOne(a => a.Conciliacion).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Conciliacion>().HasOne(e => e.Cliente).WithMany(a => a.Conciliaciones).OnDelete(DeleteBehavior.NoAction);
            b.Entity<ConciliacionDetalle>().HasMany(e => e.ConciliacionesDetallesMovimientos).WithOne(a => a.ConciliacionDetalle).OnDelete(DeleteBehavior.NoAction);
            b.Entity<ConciliacionDetalle>().HasMany(e => e.ConciliacionesDetallesComprobantes).WithOne(a => a.ConciliacionDetalle).OnDelete(DeleteBehavior.NoAction);
            b.Entity<ConciliacionDetalleComprobante>().HasOne(e => e.Comprobante).WithOne(a => a.ConciliacionDetalleComprobante).OnDelete(DeleteBehavior.NoAction);
            b.Entity<ConciliacionDetalleMovimiento>().HasOne(e => e.MovimientoBancario).WithOne(a => a.ConciliacionDetalleMovimiento).OnDelete(DeleteBehavior.NoAction);

            b.Entity<Conciliacion>().Property(t => t.Total).HasPrecision(24, 6);
            b.Entity<MovimientoBancario>().Property(t => t.Importe).HasPrecision(24, 6);

            b.Entity<Banco>()
                .HasData(
                    new Banco() {Id = 1, Nombre = "Alquimia" },
                    new Banco() { Id = 2, Nombre = "Afirme" },
                    new Banco() { Id = 3, Nombre = "Azteca" },
                    new Banco() { Id = 4, Nombre = "American Express" },
                    new Banco() { Id = 5, Nombre = "Bancomer" },
                    new Banco() { Id = 6, Nombre = "Bancoppel" },
                    new Banco() { Id = 7, Nombre = "Banorte" },
                    new Banco() { Id = 8, Nombre = "Banregio" },
                    new Banco() { Id = 9, Nombre = "Bajio" },
                    new Banco() { Id = 10, Nombre = "Base" },
                    new Banco() { Id = 11, Nombre = "Bx" },
                    new Banco() { Id = 12, Nombre = "Cibanco" },
                    new Banco() { Id = 13, Nombre = "Citibanamex" },
                    new Banco() { Id = 14, Nombre = "Fortuna" },
                    new Banco() { Id = 15, Nombre = "HSBC" },
                    new Banco() { Id = 16, Nombre = "Inbursa" },
                    new Banco() { Id = 17, Nombre = "Intercam" },
                    new Banco() { Id = 18, Nombre = "Invex" },
                    new Banco() { Id = 19, Nombre = "Jeeves" },
                    new Banco() { Id = 20, Nombre = "Konfio" },
                    new Banco() { Id = 21, Nombre = "Mercado Pago" },
                    new Banco() { Id = 22, Nombre = "Mifel" },
                    new Banco() { Id = 23, Nombre = "Monex" },
                    new Banco() { Id = 24, Nombre = "Multiva" },
                    new Banco() { Id = 25, Nombre = "Santander" },
                    new Banco() { Id = 26, Nombre = "Scotiabank" }
                );
        }

        private static void BuildEmpresas(ModelBuilder b) 
		{
			b.Entity<Empresa>().HasOne(e => e.Perfil).WithMany(p => p.Empresas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empresa>().HasOne(e => e.Origen).WithMany(o => o.Empresas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Empresa>().HasOne(e => e.Nivel).WithMany(o => o.Empresas).OnDelete(DeleteBehavior.NoAction);;
			b.Entity<Empresa>().HasMany(e => e.BancosEmpresa).WithOne(b => b.Empresa).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Empresa>().HasMany(e => e.ArchivosEmpresa).WithOne(a => a.Empresa).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empresa>().HasMany(e => e.ActividadesEconomicasEmpresa).WithOne(a => a.Empresa).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Empresa>().HasOne(e => e.RegimenFiscal).WithMany(f => f.Empresas).OnDelete(DeleteBehavior.NoAction);

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
                    new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.Otro, "Otro"),
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.CER, "CER"),
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.KEY, "KEY"),
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.Logo, "Logo"),
					new TipoArchivoEmpresa((int)Entities.Empresas.FileTypes.HojaMembretada, "HojaMembretada")
				);
		}

		private static void BuildEmpleados(ModelBuilder b)
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

		private static void BuildSAT(ModelBuilder b)
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
            b.Entity<Prefactura>().HasOne(p => p.UsuarioTimbrador).WithMany(e => e.PrefacturasTimbradas).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Prefactura>().HasOne(p => p.Estatus).WithMany(e => e.Prefacturas).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Prefactura>().Property(c => c.TipoCambio).HasPrecision(18, 6);

			b.Entity<Concepto>().HasOne(c => c.UnidadMedida).WithMany(e => e.Conceptos).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Concepto>().HasOne(c => c.ObjetoImpuesto).WithMany(e => e.Conceptos).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Concepto>().Property(c => c.PrecioUnitario).HasPrecision(18, 6);
			b.Entity<Concepto>().Property(c => c.Descuento).HasPrecision(18, 6);
            b.Entity<Concepto>().Property(c => c.TasaTraslado).HasPrecision(18, 6);
            b.Entity<Concepto>().Property(c => c.TasaRetencion).HasPrecision(18, 6);
            b.Entity<Concepto>().Property(c => c.Traslado).HasPrecision(18, 6);
			b.Entity<Concepto>().Property(c => c.Retencion).HasPrecision(18, 6);

			b.Entity<AutorizacionesPrefactura>().HasOne(ap => ap.Prefactura).WithMany(p => p.Autorizaciones).OnDelete(DeleteBehavior.NoAction);
			b.Entity<AutorizacionesPrefactura>().HasOne(ap => ap.Usuario).WithMany(p => p.AutorizacionesPrefacturas).OnDelete(DeleteBehavior.NoAction);

            b.Entity<EstatusPrefactura>()
                .HasData(
                    new EstatusPrefactura() { Id = 1, Descripcion = "Solicitada" },
                    new EstatusPrefactura() { Id = 2, Descripcion = "Autorizada" },
                    new EstatusPrefactura() { Id = 3, Descripcion = "Timbrada" }
                );

			b.Entity<Comprobante>().Property(c => c.Descuento).HasPrecision(18, 6);
			b.Entity<Comprobante>().Property(c => c.SubTotal).HasPrecision(18, 6);
			b.Entity<Comprobante>().Property(c => c.TipoCambio).HasPrecision(18, 6);
			b.Entity<Comprobante>().Property(c => c.Total).HasPrecision(18, 6);

			b.Entity<ComprobanteConcepto>().Property(c => c.Cantidad).HasPrecision(18, 6);
			b.Entity<ComprobanteConcepto>().Property(c => c.Descuento).HasPrecision(18, 6);
			b.Entity<ComprobanteConcepto>().Property(c => c.Importe).HasPrecision(18, 6);
			b.Entity<ComprobanteConcepto>().Property(c => c.ValorUnitario).HasPrecision(18, 6);

			b.Entity<ComprobanteConceptoImpuestosRetencion>().Property(c => c.Base).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoImpuestosRetencion>().Property(c => c.Importe).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoImpuestosRetencion>().Property(c => c.TasaOCuota).HasPrecision(18, 6);

			b.Entity<ComprobanteConceptoImpuestosTraslado>().Property(c => c.Base).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoImpuestosTraslado>().Property(c => c.Importe).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoImpuestosTraslado>().Property(c => c.TasaOCuota).HasPrecision(18, 6);

			b.Entity<ComprobanteConceptoParte>().Property(c => c.Cantidad).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoParte>().Property(c => c.Importe).HasPrecision(18, 6);
			b.Entity<ComprobanteConceptoParte>().Property(c => c.ValorUnitario).HasPrecision(18, 6);

			b.Entity<ComprobanteImpuestos>().Property(c => c.TotalImpuestosRetenidos).HasPrecision(18, 6);
			b.Entity<ComprobanteImpuestos>().Property(c => c.TotalImpuestosTrasladados).HasPrecision(18, 6);

			b.Entity<ComprobanteImpuestosRetencion>().Property(c => c.Importe).HasPrecision(18, 6);

			b.Entity<ComprobanteImpuestosTraslado>().Property(c => c.Base).HasPrecision(18, 6);
			b.Entity<ComprobanteImpuestosTraslado>().Property(c => c.TasaOCuota).HasPrecision(18, 6);
			b.Entity<ComprobanteImpuestosTraslado>().Property(c => c.Importe).HasPrecision(18, 6);
		}

		private static void BuildAccesos(ModelBuilder b)
		{
			b.Entity<AppRole>().HasMany(r => r.Accesos).WithOne(am => am.Rol).OnDelete(DeleteBehavior.NoAction);
			b.Entity<Modulo>().HasMany(m => m.Accesos).WithOne(am => am.Modulo).OnDelete(DeleteBehavior.NoAction);

			b.Entity<Modulo>()
				.HasData(
					new Modulo() { Id = 1, Nombre = "Gestión de Talento", NombreNormalizado = "gestiondetalento", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 2, Nombre = "Usuarios", NombreNormalizado = "usuarios", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 3, Nombre = "Puestos", NombreNormalizado = "puestos", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 4, Nombre = "Áreas", NombreNormalizado = "areas", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 5, Nombre = "Subareas", NombreNormalizado = "subareas", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 6, Nombre = "Oficinas", NombreNormalizado = "oficinas", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 7, Nombre = "Empresas", NombreNormalizado = "empresas", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 8, Nombre = "Orígenes", NombreNormalizado = "origenes", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 9, Nombre = "Niveles", NombreNormalizado = "niveles", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 10, Nombre = "Perfiles", NombreNormalizado = "perfiles", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 11, Nombre = "Vacaciones", NombreNormalizado = "vacaciones", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 12, Nombre = "Incapacidades", NombreNormalizado = "incapacidades", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 13, Nombre = "Permisos", NombreNormalizado = "permisos", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 14, Nombre = "Prefacturas", NombreNormalizado = "prefacturas", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 15, Nombre = "Organigrama", NombreNormalizado = "organigrama", Deshabilitado = 0, Categoria = "reporte" },
					new Modulo() { Id = 16, Nombre = "Asistencia", NombreNormalizado = "asistencia", Deshabilitado = 0, Categoria = "reporte" },
					new Modulo() { Id = 17, Nombre = "Roles", NombreNormalizado = "roles", Deshabilitado = 0, Categoria = "catalogo" },
					new Modulo() { Id = 18, Nombre = "Activos Fijos", NombreNormalizado = "activosfijos", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 19, Nombre = "Conciliaciones", NombreNormalizado = "conciliaciones", Deshabilitado = 0, Categoria = "erp" },
					new Modulo() { Id = 20, Nombre = "Administrador de Comprobantes", NombreNormalizado = "administradordecomprobantes", Deshabilitado = 0, Categoria = "erp" }
				);
		}
	}
}