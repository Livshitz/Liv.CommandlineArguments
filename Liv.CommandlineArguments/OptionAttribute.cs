using System;
using System.Reflection;

namespace Liv.CommandlineArguments
{
	[AttributeUsage(AttributeTargets.All)]
	public class OptionAttribute : Attribute
	{
		public string ShortName { get; set; }
		public bool NoShortName { get; set; }
		public string LongName { get; set; }
		public string Description { get; set; }
		public Type Type { get; set; }
		public string DefaultValue { get; set; }
		public string DefaultValueExtend { get; set; }
		public bool IsRequired { get; set; }

		public PropertyInfo AssignedProperty { get; set; }
		public string Value { get; set; }
	}
}