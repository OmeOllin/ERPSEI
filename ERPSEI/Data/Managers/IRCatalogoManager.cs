namespace ERPSEI.Data.Managers
{
	public interface IRCatalogoManager<T>
	{
		public Task<List<T>> GetAllAsync();

		T? GetById(int id);
	}
}
