using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace ERPSEI.Data.Entities
{
    public class UserFile
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] File { get; set; } = new byte[0];

        public int FileTypeId { get; set; }

        public UserFile()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
