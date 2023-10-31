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

        public async Task CreateAsync(Subarea subarea)
        {
            db.Subareas.Add(subarea);
            await db.SaveChangesAsync();
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
			Subarea? subarea = GetById(id);
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
                    Subarea? subarea = GetById(int.Parse(id));
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

        public Subarea? GetById(int id)
        {
            return db.Subareas.Where(p => p.Id == id).FirstOrDefault();
        }

    }
}
