using ONE_Classics_Nexus.WinForms.Forms.Dashboard;

namespace ONE_Classics_Nexus.WinForms.Forms.Shell
{
    partial class MainShellForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelNav;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblSectionTitle;
        private System.Windows.Forms.ListBox lstNavigation;

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
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelNav = new System.Windows.Forms.Panel();
            this.lstNavigation = new System.Windows.Forms.ListBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSectionTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelNav.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelNav
            // 
            this.panelNav.BackColor = System.Drawing.Color.FromArgb(24, 30, 54);
            this.panelNav.Controls.Add(this.lstNavigation);
            this.panelNav.Controls.Add(this.lblBrand);
            this.panelNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNav.Location = new System.Drawing.Point(0, 0);
            this.panelNav.Name = "panelNav";
            this.panelNav.Size = new System.Drawing.Size(220, 661);
            this.panelNav.TabIndex = 0;
            // 
            // lstNavigation
            // 
            this.lstNavigation.BackColor = System.Drawing.Color.FromArgb(24, 30, 54);
            this.lstNavigation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstNavigation.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lstNavigation.ForeColor = System.Drawing.Color.White;
            this.lstNavigation.FormattingEnabled = true;
            this.lstNavigation.ItemHeight = 19;
            this.lstNavigation.Items.AddRange(new object[] {
            "Dashboard",
            "Vehicles",
            "Appraisals",
            "Inspections",
            "Comparables",
            "Valuations",
            "Reports",
            "Admin"});
            this.lstNavigation.Location = new System.Drawing.Point(16, 82);
            this.lstNavigation.Name = "lstNavigation";
            this.lstNavigation.Size = new System.Drawing.Size(186, 247);
            this.lstNavigation.TabIndex = 1;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(16, 20);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(171, 25);
            this.lblBrand.TabIndex = 0;
            this.lblBrand.Text = "ONE Classics Nexus";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelHeader.Controls.Add(this.lblSectionTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(220, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(980, 70);
            this.panelHeader.TabIndex = 1;
            // 
            // lblSectionTitle
            // 
            this.lblSectionTitle.AutoSize = true;
            this.lblSectionTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblSectionTitle.Location = new System.Drawing.Point(18, 20);
            this.lblSectionTitle.Name = "lblSectionTitle";
            this.lblSectionTitle.Size = new System.Drawing.Size(127, 30);
            this.lblSectionTitle.TabIndex = 0;
            this.lblSectionTitle.Text = "Dashboard";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.Gainsboro;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 70);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(0);
            this.panelContent.Size = new System.Drawing.Size(980, 569);
            this.panelContent.TabIndex = 2;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStripMain.Location = new System.Drawing.Point(220, 639);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(980, 22);
            this.statusStripMain.TabIndex = 3;
            this.statusStripMain.Text = "statusStripMain";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // MainShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 661);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelNav);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "MainShellForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ONE Classics Nexus";
            this.panelNav.ResumeLayout(false);
            this.panelNav.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}