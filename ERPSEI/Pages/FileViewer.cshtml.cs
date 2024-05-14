using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Data.Managers.SAT;
using iText.Html2pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging.Signing;
using System.Transactions;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ERPSEI.Pages
{
    [Authorize]
    public class FileViewerModel : PageModel
    {
        private readonly IPrefacturaManager _prefacturaManager;
        private readonly IArchivoEmpleadoManager _userFileManager;
        private readonly IArchivoEmpresaManager _archivoEmpresaManager;

        public string htmlContainer { get; set; } = string.Empty;

        private class FileToRender
        {
            public string src { get; set; } = string.Empty;
            public string extension { get; set; } = string.Empty;

            public FileToRender(){}
        }

        public FileViewerModel(
            IPrefacturaManager prefacturaManager,
            IArchivoEmpleadoManager userFileManager,
            IArchivoEmpresaManager archivoEmpresaManager
        ) 
        { 
            _prefacturaManager = prefacturaManager;
            _userFileManager = userFileManager;
            _archivoEmpresaManager = archivoEmpresaManager;
        }

        public async Task<IActionResult> OnGet(string id, string module)
        {
            try
            {
				if (id == null || module == null) { return RedirectToPage("/404"); }

				FileToRender? ftr = null;
				switch (module)
				{
					case "empleados":
					case "perfil":
						ftr = GetArchivoEmpleadoB64(id);
						break;
					case "empresas":
						ftr = GetArchivoEmpresaB64(id);
						break;
					case "prefacturas":
						int idPf = 0;
						if (!int.TryParse(id, out idPf)) { idPf = 0; }
						ftr = await GetPrefacturaB64(idPf);
						break;
					default:
						htmlContainer = string.Empty;
						break;
				}

				if (ftr == null) { return RedirectToPage("/404"); }
				if (ftr.src.Length <= 0) { return RedirectToPage("/404"); }

				if (ftr.extension == "pdf")
				{
					htmlContainer = $"<iframe src=\"data:application/pdf;base64,{ftr.src}\" style=\"position:fixed; top:0; left:0; bottom:0; right:0; width:100%; height:100%; border:none; margin:0; padding:0; overflow:hidden; z-index:999999;\"></iframe>";
				}
				else
				{
					htmlContainer = $"<img src=\"data:image/{ftr.extension};base64,{ftr.src}\" style=\"height:100%\" />";
				}

				return Page();
			}
            catch (Exception)
            {
				return RedirectToPage("/404");
			}
        }

        private FileToRender? GetArchivoEmpleadoB64(string fileId) 
        {
            FileToRender ftr = new FileToRender();
			ArchivoEmpleado? f1 = _userFileManager.GetFileById(fileId);
			if (f1 == null) { return null; }
			ftr.src = Convert.ToBase64String(f1.Archivo);
			ftr.extension = f1.Extension;

            return ftr;
		}

		private FileToRender? GetArchivoEmpresaB64(string fileId)
		{
			FileToRender ftr = new FileToRender();
			ArchivoEmpresa? f2 = _archivoEmpresaManager.GetFileById(fileId);
			if (f2 == null) { return null; }
			ftr.src = Convert.ToBase64String(f2.Archivo);
			ftr.extension = f2.Extension;

            return ftr;
		}

		private async Task<FileToRender?> GetPrefacturaB64(int idPf) 
        {
			FileToRender ftr = new FileToRender();

			Prefactura? pf = await _prefacturaManager.GetByIdAsync(idPf);
			if (pf == null) { return null; }

			string filepath = $"wwwroot/templates/{Guid.NewGuid()}.pdf";
			HtmlConverter.ConvertToPdf(prefacturaToHTML(pf), new FileStream(filepath, FileMode.Create));

			ftr.src = await fileToB64(filepath);
			ftr.extension = "pdf";

            return ftr;
		}
        private string prefacturaToHTML(Prefactura pf)
        {
			decimal subtotalTotal = 0,
					descuentosTotal = 0,
					trasladosTotal = 0,
					retencionesTotal = 0,
					totalTotal = 0;

			List<string> htmlConceptos = new List<string>();
            foreach (Concepto c in pf.Conceptos)
            {
				decimal subtotal = c.Cantidad * c.PrecioUnitario,
						total = subtotal - c.Descuento + c.Traslado - c.Retencion;

				subtotalTotal += subtotal;
				descuentosTotal += c.Descuento;
				trasladosTotal += c.Traslado;
				retencionesTotal += c.Retencion;
				totalTotal += total;

				htmlConceptos.Add("<tr style='text-align: center;'>" +
									  $"<td>{c.Cantidad}</td>" +
									  $"<td>{c.UnidadMedida?.Clave}</td>" +
									  $"<td>{c.ProductoServicio?.Clave}</td>" +
									  $"<td>{c.Descripcion}</td>" +
									  $"<td>$ {c.Descuento}</td>" +
									  $"<td>$ {c.PrecioUnitario}</td>" +
									  $"<td>$ {total}</td>" +
								  "</tr>");
            }

            string html = "<html style='font-size: 12px;'>" +
                               "<body>" +
                                   $"<center><h1>{pf.Emisor?.RazonSocial}</h1></center>" +
                                   "<table border='0'>" +
                                        "<tr>" +
                                            "<td width='15%' align='right'><b>RFC: </b></td>" +
											$"<td width='45%'>{pf.Emisor?.RFC}</td>" +
											"<td width='15%' align='right'><b>Comprobante fiscal digital: </b></td>" +
											$"<td width='25%'>({pf.TipoComprobante?.Clave}){pf.TipoComprobante?.Descripcion}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Regimen fiscal: </b></td>" +
											$"<td>({pf.Emisor?.RegimenFiscal?.Clave}){pf.Emisor?.RegimenFiscal?.Descripcion}</td>" +
											"<td align='right'><b>Serie: </b></td>" +
											$"<td>{pf.Serie}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Domicilio fiscal: </b></td>" +
											$"<td>{pf.Emisor?.DomicilioFiscal}</td>" +
											"<td align='right'><b>Folio: </b></td>" +
											$"<td>{pf.Folio}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Expedido en: </b></td>" +
											$"<td> --- </td>" +
											"<td align='right'><b>Fecha: </b></td>" +
											$"<td>{pf.Fecha:yyyy-MM-ddThh:mm:ss}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Lugar de expedición: </b></td>" +
											$"<td> --- </td>" +
											"<td align='right'><b>Forma de pago: </b></td>" +
											$"<td>({pf.FormaPago?.Clave}){pf.FormaPago?.Descripcion}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Facturado a RFC: </b></td>" +
											$"<td>{pf.Receptor?.RFC} - {pf.Receptor?.RazonSocial}</td>" +
											"<td align='right'><b>Método de pago: </b></td>" +
											$"<td>({pf.MetodoPago?.Clave}){pf.MetodoPago?.Descripcion}</td>" +
										"</tr>" +
										"<tr>" +
											"<td align='right'><b>Régimen fiscal: </b></td>" +
											$"<td>({pf.Receptor?.RegimenFiscal?.Clave}){pf.Receptor?.RegimenFiscal?.Descripcion}</td>" +
											"<td align='right'><b>Uso de CFDI: </b></td>" +
											$"<td>({pf.UsoCFDI?.Clave}){pf.UsoCFDI?.Descripcion}</td>" +
										"</tr>" +
								   "</table>" +
								   "<br />" +
								   "<br />" +
								   "<table border='1' style='border-spacing: 0px;'>" +
										"<tr>" +
											"<th width='14.28%'>Cantidad</th>" +
											"<th width='14.28%'>Unidad</th>" +
											"<th width='14.28%'>Clave</th>" +
											"<th width='14.28%'>Descripcion</th>" +
											"<th width='14.28%'>Desc</th>" +
											"<th width='14.28%'>P/U</th>" +
											"<th width='14.28%'>Importe</th>" +
										"</tr>" +
										$"{string.Join("", htmlConceptos.ToArray())}" +
										"<tr>" +
											"<th colspan='5'></th>" +
											"<th>Subtotal</th>" +
											$"<th>$ {subtotalTotal}</th>" +
										"</tr>" +
										"<tr>" +
											"<th colspan='5'></th>" +
											"<th>Descuento</th>" +
											$"<th>$ {descuentosTotal}</th>" +
										"</tr>" +
										"<tr>" +
											"<th colspan='5'></th>" +
											"<th>Traslados</th>" +
											$"<th>$ {trasladosTotal}</th>" +
										"</tr>" +
										"<tr>" +
											"<th colspan='5'></th>" +
											"<th>Retenciones</th>" +
											$"<th>$ {retencionesTotal}</th>" +
										"</tr>" +
										"<tr>" +
											"<th colspan='5'></th>" +
											"<th>Total</th>" +
											$"<th>$ {totalTotal}</th>" +
										"</tr>" +
								   "</table>" +
							   "</body>" +
                          "</html>";

            return html;
		}
        private async Task<string> fileToB64(string filepath)
        {
			byte[] buffer;
			using (FileStream pdf = System.IO.File.OpenRead(filepath))
			{
				buffer = new byte[pdf.Length];
				await pdf.ReadAsync(buffer);
				pdf.Close();
			}
			System.IO.File.Delete(filepath);

            return Convert.ToBase64String(buffer);
		}
	}
}
