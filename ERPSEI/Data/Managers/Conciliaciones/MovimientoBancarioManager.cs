using ERPSEI.Data.Entities.Conciliaciones;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class MovimientoBancarioManager(ApplicationDbContext db) : IMovimientoBancarioManager
    {
        private async Task<int> GetNextId()
        {
            List<MovimientoBancario> MovimientoBancario = await db.MovimientosBancarios.ToListAsync();
            MovimientoBancario? last = MovimientoBancario.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(MovimientoBancario movimientoBancario)
        {
            movimientoBancario.Id = await GetNextId();
            db.MovimientosBancarios.Add(movimientoBancario);
            await db.SaveChangesAsync();
            return movimientoBancario.Id;
        }
        public async Task UpdateAsync(MovimientoBancario movimientoBancario)
        {
            MovimientoBancario? a = db.Find<MovimientoBancario>(movimientoBancario.Id);
            if (a != null)
            {
                a.Fecha = movimientoBancario.Fecha;
                a.Descripcion = movimientoBancario.Descripcion;
                a.Importe = movimientoBancario.Importe;
                a.Conciliado = movimientoBancario.Conciliado;
                //a.Conciliacion = movimientoBancario.Conciliacion;
                //a.ConciliacionDetalleMovimiento = movimientoBancario.ConciliacionDetalleMovimiento;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(MovimientoBancario movimientoBancario)
        {
            db.MovimientosBancarios.Remove(movimientoBancario);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            MovimientoBancario? movimientoBancario = await GetByIdAsync(id);
            if (movimientoBancario != null)
            {
                db.Remove(movimientoBancario);
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
                    MovimientoBancario? movimientoBancario = await GetByIdAsync(int.Parse(id));
                    if (movimientoBancario != null)
                    {
                        db.Remove(movimientoBancario);
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

        public async Task<List<MovimientoBancario>> GetAllAsync()
        {
            return await db.MovimientosBancarios.ToListAsync();
        }

        public async Task<MovimientoBancario?> GetByIdAsync(int id)
        {
            return await db.MovimientosBancarios.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        //Verificar por el nombre que no existe en la entidad
        public async Task<MovimientoBancario?> GetByNameAsync(string name)
        {
            return await db.MovimientosBancarios.Where(a => a.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
    }
}
