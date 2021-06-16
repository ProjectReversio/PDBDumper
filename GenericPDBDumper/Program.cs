using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PDBInfo;

namespace GenericPDBDumper
{
	static class Program
	{
		private static string[] _fileWhitelist;
		private static string[] _whiteList;
		private static string[] _blackList;
		private static string[] _sourceExtensions;

		private static string _stripBaseDir = "";

		private static string _inputFile;
		private static string _outputFile;

		private static string[] _programArguments;
		private static int _argIndex = -1;

		private struct ArgumentOption
		{
			public string ID { get; set; }
			public string Name { get; set; }
			public string ShortName { get; set; }
			public bool Required { get; set; }
			public string Usage { get; set; }
			public bool NeedsValue { get; set; }
			public string DefaultValue { get; set; }
		}

		private static List<ArgumentOption> _argumentOptions = new List<ArgumentOption>();

		private static void AddArgument(string id, string name, string shortName, bool required = true, string usage = null, bool needsValue = true, string defaultValue = null)
		{
			_argumentOptions.Add(new ArgumentOption()
			{
				ID = id,
				Name = name,
				ShortName = shortName,
				Required = required,
				Usage = usage,
				NeedsValue = needsValue,
				DefaultValue = needsValue ? defaultValue : ""
			});
		}

		private static bool ParseArg(string arg, Dictionary<ArgumentOption, string> result)
		{
			bool shortHand;
			if (arg.StartsWith("--"))
				shortHand = false;
			else if (arg.StartsWith("-"))
				shortHand = true;
			else
				return false;

			string argName = shortHand ? arg.Substring(1) : arg.Substring(2);

			bool found = false;
			foreach (var option in _argumentOptions)
			{
				string optionName = shortHand ? option.ShortName : option.Name;
				if (optionName != argName)
					continue;

				found = true;
				if (option.NeedsValue)
				{
					if (HasNextArg())
						result.Add(option, NextArg());
					else
						return false;
				}
				else
				{
					result.Add(option, null);
				}

				break;
			}

			if (!found)
				return false;

			return true;
		}

		private static Dictionary<ArgumentOption, string> ProcessArgs(string[] args, int offset = 0)
		{
			Dictionary<ArgumentOption, string> result = new Dictionary<ArgumentOption, string>();

			_programArguments = args;
			_argIndex += offset;

			while (HasNextArg())
			{
				string arg = NextArg();
				if (!ParseArg(arg, result))
					return null;
			}

			foreach (var option in _argumentOptions)
			{
				if (option.Required && option.DefaultValue == null && !result.ContainsKey(option))
					return null;
				if (option.DefaultValue != null && !result.ContainsKey(option))
					result.Add(option, option.DefaultValue);
			}

			return result;
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: GenericPDBDumper <file.pdb> OPTIONS");
			if (_argumentOptions.Count > 0)
			{
				Console.WriteLine("Options:");
				foreach (var option in _argumentOptions)
				{
					string defaultValue = option.DefaultValue != null ? "=\"" + option.DefaultValue + "\"" : "";
					string required = option.Required ? "\t[REQUIRED]" : "";
					Console.WriteLine("-" + option.ShortName + "\t--" + option.Name + defaultValue + "\t" + option.Usage + required);
				}
			}
		}

		private static bool HasNextArg()
		{
			return _argIndex + 1 < _programArguments.Length;
		}

		private static string NextArg()
		{
			_argIndex++;
			if (_argIndex >= _programArguments.Length)
				throw new ArgumentException();
			return _programArguments[_argIndex];
		}

		private static bool ParseArgs(string[] args)
		{
			AddArgument("out", "output", "o", false, "Output File (json)");
			AddArgument("bd", "strip-basedir", "s", false, "Strip this base directory from paths");
			AddArgument("wl", "whitelist", "w", false, "Path to whitelisted strings file (for symbols)");
			AddArgument("bl", "blacklist", "b", false, "Path to blacklisted strings file (for symbols)");
			AddArgument("ext", "extensions", "e", false, "Append an extension to the list of valid source file extensions", true, ".cpp;.cc;.c");
			AddArgument("fwl", "file-whitelist", "W", false, "Path to whitelisted strings file (for file names)");
			var options = ProcessArgs(args, 1);
			if (options == null || args.Length < 1)
			{
				PrintUsage();
				return false;
			}

			_inputFile = args[0];
			_outputFile ??= Path.ChangeExtension(_inputFile, "json");

			foreach (var option in options)
			{
				switch (option.Key.ID)
				{
					case "out":
						_outputFile = option.Value;
						break;
					case "bd":
						_stripBaseDir = option.Value;
						break;
					case "wl":
						_whiteList = LoadList(option.Value);
						if (_whiteList == null)
							return false;
						break;
					case "bl":
						_blackList = LoadList(option.Value);
						if (_blackList == null)
							return false;
						break;
					case "ext":
						//var extList = new List<string>(_sourceExtensions);
						//extList.AddRange(option.Value.Split(";"));
						//_sourceExtensions = extList.ToArray();
						_sourceExtensions = option.Value.Split(";");
						break;
					case "fwl":
						_fileWhitelist = LoadList(option.Value);
						if (_fileWhitelist == null)
							return false;
						break;
				}
			}

			return true;
		}

		private static string[] LoadList(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("File does not exist: '" + path + "'");
				return null;
			}

			return File.ReadAllLines(path);
		}

