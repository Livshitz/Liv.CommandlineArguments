using System.Linq;

namespace Liv.CommandlineArguments
{
	public class BaseOptionsClass
	{
		public string[] OriginalParameters { get; set; }
		internal OptionAttribute[] Options { get; set; }

	    internal OptionAttribute GetOptionByName(string optionLongName)
	    {
	        return Options.SingleOrDefault(x => x.LongName == optionLongName);
	    }

	    public string GetDefaultValue(string optionLongName)
	    {
	        return GetOptionByName(optionLongName).DefaultValue;
	    }

		public string PrintArguments(bool writeToConsole = true)
		{
			return ConsoleOptions.PrintArguments(this, writeToConsole);
		}
	}
}