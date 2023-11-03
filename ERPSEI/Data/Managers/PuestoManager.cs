using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class PuestoManager : IRWCatalogoManager<Puesto>
    {
        ApplicationDbContext db { get; set; }

        public PuestoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Puesto> registros = await GetAllAsync();
			Puesto? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}
		public async Task CreateAsync(Puesto puesto)
        {
            puesto.Id = await getNextId();
            db.Puestos.Add(puesto);
            await db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Puesto puesto)
        {
            Puesto? p = db.Find<Puesto>(puesto.Id);
            if (p != null)
            {
                p.Nombre = puesto.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Puesto puesto)
        {
            db.Puestos.Remove(puesto);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Puesto? puesto = GetById(id);
            if (puesto != null)
            {
                db.Remove(puesto);
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
                    Puesto? puesto = GetById(int.Parse(id));
                    if (puesto != null)
                    {
                        db.Remove(puesto);
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

		public async Task<List<Puesto>> GetAllAsync()
        {
            return await db.Puestos.ToListAsync();
        }

        public Puesto? GetById(int id)
        {
            return db.Puestos.Where(p => p.Id == id).FirstOrDefault();
        }

    }
}
