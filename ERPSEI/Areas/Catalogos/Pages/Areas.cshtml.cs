using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Areas.Catalogos.Pages
{
    public class AreasModel : PageModel
    {
		private readonly IAreaManager _areaManager;

		public AreasModel(IAreaManager areaManager)
		{
			_areaManager = areaManager;
		}

		public void OnGet()
        {
        }

		public JsonResult OnGetAreasList()
		{
			List<Area> areas = _areaManager.GetAllAsync().Result;

			return new JsonResult(areas);
		}
	}
}
