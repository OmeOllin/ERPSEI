using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
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

        public AsistenciaModel(
            IStringLocalizer<AsistenciaModel> stringLocalizer,
            ILogger<AsistenciaModel> logger
        )
        {
            _strLocalizer = stringLocalizer;
            _logger = logger;

            InputFiltro = new FiltroModel();
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}
