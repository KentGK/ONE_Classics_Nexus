using ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

partial class AppraisalWorkspaceForm
{
    private System.ComponentModel.IContainer? components = null;
    private AppraisalHeaderControl headerControl;
    private AppraisalSummaryControl summaryControl;
    private SplitContainer bodySplit;
    private AppraisalWorkflowControl workflowControl;
    private Panel contentHost;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private ToolStripStatusLabel springLabel;
    private ToolStripStatusLabel rowVersionLabel;
    private ToolStripStatusLabel apiLabel;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _loadCancellation?.Cancel();
            _loadCancellation?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        headerControl = new AppraisalHeaderControl();
        summaryControl = new AppraisalSummaryControl();
        bodySplit = new SplitContainer();
        workflowControl = new AppraisalWorkflowControl();
        contentHost = new Panel();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        springLabel = new ToolStripStatusLabel();
        rowVersionLabel = new ToolStripStatusLabel();
        apiLabel = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)bodySplit).BeginInit();
        bodySplit.Panel1.SuspendLayout();
        bodySplit.Panel2.SuspendLayout();
        bodySplit.SuspendLayout();
        SuspendLayout();

        headerControl.Dock = DockStyle.Top;
        headerControl.Height = 92;

        summaryControl.Dock = DockStyle.Top;
        summaryControl.Height = 92;

        bodySplit.Dock = DockStyle.Fill;
        bodySplit.FixedPanel = FixedPanel.Panel1;
        bodySplit.IsSplitterFixed = false;
        bodySplit.SplitterDistance = 160;
        bodySplit.SplitterWidth = 1;
        bodySplit.Panel1.Controls.Add(workflowControl);
        bodySplit.Panel2.Controls.Add(contentHost);

        workflowControl.Dock = DockStyle.Fill;
        contentHost.Dock = DockStyle.Fill;

        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, springLabel, rowVersionLabel, apiLabel });
        statusStrip.Dock = DockStyle.Bottom;
        statusLabel.Text = "Ready";
        springLabel.Spring = true;
        rowVersionLabel.Text = "Row v—";
        apiLabel.Text = "API";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 820);
        Controls.Add(bodySplit);
        Controls.Add(summaryControl);
        Controls.Add(headerControl);
        Controls.Add(statusStrip);
        MinimumSize = new Size(1024, 680);
        Name = "AppraisalWorkspaceForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ONE Classics Nexus — Appraisal Workspace";
        bodySplit.Panel1.ResumeLayout(false);
        bodySplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)bodySplit).EndInit();
        bodySplit.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
