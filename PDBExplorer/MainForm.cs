using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDBExplorer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void Open(string file = null)
		{
			if (!string.IsNullOrWhiteSpace(file))
			{
				DoOpen(file);
				return;
			}

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Program Database Files|*.pdb|PDB Explorer Database File|*.pdbx|All Files|*.*";
			if (ofd.ShowDialog(this) != DialogResult.Cancel)
			{
				if (string.IsNullOrWhiteSpace(ofd.FileName))
					return;

				DoOpen(ofd.FileName);
			}
		}

		private void DoOpen(string file)
		{

		}

		private void Export()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "*.pdbx|PDB Explorer Database File|*.*|All Files";
			if (sfd.ShowDialog(this) != DialogResult.Cancel)
			{
				if (string.IsNullOrWhiteSpace(sfd.FileName))
					return;

				DoExport(sfd.FileName);
			}
		}

		private void DoExport(string file)
		{

		}

		private void Exit()
		{
			Application.Exit();
		}

		private void About()
		{
			MessageBox.Show(this, "This program was created by Human Gamer.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mniOpen_Click(object sender, EventArgs e)
		{
			Open();
		}

		private void mniExport_Click(object sender, EventArgs e)
		{
			Export();
		}

		private void mniExit_Click(object sender, EventArgs e)
		{
			Exit();
		}

		private void mniAbout_Click(object sender, EventArgs e)
		{
			About();
		}

		private void tsbOpen_Click(object sender, EventArgs e)
		{
			Open();
		}

		private void tsbExport_Click(object sender, EventArgs e)
		{
			Export();
		}
	}
}
