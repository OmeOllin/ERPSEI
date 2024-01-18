using ERPSEI.Data.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.Catalogos.Pages
{
    [Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
    public class NotificadorModel : PageModel
    {
        private readonly IEmpleadoManager _empleadoManager;
        private readonly IStringLocalizer<NotificadorModel> _strLocalizer;
        private readonly ILogger<NotificadorModel> _logger;

        public NotificadorModel(
            IEmpleadoManager empleadoManager,
            IStringLocalizer<NotificadorModel> stringLocalizer,
            ILogger<NotificadorModel> logger
        )
        {
            _empleadoManager = empleadoManager;
            _strLocalizer = stringLocalizer;
            _logger = logger;

        }

    }
}
