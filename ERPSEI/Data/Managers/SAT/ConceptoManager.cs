using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class ConceptoManager : IConceptoManager
	{
        ApplicationDbContext db { get; set; }

        public ConceptoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Concepto> registros = await db.Conceptos.ToListAsync();
			Concepto? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Concepto c)
		{
			c.Id = await getNextId();
			db.Conceptos.Add(c);
			await db.SaveChangesAsync();
			return c.Id;
		}
		public async Task UpdateAsync(Concepto c)
		{
			Concepto? n = db.Find<Concepto>(c.Id);
			if (n != null)
			{
				n.PrefacturaId = c.PrefacturaId;
				n.ProductoServicioId = c.ProductoServicioId;
				n.Cantidad = c.Cantidad;
				n.PrecioUnitario = c.PrecioUnitario;
				n.Descuento = c.Descuento;
				n.UnidadMedidaId = c.UnidadMedidaId;
				n.Descripcion = c.Descripcion;
				n.ObjetoImpuestoId = c.ObjetoImpuestoId;
				n.TasaTraslado = c.TasaTraslado;
				n.TasaRetencion = c.TasaRetencion;

				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Concepto c)
		{
			db.Conceptos.Remove(c);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Concepto? c = await GetByIdAsync(id);
			if (c != null)
			{
				db.Remove(c);
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
					Concepto? c = await GetByIdAsync(int.Parse(id));
					if (c != null)
					{
						db.Remove(c);
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

		public async Task DeleteByPrefacturaIdAsync(int id) {
			List<Concepto> conceptos = await db.Conceptos.Where(c => c.PrefacturaId == id).ToListAsync();
			if (conceptos != null && conceptos.Count >= 1) { db.Conceptos.RemoveRange(conceptos); }
			await db.SaveChangesAsync();
		}

		public async Task<List<Concepto>> GetAllAsync()
		{
			return await db.Conceptos.ToListAsync();
		}

		public async Task<Concepto?> GetByIdAsync(int id)
        {
            return await db.Conceptos.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Concepto?> GetByNameAsync(string name)
		{
			return await db.Conceptos.Where(c => c.Descripcion == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
