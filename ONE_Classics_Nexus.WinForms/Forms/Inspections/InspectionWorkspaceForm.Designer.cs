// ============================================================
// InspectionWorkspaceForm.Designer.cs
// ============================================================
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections
{
    partial class InspectionWorkspaceForm
    {
        private System.ComponentModel.IContainer components = null;

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
            components = new System.ComponentModel.Container();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "InspectionWorkspaceForm";
            Text = "InspectionWorkspaceForm";
            ClientSize = new System.Drawing.Size(1200, 700);
        }
    }
}