using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class UnidadMedidaManager : IUnidadMedidaManager
	{
        ApplicationDbContext db { get; set; }

        public UnidadMedidaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<UnidadMedida>> GetAllAsync()
		{
			return await db.UnidadesMedida.ToListAsync();
		}

		public async Task<UnidadMedida?> GetByIdAsync(int id)
        {
            return await db.UnidadesMedida.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

		public async Task<UnidadMedida?> GetByNameAsync(string name)
		{
			return await db.UnidadesMedida.Where(u => u.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

		public async Task<List<UnidadMedida>> SearchUnidades(string texto)
		{
			string sql = $"SELECT TOP (20) *" +
						 $"FROM UnidadesMedida " +
						 $"WHERE Clave LIKE '%{texto}%' OR Nombre LIKE '%{texto}%' OR Simbolo LIKE '%{texto}%' AND Deshabilitado = 0";
			List<UnidadMedida> emp = await db.Database.SqlQueryRaw<UnidadMedida>(sql).ToListAsync();

			return emp;
		}

	}
}
