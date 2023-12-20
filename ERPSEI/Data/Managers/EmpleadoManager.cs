using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class EmpleadoManager : IEmpleadoManager
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
			Empleado? empleado = await GetByIdAsync(id);
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
					Empleado? empleado = await GetByIdAsync(int.Parse(id));
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
				throw;

			}
		}

		public async Task<List<Empleado>> GetAllAsync()
		{
			return await db.Empleados
				.Include(e => e.Oficina)
				.Include(e => e.Puesto)
				.Include(e => e.Area)
				.Include(e => e.Subarea)
				.Include(e => e.Genero)
				.Include(e => e.EstadoCivil)
				.Include(e => e.ContactosEmergencia)
				.Include(e => e.ArchivosEmpleado)
				.ToListAsync();
		}

		public async Task<Empleado?> GetEmpleadoOrganigramaAsync(int id)
		{
			return await db.Empleados
				.Where(e => e.Id == id)
				.Include(e => e.Oficina)
				.Include(e => e.Puesto)
				.Include(e => e.Area)
				.Include(e => e.Subarea)
				.Include(e => e.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil))
				.FirstOrDefaultAsync();
		}

		public async Task<List<Empleado>> GetEmpleadosOrganigramaAsync(int? jefeId, int? areaId, int? subareaId)
		{
			return await db.Empleados
				.Where(e => jefeId.HasValue ? e.JefeId == jefeId : true)
				.Where(e => areaId.HasValue ? e.AreaId == areaId : true)
				.Where(e => subareaId.HasValue ? e.SubareaId == subareaId : true)
				.Include(e => e.Oficina)
				.Include(e => e.Puesto)
				.Include(e => e.Area)
				.Include(e => e.Subarea)
				.Include(e => e.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil))
				.ToListAsync();
		}

		public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await db.Empleados.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Empleado?> GetByCURPAsync(string curp)
		{
			return await db.Empleados.Where(e => e.CURP == curp).FirstOrDefaultAsync();
		}

		public async Task<Empleado?> GetByNameAsync(string name)
		{
			return await db.Empleados.Where(e => e.NombreCompleto.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
