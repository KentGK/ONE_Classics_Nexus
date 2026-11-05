using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests.Dialogs;

public sealed class IntendedUserEditorDialog : Form
{
    private readonly TextBox _txtDisplayName = new();
    private readonly TextBox _txtOrganization = new();
    private readonly TextBox _txtRelationship = new();
    private readonly TextBox _txtEmail = new();
    private readonly TextBox _txtPhone = new();
    private readonly TextBox _txtRestrictions = new();

    public IntendedUserEditorDialog(string title, ProposedIntendedUserSnapshot? existing = null)
    {
        BuildUi(title);

        if (existing is null)
            return;

        _txtDisplayName.Text = existing.DisplayName;
        _txtOrganization.Text = existing.OrganizationName ?? string.Empty;
        _txtRelationship.Text = existing.RelationshipToClient;
        _txtEmail.Text = existing.Email ?? string.Empty;
        _txtPhone.Text = existing.Phone ?? string.Empty;
        _txtRestrictions.Text = existing.UseRestrictions ?? string.Empty;
    }

    public string? DisplayName => ReadRequired(_txtDisplayName);
    public string? OrganizationName => ReadOptional(_txtOrganization);
    public string? RelationshipToClient => ReadRequired(_txtRelationship);
    public string? Email => ReadOptional(_txtEmail);
    public string? Phone => ReadOptional(_txtPhone);
    public string? UseRestrictions => ReadOptional(_txtRestrictions);

    private void BuildUi(string title)
    {
        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Width = 720;
        Height = 420;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(12)
        };

        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (var i = 0; i < 6; i++)
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        AddRow(root, 0, "Display Name", _txtDisplayName);
        AddRow(root, 1, "Organization", _txtOrganization);
        AddRow(root, 2, "Relationship to Client", _txtRelationship);
        AddRow(root, 3, "Email", _txtEmail);
        AddRow(root, 4, "Phone", _txtPhone);

        var lblRestrictions = new Label
        {
            Text = "Use Restrictions",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _txtRestrictions.Dock = DockStyle.Fill;
        _txtRestrictions.Multiline = true;
        _txtRestrictions.ScrollBars = ScrollBars.Vertical;
        _txtRestrictions.BackColor = NexusTheme.Surface;

        root.Controls.Add(lblRestrictions, 0, 5);
        root.Controls.Add(_txtRestrictions, 1, 5);
        root.SetRowSpan(_txtRestrictions, 2);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Margin = new Padding(0)
        };

        var btnCancel = new Button { Text = "Cancel", Width = 94, DialogResult = DialogResult.Cancel };
        var btnOk = new Button { Text = "Save", Width = 94, DialogResult = DialogResult.OK };
        btnOk.Click += BtnOk_Click;

        buttons.Controls.Add(btnCancel);
        buttons.Controls.Add(btnOk);

        root.Controls.Add(buttons, 1, 6);

        Controls.Add(root);

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DisplayName))
        {
            MessageBox.Show(this, "Display name is required.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        if (string.IsNullOrWhiteSpace(RelationshipToClient))
        {
            MessageBox.Show(this, "Relationship to client is required.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
        }
    }

    private static void AddRow(TableLayoutPanel root, int row, string caption, TextBox input)
    {
        var label = new Label
        {
            Text = caption,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        input.Dock = DockStyle.Fill;
        input.BackColor = NexusTheme.Surface;

        root.Controls.Add(label, 0, row);
        root.Controls.Add(input, 1, row);
    }

    private static string? ReadRequired(TextBox textBox)
    {
        var value = ReadOptional(textBox);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static string? ReadOptional(TextBox textBox) =>
        string.IsNullOrWhiteSpace(textBox.Text) ? null : textBox.Text.Trim();
}
