using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liv.CommandlineArguments
{
	[Obsolete]
	public class OptionEx : OptionAttribute
    {
        internal string Name { get; set; } // Will be set dynamically
        public string Value { get; set; }

        public OptionEx(OptionAttribute fromOption)
        {
            ShortName = fromOption.ShortName;
            LongName = fromOption.LongName;
            Description = fromOption.Description;
            Type = fromOption.Type;
            DefaultValue = fromOption.DefaultValue;
            DefaultValueExtend = fromOption.DefaultValueExtend;
            IsRequired = fromOption.IsRequired;
        }

        public string GetValue()
        {
            return Value;
        }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(GetValue(), typeof(T));
        }
    }
}