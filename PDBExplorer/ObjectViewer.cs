using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDBInfo;

namespace PDBExplorer
{
	public partial class ObjectViewer : UserControl
	{
		public struct PDBObject
		{
			public PDB PDB { get; }
			public PDB.ObjectFile ObjectFile { get; }

			public PDBObject(PDB pdb, PDB.ObjectFile objectFile)
			{
				PDB = pdb;
				ObjectFile = objectFile;
			}
		}

		private PDBObject _pdb;
		public PDBObject PDB
		{
			get => _pdb;
			set
			{
				_pdb = value;
				UpdateDisplay();
			}
		}

		public ObjectViewer()
		{
			InitializeComponent();
		}

		public void UpdateDisplay()
		{
			lstSymbols.Items.Clear();
			lstSourceFiles.Items.Clear();

			lblName.Text = Path.GetFileName(PDB.ObjectFile.FileName);
			lblDetails.Text = GetDetailsText();

			foreach (var i in PDB.ObjectFile.SymbolIndices)
			{
				lstSymbols.Items.Add(new ListViewItem(new string[]
				{
					PDB.PDB.Symbols[i]
				}));
			}

			foreach (var i in PDB.ObjectFile.SourceFileIndices)
			{
				lstSourceFiles.Items.Add(new ListViewItem(new string[]
				{
					PDB.PDB.SourceFiles[i]
				}));
			}
		}

		public string GetDetailsText()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Name: " + PDB.ObjectFile.FileName);

			return sb.ToString();
		}
	}
}
