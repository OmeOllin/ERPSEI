using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empresas
{
    public class ArchivoEmpresaManager : IArchivoEmpresaManager
    {
        ApplicationDbContext db { get; set; }

        public ArchivoEmpresaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<string> CreateAsync(ArchivoEmpresa file)
        {
            if (file.Id.Length <= 0) { file.Id = Guid.NewGuid().ToString(); }
            db.ArchivosEmpresa.Add(file);
            await db.SaveChangesAsync();
            return file.Id;
        }
        public async Task UpdateAsync(ArchivoEmpresa file)
        {
			ArchivoEmpresa? uf = db.Find<ArchivoEmpresa>(file.Id);
            if (uf != null)
            {
                uf.EmpresaId = file.EmpresaId;
                uf.Nombre = file.Nombre;
                uf.Extension = file.Extension;
                uf.Archivo = file.Archivo;
                uf.TipoArchivoId = file.TipoArchivoId;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ArchivoEmpresa file)
        {
            db.ArchivosEmpresa.Remove(file);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string fileId)
        {
			ArchivoEmpresa? file = GetFileById(fileId);
            if (file != null)
            {
                db.Remove(file);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteByEmpresaIdAsync(int eId)
        {
            List<ArchivoEmpresa> archivos = await db.ArchivosEmpresa.Where(a => a.EmpresaId == eId).ToListAsync();
            if (archivos != null && archivos.Count >= 1) { db.ArchivosEmpresa.RemoveRange(archivos); }
			await db.SaveChangesAsync();
		}

        public async Task<List<SemiArchivoEmpresa>> GetFilesByEmpresaIdAsync(int id)
        {
            FormattableString sql = $"SELECT Id, Nombre, Extension, 0x AS Archivo, DATALENGTH(Archivo) AS FileSize, TipoArchivoId, EmpresaId FROM ArchivosEmpresa WHERE EmpresaId = {id}";
            var resp = await db.Database.SqlQuery<SemiArchivoEmpresa>(sql).ToListAsync();
            return resp;
        }

        public ArchivoEmpresa? GetFileById(string id)
        {
            return db.ArchivosEmpresa.Where(uf => uf.Id == id).FirstOrDefault();
        }

    }
}
