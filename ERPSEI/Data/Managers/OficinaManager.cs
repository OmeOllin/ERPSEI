using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class OficinaManager : IRWCatalogoManager<Oficina>
    {
        ApplicationDbContext db { get; set; }

        public OficinaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task CreateAsync(Oficina oficina)
        {
            db.Oficinas.Add(oficina);
            await db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Oficina oficina)
        {
            Puesto? p = db.Find<Puesto>(oficina.Id);
            if (p != null)
            {
                p.Nombre = oficina.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Oficina oficina)
        {
            db.Oficinas.Remove(oficina);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Oficina? oficina = GetById(id);
            if (oficina != null)
            {
                db.Remove(oficina);
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
                    Oficina? oficina = GetById(int.Parse(id));
                    if (oficina != null)
                    {
                        db.Remove(oficina);
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

		public async Task<List<Oficina>> GetAllAsync()
        {
            return await db.Oficinas.ToListAsync();
        }

        public Oficina? GetById(int id)
        {
            return db.Oficinas.Where(p => p.Id == id).FirstOrDefault();
        }

    }
}
