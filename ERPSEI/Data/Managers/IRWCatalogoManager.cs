namespace ERPSEI.Data.Managers
{
	public interface IRWCatalogoManager<T>
	{
		public Task CreateAsync(T element);

		public Task UpdateAsync(T element);

		public Task DeleteAsync(T element);

		public Task DeleteByIdAsync(int id);

		public Task DeleteMultipleByIdAsync(string[] ids);

		public Task<List<T>> GetAllAsync();

		T? GetById(int id);
	}
}
