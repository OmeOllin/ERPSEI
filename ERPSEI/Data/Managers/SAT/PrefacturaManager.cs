using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class PrefacturaManager(ApplicationDbContext _db) : IPrefacturaManager
	{
		private async Task<int> GetNextId()
		{
			List<Prefactura> registros = await _db.Prefacturas.ToListAsync();
			Prefactura? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Prefactura p)
		{
			p.Id = await GetNextId();
			_db.Prefacturas.Add(p);
			await _db.SaveChangesAsync();
			return p.Id;
		}
		public async Task UpdateAsync(Prefactura p)
		{
			Prefactura? n = _db.Find<Prefactura>(p.Id);
			if (n != null)
			{
				n.Fecha = p.Fecha;
				n.Serie = p.Serie;
				n.Folio = p.Folio;
				n.TipoCambio = p.TipoCambio;
				n.ExportacionId = p.ExportacionId;
				n.FormaPagoId = p.FormaPagoId;
				n.MetodoPagoId = p.MetodoPagoId;
				n.UsoCFDIId = p.UsoCFDIId;
				n.MonedaId = p.MonedaId;
				n.NumeroOperacion = p.NumeroOperacion;
				n.TipoComprobanteId = p.TipoComprobanteId;
				n.UsuarioCreadorId = p.UsuarioCreadorId;
                n.UsuarioTimbradorId = p.UsuarioTimbradorId;
				n.EstatusId = p.EstatusId;
				n.RequiereAutorizacion = p.RequiereAutorizacion;

				await _db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Prefactura p)
		{
			_db.Prefacturas.Remove(p);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Prefactura? p = await GetByIdAsync(id);
			if (p != null)
			{
				_db.Remove(p);
				await _db.SaveChangesAsync();
			}
		}

		public async Task DeleteMultipleByIdAsync(string[] ids)
		{
			//Inicia una transacción.
			await _db.Database.BeginTransactionAsync();
			try
			{
				foreach (string id in ids)
				{
					Prefactura? a = await GetByIdAsync(int.Parse(id));
					if (a != null)
					{
						_db.Remove(a);
						await _db.SaveChangesAsync();
					}
				}

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await _db.Database.RollbackTransactionAsync();
				throw;

			}
		}

		public async Task<List<Prefactura>> GetAllAsync()
		{
			return await _db.Prefacturas.ToListAsync();
		}

		public async Task<List<Prefactura>> GetAllAsync(
			DateTime? fechaInicio = null,
			DateTime? fechaFin = null,
			string? serie = null,
			int? monedaId = null,
			int? formaPagoId = null,
			int? metodoPagoId = null,
			int? usoCFDIId = null,
			bool deshabilitado = false
		)
		{
			return await _db.Prefacturas
				.Where(e => deshabilitado || e.Deshabilitado == 0)
				.Where(e => fechaInicio == null || e.Fecha >= fechaInicio)
				.Where(e => fechaFin== null || e.Fecha<= fechaFin)
				.Where(e => serie == null || e.Serie == serie)
				.Where(e => monedaId == null || e.MonedaId == monedaId)
				.Where(e => formaPagoId == null || e.FormaPagoId == formaPagoId)
				.Where(e => metodoPagoId == null || e.MetodoPagoId == metodoPagoId)
				.Where(e => usoCFDIId == null || e.UsoCFDIId == usoCFDIId)
				.Include(e => e.Emisor).ThenInclude(e => e.RegimenFiscal)
				.Include(e => e.Receptor).ThenInclude(r => r.RegimenFiscal)
				.Include(e => e.TipoComprobante)
				.Include(e => e.Moneda)
				.Include(e => e.FormaPago)
				.Include(e => e.MetodoPago)
				.Include(e => e.UsoCFDI)
				.Include(e => e.Exportacion)
				.Include(e => e.Estatus)
				.Include(e => e.Autorizaciones)
				.ToListAsync();
		}

		public async Task<Prefactura?> GetByIdWithAdicionalesAsync(int id)
		{
			return await _db.Prefacturas
				.Where(e => e.Deshabilitado == 0)
				.Where(e => e.Id == id)
				.Include(e => e.Conceptos).ThenInclude(c => c.UnidadMedida)
				.Include(e => e.Conceptos).ThenInclude(c => c.ProductoServicio)
				.FirstOrDefaultAsync();
		}

		public async Task<Prefactura?> GetByIdAsync(int id)
        {
            return await _db.Prefacturas
				.Where(e => e.Id == id)
				.Include(e => e.Emisor).ThenInclude(e => e.RegimenFiscal)
				.Include(e => e.Receptor).ThenInclude(r => r.RegimenFiscal)
				.Include(e => e.TipoComprobante)
				.Include(e => e.Moneda)
				.Include(e => e.FormaPago)
				.Include(e => e.MetodoPago)
				.Include(e => e.UsoCFDI)
				.Include(e => e.Exportacion)
				.Include(e => e.Conceptos).ThenInclude(c => c.ObjetoImpuesto)
				.Include(e => e.Conceptos).ThenInclude(c => c.UnidadMedida)
				.Include(e => e.Conceptos).ThenInclude(c => c.ProductoServicio)
				.FirstOrDefaultAsync();
        }

		public async Task<Prefactura?> GetByNameAsync(string name)
		{
			return await _db.Prefacturas.Where(p => $"{p.Serie.ToLower()}{p.Folio.ToLower()}".Equals(name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();
		}

	}
}
