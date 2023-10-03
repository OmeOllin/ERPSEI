namespace ERPSEI.Data.Entities
{
    public interface IUserFileManager
    {

        public Task CreateAsync(UserFile file);

        public Task UpdateAsync(UserFile file);

        public Task DeleteAsync(UserFile file);

        public Task<List<UserFile>> GetFilesByUserIdAsync(string userId);

		UserFile GetFileById(string id);
	}
}