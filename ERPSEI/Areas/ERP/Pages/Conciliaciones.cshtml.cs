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
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Conciliaciones;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.Reportes;

namespace ERPSEI.Areas.ERP.Pages
{
    public class ConciliacionesModel : PageModel
    {
        private readonly IStringLocalizer<ConciliacionesModel> stringLocalizer;
        private readonly ILogger<ConciliacionesModel> logger;
        //private readonly IRCatalogoManager<Banco> bancoManager;
        private readonly IBancoManager bancoManager;
        private readonly IConciliacionManager conciliacionManager;
        private readonly IConciliacionDetalleManager conciliacionDetalleManager;
        private readonly IConciliacionDetalleComprobanteManager conciliacionDetalleComprobanteManager;
        private readonly IConciliacionDetalleMovimientoManager conciliacionDetalleMovimientoManager;
        private readonly IClienteManager clienteManager;
        private readonly IMovimientoBancarioManager movimientoBancarioManager;

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
            public DateTime? FechaElaboracionInicio { get; set; }

            [Display(Name = "FechaElaboracionFinField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaElaboracionFin { get; set; }
        }

        [BindProperty]
        public InputFiltroModelDComprobantes InputFiltroModalDComprobantes { get; set; }

        public class InputFiltroModelDComprobantes
        {
            [Display(Name = "FechaInicioModalDComprobantesField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaInicioModalDComprobantes { get; set; }

            [Display(Name = "FechaFinModalDComprobantesField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaFinModalDComprobantes { get; set; }
        }

        [BindProperty]
        public Conciliacion? ConciliacionesList { get; set; }
        public Banco? BancoList { get; set; }

        public ConciliacionesModel(
            IStringLocalizer<ConciliacionesModel> _stringLocalizer,
            ILogger<ConciliacionesModel> _logger,
            //IRCatalogoManager<Banco> _bancoManager,
            IBancoManager _bancoManager,
            IConciliacionManager _conciliacionManager,
            IConciliacionDetalleManager _conciliacionDetalleManager,
            IConciliacionDetalleComprobanteManager _conciliacionDetalleComprobanteManager,
            IConciliacionDetalleMovimientoManager _conciliacionDetalleMovimientoManager,
            IClienteManager _clienteManager,
            IMovimientoBancarioManager _movimientoBancarioManager,
            Data.ApplicationDbContext _db
        )
        {
            stringLocalizer = _stringLocalizer;
            logger = _logger;
            bancoManager = _bancoManager;
            conciliacionManager = _conciliacionManager;
            conciliacionDetalleManager = _conciliacionDetalleManager;
            conciliacionDetalleComprobanteManager = _conciliacionDetalleComprobanteManager;
            conciliacionDetalleMovimientoManager = _conciliacionDetalleMovimientoManager;
            clienteManager = _clienteManager;
            movimientoBancarioManager = _movimientoBancarioManager;
            db = _db;
            BancoList = new Banco();
            InputFiltro = new InputFiltroModel();
            ConciliacionesList = new Conciliacion();
        }

        public async Task<JsonResult> OnGetConciliacionesList()
        {
            ServerResponse resp = new(true, stringLocalizer["AsistenciaSavedUnsuccessfully"]);

            try
            {
                //var bancos = await bancoManager.GetAllAsync();
                //resp = new ServerResponse(true, "Bancos recuperados correctamente", bancos);
            }
            catch (Exception ex)
            {
                //logger.LogError(ex.Message);
                //resp = new ServerResponse(false, "Error al recuperar los bancos");
            }

            return new JsonResult(resp);
        }

        public async Task<JsonResult> OnPostSaveConciliacion()
        {
            ServerResponse resp = new(true, stringLocalizer["AsistenciaSavedUnsuccessfully"]);

            try
            {
                // Lógica para guardar la conciliación
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
                // Lógica para eliminar la conciliación
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
        }
    }
}
