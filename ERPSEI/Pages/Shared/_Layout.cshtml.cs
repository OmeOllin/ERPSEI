using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        private readonly ILogger<_LayoutModel> _logger;

        public _LayoutModel(ILogger<_LayoutModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
