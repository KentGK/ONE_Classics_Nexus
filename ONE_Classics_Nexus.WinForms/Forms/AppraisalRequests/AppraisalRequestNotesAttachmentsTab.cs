using System.ComponentModel;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public sealed class AppraisalRequestNotesAttachmentsTab : UserControl
{
    private readonly TableLayoutPanel _rootLayout = new();

    private readonly GroupBox _grpAuthenticity = new();
    private readonly GroupBox _grpModifications = new();
    private readonly GroupBox _grpRestoration = new();

    private readonly TextBox _txtAuthenticity = new();
    private readonly TextBox _txtModifications = new();
    private readonly TextBox _txtRestoration = new();

    private bool _suppressDirtyTracking;
    private bool _isDirty;

    public AppraisalRequestNotesAttachmentsTab()
    {
        BuildUi();
        ApplyTheme();
        WireDirtyTracking();
        SetDirty(false);
    }

    public event EventHandler? DirtyStateChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDirty => _isDirty;

    /// <summary>
    /// Applies read-only/editable state for Notes and Attachments narrative fields.
    /// </summary>
    public void SetEditable(bool editable)
    {
        _txtAuthenticity.ReadOnly = !editable;
        _txtModifications.ReadOnly = !editable;
        _txtRestoration.ReadOnly = !editable;

        foreach (var textBox in EnumerateTextBoxes())
            textBox.BackColor = NexusTheme.Surface;
    }

    /// <summary>
    /// Clears the dirty indicator after parent save or bind operations.
    /// </summary>
    public void AcceptChanges() => SetDirty(false);

    private void BuildUi()
    {
        Dock = DockStyle.Fill;
        Margin = Padding.Empty;
        Padding = new Padding(12);

        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.ColumnCount = 2;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _rootLayout.RowCount = 2;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        ConfigureSection(
            _grpAuthenticity,
            _txtAuthenticity,
            "Authenticity",
            "Capture authenticity indicators, matching components, and originality concerns.");

        ConfigureSection(
            _grpModifications,
            _txtModifications,
            "Modifications",
            "Capture declared or observed modifications and potential impact on valuation scope.");

        ConfigureSection(
            _grpRestoration,
            _txtRestoration,
            "Restoration",
            "Capture known restoration work, quality observations, and supporting references.");

        _rootLayout.Controls.Add(_grpAuthenticity, 0, 0);
        _rootLayout.Controls.Add(_grpModifications, 1, 0);
        _rootLayout.Controls.Add(_grpRestoration, 0, 1);
        _rootLayout.SetColumnSpan(_grpRestoration, 2);

        Controls.Add(_rootLayout);
    }

    private void ApplyTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;

        foreach (var group in EnumerateGroups())
        {
            group.BackColor = NexusTheme.Surface;
            group.ForeColor = NexusTheme.Primary;
            group.Font = NexusTheme.LabelFont;
        }

        foreach (var textBox in EnumerateTextBoxes())
        {
            textBox.BackColor = NexusTheme.Surface;
            textBox.ForeColor = NexusTheme.TextStrong;
            textBox.Font = NexusTheme.BodyFont;
        }
    }

    private void WireDirtyTracking()
    {
        foreach (var textBox in EnumerateTextBoxes())
            textBox.TextChanged += (_, _) => MarkDirty();
    }

    private static void ConfigureSection(
        GroupBox group,
        TextBox notes,
        string title,
        string placeholder)
    {
        group.Dock = DockStyle.Fill;
        group.Text = title;
        group.Padding = new Padding(10, 18, 10, 10);
        group.Margin = new Padding(4);

        notes.Dock = DockStyle.Fill;
        notes.Multiline = true;
        notes.ScrollBars = ScrollBars.Vertical;
        notes.BorderStyle = BorderStyle.FixedSingle;
        notes.PlaceholderText = placeholder;

        group.Controls.Add(notes);
    }

    private IEnumerable<GroupBox> EnumerateGroups()
    {
        yield return _grpAuthenticity;
        yield return _grpModifications;
        yield return _grpRestoration;
    }

    private IEnumerable<TextBox> EnumerateTextBoxes()
    {
        yield return _txtAuthenticity;
        yield return _txtModifications;
        yield return _txtRestoration;
    }

    private void MarkDirty()
    {
        if (_suppressDirtyTracking)
            return;

        SetDirty(true);
    }

    private void SetDirty(bool dirty)
    {
        if (_isDirty == dirty)
            return;

        _isDirty = dirty;
        DirtyStateChanged?.Invoke(this, EventArgs.Empty);
    }
}
