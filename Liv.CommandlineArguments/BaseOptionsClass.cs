namespace Liv.CommandlineArguments
{
	public class BaseOptionsClass
	{
		public OptionAttribute[] Options { get; set; }

		public string PrintArguments()
		{
			return ConsoleOptions.PrintArguments(this);
		}
	}
}