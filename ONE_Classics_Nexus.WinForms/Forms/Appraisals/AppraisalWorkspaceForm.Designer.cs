#nullable enable
// ============================================================
// AppraisalWorkspaceForm.Designer.cs
// ============================================================
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Controls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals;

partial class AppraisalWorkspaceForm
{
    private System.ComponentModel.IContainer? components = null;
    private AppraisalHeaderControl headerControl = null!;
    private SplitContainer bodySplit = null!;
    private AppraisalWorkflowControl workflowControl = null!;
    private Panel contentHost = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;
    private ToolStripStatusLabel springLabel = null!;
    private ToolStripStatusLabel rowVersionLabel = null!;
    private ToolStripStatusLabel apiLabel = null!;
    private Form? _currentChildForm;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!(_loadCancellation?.Token.CanBeCanceled == false))
            {
                try
                {
                    _loadCancellation?.Cancel();
                }
                catch (InvalidOperationException)
                {
                    // The form is being disposed; cancellation failure can be ignored.
                }
                _loadCancellation?.Dispose();
            }
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        bodySplit = new SplitContainer();
        contentHost = new Panel();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        springLabel = new ToolStripStatusLabel();
        rowVersionLabel = new ToolStripStatusLabel();
        apiLabel = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)bodySplit).BeginInit();
        bodySplit.Panel2.SuspendLayout();
        bodySplit.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // bodySplit
        // 
        bodySplit.FixedPanel = FixedPanel.Panel1;
        bodySplit.Location = new Point(0, 0);
        bodySplit.Margin = new Padding(41, 19, 41, 19);
        bodySplit.Name = "bodySplit";
        // 
        // bodySplit.Panel2
        // 
        bodySplit.Panel2.Controls.Add(contentHost);
        bodySplit.Size = new Size(1522, 823);
        bodySplit.SplitterDistance = 1058;
        bodySplit.SplitterWidth = 14;
        bodySplit.TabIndex = 0;
        // 
        // contentHost
        // 
        contentHost.Dock = DockStyle.Fill;
        contentHost.Location = new Point(0, 0);
        contentHost.Margin = new Padding(41, 19, 41, 19);
        contentHost.Name = "contentHost";
        contentHost.Size = new Size(450, 823);
        contentHost.TabIndex = 0;
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, springLabel, rowVersionLabel, apiLabel });
        statusStrip.Location = new Point(0, 822);
        statusStrip.Name = "statusStrip";
        statusStrip.Padding = new Padding(14, 0, 192, 0);
        statusStrip.Size = new Size(1522, 22);
        statusStrip.TabIndex = 3;
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(39, 17);
        statusLabel.Text = "Ready";
        // 
        // springLabel
        // 
        springLabel.Name = "springLabel";
        springLabel.Size = new Size(1201, 17);
        springLabel.Spring = true;
        // 
        // rowVersionLabel
        // 
        rowVersionLabel.Name = "rowVersionLabel";
        rowVersionLabel.Size = new Size(51, 17);
        rowVersionLabel.Text = "Row v—";
        // 
        // apiLabel
        // 
        apiLabel.Name = "apiLabel";
        apiLabel.Size = new Size(25, 17);
        apiLabel.Text = "API";
        // 
        // AppraisalWorkspaceForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1522, 844);
        Controls.Add(statusStrip);
        Controls.Add(bodySplit);
        Margin = new Padding(41, 19, 41, 19);
        MinimumSize = new Size(1538, 822);
        Name = "AppraisalWorkspaceForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Appraisal Workspace";
        bodySplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)bodySplit).EndInit();
        bodySplit.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    private void CloseCurrentChildForm()
    {
        _currentChildForm?.Close(); // already disposes since TopLevel/non-modal
        _currentChildForm = null;
    }
}
