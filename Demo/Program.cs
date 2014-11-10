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
        }


        static void Main(string[] _args)
        {
            args = new ArgsManager<Args>(_args);

            Console.WriteLine(args.TraceArguments());

            var x = args.GetValue<int>(Args.OptionInt);
            Console.WriteLine(x * 2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
