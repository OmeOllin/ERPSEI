using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT.Catalogos;
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
			List<UnidadMedida> unidades = await db.UnidadesMedida
				.Where(u => u.Deshabilitado == 0)
				.Where(u => u.Clave.Contains(texto) || u.Nombre.Contains(texto)|| u.Simbolo.Contains(texto))
				.Take(20)
				.ToListAsync();

			return unidades;
		}

	}
}