		static void Main(string[] args)
		{
			if (!ParseArgs(args))
				return;

			Console.WriteLine("Loading PDB `" + _inputFile + "`...");
			Dictionary<string, List<string>> symbolMap = GetSymbols(_inputFile);

			Console.WriteLine("Writing to file");
			File.WriteAllText(_outputFile, JsonConvert.SerializeObject(symbolMap, Formatting.Indented));

			Console.WriteLine("Done!");
		}

		private static Dictionary<string, List<string>> GetSymbols(string filename)
		{
			PDB pdb;

			try
			{
				pdb = PDB.LoadPDB(filename);
			}
			catch (PDBException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return null;
			}

			Console.WriteLine("Parsing Symbols...");

			Dictionary<string, List<string>> sourceMap = new Dictionary<string, List<string>>();

			foreach (var obj in pdb.Objects)
			{
				string file = Path.GetFileName(obj.FileName);

				//if (_fileWhitelist != null && !file.ContainsAnyNoCase(_fileWhitelist))
				//	continue;
				
				List<string> methods = new List<string>();

				foreach (var i in obj.SymbolIndices)
				{
					string symbol = pdb.Symbols[i];

					if (ShouldAddSymbol(symbol))
						methods.Add(cleanupMethod(symbol));
				}

				bool ignoreSymbol = false;

				string sourceFile = null;
				foreach (var i in obj.SourceFileIndices)
				{
					string curFile = pdb.SourceFiles[i];
					string ext = Path.GetExtension(curFile);

					if (!ext.MatchesAnyNoCase(_sourceExtensions))
						continue;

					if (!string.IsNullOrWhiteSpace(_stripBaseDir))
						sourceFile = curFile.Replace(_stripBaseDir, "").Replace('\\', '/');
					else
						sourceFile = curFile.Replace('\\', '/');

					if (_fileWhitelist != null && !sourceFile.ContainsAnyNoCase(_fileWhitelist))
						ignoreSymbol = true;

					if (!sourceMap.ContainsKey(sourceFile) && methods.Count > 0)
						sourceMap.Add(sourceFile, new List<string>());

					break;
				}

				if (sourceFile == null)
				{
					// Make sure to only do .res files as a last resort
					foreach (var i in obj.SourceFileIndices)
					{
						string curFile = pdb.SourceFiles[i];
						string ext = Path.GetExtension(curFile);

						if (ext != ".res")
							continue;

						if (!string.IsNullOrWhiteSpace(_stripBaseDir))
							sourceFile = curFile.Replace(_stripBaseDir, "").Replace('\\', '/');
						else
							sourceFile = curFile.Replace('\\', '/');

						if (_fileWhitelist != null && !sourceFile.ContainsAnyNoCase(_fileWhitelist))
							ignoreSymbol = true;

						if (!sourceMap.ContainsKey(sourceFile) && methods.Count > 0)
							sourceMap.Add(sourceFile, new List<string>());

						break;
					}
				}

				if (ignoreSymbol)
					continue;

				if (sourceFile == null && methods.Count == 0)
				{
					Console.WriteLine($"Object with no methods? Object: {obj.FileName}");
					continue;
				}

				if (sourceFile == null)
				{
					Console.WriteLine(
						$"Object with no source file? Perhaps it uses a non standard extension. Object: {obj.FileName}");

					// Don't fail the whole thing if this happens
					if (obj.FileName == null)
						sourceFile = "<unknown>";
					else
						sourceFile = "<unknown obj:" + obj.FileName + ">";
					if (!sourceMap.ContainsKey(sourceFile))
						sourceMap.Add(sourceFile, new List<string>());
				}

				foreach (var method in methods)
					sourceMap[sourceFile].Add(method);
			}

			return sourceMap;
		}

		private static bool ShouldAddSymbol(string symbol)
		{
			bool onWhiteList = _whiteList == null || symbol.ContainsAnyNoCase(_whiteList);
			bool notOnBlackList = _blackList == null || !symbol.ContainsAnyNoCase(_blackList);

			return onWhiteList && notOnBlackList;
		}

		private static string cleanupMethod(string symbol)
		{
			symbol = symbol.Replace("__thiscall ", "");
			symbol = symbol.Replace(")const ", ") const");
			symbol = symbol.Replace("class ", "");
			symbol = symbol.Replace("struct ", "");
			symbol = symbol.Replace("(void)", "()");

			//symbol = symbol.replaceType("unsigned int", "U32");
			//symbol = symbol.replaceType("int", "S32");
			//symbol = symbol.replaceType("float", "F32");
			//symbol = symbol.replaceType("double", "F64");

			return symbol;
		}

		private static string replaceType(this string symbol, string type, string replacement)
		{
			symbol = symbol.Replace($"({type})", $"({replacement})");
			symbol = symbol.Replace($", {type}", $", {replacement}");
			symbol = symbol.Replace($"{type}, ", $"{replacement}, ");
			symbol = symbol.Replace($" {type} ", $" {replacement} ");

			return symbol;
		}


		private static bool ContainsNoCase(this string self, string other)
		{
			return self.ToLower().Contains(other.ToLower());
		}

		private static bool ContainsAny(this string self, params string[] strings)
		{
			foreach (var s in strings)
			{
				if (self.Contains(s))
					return true;
			}

			return false;
		}

		private static bool ContainsAnyNoCase(this string self, params string[] strings)
		{
			foreach (var s in strings)
			{
				if (self.ContainsNoCase(s))
					return true;
			}

			return false;
		}

		private static bool MatchesAnyNoCase(this string self, params string[] strings)
		{
			foreach (var s in strings)
			{
				if (string.Equals(self, s, StringComparison.CurrentCultureIgnoreCase))
					return true;
			}

			return false;
		}
	}
}