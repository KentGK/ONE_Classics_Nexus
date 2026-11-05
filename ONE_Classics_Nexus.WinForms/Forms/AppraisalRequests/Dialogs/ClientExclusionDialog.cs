using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests.Dialogs;

public sealed class ClientExclusionDialog : Form
{
    private readonly TextBox _txtExplanation = new();

    public ClientExclusionDialog(string? existingExplanation = null)
    {
        BuildUi();
        _txtExplanation.Text = existingExplanation ?? string.Empty;
    }

    public string? Explanation =>
        string.IsNullOrWhiteSpace(_txtExplanation.Text) ? null : _txtExplanation.Text.Trim();

    private void BuildUi()
    {
        Text = "Client Exclusion";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Width = 720;
        Height = 300;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12)
        };

        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

        var lblPrompt = new Label
        {
            Text = "Explain why the client is not a proposed intended user.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _txtExplanation.Dock = DockStyle.Fill;
        _txtExplanation.Multiline = true;
        _txtExplanation.ScrollBars = ScrollBars.Vertical;
        _txtExplanation.BackColor = NexusTheme.Surface;

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false
        };

        var btnCancel = new Button { Text = "Cancel", Width = 94, DialogResult = DialogResult.Cancel };
        var btnSave = new Button { Text = "Save", Width = 94, DialogResult = DialogResult.OK };

        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnSave);

        root.Controls.Add(lblPrompt, 0, 0);
        root.Controls.Add(_txtExplanation, 0, 1);
        root.Controls.Add(buttonPanel, 0, 2);

        Controls.Add(root);

        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }
}
