using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class PrefacturaManager : IPrefacturaManager
	{
        ApplicationDbContext db { get; set; }

        public PrefacturaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Prefactura> registros = await db.Prefacturas.ToListAsync();
			Prefactura? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Prefactura p)
		{
			p.Id = await getNextId();
			db.Prefacturas.Add(p);
			await db.SaveChangesAsync();
			return p.Id;
		}
		public async Task UpdateAsync(Prefactura p)
		{
			Prefactura? n = db.Find<Prefactura>(p.Id);
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
                n.UsuarioAutorizadorId = p.UsuarioAutorizadorId;
                n.UsuarioFinalizadorId = p.UsuarioFinalizadorId;

                await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Prefactura p)
		{
			db.Prefacturas.Remove(p);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Prefactura? p = await GetByIdAsync(id);
			if (p != null)
			{
				db.Remove(p);
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteMultipleByIdAsync(string[] ids)
		{
			//Inicia una transacción.
			await db.Database.BeginTransactionAsync();
			try
			{
				foreach (string id in ids)
				{
					Prefactura? a = await GetByIdAsync(int.Parse(id));
					if (a != null)
					{
						db.Remove(a);
						await db.SaveChangesAsync();
					}
				}

				await db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await db.Database.RollbackTransactionAsync();
				throw;

			}
		}

		public async Task<List<Prefactura>> GetAllAsync()
		{
			return await db.Prefacturas.ToListAsync();
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
			return await db.Prefacturas
				.Where(e => deshabilitado ? true : e.Deshabilitado == 0)
				.Where(e => fechaInicio != null ? e.Fecha >= fechaInicio : true)
				.Where(e => fechaFin!= null ? e.Fecha<= fechaFin : true)
				.Where(e => serie != null ? e.Serie == serie : true)
				.Where(e => monedaId != null ? e.MonedaId == monedaId : true)
				.Where(e => formaPagoId != null ? e.FormaPagoId == formaPagoId : true)
				.Where(e => metodoPagoId != null ? e.MetodoPagoId == metodoPagoId : true)
				.Where(e => usoCFDIId != null ? e.UsoCFDIId == usoCFDIId : true)
				.Include(e => e.TipoComprobante)
				.Include(e => e.Moneda)
				.Include(e => e.FormaPago)
				.Include(e => e.MetodoPago)
				.Include(e => e.UsoCFDI)
				.Include(e => e.Exportacion)
				.ToListAsync();
		}

		public async Task<Prefactura?> GetByIdWithAdicionalesAsync(int id)
		{
			return await db.Prefacturas
				.Where(e => e.Deshabilitado == 0)
				.Where(e => e.Id == id)
				.Include(e => e.Conceptos)
				.FirstOrDefaultAsync();
		}

		public async Task<Prefactura?> GetByIdAsync(int id)
        {
            return await db.Prefacturas
				.Where(e => e.Id == id)
				.Include(e => e.Emisor).ThenInclude(e => e.RegimenFiscal)
				.Include(e => e.Receptor).ThenInclude(r => r.RegimenFiscal)
				.Include(e => e.TipoComprobante)
				.Include(e => e.FormaPago)
				.Include(e => e.MetodoPago)
				.Include(e => e.UsoCFDI)
				.Include(e => e.Conceptos).ThenInclude(c => c.UnidadMedida)
				.Include(e => e.Conceptos).ThenInclude(c => c.ProductoServicio)
				.FirstOrDefaultAsync();
        }

		public async Task<Prefactura?> GetByNameAsync(string name)
		{
			return await db.Prefacturas.Where(p => $"{p.Serie.ToLower()}{p.Folio.ToLower()}" == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
