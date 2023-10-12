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

        public static string GestionDeTalento => "GestionDeTalento";
        public static string GestionDeTalentoNavClass(ViewContext viewContext) => PageNavClass(viewContext, GestionDeTalento);

        public static string CatalogoPuestos => "CatalogoPuestos";
        public static string CatalogoPuestosNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoPuestos);


        public static string ERP => "ERP";
        public static string ERPNavClass(ViewContext viewContext) => PageNavClass(viewContext, ERP);

        public static string Organigrama => "Organigrama";
        public static string OrganigramaNavClass(ViewContext viewContext) => PageNavClass(viewContext, Organigrama);

        public static string Vacaciones => "Vacaciones";
        public static string VacacionesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Vacaciones);

        public static string Incapacidades => "Incapacidades";
        public static string IncapacidadesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Incapacidades);

        public static string Permisos => "Organigrama";
        public static string PermisosNavClass(ViewContext viewContext) => PageNavClass(viewContext, Permisos);


        public static string PageNavClass(ViewContext viewContext, string page)
        {
            string? activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
        }
    }
}
