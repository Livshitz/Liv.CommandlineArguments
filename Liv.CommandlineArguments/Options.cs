using Liv.CommandlineArguments;

[OptionsClass]
public class OptionsDefinition : BaseOptionsClass
{
	[Option(Description = "Required value", DefaultValue = "myValue", IsRequired = true, ShortName = "o")]
	public string OptionReqStr { get; set; }
	[Option(Description = "integer option")]
	public int OptionInt { get; set; }
	[Option(DefaultValue = "false", NoShortName = true, Description = "boolean option")]
	public bool BoolTest { get; set; }
}

/*
Use the options:

// Initialize your options: Parse and convert arguments to your OptionsClass
// Second argument: Check arguments for help request (e.g: "-?", "--help", etc.):
Options = ConsoleOptions.Init<OptionsDefinition>(args, true);

Options.PrintArguments(); // Trace out the inputed arguments, as they are in OptionsClass as properties

	
Console.WriteLine("Got arguments: {0}", String.Join(" ", args));
Console.WriteLine();
*/
