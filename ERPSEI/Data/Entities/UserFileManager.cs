using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Entities
{
    public class UserFileManager : IUserFileManager
    {
        ApplicationDbContext db {  get; set; }

        public UserFileManager(ApplicationDbContext _db) 
        { 
            db = _db;
        }

        public async Task CreateAsync(UserFile file)
        {
            db.UserFiles.Add(file);
            await db.SaveChangesAsync();
        }
        public async Task UpdateAsync(UserFile file)
        {
            UserFile? uf = db.Find<UserFile>(file.Id);
            if (uf != null)
            {
                uf.UserId = file.UserId;
                uf.Name = file.Name;
                uf.Extension = file.Extension;
                uf.File = file.File;
                uf.FileTypeId = file.FileTypeId;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(UserFile file)
        {
            db.UserFiles.Remove(file);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string fileId)
        {
            UserFile? file = GetFileById(fileId);
            if (file != null) 
            {  
                db.Remove(file);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<UserFile>> GetFilesByUserIdAsync(string userId)
        {
            return await db.UserFiles.Where(uf => uf.UserId == userId).ToListAsync();
        }

        public UserFile? GetFileById(string id)
        {
            return db.UserFiles.Where(uf => uf.Id == id).FirstOrDefault();
        }

	}
}
