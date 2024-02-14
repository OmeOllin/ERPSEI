using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empresas
{
    public class OrigenManager : IRWCatalogoManager<Origen>
    {
        ApplicationDbContext db { get; set; }

        public OrigenManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Origen> registros = await db.Origenes.ToListAsync();
			Origen? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Origen o)
        {
            o.Id = await getNextId();
            db.Origenes.Add(o);
            await db.SaveChangesAsync();
            return o.Id;
        }
        public async Task UpdateAsync(Origen o)
        {
            Origen? a = db.Find<Origen>(o.Id);
            if (a != null)
            {
                a.Nombre = o.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Origen o)
        {
            db.Origenes.Remove(o);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Origen? o = await GetByIdAsync(id);
            if (o != null)
            {
                db.Remove(o);
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
					Origen? o = await GetByIdAsync(int.Parse(id));
					if (o != null)
					{
						db.Remove(o);
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

		public async Task<List<Origen>> GetAllAsync()
		{
			return await db.Origenes.ToListAsync();
		}

		public async Task<Origen?> GetByIdAsync(int id)
        {
            return await db.Origenes.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Origen?> GetByNameAsync(string name)
		{
			return await db.Origenes.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
