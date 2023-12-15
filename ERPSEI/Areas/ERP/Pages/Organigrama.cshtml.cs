using ERPSEI.Areas.Catalogos.Pages;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using static ERPSEI.Areas.Catalogos.Pages.GestionDeTalentoModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using ERPSEI.Data.Migrations;
using System.Collections.Generic;

namespace ERPSEI.Areas.ERP.Pages
{
    public class OrganigramaModel : PageModel
    {
        private readonly IEmpleadoManager _empleadoManager;
        private readonly IStringLocalizer<OrganigramaModel> _strLocalizer;
        private readonly ILogger<OrganigramaModel> _logger;

        [BindProperty]
        public FiltroModel InputFiltro { get; set; }

        public class FiltroModel
        {
            [Display(Name = "AreaField")]
            public int? AreaId { get; set; }

            [Display(Name = "SubareaField")]
            public int? SubareaId { get; set; }
        }

        public OrganigramaModel(
            IEmpleadoManager empleadoManager,
            IStringLocalizer<OrganigramaModel> stringLocalizer,
            ILogger<OrganigramaModel> logger
        )
        {
            _empleadoManager = empleadoManager;
            _strLocalizer = stringLocalizer;
            _logger = logger;

            InputFiltro = new FiltroModel();
        }

        public void OnGet()
        {
        }

        private Task<string> CreateNode(Empleado emp, ref List<Empleado> empleadosRef, int levelOffset)
        {
            string nombreArea = emp.Area != null ? emp.Area.Nombre : "";
            string nombreSubarea = emp.Subarea != null ? emp.Subarea.Nombre : "";
            string nombrePuesto = emp.Puesto != null ? emp.Puesto.Nombre : "";
            string nombreOficina = emp.Oficina != null ? emp.Oficina.Nombre : "";
            string nombreJefe = emp.Jefe != null ? emp.Jefe.NombreCompleto : "";
            List<string> jsonChildren = new List<string>();
            string nombre = string.Empty;
            string firstApellido = string.Empty;
            ArchivoEmpleado? profilePicFile = null;
            string profilePic = string.Empty;

            //Crea los nodos hijos del empleado.
            bool loff = true;
            foreach (Empleado c in emp.Empleados ?? new List<Empleado>()) { 
                jsonChildren.Add(CreateNode(c, ref empleadosRef, loff ? 2 : 0).Result);
                loff = !loff;
            }

            //Quita al empleado procesado del listado de referencias para ya no volver procesarlo en el recorrido del listado principal.
            empleadosRef.Remove(emp);
            if (emp.NombrePreferido != null && emp.NombrePreferido.Length >= 1)
            {
                //Si el usuario tiene nombre preferido, lo usa para mostrar.
                nombre = emp.NombrePreferido;
            }
            else
            {
                //De lo contrario, por default usa siempre el primer nombre en caso de tener más de uno, o usa el único nombre en caso de solo tener 1
                string[] nombres = emp.Nombre.Split(" ");
                nombre = nombres.Length >= 1 ? nombres[0] : emp.Nombre;
            }
            firstApellido = emp.ApellidoPaterno.Split(" ")[0];
            if (emp.ArchivosEmpleado != null)
            {
                profilePicFile = emp.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil).FirstOrDefault();

                //Si el archivo tiene contenido
                if (profilePicFile != null && profilePicFile.Archivo.Length >= 1)
                {
                    //Asigna la información del archivo al arreglo de datos.
                    string b64 = Convert.ToBase64String(profilePicFile.Archivo);
                    bool isJPG = profilePicFile.Extension == "jpg" || profilePicFile.Extension == "jpeg";
                    bool isPNG = profilePicFile.Extension == "png";

                    if (isJPG)
                    {
                        profilePic = $"data:image/jpeg;base64,{b64}";
                    }
                    else if (isPNG)
                    {
                        profilePic = $"data:image/png;base64,{b64}";
                    }
                }
            }

            return Task.FromResult("{" +
                        $"\"id\": {emp.Id}, " +
						$"\"levelOffset\": {levelOffset}, " +
						$"\"name\": \"{ nombre + ' ' + firstApellido}\", " +
                        $"\"title\": \"{nombrePuesto}\", " +
                        $"\"children\": [{string.Join(",", jsonChildren)}], " +
                        $"\"fechaIngresoJS\": \"{emp.FechaIngreso:yyyy-MM-dd}\", " +
                        $"\"fechaNacimientoJS\": \"{emp.FechaNacimiento:yyyy-MM-dd}\", " +
                        $"\"telefono\": \"{emp.Telefono}\", " +
                        $"\"email\": \"{emp.Email}\", " +
                        $"\"subarea\": \"{nombreSubarea}\", " +
                        $"\"oficina\": \"{nombreOficina}\", " +
                        $"\"puesto\": \"{nombrePuesto}\", " +
                        $"\"area\": \"{nombreArea}\", " +
                        $"\"jefe\": \"{nombreJefe}\", " +
                        $"\"profilePic\": \"{profilePic}\" " +
                    "}");
        }

        private async Task<string> GetTalentList(FiltroModel? filtro = null)
        {
            string jsonResponse;
            List<string> jsonEmpleados = new List<string>();
            //Lista de empleados principal
            List<Empleado> empleados = await _empleadoManager.GetAllAsync();
            //Lista de empleados clonada
            List<Empleado> empleadosRef = new List<Empleado>(empleados);

            if (filtro != null)
            {
                if (filtro.AreaId != null) { empleados = empleados.Where(e => e.AreaId == filtro.AreaId).ToList(); }
                if (filtro.SubareaId != null) { empleados = empleados.Where(e => e.SubareaId == filtro.SubareaId).ToList(); }
            }

            bool levelOffset = true;
            //Crea los nodos de cada empleado.
            foreach (Empleado e in empleados) {
                //Si el empleado no se encuentra en el listado de empleados de referencia, entonces omite su procesamiento en el listado principal.
                if (!empleadosRef.Contains(e)) { continue; }

                jsonEmpleados.Add(await CreateNode(e, ref empleadosRef, levelOffset ? 2 : 0));

                levelOffset = !levelOffset;
            }

            jsonResponse = $"[{string.Join(",", jsonEmpleados)}]";

            return jsonResponse;
        }
        public async Task<JsonResult> OnPostFiltrarEmpleados()
        {
            ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosFiltradosUnsuccessfully"]);
            try
            {
                resp.Datos = await GetTalentList(InputFiltro);
                resp.TieneError = false;
                resp.Mensaje = _strLocalizer["EmpleadosFiltradosSuccessfully"];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new JsonResult(resp);
        }
    }
}
