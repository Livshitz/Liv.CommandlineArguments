//css_import Modules/ConvertionHelper.cs

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
//using CSScriptLibrary;
//using csscript;

namespace Liv.CommandlineArguments
{
    public class ArgsManager<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        private string[] _Args = null;
        private OptionEx[] _Values = new OptionEx[] { };

        public ArgsManager(string[] args, bool isRequireArgs = false)
        {
            try
            {
                if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

                var _Values = PrepareValuesList(); ;
                SetArgsNames(ref _Values);

                if (!typeof (T).IsEnum) throw new Exception("ArgsManager:CTOR: Argument enumType is not an enum!");

                _Args = args;

                if (_Args == null) _Args = new string[] {};

                if (IsRequestHelp(isRequireArgs))
                {
                    WriteHelp(_Args, _Values);
                    System.Environment.Exit(-1);
                }

                SetValues(_Values);

                WriteHelp(_Args, _Values);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ArgsManager: Unable to initialize arguments!", ex);
                System.Environment.Exit(-1);
            }
        }

        private OptionEx[] PrepareValuesList()
        {
            var opts = new List<OptionEx>();
            var type = typeof(T);
            var enumValues = Enum.GetValues(typeof(T));
            foreach (var e in enumValues)
            {
                var memberInfo = type.GetMember(e.ToString())[0];
                var newOpt = new OptionEx(memberInfo.GetCustomAttributes(typeof(OptionAttribute), true)[0] as OptionAttribute);

                newOpt.Name = memberInfo.Name;
                opts.Add(newOpt);
            }
            return opts.ToArray();
        }

        private void SetArgsNames(ref OptionEx[] values)
        {
            foreach (var valueItem in values)
            {
                string name = valueItem.Name.ToString();

                valueItem.LongName = name;

                if (valueItem.ShortName != null) continue;

                // capitals
                string capitals = "";
                foreach (var _char in name)
                {
                    if (char.IsUpper(_char)) capitals += _char;
                }
                valueItem.ShortName = capitals.ToLower();


                bool isCapitalUsed = false;
                foreach (var valueItem2 in values)
                {
                    if (valueItem2 == valueItem) continue;
                    if (valueItem2.ShortName == valueItem.ShortName)
                    {
                        isCapitalUsed = true;
                        break;
                    }
                }
                if (!isCapitalUsed) continue;

                // letters
                for (int i = 1; i < valueItem.LongName.Length - 1; i++)
                {
                    valueItem.ShortName = valueItem.LongName.Substring(0, i);
                    bool isUsed = false;
                    foreach (var valueItem2 in values)
                    {
                        if (valueItem2 == valueItem) continue;
                        if (valueItem2.ShortName == valueItem.ShortName)
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    if (!isUsed) break;
                }

            }
        }

        private void SetValuesList()
        {
            //System.Array enumValues = System.Enum.GetValues(typeof (T));
            //MemberInfo[] memberInfos = typeof (T).GetMembers(BindingFlags.Public | BindingFlags.Static);
            //string alerta = "";
            //for (int i = 0; i < memberInfos.Length; i++)
            //{
            //    var enumMemberIndex = (int) enumValues.GetValue(i);
            //    var enumMemberName = memberInfos[i].Name;
            //    _Values.Add((T) Enum.Parse(typeof (T), enumMemberName), _Args[enumMemberIndex]);
            //}
        }

        private void SetValues(OptionEx[] values)
        {
            _Values = values;

            foreach (var valueItem in _Values)
            {
                valueItem.Value = valueItem.DefaultValue ?? valueItem.DefaultValueExtend;
                if (valueItem.Type == typeof (bool) && valueItem.Value == null) valueItem.Value = "false";
            }

            string cmdArgs = "";
            for (int i = 0; i < _Args.Length; i++)
            {
                if (!Regex.IsMatch(_Args[i], @"^\s*\-")) cmdArgs += " \"" + _Args[i] + "\"";
                else cmdArgs += " " + _Args[i];
            }
            //string cmdArgs = String.Join(" ", _Args); //Environment.GetCommandLineArgs());

            var re =
                new Regex(
                    "(?:-{1,2}|\\/)(?<name>\\w+)(?:[=:]?|\\s+)(?<value>[^-\\s\"][^\"]*?|\"[^\"]*\")?(?=\\s+[-\\/]|$)",
                    RegexOptions.IgnoreCase);
            var x = re.Matches(cmdArgs);

            foreach (Match match in x)
            {
                string name = match.Groups["name"].ToString().ToLower();
                string value = match.Groups["value"].ToString();
                value = Regex.Replace(value, @"^\s*""|""$", "");

                bool isFound = false;
                foreach (var valueItem in _Values)
                {
                    if (valueItem.ShortName == name)
                    {
                        if (valueItem.Type == typeof (int)) value = value.Replace("(", "").Replace(")", "");
                        if (valueItem.Type == typeof (bool)) value = "true";

                        valueItem.Value = value;
                        if (!String.IsNullOrEmpty(valueItem.DefaultValueExtend))
                            valueItem.Value = value + valueItem.DefaultValueExtend;
                        isFound = true;
                        break;
                    }
                }

                if (!isFound) throw new ArgumentException("Invalid parameter \"" + name + "\", aborting");
            }

            foreach (var valueItem in _Values)
            {
                if (!valueItem.IsRequired) continue;
                if (valueItem.Value == null) throw new ArgumentException("Parameter \"" + valueItem.Name + "\" is required, aborting");
            }
            
            /*
        for (int i = 0; i < _Values.Length; i++)
        {
            bool hasPassedValue = _Args.Length > i && _Args[i] != null && _Args[i] != "";
            if (hasPassedValue)
            {
                _Values[i].Value = _Args[i];
                continue;
            }
            else
            {
                _Values[i].Value = _Values[i].DefaultValue;
            }
        }
        */
            //var tempArgs = new List<String>(_Args);
            //tempArgs.Add(defaults.ElementAt(i).Value);
            //_Args = tempArgs.ToArray();
        }

        private OptionEx FindValueItem(T ArgumentName)
        {
            return _Values.Where(x => x.Name.Equals(ArgumentName.ToString())).Single();
        }

        public string this[T ArgumentName]
        {
            get { return FindValueItem(ArgumentName).Value; }
            set { FindValueItem(ArgumentName).Value = value; }
        }

        public string GetValue(T ArgumentName)
        {
            return this[ArgumentName];
        }

        public T2 GetValue<T2>(T ArgumentName)
        {
            return FindValueItem(ArgumentName).GetValue<T2>();
        }

        private bool IsRequestHelp(bool isRequireArgs)
        {
            bool hasArgs = (_Args != null && _Args.Length >= 1);
            if (isRequireArgs && !hasArgs) return true;
            bool isHelpArgument = hasArgs &&
                                  (_Args[0] == "/?" || _Args[0] == "-?" || _Args[0] == "--?" || _Args[0] == "--help");
            if (isHelpArgument) return true;
            return false;
        }

        public string TraceArguments()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("| TraceArguments:" + Environment.NewLine);
            foreach (var valuePair in _Values)
            {
                sb.Append("|\t" + Fill(valuePair.Name, 25) + " = " + valuePair.Value + Environment.NewLine);
            }
            return sb.ToString();
        }

