
namespace PDBExplorer
{
	partial class ObjectViewer
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tbpOverview = new System.Windows.Forms.TabPage();
			this.tbpSymbols = new System.Windows.Forms.TabPage();
			this.tbpSourceFiles = new System.Windows.Forms.TabPage();
			this.lblName = new System.Windows.Forms.Label();
			this.lblDetails = new System.Windows.Forms.Label();
			this.lstSymbols = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lstSourceFiles = new System.Windows.Forms.ListView();
			this.tabMain.SuspendLayout();
			this.tbpOverview.SuspendLayout();
			this.tbpSymbols.SuspendLayout();
			this.tbpSourceFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tbpOverview);
			this.tabMain.Controls.Add(this.tbpSymbols);
			this.tabMain.Controls.Add(this.tbpSourceFiles);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(590, 486);
			this.tabMain.TabIndex = 0;
			// 
			// tbpOverview
			// 
			this.tbpOverview.Controls.Add(this.lblDetails);
			this.tbpOverview.Controls.Add(this.lblName);
			this.tbpOverview.Location = new System.Drawing.Point(4, 22);
			this.tbpOverview.Name = "tbpOverview";
			this.tbpOverview.Padding = new System.Windows.Forms.Padding(3);
			this.tbpOverview.Size = new System.Drawing.Size(582, 460);
			this.tbpOverview.TabIndex = 0;
			this.tbpOverview.Text = "Overview";
			this.tbpOverview.UseVisualStyleBackColor = true;
			// 
			// tbpSymbols
			// 
			this.tbpSymbols.Controls.Add(this.lstSymbols);
			this.tbpSymbols.Location = new System.Drawing.Point(4, 22);
			this.tbpSymbols.Name = "tbpSymbols";
			this.tbpSymbols.Padding = new System.Windows.Forms.Padding(3);
			this.tbpSymbols.Size = new System.Drawing.Size(582, 460);
			this.tbpSymbols.TabIndex = 1;
			this.tbpSymbols.Text = "Symbols";
			this.tbpSymbols.UseVisualStyleBackColor = true;
			// 
			// tbpSourceFiles
			// 
			this.tbpSourceFiles.Controls.Add(this.lstSourceFiles);
			this.tbpSourceFiles.Location = new System.Drawing.Point(4, 22);
			this.tbpSourceFiles.Name = "tbpSourceFiles";
			this.tbpSourceFiles.Size = new System.Drawing.Size(582, 460);
			this.tbpSourceFiles.TabIndex = 2;
			this.tbpSourceFiles.Text = "Source Files";
			this.tbpSourceFiles.UseVisualStyleBackColor = true;
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(20, 15);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(95, 25);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "_name_";
			// 
			// lblDetails
			// 
			this.lblDetails.AutoSize = true;
			this.lblDetails.Location = new System.Drawing.Point(22, 50);
			this.lblDetails.Name = "lblDetails";
			this.lblDetails.Size = new System.Drawing.Size(49, 13);
			this.lblDetails.TabIndex = 1;
			this.lblDetails.Text = "_details_";
			// 
			// lstSymbols
			// 
			this.lstSymbols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
			this.lstSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSymbols.FullRowSelect = true;
			this.lstSymbols.GridLines = true;
			this.lstSymbols.HideSelection = false;
			this.lstSymbols.Location = new System.Drawing.Point(3, 3);
			this.lstSymbols.Name = "lstSymbols";
			this.lstSymbols.Size = new System.Drawing.Size(576, 454);
			this.lstSymbols.TabIndex = 0;
			this.lstSymbols.UseCompatibleStateImageBehavior = false;
			this.lstSymbols.View = System.Windows.Forms.View.Details;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 480;
			// 
			// colFileName
			// 
			this.colFileName.Text = "Filename";
			this.colFileName.Width = 480;
			// 
			// lstSourceFiles
			// 
			this.lstSourceFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName});
			this.lstSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSourceFiles.FullRowSelect = true;
			this.lstSourceFiles.GridLines = true;
			this.lstSourceFiles.HideSelection = false;
			this.lstSourceFiles.Location = new System.Drawing.Point(0, 0);
			this.lstSourceFiles.Name = "lstSourceFiles";
			this.lstSourceFiles.Size = new System.Drawing.Size(582, 460);
			this.lstSourceFiles.TabIndex = 1;
			this.lstSourceFiles.UseCompatibleStateImageBehavior = false;
			this.lstSourceFiles.View = System.Windows.Forms.View.Details;
			// 
			// ObjectViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabMain);
			this.Name = "ObjectViewer";
			this.Size = new System.Drawing.Size(590, 486);
			this.tabMain.ResumeLayout(false);
			this.tbpOverview.ResumeLayout(false);
			this.tbpOverview.PerformLayout();
			this.tbpSymbols.ResumeLayout(false);
			this.tbpSourceFiles.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tbpOverview;
		private System.Windows.Forms.TabPage tbpSymbols;
		private System.Windows.Forms.TabPage tbpSourceFiles;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblDetails;
		private System.Windows.Forms.ListView lstSymbols;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ListView lstSourceFiles;
		private System.Windows.Forms.ColumnHeader colFileName;
	}
}
