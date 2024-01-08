using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Text;

namespace ERPSEI.Data.Entities
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(
            IUserStore<AppUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<AppUser> passwordHasher,
            IEnumerable<IUserValidator<AppUser>> userValidators,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<AppUserManager> logger) :
            base(store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
        }

		//Genera un password aleatorio del tamaño definidio (opcional)
		public string GenerateRandomPassword(int size = 6)
		{
			if(size <= 0) { size = 6; }

			size -= 4;

			int half = size / 2;

			StringBuilder builder = new StringBuilder();
			builder.Append(RandomString(half, false));
			builder.Append(RandomNumber(1000, 9999));
			builder.Append(RandomString(half - 1, true));
			builder.Append(RandomSymbol());
			return builder.ToString();
		}

		//Genera un número aleatorio de un rango dado.
        private string RandomNumber(int min, int max)
        {
			// Generate a random number
			Random random = new Random();

			// Any random integer
			int num = random.Next(min, max);

            return num.ToString();
		}

		//Genera un string aleatorio con el tamaño y el case dados.
		//Si el segundo parámetro es verdadero, el string resultante será lowercase.
		private string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}
			if (lowerCase)
				return builder.ToString().ToLower();
			return builder.ToString();
		}

		private string RandomSymbol()
		{
			// Caracteres especiales permitidos
			string validChars = "!@#$%&*?";
			Random random = new Random();

			//Selecciona un caracter especial aleatorio
			char[] chars = new char[1];
			for (int i = 0; i < 1; i++)
			{
				chars[i] = validChars[random.Next(0, validChars.Length)];
			}
			return new string(chars);
		}



	}
}
