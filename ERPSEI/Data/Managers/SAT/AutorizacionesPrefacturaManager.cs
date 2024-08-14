using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class AutorizacionesPrefacturaManager : IAutorizacionesPrefactura
	{
        ApplicationDbContext db { get; set; }

        public AutorizacionesPrefacturaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<AutorizacionesPrefactura> registros = await db.AutorizacionesPrefacturas.ToListAsync();
			AutorizacionesPrefactura? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(AutorizacionesPrefactura a)
		{
			a.Id = await getNextId();
			db.AutorizacionesPrefacturas.Add(a);
			await db.SaveChangesAsync();
			return a.Id;
		}
		public async Task UpdateAsync(AutorizacionesPrefactura a)
		{
			AutorizacionesPrefactura? n = db.Find<AutorizacionesPrefactura>(a.Id);
			if (n != null)
			{
				n.PrefacturaId = a.PrefacturaId;
				n.UsuarioId = a.UsuarioId;
				n.FechaHoraAutorizacion = a.FechaHoraAutorizacion;

				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(AutorizacionesPrefactura a)
		{
			db.AutorizacionesPrefacturas.Remove(a);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			AutorizacionesPrefactura? c = await GetByIdAsync(id);
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
					AutorizacionesPrefactura? a = await GetByIdAsync(int.Parse(id));
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

		public async Task<List<AutorizacionesPrefactura>> GetAllAsync()
		{
			return await db.AutorizacionesPrefacturas.ToListAsync();
		}

		public async Task<AutorizacionesPrefactura?> GetByIdAsync(int id)
        {
            return await db.AutorizacionesPrefacturas.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

		public async Task<AutorizacionesPrefactura?> GetByNameAsync(string name)
		{
			return await db.AutorizacionesPrefacturas.Where(c => false).FirstOrDefaultAsync();
		}

	}
}
