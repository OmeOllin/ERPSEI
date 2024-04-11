using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ERPSEI.Data.Managers.SAT
{
    public class ProductoServicioManager : IProductoServicioManager
    {
        ApplicationDbContext db { get; set; }

        public ProductoServicioManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        private async Task<int> getNextId()
        {
            List<ProductoServicio> registros = await db.ProductosServicios.ToListAsync();
            ProductoServicio? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ProductoServicio p)
        {
            p.Id = await getNextId();
            db.ProductosServicios.Add(p);
            await db.SaveChangesAsync();
            return p.Id;
        }
        public async Task UpdateAsync(ProductoServicio p)
        {
            ProductoServicio? a = db.Find<ProductoServicio>(p.Id);
            if (a != null)
            {
                a.Clave = p.Clave;
                a.PalabrasSimilares = p.PalabrasSimilares;
                a.IncluirIVATraslado = p.IncluirIVATraslado;
                a.IncluirIEPSTraslado = p.IncluirIEPSTraslado;
                a.Descripcion = p.Descripcion;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ProductoServicio p)
        {
            db.ProductosServicios.Remove(p);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ProductoServicio? p = await GetByIdAsync(id);
            if (p != null)
            {
                db.Remove(p);
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
                    ProductoServicio? p = await GetByIdAsync(int.Parse(id));
                    if (p != null)
                    {
                        db.Remove(p);
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

        public async Task<List<ProductoServicio>> GetAllAsync()
        {
            return await db.ProductosServicios.ToListAsync();
        }

        public async Task<List<ProductoServicioBuscado>> SearchProductService(string texto)
        {
            string sql = $"SELECT TOP (20) Id, Clave, Descripcion, IncluirIVATraslado, IncluirIEPSTraslado, PalabrasSimilares FROM ProductosServicios WHERE Descripcion LIKE '%{texto}%' OR Clave LIKE '%{texto}%' OR PalabrasSimilares LIKE '%{texto}%'";
            List<ProductoServicioBuscado> prodserv = await db.Database.SqlQueryRaw<ProductoServicioBuscado>(sql).ToListAsync();

            return prodserv;
        }

        public async Task<ProductoServicio?> GetByIdAsync(int id)
        {
            return await db.ProductosServicios.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProductoServicio?> GetByNameAsync(string name)
        {
            return await db.ProductosServicios.Where(a => a.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
