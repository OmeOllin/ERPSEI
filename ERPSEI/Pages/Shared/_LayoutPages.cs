﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols;

namespace ERPSEI.Pages.Shared
{
    public static class _LayoutPages
    {
        public static string Inicio => "Inicio";
        public static string InicioNavClass(ViewContext viewContext) => PageNavClass(viewContext, Inicio);

        public static string AvisoPrivacidad => "AvisoPrivacidad";
        public static string AvisoPrivacidadNavClass(ViewContext viewContext) => PageNavClass(viewContext, AvisoPrivacidad);

        #region Catalogos

        public static string Catalogos => "Catalogos";
        public static string CatalogosNavClass(ViewContext viewContext) => PageNavClass(viewContext, Catalogos);

        #region Recursos Humanos

        public static string GestionDeTalento => "GestionDeTalento";
        public static string GestionDeTalentoNavClass(ViewContext viewContext) => PageNavClass(viewContext, GestionDeTalento);

        public static string CatalogoPuestos => "Puestos";
        public static string CatalogoPuestosNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoPuestos);

		public static string CatalogoAreas => "Areas";
		public static string CatalogoAreasNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoAreas);

		public static string CatalogoOficinas => "Oficinas";
		public static string CatalogoOficinasNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoOficinas);

		public static string CatalogoSubareas => "Subareas";
		public static string CatalogoSubareasNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoSubareas);

		public static string CatalogoUsuarios => "Usuarios";
		public static string CatalogoUsuariosNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoUsuarios);

        #endregion

        #region Facturacion

        public static string CatalogoEmpresas => "Empresas";
        public static string CatalogoEmpresasNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoEmpresas);

        public static string CatalogoOrigenes => "Origenes";
        public static string CatalogoOrigenesNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoOrigenes);

        public static string CatalogoNiveles => "Niveles";
        public static string CatalogoNivelesNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoNiveles);

		public static string CatalogoActividadesEconomicas => "ActividadesEconomicas";
		public static string CatalogoActividadesEconomicasNavClass(ViewContext viewContext) => PageNavClass(viewContext, CatalogoActividadesEconomicas);

        #endregion

        #endregion

        #region ERP

        public static string ERP => "ERP";
        public static string ERPNavClass(ViewContext viewContext) => PageNavClass(viewContext, ERP);

        #region Recursos Humanos

        public static string Vacaciones => "Vacaciones";
        public static string VacacionesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Vacaciones);

        public static string Incapacidades => "Incapacidades";
        public static string IncapacidadesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Incapacidades);

        public static string Permisos => "Permisos";
        public static string PermisosNavClass(ViewContext viewContext) => PageNavClass(viewContext, Permisos);

        #endregion

        #endregion

        #region Reportes

        public static string Reportes => "Reportes";
        public static string ReportesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reportes);

        #region Recuros Humanos
        public static string Organigrama => "Organigrama";
        public static string OrganigramaNavClass(ViewContext viewContext) => PageNavClass(viewContext, Organigrama);

        public static string Asistencia => "Asistencia";
        public static string AsistenciaNavClass(ViewContext viewContext) => PageNavClass(viewContext, Asistencia);
        #endregion

        #endregion

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            string? activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
        }
    }
}
