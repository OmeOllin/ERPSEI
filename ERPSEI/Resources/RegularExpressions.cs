namespace ERPSEI.Resources
{
	public static class RegularExpressions
	{
		public const string PersonName = "";

		public const string AlphanumSpace = @"^[áÁéÉíÍóÓúÚñÑ\w ]+$";

		public const string AlphanumNoSpace = @"^[áÁéÉíÍóÓúÚñÑ\w]+$";

		public const string Numeric = @"^[1-9][\d]+$";
	}
}
