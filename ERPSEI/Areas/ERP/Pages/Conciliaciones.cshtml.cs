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
            [Display(Name = "Id")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 1)]
            [RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "PersonName")]
            public int Id { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Cliente")]
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

            // Crear una lista manualmente con registros de conciliaciones
            var conciliaciones = new List<object>
            {
                new
                {
                    Id = 1,
                    Fecha = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss"),
                    Descripcion = "Conciliación de prueba 1",
                    Total = 1000.50,
                    BancoId = 101,
                    ClienteId = 202,
                    EmpresaId = 303,
                    UsuarioCreador = "usuario1",
                    UsuarioModificador = "usuario2",
                    DetallesConciliacion = "Detalles adicionales para la conciliación de prueba 1"
                },
                new
                {
                    Id = 2,
                    Fecha = DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss"),
                    Descripcion = "Conciliación de prueba 2",
                    Total = 2000.75,
                    BancoId = 104,
                    ClienteId = 205,
                    EmpresaId = 306,
                    UsuarioCreador = "usuario3",
                    UsuarioModificador = "usuario4",
                    DetallesConciliacion = "Detalles adicionales para la conciliación de prueba 2"
                }
            };

            // Retorna los datos en formato JSON
            return new JsonResult(new { data = conciliaciones });
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
    }
}