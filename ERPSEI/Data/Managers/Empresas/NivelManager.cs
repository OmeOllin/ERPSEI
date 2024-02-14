using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empresas
{
	public class NivelManager : IRWCatalogoManager<Nivel>
    {
        ApplicationDbContext db { get; set; }

        public NivelManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Nivel> registros = await db.Niveles.ToListAsync();
			Nivel? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Nivel n)
        {
            n.Id = await getNextId();
            db.Niveles.Add(n);
            await db.SaveChangesAsync();
            return n.Id;
        }
        public async Task UpdateAsync(Nivel n)
        {
			Nivel? a = db.Find<Nivel>(n.Id);
            if (a != null)
            {
                a.Nombre = n.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Nivel n)
        {
            db.Niveles.Remove(n);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			Nivel? n = await GetByIdAsync(id);
            if (n != null)
            {
                db.Remove(n);
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
					Nivel? n = await GetByIdAsync(int.Parse(id));
					if (n != null)
					{
						db.Remove(n);
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

		public async Task<List<Nivel>> GetAllAsync()
		{
			return await db.Niveles.ToListAsync();
		}

		public async Task<Nivel?> GetByIdAsync(int id)
        {
            return await db.Niveles.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Nivel?> GetByNameAsync(string name)
		{
			return await db.Niveles.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
