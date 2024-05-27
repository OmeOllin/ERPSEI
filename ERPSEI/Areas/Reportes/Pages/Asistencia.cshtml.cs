using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class AsistenciaModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IStringLocalizer<AsistenciaModel> _strLocalizer;
		private readonly ILogger<AsistenciaModel> _logger;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }
		public Asistencia ListaAsistencia { get; set; }

		public class FiltroModel
		{
			[DataType(DataType.Text)]
			[Display(Name = "EmployeeNameField")]
			public string? NombreEmpleado { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaInicioField")]
			public DateTime? FechaIngresoInicio { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaFinField")]
			public DateTime? FechaIngresoFin { get; set; }
		}
		public class Asistencia
		{
			[Display(Name = "Id")]
			public int Id { get; set; }
			public string? Nombre { get; set; }
			public DateOnly? Fecha { get; set; }
			public TimeOnly? HoraEntrada { get; set; }
			public TimeOnly? HoraSalida { get; set; }
			public int? Retardo { get; set; }
			public int? Total { get; set; }
			public int? Faltas { get; set; }
		}
		public AsistenciaModel(
			ApplicationDbContext db,
			IStringLocalizer<AsistenciaModel> stringLocalizer,
			ILogger<AsistenciaModel> logger
		)
		{
			_db = db;
			_strLocalizer = stringLocalizer;
			_logger = logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
		}
	}
}
