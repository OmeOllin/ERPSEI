using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Pages.Catalogos
{
    public class GestionDeTalentoModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {	
			[Display(Name = "Fecha ingreso (Inicio)")]
			public string FechaIngresoInicio { get; set; }

			[Display(Name = "Fecha ingreso (Fin)")]
			public string FechaIngresoFin { get; set; }

			[Display(Name = "Fecha nacimiento (Inicio)")]
			public string FechaNacimientoInicio { get; set; }

			[Display(Name = "Fecha nacimiento (Fin)")]
			public string FechaNacimientoFin { get; set; }

			[Display(Name = "Nombre")]
			public string Nombre { get; set; }

			[Display(Name = "Puesto")]
			public string Puesto { get; set; }

			[Display(Name = "Área")]
			public string Area { get; set; }

			[Display(Name = "CURP")]
			public string CURP { get; set; }
        }

        public void OnGet()
        {
        }
    }
}
