namespace Liv.CommandlineArguments
{
	public class BaseOptionsClass
	{
		public string[] OriginalParameters { get; set; }
		internal OptionAttribute[] Options { get; set; }

		public string PrintArguments(bool writeToConsole = true)
		{
			return ConsoleOptions.PrintArguments(this, writeToConsole);
		}
	}
}