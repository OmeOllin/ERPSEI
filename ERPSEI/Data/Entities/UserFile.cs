using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities
{
    public class UserFile
    {
        public string Id { get; set; } = string.Empty;

        public String UserId { get; set; } = String.Empty;

        public AppUser User {  get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Document { get; set; } = new byte[0];
    }
}
