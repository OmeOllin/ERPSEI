using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace ERPSEI.Data.Managers.Empresas
{
    public class BancoEmpresaManager : IBancoEmpresaManager
    {
        ApplicationDbContext db { get; set; }

        public BancoEmpresaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<BancoEmpresa> registros = await db.BancosEmpresa.ToListAsync();
			BancoEmpresa? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(BancoEmpresa banco)
        {
			banco.Id = await getNextId();
			db.BancosEmpresa.Add(banco);
			await db.SaveChangesAsync();
			return banco.Id;
		}
        public async Task UpdateAsync(BancoEmpresa banco)
        {
			BancoEmpresa? be = db.Find<BancoEmpresa>(banco.Id);
            if (be != null)
            {
                be.EmpresaId = banco.EmpresaId;
                be.Banco = banco.Banco;
                be.Responsable = banco.Responsable;
                be.Firmante = banco.Firmante;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(BancoEmpresa banco)
        {
            db.BancosEmpresa.Remove(banco);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			BancoEmpresa? banco = GetBancoById(id);
            if (banco != null)
            {
                db.Remove(banco);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteByEmpresaIdAsync(int id)
        {
            List<BancoEmpresa> bancos = await db.BancosEmpresa.Where(a => a.EmpresaId == id).ToListAsync();
            if (bancos != null && bancos.Count >= 1) { db.BancosEmpresa.RemoveRange(bancos); }
			await db.SaveChangesAsync();
		}

        public async Task<List<BancoEmpresa>> GetBancosByEmpresaIdAsync(int id)
        {
            return await db.BancosEmpresa.Where(b => b.EmpresaId == id).ToListAsync();
        }

        public BancoEmpresa? GetBancoById(int id)
        {
            return db.BancosEmpresa.Where(b => b.Id == id).FirstOrDefault();
        }

    }
}
