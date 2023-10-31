using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
	public class GestionDeTalentoModel : PageModel
    {
        [BindProperty]
        public FiltroModel InputFiltro { get; set; }

        public class FiltroModel
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

        [BindProperty]
        public EmpleadoModel InputEmpleado { get; set; }

        public class EmpleadoModel
        {
			public int Id { get; set; }

			[Display(Name = "Primer Nombre")]
			public string PrimerNombre { get; set; } = string.Empty;

			[Display(Name = "Segundo Nombre")]
			public string SegundoNombre { get; set; } = string.Empty;

			[Display(Name = "Apellido Paterno")]
			public string ApellidoPaterno { get; set; } = string.Empty;

			[Display(Name = "Apellido Materno")]
			public string ApellidoMaterno { get; set; } = string.Empty;

			[Display(Name = "Fecha Nacimiento")]
			public string FechaNacimiento { get; set; } = string.Empty;

			[Display(Name = "Fecha Ingreso")]
			public string FechaIngreso { get; set; } = string.Empty;

			[Display(Name = "Dirección")]
			public string Direccion { get; set; } = string.Empty;

			[Display(Name = "Teléfono")]
			public string Telefono { get; set; } = string.Empty;

			[Display(Name = "Correo Empresarial")]
			public string Email { get; set; } = string.Empty;

            public string Genero { get; set; } = string.Empty;

            public string EstadoCivil {  get; set; } = string.Empty;

            public string Puesto { get; set; } = string.Empty;

			public string Area { get; set; } = string.Empty;
		}

        public GestionDeTalentoModel()
        {
            InputFiltro = new FiltroModel();
            InputEmpleado = new EmpleadoModel();
        }

        public IActionResult OnGet()
        {
            return Page();
		}

        public JsonResult OnGetTalentList()
        {
            List<string> talent = new();
            for (int i = 0; i < 20; i++)
            {
                int rn1 = i + 1;
				
                talent.Add("{" +
                    "\"id\": " + rn1 + "," +
                    "\"nombre\": \"Luis Alberto Linares Hernández\"," +
                    "\"fechaIngreso\": \"11/09/2023\"," +
                    "\"puesto\": \"Desarrollador de software\"," +
                    "\"area\": \"Desarrollo\"," +
                    "\"telefono\": \"5529300993\"," +
                    "\"correo\": \"luis_linares75@hotmail.com\"}");
            }


            return new JsonResult(talent);
        }
    }
}
