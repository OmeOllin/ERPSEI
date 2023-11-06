using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class EmpleadoManager : IRWCatalogoManager<Empleado>
    {
        ApplicationDbContext db { get; set; }

        public EmpleadoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<Empleado> registros = await GetAllAsync();
			Empleado? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Empleado empleado)
        {
            empleado.Id = await getNextId();
            db.Empleados.Add(empleado);
            await db.SaveChangesAsync();
            return empleado.Id;
        }
        public async Task UpdateAsync(Empleado empleado)
        {
			Empleado? a = db.Find<Empleado>(empleado.Id);
            if (a != null)
            {
                //a.Nombre = empleado.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Empleado empleado)
        {
            db.Empleados.Remove(empleado);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			Empleado? empleado = GetById(id);
            if (empleado != null)
            {
                db.Remove(empleado);
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
					Empleado? empleado = GetById(int.Parse(id));
					if (empleado != null)
					{
						db.Remove(empleado);
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

		public async Task<List<Empleado>> GetAllAsync()
		{
			return await db.Empleados.ToListAsync();
		}

		public Empleado? GetById(int id)
        {
            return db.Empleados.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
