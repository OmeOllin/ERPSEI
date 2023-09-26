using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols;

namespace ERPSEI.Pages.Shared
{
    public static class _LayoutPages
    {
        public static string Inicio => "Inicio";
        public static string InicioNavClass(ViewContext viewContext) => PageNavClass(viewContext, Inicio);

        public static string AvisoPrivacidad => "AvisoPrivacidad";
        public static string AvisoPrivacidadNavClass(ViewContext viewContext) => PageNavClass(viewContext, AvisoPrivacidad);

        public static string Catalogos => "Catalogos";
        public static string CatalogosNavClass(ViewContext viewContext) => PageNavClass(viewContext, Catalogos);

        public static string ERP => "ERP";
        public static string ERPNavClass(ViewContext viewContext) => PageNavClass(viewContext, ERP);


        public static string PageNavClass(ViewContext viewContext, string page)
        {
            string? activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
        }
    }
}
