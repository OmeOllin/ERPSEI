using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.ERP.Pages
{
    public class ConciliacionesModel : PageModel
    {
        public class InputFiltroModel
        {
            [Display(Name = "Id")]
            public int Id { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Fecha")]
            public TimeSpan? Fecha { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Cliente")]
            public string? Cliente { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Usuario que creó")]
            public string? UsuarioCreador { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Usuario que modificó")]
            public string? UsuarioModificador { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Fecha Elaboracion (Inicio)")]
            public TimeSpan? FechaElaboracionInicio { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Fecha Elaboracion (Fin)")]
            public TimeSpan? FechaElaboracionFin { get; set; }
        }

        [BindProperty]
        public InputFiltroModel? InputFiltro { get; set; }

        [BindProperty]
        public Asistencia ListaConciliaciones { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            // Manejar la lógica de los filtros cuando se hace la búsqueda.
            return Page();
        }
    }
}
