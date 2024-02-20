using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.ERP.Pages
{
    public class PrefacturasModel : PageModel
    {
        private readonly IStringLocalizer<PrefacturasModel> _strLocalizer;
        private readonly ILogger<PrefacturasModel> _logger;

        public PrefacturasModel(
            IStringLocalizer<PrefacturasModel> stringLocalizer,
            ILogger<PrefacturasModel> logger
        )
        {
            _strLocalizer = stringLocalizer;
            _logger = logger;
        }

        public void OnGet()
        {
        }
        public void OnPost()
        {
        }
    }
}
