using ERPSEI.Data.Entities.Conciliaciones;
using ERPSEI.Resources;
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
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 1)]
            [RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "PersonName")]
            public string Id { get; set; } = string.Empty;

            [Display(Name = "Cliente")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesisAmpersandMiddleDash, ErrorMessage = "PersonName")]
            public string Cliente { get; set; } = string.Empty;

            [Display(Name = "UsuarioCreoField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumNoSpace, ErrorMessage = "PersonName")]
            public string UsuarioCreador { get; set; } = string.Empty;

            [Display(Name = "UsuarioModificadorField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumNoSpace, ErrorMessage = "PersonName")]
            public string UsuarioModificador { get; set; } = string.Empty;

            [Display(Name = "FechaElaboracionInicioField)")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.DateTime)]
            public DateTime? FechaElaboracionInicio { get; set; }

            [Display(Name = "FechaElaboracionFinField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.DateTime)]
            public DateTime? FechaElaboracionFin { get; set; }
        }

        [BindProperty]
        public InputFiltroModel? InputFiltro { get; set; }

        [BindProperty]
        public Conciliacion? ConciliacionesList { get; set; }

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
