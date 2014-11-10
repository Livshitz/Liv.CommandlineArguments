using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liv.CommandlineArguments
{
    [AttributeUsage(AttributeTargets.All)]
    public class OptionAttribute : Attribute
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public string DefaultValue { get; set; }
        public string DefaultValueExtend { get; set; }
        public bool IsRequired { get; set; }
    }

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