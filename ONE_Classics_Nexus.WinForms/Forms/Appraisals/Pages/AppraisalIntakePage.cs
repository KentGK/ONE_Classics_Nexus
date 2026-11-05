using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

public sealed class AppraisalIntakePage : WorkspacePageBase
{
    private readonly TextBox _appraisalType;
    private readonly TextBox _status;
    private readonly TextBox _intakeStatus;
    private readonly TextBox _inspectionStatus;
    private readonly TextBox _valuationStatus;
    private readonly TextBox _reportStatus;
    private readonly TextBox _clientId;
    private readonly TextBox _appraiserId;
    private readonly TextBox _reviewerId;
    private readonly TextBox _appraisalTypeId;
    private readonly TextBox _appraisalStatusId;
    private readonly DateTimePicker _effectiveDate;
    private bool _binding;

    public AppraisalIntakePage()
    {
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Intake & Workflow"));
        stack.Controls.Add(CreateMuted("Edit canonical appraisal fields. Save uses optimistic concurrency through row_version."));

        var form = new TableLayoutPanel
        {
            ColumnCount = 4,
            RowCount = 6,
            Width = 930,
            Height = 390,
            BackColor = AppraisalWorkspaceTheme.Surface,
            Padding = new Padding(18),
            Margin = new Padding(0, 12, 0, 0)
        };
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (var i = 0; i < 6; i++)
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

        _appraisalType = AddTextField(form, 0, 0, "Appraisal Type");
        _status = AddTextField(form, 2, 0, "Overall Status");
        _intakeStatus = AddTextField(form, 0, 1, "Intake Status");
        _inspectionStatus = AddTextField(form, 2, 1, "Inspection Status");
        _valuationStatus = AddTextField(form, 0, 2, "Valuation Status");
        _reportStatus = AddTextField(form, 2, 2, "Report Status");
        _clientId = AddTextField(form, 0, 3, "Client ID");
        _appraiserId = AddTextField(form, 2, 3, "Appraiser ID");
        _reviewerId = AddTextField(form, 0, 4, "Reviewer ID");
        _appraisalTypeId = AddTextField(form, 2, 4, "Type Lookup ID");
        MakeReadOnlyLookup(_appraisalTypeId);
        _appraisalStatusId = AddTextField(form, 0, 5, "Status Lookup ID");
        MakeReadOnlyLookup(_appraisalStatusId);

        var effectiveCaption = CreateFieldCaption("Effective Date");
        form.Controls.Add(effectiveCaption, 2, 5);
        _effectiveDate = new DateTimePicker
        {
            Dock = DockStyle.Fill,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "MMM d, yyyy h:mm tt",
            ShowCheckBox = true,
            Font = AppraisalWorkspaceTheme.BodyFont(),
            Margin = new Padding(4, 9, 4, 9)
        };
        form.Controls.Add(_effectiveDate, 3, 5);

        foreach (var textBox in TextFields())
            textBox.TextChanged += (_, _) => RaiseDirty();
        _effectiveDate.ValueChanged += (_, _) => RaiseDirty();

        stack.Controls.Add(form);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Intake;

    public event EventHandler? DirtyStateChanged;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _binding = true;
        try
        {
            _appraisalType.Text = appraisal.AppraisalType;
            _status.Text = appraisal.Status;
            _intakeStatus.Text = appraisal.IntakeStatus ?? string.Empty;
            _inspectionStatus.Text = appraisal.InspectionStatus ?? string.Empty;
            _valuationStatus.Text = appraisal.ValuationStatus ?? string.Empty;
            _reportStatus.Text = appraisal.ReportStatus ?? string.Empty;
            _clientId.Text = appraisal.ClientId?.ToString("D") ?? string.Empty;
            _appraiserId.Text = appraisal.AssignedAppraiserId?.ToString("D") ?? string.Empty;
            _reviewerId.Text = appraisal.AssignedReviewerId?.ToString("D") ?? string.Empty;
            _appraisalTypeId.Text = appraisal.AppraisalTypeId?.ToString("D") ?? string.Empty;
            _appraisalStatusId.Text = appraisal.AppraisalStatusId?.ToString("D") ?? string.Empty;
            _effectiveDate.Checked = appraisal.EffectiveDateUtc.HasValue;
            if (appraisal.EffectiveDateUtc.HasValue)
                _effectiveDate.Value = appraisal.EffectiveDateUtc.Value.ToLocalTime();
        }
        finally
        {
            _binding = false;
        }
    }

