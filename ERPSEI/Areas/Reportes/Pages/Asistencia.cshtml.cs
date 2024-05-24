using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Reportes.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class AsistenciaModel : PageModel
	{
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
			public int? Id { get; set; }

			[Display(Name = "EmployeeNameField")]
			public string Nombre { get; set; } = string.Empty;

			[DataType(DataType.Date)]
			[Display(Name = "FechaInicioField")]
			public DateTime? Fecha { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "HoraEntradaField")]
			public DateTime? HoraEntrada { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "HoraSalidaField")]
			public DateTime? HoraSalida { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "RetardoField")]
			public string? Retardo { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "TotalField")]
			public string? Total { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "FaltasField")]
			public string? Faltas { get; set; }
		}

		public AsistenciaModel(
			IStringLocalizer<AsistenciaModel> stringLocalizer,
			ILogger<AsistenciaModel> logger
		)
		{
			_strLocalizer = stringLocalizer;
			_logger = logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
		}

		public void OnGet()
		{
			// Lógica para obtener los datos de asistencia y asignarlos a ListaAsistencia
		}

		public IActionResult OnPost()
		{
			// Lógica para manejar el envío del formulario (opcional)
			return Page();
		}
	}
}
