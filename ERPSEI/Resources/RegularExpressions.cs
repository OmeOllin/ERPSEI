namespace ERPSEI.Resources
{
	public static class RegularExpressions
	{
		public const string PersonName = @"^[A-ZÁÉÍÓÚÑ][a-zA-ZáÁéÉíÍóÓúÚñÑ ]+$";

		public const string AlphanumSpace = @"^[áÁéÉíÍóÓúÚñÑ\w ]+$";

		public const string AlphanumNoSpace = @"^[áÁéÉíÍóÓúÚñÑ\w]+$";

		public const string Numeric = @"^[1-9][\d]+$";
	}
}
