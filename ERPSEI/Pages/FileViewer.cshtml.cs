using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
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

        public string iframesrc { get; set; } = string.Empty;

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

            switch (module)
            {
                case "empleados":
                    ArchivoEmpleado? f1 = _userFileManager.GetFileById(id);
                    if (f1 == null) { return RedirectToPage("/404"); }
                    string src1 = Convert.ToBase64String(f1.Archivo);
                    if(f1.Extension == "pdf")
                    {
                        iframesrc = $"data:application/pdf;base64,{src1}";
                    }
                    else
                    {
                        iframesrc = $"data:image/{f1.Extension};base64,{src1}";
                    }
                    break;
                case "empresas":
                    ArchivoEmpresa? f2 = _archivoEmpresaManager.GetFileById(id);
                    if (f2 == null) { return RedirectToPage("/404"); }
                    string src2 = Convert.ToBase64String(f2.Archivo);
                    if(f2.Extension == "pdf")
                    {
                        iframesrc = $"data:application/pdf;base64,{src2}";
                    }
                    else
                    {
                        iframesrc = $"data:image/{f2.Extension};base64,{src2}";
                    }
					break;
                default:
                    iframesrc = string.Empty;
                    break;
            }
            
            if(iframesrc.Length <= 0) { return RedirectToPage("/404"); }

            return Page();
        }
    }
}
