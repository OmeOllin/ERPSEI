using ERPSEI.Data.Entities.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Usuarios
{
	public class AccesoModuloManager : IAccesoModuloManager
	{
        ApplicationDbContext db { get; set; }

        public AccesoModuloManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<AccesoModulo>> GetAllAsync()
		{
			return await db.AccesosModulos.ToListAsync();
		}

		public async Task<AccesoModulo?> GetByIdAsync(int id)
        {
            return await db.AccesosModulos.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

		public Task<AccesoModulo?> GetByNameAsync(string name)
		{
			//Un acceso a módulo no tiene nombre, por ello este método siempre devuelve null.
			return Task.FromResult<AccesoModulo?>(null);
		}

		public async Task<List<AccesoModulo>> GetByRolIdAsync(string idRol)
		{
			return await db.AccesosModulos.Include(a => a.Modulo).Where(a => a.RolId == idRol).ToListAsync();
		}

		public async Task<int> CreateAsync(AccesoModulo a)
		{
			db.AccesosModulos.Add(a);
			await db.SaveChangesAsync();
			return a.Id;
		}
		public async Task UpdateAsync(AccesoModulo a)
		{
			AccesoModulo? n = db.Find<AccesoModulo>(a.Id);
			if (n != null)
			{
				n.PuedeConsultar = a.PuedeConsultar;
				n.PuedeEditar = a.PuedeEditar;
				n.PuedeEliminar = a.PuedeEliminar;
				n.ModuloId = a.ModuloId;
				n.RolId = a.RolId;
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(AccesoModulo a)
		{
			db.AccesosModulos.Remove(a);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			AccesoModulo? a = await GetByIdAsync(id);
			if (a != null)
			{
				db.Remove(a);
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
					AccesoModulo? a = await GetByIdAsync(int.Parse(id));
					if (a != null)
					{
						db.Remove(a);
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
	}
}
