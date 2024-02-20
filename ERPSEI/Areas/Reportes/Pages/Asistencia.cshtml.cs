using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.Reportes.Pages
{
	public class AsistenciaModel : PageModel
    {
        private readonly IStringLocalizer<AsistenciaModel> _strLocalizer;
        private readonly ILogger<AsistenciaModel> _logger;

        [BindProperty]
        public FiltroModel InputFiltro { get; set; }

        public class FiltroModel
        {
            
        }

        public AsistenciaModel(
            IStringLocalizer<AsistenciaModel> stringLocalizer,
            ILogger<AsistenciaModel> logger
        )
        {
            _strLocalizer = stringLocalizer;
            _logger = logger;

            InputFiltro = new FiltroModel();
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}
