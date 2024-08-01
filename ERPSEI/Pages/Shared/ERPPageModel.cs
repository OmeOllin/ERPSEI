using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Pages.Shared
{
	public class ERPPageModel : PageModel
	{
		protected bool PuedeTodo { get { return User.Claims.Where(c => c.Type.Equals("PuedeTodo", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value == "1"; } }
		protected bool PuedeConsultar { get { return User.Claims.Where(c => c.Type.Equals("PuedeConsultar", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value == "1"; } }
		protected bool PuedeEditar { get { return User.Claims.Where(c => c.Type.Equals("PuedeEditar", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value == "1"; } }
		protected bool PuedeEliminar { get { return User.Claims.Where(c => c.Type.Equals("PuedeEliminar", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value == "1"; } }
		protected bool PuedeAutorizar { get { return User.Claims.Where(c => c.Type.Equals("PuedeAutorizar", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value == "1"; } }
	}
}
