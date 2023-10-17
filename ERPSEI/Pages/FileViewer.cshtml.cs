using ERPSEI.Data.Entities.Empleados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Pages
{
    [Authorize]
    public class FileViewerModel : PageModel
    {
        private readonly IArchivoEmpleadoManager _userFileManager;

        public string iframesrc { get; set; } = string.Empty;

        public FileViewerModel(IArchivoEmpleadoManager userFileManager) { 
            _userFileManager = userFileManager;
        }

        public IActionResult OnGet(string id)
        {
            if (id == null) { return RedirectToPage("/404"); }
            ArchivoEmpleado? file = _userFileManager.GetFileById(id);
            if (file == null) { return RedirectToPage("/404"); }

            string src = Convert.ToBase64String(file.Archivo);
            if(file.Extension == "pdf")
            {
                iframesrc = $"data:application/pdf;base64,{src}";
            }
            else
            {
                iframesrc = $"data:image/{file.Extension};base64,{src}";
            }
            return Page();
        }
    }
}
