using System;
using System.Collections.Generic;
using PDBInfo;

namespace PDBDumperSharp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Usage: pdbdump <pdbfile>");
				return;
			}

			string filename = args[0];

			Console.WriteLine($"Loading PDB '{filename}'...");

			using PDB pdb = new PDB();
			if (!pdb.LoadPDB(filename))
				return;

			Console.WriteLine("PDB Loaded... Processing Information");

			var symbols = pdb.Symbols;
			var sources = pdb.SourceFiles;
			var objects = pdb.Objects;

			foreach (var obj in objects)
			{
				Console.WriteLine(obj.FileName);

				foreach (var index in obj.SymbolIndices)
					Console.WriteLine("  " + symbols[(int)index]);

				foreach (var index in obj.SourceFileIndices)
				{
					Console.WriteLine("    " + sources[(int)index]);
				}
			}
		}
	}
}
