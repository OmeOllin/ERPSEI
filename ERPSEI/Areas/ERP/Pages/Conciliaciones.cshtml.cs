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
using ERPSEI.Pages.Shared;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Conciliaciones;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
// Para Excel usando EPPlus
using OfficeOpenXml;
using OfficeOpenXml.Style;

// Para PDF usando iTextSharp
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using ERPSEI.Data.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Areas.Catalogos.Pages;
using ERPSEI.Data.Entities.Reportes;
using ERPSEI.Data.Managers.Reportes;
using NPOI.SS.Formula.Functions;

namespace ERPSEI.Areas.ERP.Pages
{
    public class ConciliacionesModel : ERPPageModel
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
        private readonly IEmpresaManager empresaManager;
        private readonly IStringLocalizer<ConciliacionesModel> localizer;

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
        public InputFiltroModelAgregar InputFiltroModalAgregar { get; set; }
        public class InputFiltroModelAgregar
        {
            [Display(Name = "IdField")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 1)]
            [RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "PersonName")]
            public int Id { get; set; }

            [Display(Name = "FechaElaboracionInicioField")]
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            public DateTime? FechaElaboracionInicio { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "ClienteField")]
            [StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 3)]
            [RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesisAmpersandMiddleDash, ErrorMessage = "PersonName")]
            public string? Cliente { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [Display(Name = "DescripcionField")]
            [StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 1)]
            [RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesisAmpersandMiddleDash, ErrorMessage = "PersonName")]
            public string? Descripcion { get; set; } = string.Empty;
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
            IEmpresaManager _empresaManager,
            IStringLocalizer<ConciliacionesModel> _localizer,
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
            empresaManager = _empresaManager;
            localizer = _localizer;
            db = _db;
            BancoList = new Banco();
            InputFiltro = new InputFiltroModel();
            ConciliacionesList = new Conciliacion();
        }

        public async Task<JsonResult> OnGetConciliacionesList()
        {
                List<string> jsonConciliaciones = new List<string>();
                List<Conciliacion> conciliaciones = await conciliacionManager.GetAllAsync();

                foreach (Conciliacion cons in conciliaciones)
                {
                    jsonConciliaciones.Add("{" +
                        $"\"Id\": \"{cons.Id}\", " +
                        $"\"Fecha\": \"{cons.Fecha}\", " +
                        $"\"Descripcion\": \"{cons.Descripcion}\", " +
                        $"\"Total\": \"{cons.Total}\", " +
                        $"\"BancoId\": \"{cons.BancoId}\", " +
                        $"\"Cliente\": \"{cons.Cliente?.Id}\", " +
                        $"\"EmpresaId\": \"{cons.EmpresaId}\", " +
                        $"\"UsuarioCreadorId\": \"{cons.AppUserC?.Id}\", " +
                        $"\"AppUserCId\": \"{cons.AppUserC}\", " +
                        $"\"UsuarioModificadorId\": \"{cons.AppUserM?.Id}\", " +
                        $"\"AppUserMId\": \"{cons.AppUserM}\", " +
                        $"\"Deshabilitado\": \"{cons.Deshabilitado}\"" +
                        "}");
                }

                string jsonResponse = $"[{string.Join(",", jsonConciliaciones)}]";
                return new JsonResult(jsonResponse);
        }

        public async Task<JsonResult> OnGetMovimientosList()
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
                // L�gica para guardar la conciliaci�n
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
                // L�gica para eliminar la conciliaci�n
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
            return File("/templates/PlantillaMovimientosBancarios.xlsx", MediaTypeNames.Application.Octet, "PlantillaMovimientosBancarios.xlsx");
        }

        public async Task<JsonResult> OnPostGetClientesEmpresasSuggestion(string texto)
        {
            ServerResponse resp = new(true, localizer["ConsultadoUnsuccessfully"]);
            try
            {
                if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
                {
                    resp.Datos = await GetClientesEmpresasSuggestion(texto);
                    resp.TieneError = false;
                    resp.Mensaje = localizer["ConsultadoSuccessfully"];
                }
                else
                {
                    resp.Mensaje = localizer["AccesoDenegado"];
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return new JsonResult(resp);
        }
        private async Task<string> GetClientesEmpresasSuggestion(string texto)
        {
            string jsonResponse;
            List<string> jsonEmpresas = [];

            List<EmpresaBuscada> empresas = await empresaManager.SearchEmpresas(texto);

            if (empresas != null)
            {
                foreach (EmpresaBuscada e in empresas)
                {
                    string desc = $"{e.RFC} - {e.RazonSocial}";
                    jsonEmpresas.Add($"{{" +
                                        $"\"id\": \"{e.Id}\", " +
                                        $"\"value\": \"{desc}\", " +
                                        $"\"label\": \"{desc}\"" +
                                    $"}}");
                }
            }

            jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

            return jsonResponse;
        }

    }
}
