using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class ArchivoEmpleadoManager : IArchivoEmpleadoManager
    {
        ApplicationDbContext db { get; set; }

        public ArchivoEmpleadoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<string> CreateAsync(ArchivoEmpleado file)
        {
            db.ArchivosEmpleado.Add(file);
            await db.SaveChangesAsync();
            return file.Id;
        }
        public async Task UpdateAsync(ArchivoEmpleado file)
        {
            ArchivoEmpleado? uf = db.Find<ArchivoEmpleado>(file.Id);
            if (uf != null)
            {
                uf.EmpleadoId = file.EmpleadoId;
                uf.Nombre = file.Nombre;
                uf.Extension = file.Extension;
                uf.Archivo = file.Archivo;
                uf.TipoArchivoId = file.TipoArchivoId;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ArchivoEmpleado file)
        {
            db.ArchivosEmpleado.Remove(file);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string fileId)
        {
            ArchivoEmpleado? file = GetFileById(fileId);
            if (file != null)
            {
                db.Remove(file);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<ArchivoEmpleado>> GetFilesByEmpleadoIdAsync(int empleadoId)
        {

            return await db.ArchivosEmpleado.Where(uf => uf.EmpleadoId == empleadoId).ToListAsync();
        }

        public ArchivoEmpleado? GetFileById(string id)
        {
            return db.ArchivosEmpleado.Where(uf => uf.Id == id).FirstOrDefault();
        }

    }
}