    public UpdateAppraisalWorkspaceRequest CaptureUpdate(AppraisalWorkspaceDto current)
    {
        ArgumentNullException.ThrowIfNull(current);

        if (string.IsNullOrWhiteSpace(_appraisalType.Text))
            throw new InvalidOperationException("Appraisal Type is required.");
        if (string.IsNullOrWhiteSpace(_status.Text))
            throw new InvalidOperationException("Overall Status is required.");

        return new UpdateAppraisalWorkspaceRequest
        {
            AppraisalId = current.AppraisalId,
            VehicleId = current.VehicleId,
            AppraisalTypeId = ParseOptionalGuid(_appraisalTypeId.Text, "Type Lookup ID"),
            AppraisalStatusId = ParseOptionalGuid(_appraisalStatusId.Text, "Status Lookup ID"),
            ClientId = ParseOptionalGuid(_clientId.Text, "Client ID"),
            AssignedAppraiserId = ParseOptionalGuid(_appraiserId.Text, "Appraiser ID"),
            AssignedReviewerId = ParseOptionalGuid(_reviewerId.Text, "Reviewer ID"),
            AppraisalType = _appraisalType.Text.Trim(),
            Status = _status.Text.Trim(),
            IntakeStatus = NullIfWhiteSpace(_intakeStatus.Text),
            InspectionStatus = NullIfWhiteSpace(_inspectionStatus.Text),
            ValuationStatus = NullIfWhiteSpace(_valuationStatus.Text),
            ReportStatus = NullIfWhiteSpace(_reportStatus.Text),
            EffectiveDateUtc = _effectiveDate.Checked ? _effectiveDate.Value.ToUniversalTime() : null,
            RowVersion = current.RowVersion
        };
    }

    private TextBox AddTextField(TableLayoutPanel form, int captionColumn, int row, string caption)
    {
        var label = CreateFieldCaption(caption);
        var textBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(),
            Margin = new Padding(4, 12, 4, 12)
        };
        form.Controls.Add(label, captionColumn, row);
        form.Controls.Add(textBox, captionColumn + 1, row);
        return textBox;
    }

    private static void MakeReadOnlyLookup(TextBox textBox)
    {
        textBox.ReadOnly = true;
        textBox.BackColor = AppraisalWorkspaceTheme.Canvas;
        textBox.TabStop = false;
    }

    private static Label CreateFieldCaption(string caption) => new()
    {
        Text = caption,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        Font = AppraisalWorkspaceTheme.BodyFont(9f),
        ForeColor = AppraisalWorkspaceTheme.TextSecondary,
        Margin = new Padding(4)
    };

    private IEnumerable<TextBox> TextFields()
    {
        yield return _appraisalType;
        yield return _status;
        yield return _intakeStatus;
        yield return _inspectionStatus;
        yield return _valuationStatus;
        yield return _reportStatus;
        yield return _clientId;
        yield return _appraiserId;
        yield return _reviewerId;
        yield return _appraisalTypeId;
        yield return _appraisalStatusId;
    }

    private void RaiseDirty()
    {
        if (!_binding)
            DirtyStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private static Guid? ParseOptionalGuid(string? text, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        if (Guid.TryParse(text.Trim(), out var value) && value != Guid.Empty)
            return value;

        throw new InvalidOperationException($"{fieldName} must be a valid GUID or blank.");
    }

    private static string? NullIfWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
