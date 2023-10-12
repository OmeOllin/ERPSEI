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
			public string FechaIngresoInicio { get; set; } = string.Empty;

			[Display(Name = "Fecha ingreso (Fin)")]
			public string FechaIngresoFin { get; set; } = string.Empty;

            [Display(Name = "Fecha nacimiento (Inicio)")]
			public string FechaNacimientoInicio { get; set; } = string.Empty;

            [Display(Name = "Fecha nacimiento (Fin)")]
			public string FechaNacimientoFin { get; set; } = string.Empty;

            [Display(Name = "Nombre")]
			public string Nombre { get; set; } = string.Empty;

            [Display(Name = "Puesto")]
			public string Puesto { get; set; } = string.Empty;

            [Display(Name = "Área")]
			public string Area { get; set; } = string.Empty;

            [Display(Name = "CURP")]
			public string CURP { get; set; } = string.Empty;
        }

        public GestionDeTalentoModel()
        {
            Input = new InputModel();
        }

        public IActionResult OnGet()
        {
            return Page();
		}

        public JsonResult OnGetTalentList()
        {
            List<string> talent = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                int rn1 = i + 1;
				
                talent.Add("{\"id\": " + rn1 + ",\"nombre\": \"Luis Alberto Linares Hernández\",\"fechaIngreso\": \"07/05/1991\",\"puesto\": \"Desarrollador de software\",\"area\": \"Desarrollo\",\"telefono\": \"5529300993\",\"correo\": \"luis_linares75@hotmail.com\"}");
            }


            return new JsonResult(talent);
        }
    }
}
