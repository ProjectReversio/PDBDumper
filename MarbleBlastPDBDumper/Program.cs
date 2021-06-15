using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PDBInfo;

namespace MarbleBlastPDBDumper
{
	static class Program
	{
		private static bool UseWhitelist = false;
		private static bool UseFileWhitelist = false;
		private static bool UseBlacklist = false;
		
		private static readonly string[] FileWhitelist =
		{
			"marble",
			"powerup",
			"guibarctrl",
			"guienergybarctrl",
		};

		private static readonly string[] WhiteList =
		{
			"__thiscall Marble::",
			"__thiscall MarbleData::",
			"__thiscall PowerUpData::",
			"cmarble",

			"__thiscall GuiBarCtrl::",
			"__thiscall GuiEnergyBarCtrl::",
		};

		private static readonly string[] BlackList =
		{
			"caster(",
			"`",
			"abstractclassrep",
		};

		private static readonly string[] SourceExtensions =
		{
			".cpp", ".cc", ".c",
		};

		private static string StripBaseDir = "f:\\dev\\mbujugg\\build_dir\\";

		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Usage: MarbleBlastPDBDumper <file.pdb>");
				return;
			}

			string filename = args[0];
			string outfile = Path.ChangeExtension(filename, "json");

			Console.WriteLine("Loading PDB `" + filename + "`...");
			Dictionary<string, SourceData> symbolMap = GetMarbleSymbols(filename);

			Console.WriteLine("Writing to file");
			File.WriteAllText(outfile, JsonConvert.SerializeObject(symbolMap, Formatting.Indented));

			Console.WriteLine("Done!");
		}

		private class SourceData
		{
			public List<string> Methods { get; }
			public List<string> ConsoleMethods { get; }

			public SourceData()
			{
				Methods = new List<string>();
				ConsoleMethods = new List<string>();
			}
		}

		private static Dictionary<string, SourceData> GetMarbleSymbols(string filename)
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

			Console.WriteLine("Parsing Marble Symbols...");

			Dictionary<string, SourceData> sourceMap = new Dictionary<string, SourceData>();

			foreach (var obj in pdb.Objects)
			{
				string file = Path.GetFileName(obj.FileName);
				
				if (UseFileWhitelist && !file.ContainsAnyNoCase(FileWhitelist))
					continue;

				List<string> consoleMethods = new List<string>();
				List<string> methods = new List<string>();

				foreach (var i in obj.SymbolIndices)
				{
					string symbol = pdb.Symbols[i];

					if (ShouldAddSymbol(symbol))
					{
						if (isConsoleMethod(symbol))
							consoleMethods.Add(cleanupConsoleMethod(symbol));
						else
							methods.Add(cleanupMethod(symbol));
					}
				}

				string sourceFile = null;
				foreach (var i in obj.SourceFileIndices)
				{
					string curFile = pdb.SourceFiles[i];
					string ext = Path.GetExtension(curFile);

					if (!ext.MatchesAnyNoCase(SourceExtensions))
						continue;

					sourceFile = curFile.Replace(StripBaseDir, "").Replace('\\', '/');

					if (!sourceMap.ContainsKey(sourceFile) && (consoleMethods.Count > 0 || methods.Count > 0))
						sourceMap.Add(sourceFile, new SourceData());

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

						sourceFile = curFile.Replace(StripBaseDir, "").Replace('\\', '/');

						if (!sourceMap.ContainsKey(sourceFile) && (consoleMethods.Count > 0 || methods.Count > 0))
							sourceMap.Add(sourceFile, new SourceData());

						break;
					}
				}

				if (sourceFile == null)
				{
					Console.WriteLine(
						$"Object with no source file? Perhaps it uses a non standard extension. Object: {obj.FileName}");
					
					// Don't fail the whole thing if this happens
					sourceFile = "<error>";
					if (!sourceMap.ContainsKey(sourceFile))
						sourceMap.Add(sourceFile, new SourceData());
				}

				foreach (var consoleMethod in consoleMethods)
					sourceMap[sourceFile].ConsoleMethods.Add(consoleMethod);

				foreach (var method in methods)
					sourceMap[sourceFile].Methods.Add(method);
			}

			return sourceMap;
		}

		private static bool ShouldAddSymbol(string symbol)
		{
			bool onWhiteList = !UseWhitelist || symbol.ContainsAnyNoCase(WhiteList);
			bool notOnBlackList = !UseBlacklist || !symbol.ContainsAnyNoCase(BlackList);

			return onWhiteList && notOnBlackList;
		}

		private static bool isConsoleMethod(string symbol)
		{
			// we need a way to auto detect this
			return symbol.ToLower().Contains("cmarble");
		}

		private static string cleanupConsoleMethod(string symbol)
		{
			symbol = cleanupMethod(symbol);

			if (symbol.StartsWith("static "))
				symbol = symbol.Substring(7);

			int index = symbol.IndexOf(" cMarble");
			symbol = symbol.Replace("cMarble", "");

			int index2 = symbol.IndexOf("(");
			if (index2 > -1)
				symbol = symbol.Substring(0, index2);

			if (index > -1)
			{
				string type = symbol.Substring(0, index);
				symbol = symbol.Substring(index);

				symbol = "ConsoleMethod(Marble," + symbol + ", " + type + ")";
			}

			return symbol;
		}

		private static string cleanupMethod(string symbol)
		{
			symbol = symbol.Replace("__thiscall ", "");
			symbol = symbol.Replace(")const ", ") const");
			symbol = symbol.Replace("class ", "");
			symbol = symbol.Replace("struct ", "");
			symbol = symbol.Replace("(void)", "()");

			symbol = symbol.replaceType("unsigned int", "U32");
			symbol = symbol.replaceType("int", "S32");
			symbol = symbol.replaceType("float", "F32");
			symbol = symbol.replaceType("double", "F64");

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
