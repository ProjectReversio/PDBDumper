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

			using PDB pdb = new PDB();
			if (!pdb.LoadPDB(args[0]))
				return;

			var sources = pdb.SourceFiles;
			var objects = pdb.Objects;

			foreach (var obj in objects)
			{
				Console.WriteLine(obj.FileName);

				foreach (var symbol in obj.Symbols)
					Console.WriteLine("  " + symbol);

				foreach (var index in obj.SourceFileIndices)
				{
					Console.WriteLine("    " + sources[(int)index]);
				}
			}
		}
	}
}
