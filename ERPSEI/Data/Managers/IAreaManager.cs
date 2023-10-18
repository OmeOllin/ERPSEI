using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IAreaManager
    {
        public Task CreateAsync(Area area);

        public Task UpdateAsync(Area area);

        public Task DeleteAsync(Area area);

        public Task DeleteByIdAsync(int id);

		public Task<List<Area>> GetAllAsync();

		Area? GetById(int id);
    }
}
