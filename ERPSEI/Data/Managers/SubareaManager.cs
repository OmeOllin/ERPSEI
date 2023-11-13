using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class SubareaManager : IRWCatalogoManager<Subarea>
    {
        ApplicationDbContext db { get; set; }

        public SubareaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Subarea> registros = await GetAllAsync();
			Subarea? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

        public async Task<int> CreateAsync(Subarea subarea)
        {
            subarea.Id = await getNextId();
            db.Subareas.Add(subarea);
            await db.SaveChangesAsync();
            return subarea.Id;
        }
        public async Task UpdateAsync(Subarea subarea)
        {
            Subarea? sa = db.Find<Subarea>(subarea.Id);
            if (sa != null)
            {
                sa.Nombre = subarea.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Subarea subarea)
        {
            db.Subareas.Remove(subarea);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			Subarea? subarea = await GetByIdAsync(id);
            if (subarea != null)
            {
                db.Remove(subarea);
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
                    Subarea? subarea = await GetByIdAsync(int.Parse(id));
                    if (subarea != null)
                    {
                        db.Remove(subarea);
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

		public async Task<List<Subarea>> GetAllAsync()
        {
            return await db.Subareas.ToListAsync();
        }

        public async Task<Subarea?> GetByIdAsync(int id)
        {
            return await db.Subareas.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Subarea?> GetByNameAsync(string name)
		{
			return await db.Subareas.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
