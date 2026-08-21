namespace Casino
{
	public static class Settings
	{
		static bool showRules = true;

		public static bool IsShowRulesEnabled() =>
		showRules;

		public static void ToggleRules() =>
		showRules = !showRules;
	}
}