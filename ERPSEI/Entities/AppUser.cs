using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Entities
{
    public class AppUser : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string FathersLastName {  get; set; } = string.Empty;

        public string MothersLastName { get; set; } = string.Empty;

        public byte[] ProfilePicture { get; set; } = new byte[0];
 
    }
}