        private string WriteHelp(string[] args,OptionEx[] _Values)
        {
            /*
            Usage: cscs.exe <switch 1> <switch 2> <file> [params] [//x]

            <switch 1>
             /?    - Display help info.
             /e    - Compile script into console application executable.
             /ew   - Compile script into Windows application executable.
            */
            StringBuilder sb = new StringBuilder();
            sb.Append("Help:" + Environment.NewLine + Environment.NewLine);
            sb.Append("#########################" + Environment.NewLine + Environment.NewLine);
            try
            {
                //sb.Append("Usage: cscs.exe " + new FileInfo(Environment.GetEnvironmentVariable("EntryScript")).Name +
                //          " [params] " + Environment.NewLine);
                string filename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                sb.Append("Usage: " + filename + " [params] " + Environment.NewLine);
            }
            catch
            {
            }
            sb.Append("[params]:" + Environment.NewLine);
            foreach (var valueItem in _Values)
            {
                sb.Append(" -" + Fill(valueItem.ShortName, 5) + " --" + Fill(valueItem.LongName, 25) + ": " +
                          valueItem.Description);

                if (valueItem.DefaultValue != null)
                {
                    if (!String.IsNullOrEmpty(valueItem.DefaultValueExtend))
                    {
                        sb.Append(" (defaultExtended= [YourValue;]" + valueItem.DefaultValueExtend + ") ");
                    }
                    else
                    {
                        sb.Append(" (default=" +
                                  ((valueItem.Type == typeof (int))
                                      ? "(" + valueItem.DefaultValue + ")"
                                      : valueItem.DefaultValue) + ") ");
                        //+ ((!String.IsNullOrEmpty(valueItem.Description)) ?  (Environment.NewLine + "\t " + valueItem.Description) : "") 
                    }
                }
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            sb.Append("#########################" + Environment.NewLine);
            string ret = sb.ToString();
            Console.WriteLine(ret);
            return ret;
        }

        public static string Fill(string text, int maxChars = 30, char fillChar = ' ')
        {
            StringBuilder sb = new StringBuilder(text);

            if (sb.Length >= maxChars) return text;
            for (int i = text.Length - 1; i <= maxChars - sb.Length; i++)
            {
                sb.Append(fillChar);
            }

            return sb.ToString();
        }
    }
}