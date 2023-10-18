using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Areas.Catalogos.Pages.Puestos
{
    public class ListadoPuestosModel : PageModel
    {
        private readonly IPuestoManager _puestoManager;

        public ListadoPuestosModel(IPuestoManager puestoManager)
        {
            _puestoManager = puestoManager;
        }

        public void OnGet()
        {
        }

        public JsonResult OnGetPuestosList()
        {
            List<Puesto> puestos = _puestoManager.GetAllAsync().Result;

            return new JsonResult(puestos);
        }
    }
}
