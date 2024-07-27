using ERPSEI.Data.Managers.Empleados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Policy = "AccessPolicy")]
	public class NotificadorModel : PageModel
    {
        
        public NotificadorModel()
        {

        }

    }
}
