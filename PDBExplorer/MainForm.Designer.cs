
namespace PDBExplorer
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lsbMain = new System.Windows.Forms.ListBox();
			this.spcMain = new System.Windows.Forms.SplitContainer();
			this.stsMain = new System.Windows.Forms.StatusStrip();
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mniOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mniExport = new System.Windows.Forms.ToolStripMenuItem();
			this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.tsbOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbExport = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.spcMain)).BeginInit();
			this.spcMain.Panel1.SuspendLayout();
			this.spcMain.SuspendLayout();
			this.mnuMain.SuspendLayout();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// lsbMain
			// 
			this.lsbMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsbMain.FormattingEnabled = true;
			this.lsbMain.HorizontalScrollbar = true;
			this.lsbMain.Location = new System.Drawing.Point(0, 0);
			this.lsbMain.Name = "lsbMain";
			this.lsbMain.Size = new System.Drawing.Size(266, 359);
			this.lsbMain.TabIndex = 0;
			// 
			// spcMain
			// 
			this.spcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcMain.Location = new System.Drawing.Point(0, 49);
			this.spcMain.Name = "spcMain";
			// 
			// spcMain.Panel1
			// 
			this.spcMain.Panel1.Controls.Add(this.lsbMain);
			this.spcMain.Panel1.Controls.Add(this.txtFilter);
			this.spcMain.Size = new System.Drawing.Size(800, 379);
			this.spcMain.SplitterDistance = 266;
			this.spcMain.TabIndex = 1;
			// 
			// stsMain
			// 
			this.stsMain.Location = new System.Drawing.Point(0, 428);
			this.stsMain.Name = "stsMain";
			this.stsMain.Size = new System.Drawing.Size(800, 22);
			this.stsMain.TabIndex = 2;
			this.stsMain.Text = "statusStrip1";
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniHelp});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(800, 24);
			this.mnuMain.TabIndex = 3;
			this.mnuMain.Text = "menuStrip1";
			// 
			// txtFilter
			// 
			this.txtFilter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.txtFilter.Location = new System.Drawing.Point(0, 359);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(266, 20);
			this.txtFilter.TabIndex = 1;
			// 
			// tsMain
			// 
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.toolStripSeparator1,
            this.tsbExport});
			this.tsMain.Location = new System.Drawing.Point(0, 24);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(800, 25);
			this.tsMain.TabIndex = 4;
			this.tsMain.Text = "toolStrip1";
			// 
			// mniFile
			// 
			this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniOpen,
            this.toolStripMenuItem1,
            this.mniExport,
            this.toolStripMenuItem2,
            this.mniExit});
			this.mniFile.Name = "mniFile";
			this.mniFile.Size = new System.Drawing.Size(37, 20);
			this.mniFile.Text = "File";
			// 
			// mniOpen
			// 
			this.mniOpen.Image = global::PDBExplorer.Properties.Resources.openHS;
			this.mniOpen.Name = "mniOpen";
			this.mniOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mniOpen.Size = new System.Drawing.Size(180, 22);
			this.mniOpen.Text = "Open";
			this.mniOpen.Click += new System.EventHandler(this.mniOpen_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
			// 
			// mniExit
			// 
			this.mniExit.Name = "mniExit";
			this.mniExit.Size = new System.Drawing.Size(180, 22);
			this.mniExit.Text = "Exit";
			this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
			// 
			// mniExport
			// 
			this.mniExport.Image = global::PDBExplorer.Properties.Resources.saveHS;
			this.mniExport.Name = "mniExport";
			this.mniExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mniExport.Size = new System.Drawing.Size(180, 22);
			this.mniExport.Text = "Export pdbx";
			this.mniExport.Click += new System.EventHandler(this.mniExport_Click);
			// 
			// mniHelp
			// 
			this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAbout});
			this.mniHelp.Name = "mniHelp";
			this.mniHelp.Size = new System.Drawing.Size(44, 20);
			this.mniHelp.Text = "Help";
			// 
			// mniAbout
			// 
			this.mniAbout.Image = global::PDBExplorer.Properties.Resources.Help;
			this.mniAbout.Name = "mniAbout";
			this.mniAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.mniAbout.Size = new System.Drawing.Size(180, 22);
			this.mniAbout.Text = "About";
			this.mniAbout.Click += new System.EventHandler(this.mniAbout_Click);
			// 
			// tsbOpen
			// 
			this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbOpen.Image = global::PDBExplorer.Properties.Resources.openHS;
			this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOpen.Name = "tsbOpen";
			this.tsbOpen.Size = new System.Drawing.Size(23, 22);
			this.tsbOpen.Text = "toolStripButton1";
			this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbExport
			// 
			this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbExport.Image = global::PDBExplorer.Properties.Resources.saveHS;
			this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbExport.Name = "tsbExport";
			this.tsbExport.Size = new System.Drawing.Size(23, 22);
			this.tsbExport.Text = "toolStripButton2";
			this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.spcMain);
			this.Controls.Add(this.stsMain);
			this.Controls.Add(this.tsMain);
			this.Controls.Add(this.mnuMain);
			this.Name = "MainForm";
			this.Text = "PDB Explorer";
			this.spcMain.Panel1.ResumeLayout(false);
			this.spcMain.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.spcMain)).EndInit();
			this.spcMain.ResumeLayout(false);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lsbMain;
		private System.Windows.Forms.SplitContainer spcMain;
		private System.Windows.Forms.StatusStrip stsMain;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripMenuItem mniFile;
		private System.Windows.Forms.ToolStripMenuItem mniOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mniExport;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mniExit;
		private System.Windows.Forms.ToolStripMenuItem mniHelp;
		private System.Windows.Forms.ToolStripMenuItem mniAbout;
		private System.Windows.Forms.ToolStripButton tsbOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbExport;
	}
}

