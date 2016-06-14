using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liv.CommandlineArguments;

namespace Demo
{
	class Program
	{
		private static OptionsDefinition Options;

		[OptionsClass]
		public class OptionsDefinition : BaseOptionsClass
		{
			[Option(DefaultValue = "myValue", IsRequired = true, ShortName = "o", Description = "Required value")]
			public string OptionReqStr { get; set; }
			[Option(Description = "integer option")]
			public int OptionInt { get; set; }
			[Option(DefaultValue = "false", Description = "boolean option")]
			public bool BoolTest { get; set; }
		}

		static void Main(string[] args)
		{
			try
			{
				// Check arguments for help request (e.g: "-?", "--help", etc.):
				ConsoleOptions.PrintHelpIfNeededAndExit<OptionsDefinition>(args);

				Console.WriteLine("Got arguments: {0}", String.Join(" ", args));
				Console.WriteLine();

				// Initialize your options: Parse and convert arguments to your OptionsClass
				Options = ConsoleOptions.Init<OptionsDefinition>(args);
				Options.PrintArguments();

				// Actually using the argument values is simple:
				if (Options.BoolTest)
				{
					Console.WriteLine("Bool test is set to true!");
				}
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine("Something went wrong while parsing options, ex:" + ex.Message);
				//ConsoleOptions.PrintHelp<OptionsDefinition>(); // Print help on error
				return;
			}

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}
