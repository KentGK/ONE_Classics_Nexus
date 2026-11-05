#nullable enable
using System.ComponentModel;

namespace ONE_Classics_Nexus.WinForms.Forms.Shell
{
    partial class MainShellForm
    {
        private IContainer? components;

        private void InitializeComponent()
        {
            components = new Container();
            SuspendLayout();
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }
    }
}
