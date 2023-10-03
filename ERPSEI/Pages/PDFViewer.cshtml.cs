using ERPSEI.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERPSEI.Pages
{
    [Authorize]
    public class PDFViewerModel : PageModel
    {
        private readonly IUserFileManager _userFileManager;

        public string iframesrc { get; set; } = string.Empty;

        public PDFViewerModel(IUserFileManager userFileManager) { 
            _userFileManager = userFileManager;
        }

        public void OnGet(string id)
        {
            if (id == null) { return; }
            UserFile file = _userFileManager.GetFileById(id);
            string src = Convert.ToBase64String(file.File);
            iframesrc = $"data:application/pdf;base64,{src}";
        }
    }
}
