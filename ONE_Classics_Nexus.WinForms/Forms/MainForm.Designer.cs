namespace ONE_Classics_Nexus.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelNav;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ListBox lstNavigation;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelNav = new System.Windows.Forms.Panel();
            this.lstNavigation = new System.Windows.Forms.ListBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
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
            this.panelNav.Size = new System.Drawing.Size(220, 561);
            this.panelNav.TabIndex = 0;
            // 
            // lstNavigation
            // 
            this.lstNavigation.BackColor = System.Drawing.Color.FromArgb(24, 30, 54);
            this.lstNavigation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstNavigation.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lstNavigation.ForeColor = System.Drawing.Color.White;
            this.lstNavigation.FormattingEnabled = true;
            this.lstNavigation.ItemHeight = 20;
            this.lstNavigation.Items.AddRange(new object[] {
            "Dashboard",
            "Vehicles",
            "Appraisals",
            "Inspections",
            "Comparables",
            "Valuations",
            "Reports",
            "Admin"});
            this.lstNavigation.Location = new System.Drawing.Point(12, 80);
            this.lstNavigation.Name = "lstNavigation";
            this.lstNavigation.Size = new System.Drawing.Size(190, 240);
            this.lstNavigation.TabIndex = 1;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(12, 20);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(171, 25);
            this.lblBrand.TabIndex = 0;
            this.lblBrand.Text = "ONE Classics Nexus";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelHeader.Controls.Add(this.lblHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(220, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(764, 70);
            this.panelHeader.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(18, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(127, 30);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Dashboard";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 70);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(764, 469);
            this.panelContent.TabIndex = 2;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStripMain.Location = new System.Drawing.Point(220, 539);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(764, 22);
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
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelNav);
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
    }
}