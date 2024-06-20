using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;
using static ERPSEI.Areas.Catalogos.Pages.AsistenciaModel;

namespace ERPSEI.Data.Managers.Empleados
{
	public class AsistenciaManager: IAsistenciaManager
	{
		ApplicationDbContext db { get; set; }
		public AsistenciaManager(ApplicationDbContext _db)
		{
			db = _db;
		}

		public async Task<int> CreateAsync(Entities.Empleados.Asistencia asist)
		{
			asist.Id = await getNextId();
			db.Asistencias.Add(asist);
			await db.SaveChangesAsync();
			return asist.Id;
		}
		public async Task UpdateAsync(Entities.Empleados.Asistencia asist)
		{
			Entities.Empleados.Asistencia? sa = db.Find<Entities.Empleados.Asistencia>(asist.Id);
			if (sa != null)
			{
				sa.NombreEmpleado = asist.NombreEmpleado;
				await db.SaveChangesAsync();
			}
		}
		private async Task<int> getNextId()
		{
			List<Entities.Empleados.Asistencia> registros = await db.Asistencias.ToListAsync();
			Entities.Empleados.Asistencia? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task DeleteAsync(Entities.Empleados.Asistencia asist)
		{
			db.Asistencias.Remove(asist);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Entities.Empleados.Asistencia? asist = await GetByIdAsync(id);
			if (asist != null)
			{
				db.Remove(asist);
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
					Entities.Empleados.Asistencia? asist = await GetByIdAsync(int.Parse(id));
					if (asist != null)
					{
						db.Remove(asist);
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

		public async Task<List<Entities.Empleados.Asistencia>> GetAllAsync()
		{
			return await db.Asistencias.ToListAsync();
		}

		public async Task<Entities.Empleados.Asistencia?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Entities.Empleados.Asistencia?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => a.NombreEmpleado.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}
	}
}
