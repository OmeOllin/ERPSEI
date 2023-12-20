using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.ERP.Pages
{
	public class OrganigramaModel : PageModel
    {
		//Crea un elemento Empleado con el nombre de la corporación, que será el jefe en común.
		private Empleado organizacion = new Empleado() { Id = 0, Nombre = "SEI", ApellidoPaterno = "Consulting", Puesto = new Puesto() { Nombre = "Organización" } };

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

        private async Task<string> createNode(Empleado emp)
        {
			string nombreArea = emp.Area != null ? emp.Area.Nombre : "";
            string nombreSubarea = emp.Subarea != null ? emp.Subarea.Nombre : "";
            string nombrePuesto = emp.Puesto != null ? emp.Puesto.Nombre : "";
            string nombreOficina = emp.Oficina != null ? emp.Oficina.Nombre : "";
            string nombre = string.Empty;
            ArchivoEmpleado? profilePicFile = null;
            string profilePic = string.Empty;
            List<string> jsonChildren = new List<string>();

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

            if(emp.Empleados != null)
            {
                foreach(Empleado empleado in emp.Empleados){
                    jsonChildren.Add(await createNode(empleado));
                }
            }

            return "{" +
                        $"\"id\": {emp.Id}, " +
						$"\"name\": \"{ nombre + ' ' + emp.ApellidoPaterno}\", " +
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
                        $"\"jefe\": \"\", " +
                        $"\"profilePic\": \"{profilePic}\" " +
                    "}";
        }
        private List<Empleado> getJerarquiaPadres(List<Empleado> empleados)
        {
			List<Empleado> empOrg = new List<Empleado>(empleados);

			//Se buscan los jefes de cada empleado
			foreach (Empleado empleado in empleados)
			{
				Empleado? Jefe = _empleadoManager.GetEmpleadoOrganigramaAsync(empleado.JefeId ?? 0).Result;
                //Si no tiene jefe y si el empleado no es ya la organización, entonces se coloca a la organización como Jefe.
                if(Jefe == null && empleado.Id != 0) { 
                    //Si la lista de ayuda contiene a la organización, entonces la obtiene de la lista. De lo contrario, establece al jefe como la organización.
                    if (empOrg.Contains(organizacion)) { Jefe = empOrg[empOrg.IndexOf(organizacion)]; } else { Jefe = organizacion; }
				}

                if(Jefe != null) {
					//Si la lista de ayuda contiene a la organización, entonces la obtiene de la lista.
					if (empOrg.Contains(Jefe)) { Jefe = empOrg[empOrg.IndexOf(Jefe)]; }

					//Se añade el empleado como hijo del empleado jefe.
					if (Jefe.Empleados == null) { Jefe.Empleados = new List<Empleado>() { empleado }; } else { Jefe.Empleados.Add(empleado); }

                    //Si la lista de ayuda ya contiene al Jefe, entonces lo establece en la lista. De lo contrario lo agrega a la lista.
                    if (empOrg.Contains(Jefe)) { empOrg[empOrg.IndexOf(Jefe)] = Jefe; } else { empOrg.Add(Jefe); }

				    //Quita al empleado procesado.
				    empOrg.Remove(empleado);
                }
			}

			//Si la lista de empleados contiene más de un empleado o si el primer empleado no es la organización (Id = 0), obtiene la jerarquía de los padres. De lo contrario, la lista final es la lista resultado.
			if (empleados.Count >= 2 || empleados[0].Id >= 1) { empleados = getJerarquiaPadres(empOrg); } else { empleados = empOrg; }

            return empleados;
		}
        private List<Empleado> getJerarquiaHijos(List<Empleado> empleadosMutable, List<Empleado> empleadosIniciales, ref List<Empleado> empleadosProcesados)
        {
			//Obtiene los empleados de cada empleado.
			foreach (Empleado empleado in empleadosMutable)
			{
				//Si el empleado ya fue procesado anteriormente, lo omite.
				if (empleadosProcesados.Contains(empleado)) { continue; }

                //Obtiene el listado de empleados para el empleado actual.
				empleado.Empleados = empleadosIniciales.Where(e => e.JefeId == empleado.Id).ToList();

                 //Obtiene los hijos de los hijos.
                if(empleado.Empleados != null && empleado.Empleados.Count >= 1) { 
				    //Agrega los empleados al listado de empleados procesados.
				    empleadosProcesados.AddRange(empleado.Empleados);

                    List<Empleado> empsRef = new List<Empleado>();
                    //obtiene la jerarquía de los hijos
                    empleado.Empleados = getJerarquiaHijos(empleado.Empleados, empleadosIniciales, ref empsRef);
                    //Copia los elementos procesados al arreglo de elementos procesados principal.
                    empleadosProcesados.AddRange(empsRef);
                    empsRef.Clear();
                }
			}

            //Quita los empleados procesados del listado principal para que no sean procesados nuevamente.
            if (empleadosProcesados.Count >= 1) { foreach (Empleado e in empleadosProcesados) { empleadosMutable.Remove(e); } }

            return empleadosMutable;
		}
        private async Task<string> getTalentList(FiltroModel? filtro = null)
        {
			List<string> jsonEmpleados = new List<string>();
			int? areaId = filtro != null ? filtro.AreaId : 0;
			int? subareaId = filtro != null ? filtro.SubareaId : 0;

			//Lista de empleados principal
			List<Empleado> empleados = await _empleadoManager.GetEmpleadosOrganigramaAsync(null, areaId, subareaId);

            //Si la lista de empleados no tuvo resultados, devuelve un arreglo vacío
            if(empleados == null || empleados.Count == 0) { return "[]"; }

            //Inicializa listas de ayuda.
            List<Empleado> empleadosIniciales = new List<Empleado>(empleados);
			List<Empleado> empleadosProcesados = new List<Empleado>();

			//Obtiene la jerarquía de los hijos.
			empleados = getJerarquiaHijos(empleados, empleadosIniciales, ref empleadosProcesados);
            //Limpia las listas de ayuda.
            empleadosIniciales.Clear();
            empleadosProcesados.Clear();

			//Si la lista de empleados contiene más de un empleado o si el primer empleado no es la organización (Id = 0), obtiene la jerarquía de los padres.
			if (empleados.Count >= 2 || empleados[0].Id >= 1) { empleados = getJerarquiaPadres(empleados); }

            //Crea el JSON de todos los empleados
            foreach (Empleado empleado in empleados) { jsonEmpleados.Add(await createNode(empleado)); }

			return $"[{string.Join(",", jsonEmpleados)}]";
		}

        public async Task<JsonResult> OnPostFiltrarEmpleados()
        {
            ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosFiltradosUnsuccessfully"]);
            try
            {
                resp.Datos = await getTalentList(InputFiltro);
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
