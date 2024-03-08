using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empresas
{
	public class ProductoServicioPerfilManager : IProductoServicioPerfilManager
    {
        ApplicationDbContext db { get; set; }

        public ProductoServicioPerfilManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<ProductoServicioPerfil> registros = await db.ProductosServiciosPerfil.ToListAsync();
			ProductoServicioPerfil? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(ProductoServicioPerfil p)
        {
            p.Id = await getNextId();
            db.ProductosServiciosPerfil.Add(p);
            await db.SaveChangesAsync();
            return p.Id;
        }
        public async Task UpdateAsync(ProductoServicioPerfil p)
        {
			ProductoServicioPerfil? n = db.Find<ProductoServicioPerfil>(p.Id);
            if (n != null)
            {
                n.PerfilId = p.PerfilId;
				n.ProductoServicioId = p.ProductoServicioId;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ProductoServicioPerfil p)
        {
            db.ProductosServiciosPerfil.Remove(p);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			ProductoServicioPerfil? a = await GetByIdAsync(id);
            if (a != null)
            {
                db.Remove(a);
                await db.SaveChangesAsync();
            }
        }

		public async Task DeleteByPerfilIdAsync(int id)
		{
			List<ProductoServicioPerfil> productosServicios = await db.ProductosServiciosPerfil.Where(a => a.PerfilId == id).ToListAsync();
			if (productosServicios != null && productosServicios.Count >= 1) { db.ProductosServiciosPerfil.RemoveRange(productosServicios); }
			await db.SaveChangesAsync();
		}

		public async Task DeleteMultipleByIdAsync(string[] ids)
		{
			//Inicia una transacción.
			await db.Database.BeginTransactionAsync();
			try
			{
				foreach (string id in ids)
				{
					ProductoServicioPerfil? a = await GetByIdAsync(int.Parse(id));
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

		public async Task<List<ProductoServicioPerfil>> GetAllAsync()
		{
			return await db.ProductosServiciosPerfil.ToListAsync();
		}

		public async Task<ProductoServicioPerfil?> GetByIdAsync(int id)
        {
            return await db.ProductosServiciosPerfil.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

		public async Task<ProductoServicioPerfil?> GetByNameAsync(string name)
		{
			return await db.ProductosServiciosPerfil
				.Include(p => p.ProductoServicio)
				.Where(p => p.ProductoServicio != null && p.ProductoServicio.Descripcion == name).FirstOrDefaultAsync();
		}

	}
}
