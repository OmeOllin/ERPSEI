using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class AreaManager : IRWCatalogoManager<Area>
    {
        ApplicationDbContext db { get; set; }

        public AreaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Area> registros = await GetAllAsync();
			Area? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Area area)
        {
            area.Id = await getNextId();
            db.Areas.Add(area);
            await db.SaveChangesAsync();
            return area.Id;
        }
        public async Task UpdateAsync(Area area)
        {
            Area? a = db.Find<Area>(area.Id);
            if (a != null)
            {
                a.Nombre = area.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Area area)
        {
            db.Areas.Remove(area);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Area? area = await GetByIdAsync(id);
            if (area != null)
            {
                db.Remove(area);
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
					Area? area = await GetByIdAsync(int.Parse(id));
					if (area != null)
					{
						db.Remove(area);
						await db.SaveChangesAsync();
					}
				}

				await db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await db.Database.RollbackTransactionAsync();

			}
		}

		public async Task<List<Area>> GetAllAsync()
		{
			return await db.Areas.ToListAsync();
		}

		public async Task<Area?> GetByIdAsync(int id)
        {
            return await db.Areas.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

    }
}
