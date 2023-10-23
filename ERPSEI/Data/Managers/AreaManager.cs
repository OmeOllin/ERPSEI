using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class AreaManager : IAreaManager
    {
        ApplicationDbContext db { get; set; }

        public AreaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task CreateAsync(Area area)
        {
            db.Areas.Add(area);
            await db.SaveChangesAsync();
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
            Area? area = GetById(id);
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
					Area? area = GetById(int.Parse(id));
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

		public Area? GetById(int id)
        {
            return db.Areas.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
