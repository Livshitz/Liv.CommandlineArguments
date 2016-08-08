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
			[Option(Description = "Install UrlCat.Manager as a win service (will automatically install/start)", DefaultValue = "MyBlankFile.dat", ShortName = "f", TrailingSlashFix = true)]
			public string FileName { get; set; }
			[Option(Description = "Output folder", ShortName = "d", IsRequired = true, TrailingSlashFix = true)]
			public string DestinationFolder { get; set; }
			[Option(Description = "File Size (KB)", NoShortName = true, DefaultValue = "10000")]
			public int Size { get; set; }
			[Option(ShortName = "n", DefaultValue = "2", Description = "Number of files to create. DownloadDb:Progress: Waiting for temp download file to be created. If you use a StringBuilder and know its resulting length in advance, then also use an appropriate constructor, this is much more efficient because it means that only one time-consuming allocation takes place, and no unnecessary copying of data. Nonsense: of course the above code is more efficient")]
			public int NumberOfFiles { get; set; }
			[Option(Description = "Run in loops, with interval (-1 for only one run)", ShortName = "i", DefaultValue = "-1")]
			public int Interval { get; set; }
			[Option(Description = "Insert to random subfolder", ShortName = "r", DefaultValue = "false")]
			public bool ChooseRandomFolder { get; set; }
			//[Option(DefaultValue = "false", Description = "boolean option")]
			//public bool BoolTest { get; set; }
		}

		static void Main(string[] args)
		{
			try
			{
				// Check arguments for help request (e.g: "-?", "--help", etc.):
				//ConsoleOptions.PrintHelpIfNeededAndExit<OptionsDefinition>(args);

				Console.WriteLine("Got arguments: {0}", String.Join(" ", args));
				Console.WriteLine();

				// Initialize your options: Parse and convert arguments to your OptionsClass
				Options = ConsoleOptions.Init<OptionsDefinition>(args, true);
				Options.PrintArguments();

				ConsoleOptions.SaveToDisk(Options, "test.data");
				var o = ConsoleOptions.LoadFromDisk<OptionsDefinition>("test.data");

				// Actually using the argument values is simple:
				if (Options.ChooseRandomFolder)
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
