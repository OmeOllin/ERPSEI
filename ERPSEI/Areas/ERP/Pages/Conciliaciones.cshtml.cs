using ERPSEI.Areas.Reportes.Pages;
using ERPSEI.Data.Entities.Conciliaciones;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Net.Mime;

namespace ERPSEI.Areas.ERP.Pages
{
    //[Authorize(Policy = "AccessPolicy")]
    public class ConciliacionesModel : PageModel
    {
        private readonly IStringLocalizer<ConciliacionesModel> stringLocalizer;
        private readonly ILogger<ConciliacionesModel> logger;
        private readonly Data.ApplicationDbContext db;

        [BindProperty]
        public InputFiltroModel? InputFiltro { get; set; }
        public class InputFiltroModel
        {
            [Display(Name = "IdField")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 1)]
            [RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "PersonName")]
            public int Id { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "ClienteField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesisAmpersandMiddleDash, ErrorMessage = "PersonName")]
            public string? Cliente { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [Display(Name = "UsuarioCreadorField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumNoSpace, ErrorMessage = "PersonName")]
            public string? UsuarioCreador { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [Display(Name = "UsuarioModificadorField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumNoSpace, ErrorMessage = "PersonName")]
            public string? UsuarioModificador { get; set; } = string.Empty;

            [Display(Name = "FechaElaboracionInicioField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaElaboracionInicio { get; set; } = null;

            [Display(Name = "FechaElaboracionFinField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaElaboracionFin { get; set; } = null;
        }

        [BindProperty]
        public InputFiltroModelDComprobantes InputFiltroModalDComprobantes { get; set; }
        public class InputFiltroModelDComprobantes
        {
            [Display(Name = "FechaInicioModalDComprobantesField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaInicioModalDComprobantes { get; set; } = null;

            [Display(Name = "FechaFinModalDComprobantesField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaFinModalDComprobantes { get; set; } = null;
        }

            [BindProperty]
        public Conciliacion? ConciliacionesList { get; set; }

        public ConciliacionesModel(
            IStringLocalizer<ConciliacionesModel> _stringLocalizer,
            ILogger<ConciliacionesModel> _logger,
            Data.ApplicationDbContext _db
        )
        {
            stringLocalizer = _stringLocalizer;
            logger = _logger;
            db = _db;

            InputFiltro = new InputFiltroModel();
            ConciliacionesList = new Conciliacion();
        }

        public async Task<JsonResult> OnGetConciliacionesList()
        {
            // Asegúrate de que ServerResponse esté bien definido y se adapte a tus necesidades
            ServerResponse resp = new(true, stringLocalizer["AsistenciaSavedUnsuccessfully"]);

            // Retorna los datos en formato JSON
            return new JsonResult(resp);
        }

        public async Task<JsonResult> OnPostSaveConciliacion()
        {
            ServerResponse resp = new(true, stringLocalizer["AsistenciaSavedUnsuccessfully"]);
            try
            {

                // Implementa la lógica para guardar la conciliación
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return new JsonResult(resp);
        }

        public async Task<JsonResult> OnPostDeleteConciliacion(string[] ids)
        {
            ServerResponse resp = new(true, stringLocalizer["RolDeletedUnsuccessfully"]);
            try
            {
                // Implementa la lógica para eliminar la conciliación
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await db.Database.RollbackTransactionAsync();
            }

            return new JsonResult(resp);
        }
        public ActionResult OnGetDownloadPlantilla()
        {
            return File("/templates/PlantillaAsistencia.xlsx", MediaTypeNames.Application.Octet, "PlantillaAsistencia.xlsx");

            /*if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
            {
                return File("/templates/PlantillaAsistencia.xlsx", MediaTypeNames.Application.Octet, "PlantillaAsistencia.xlsx");
            }
            else
            {
                return new EmptyResult();
            }*/
        }
    }
}