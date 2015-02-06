using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liv.CommandlineArguments;

namespace Demo
{
    class Program
    {
        private static ArgsManager<Args> args;
        public enum Args
        {
            [Option(DefaultValue = "myValue", IsRequired = true, ShortName = "o", Description = "Required value")]
            OptionReqStr,
            [Option(DefaultValue = "123", Type = typeof(int), Description = "integer option")]
            OptionInt,
			[Option(DefaultValue = "false", Type = typeof(Boolean), Description = "boolean option")]
			BoolTest,
        }


        static void Main(string[] _args)
        {
			try
			{
				args = new ArgsManager<Args>(_args);
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine("Something went wrong while parsing input, ex:" + ex.Message);
				return;
			}

            Console.WriteLine(args.TraceArguments());

			var isBoolTest = args.GetValue<Boolean>(Args.BoolTest);
			Console.WriteLine("isBoolTest=" + isBoolTest);


            var x = args.GetValue<int>(Args.OptionInt);
            Console.WriteLine(x * 2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
