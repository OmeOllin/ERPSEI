namespace ERPSEI.Data.Managers
{
	public interface IRCatalogoManager<T>
	{
		public Task<List<T>> GetAllAsync();

		public Task<T?> GetByIdAsync(int id);

		public Task<T?> GetByNameAsync(string name);
	}
}
