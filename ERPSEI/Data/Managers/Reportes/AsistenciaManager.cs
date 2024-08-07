using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
    public class AsistenciaManager(ApplicationDbContext db) : IAsistenciaManager
	{
		private async Task<int> GetNextId()
		{
			List<Asistencia> registros = await db.Asistencias.ToListAsync();
			Asistencia? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Asistencia asistencia)
		{
			asistencia.Id = await GetNextId();
			db.Asistencias.Add(asistencia);
			await db.SaveChangesAsync();
			return asistencia.Id;
		}
		public async Task UpdateAsync(Asistencia asistencia)
		{
			Asistencia? a = db.Find<Asistencia>(asistencia.Id);
			if (a != null)
			{
				a.EmpleadoId = asistencia.EmpleadoId;
				a.Salida = asistencia.Salida;
				a.Entrada = asistencia.Entrada;
				a.ResultadoE = asistencia.ResultadoE;
				a.ResultadoS = asistencia.ResultadoS;
				a.HorarioId = asistencia.HorarioId;
				a.Dia = asistencia.Dia;
				a.Fecha = asistencia.Fecha;
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Asistencia asistencia)
		{
			db.Asistencias.Remove(asistencia);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Asistencia? asistencia = await GetByIdAsync(id);
			if (asistencia != null)
			{
				db.Remove(asistencia);
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
					Asistencia? asistencia = await GetByIdAsync(int.Parse(id));
					if (asistencia != null)
					{
						db.Remove(asistencia);
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

		public async Task<List<Asistencia>> GetAllAsync()
		{
			return await db.Asistencias.Include(h => h.Horario).Include(e => e.Empleado).ToListAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => false).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(string id)
		{
			return await GetByIdAsync(id.ToString());
		}
	}
}
