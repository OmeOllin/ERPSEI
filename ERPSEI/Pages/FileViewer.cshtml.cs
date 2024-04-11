using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.Empresas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Pages
{
    [Authorize]
    public class FileViewerModel : PageModel
    {
        private readonly IArchivoEmpleadoManager _userFileManager;
        private readonly IArchivoEmpresaManager _archivoEmpresaManager;

        public string htmlContainer { get; set; } = string.Empty;

        public FileViewerModel(
            IArchivoEmpleadoManager userFileManager,
            IArchivoEmpresaManager archivoEmpresaManager
        ) 
        { 
            _userFileManager = userFileManager;
            _archivoEmpresaManager = archivoEmpresaManager;
        }

        public IActionResult OnGet(string id, string module)
        {
            if (id == null || module == null) { return RedirectToPage("/404"); }

            string extension = string.Empty;
            string src = string.Empty; 
            switch (module)
            {
                case "empleados":
                case "perfil":
                    ArchivoEmpleado? f1 = _userFileManager.GetFileById(id);
                    if (f1 == null) { return RedirectToPage("/404"); }
                    src = Convert.ToBase64String(f1.Archivo);
                    extension = f1.Extension;
                    break;
                case "empresas":
                    ArchivoEmpresa? f2 = _archivoEmpresaManager.GetFileById(id);
                    if (f2 == null) { return RedirectToPage("/404"); }
                    src = Convert.ToBase64String(f2.Archivo);
                    extension = f2.Extension;
					break;
                default:
                    htmlContainer = string.Empty;
                    break;
            }
            
            if(src.Length <= 0) { return RedirectToPage("/404"); }

            if (extension == "pdf")
            {
                htmlContainer = $"<iframe src=\"data:application/pdf;base64,{src}\" style=\"position:fixed; top:0; left:0; bottom:0; right:0; width:100%; height:100%; border:none; margin:0; padding:0; overflow:hidden; z-index:999999;\"></iframe>";
            }
            else
            {
                htmlContainer = $"<img src=\"data:image/{extension};base64,{src}\" style=\"height:100%\" />";
            }

            return Page();
        }
    }
}
