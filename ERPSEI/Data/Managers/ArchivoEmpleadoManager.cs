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
            if (file.Id.Length <= 0) { file.Id = Guid.NewGuid().ToString(); }
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

		public async Task DeleteByEmpleadoIdAsync(int empleadoId)
		{
			List<ArchivoEmpleado> archivos = await db.ArchivosEmpleado.Where(a => a.EmpleadoId == empleadoId).ToListAsync();
			if (archivos != null && archivos.Count >= 1) { db.ArchivosEmpleado.RemoveRange(archivos); }
		}

        public async Task<ProfilePicture?> GetProfilePicByEmpleadoId(int empleadoId)
        {
            FormattableString sql = $"SELECT Id, Nombre, Extension, Archivo FROM ArchivosEmpleado WHERE EmpleadoId = {empleadoId} AND TipoArchivoId = {FileTypes.ImagenPerfil}";
            var resp = await db.Database.SqlQuery<ProfilePicture>(sql).FirstOrDefaultAsync();
            return resp;
        }

		public async Task<List<SemiArchivoEmpleado>> GetFilesByEmpleadoIdAsync(int empleadoId)
        {
            FormattableString sql = $"SELECT Id, Nombre, Extension, 0x AS Archivo, DATALENGTH(Archivo) AS FileSize, TipoArchivoId, EmpleadoId FROM ArchivosEmpleado WHERE EmpleadoId = {empleadoId}";
            var resp = await db.Database.SqlQuery<SemiArchivoEmpleado>(sql).ToListAsync();
            return resp;
        }

        public ArchivoEmpleado? GetFileById(string id)
        {
            return db.ArchivosEmpleado.Where(uf => uf.Id == id).FirstOrDefault();
        }

    }
}
