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
			[Option(DefaultValue = "123", Description = "integer option")]
			public int OptionInt { get; set; }
			[Option(DefaultValue = "false", Description = "boolean option")]
			public bool BoolTest { get; set; }
		}


        static void Main(string[] args)
        {
			try
			{
				ConsoleOptions.PrintHelpIfNeededAndExit<OptionsDefinition>(args);

				Console.WriteLine("Got arguments: {0}", String.Join(" ", args));
				Console.WriteLine();

				Options = ConsoleOptions.Init<OptionsDefinition>(args);
				Options.PrintArguments();
				if (Options.BoolTest)
				{
					Console.WriteLine("Bool test is set to true!");
				}
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine("Something went wrong while parsing options, ex:" + ex.Message);
				//ConsoleOptions.PrintHelp<OptionsDefinition>();
				return;
			}

			/*
			 * 
            Console.WriteLine(args.PrintArguments());

			var isBoolTest = args.GetValue<Boolean>(Args.BoolTest);
			Console.WriteLine("isBoolTest=" + isBoolTest);


            var x = args.GetValue<int>(Args.OptionInt);
            Console.WriteLine(x * 2);

			*/

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
