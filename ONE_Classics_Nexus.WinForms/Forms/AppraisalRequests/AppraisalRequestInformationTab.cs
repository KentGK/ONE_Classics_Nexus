using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

/// <summary>
/// UI-only editor for tab "1. Request Information" on AppraisalRequestForm.
///
/// Important domain boundary:
/// Values entered here are intake/request statements. They are not approved
/// Standard 7 assignment facts until reviewed in the resulting Appraisal.
/// </summary>
public partial class AppraisalRequestInformationTab : UserControl
{
    private bool _isDirty;
    private bool _suppressDirtyTracking;

    public AppraisalRequestInformationTab()
    {
        InitializeComponent();
        ApplyNexusTheme();
        SeedLookupValues();
        WireDirtyTracking();
        SetDirty(false);
    }

    /// <summary>
    /// Raised whenever the user changes a data-entry value.
    /// The parent AppraisalRequestForm can use this to update its shell/save-state indicator.
    /// </summary>
    public event EventHandler? DirtyStateChanged;

    /// <summary>
    /// Raised when the ellipsis button beside Client is selected.
    /// The parent form owns the client lookup dialog/navigation behavior.
    /// </summary>
    public event EventHandler? BrowseClientRequested;

    public bool IsDirty => _isDirty;

    public Guid? SelectedClientId { get; private set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RequestInformationTabData Value
    {
        get => ReadValue();
        set => Bind(value);
    }

    public void Bind(RequestInformationTabData? data)
    {
        data ??= new RequestInformationTabData();

        _suppressDirtyTracking = true;
        try
        {
            txtRequestPurposeDescription.Text = data.RequestPurposeDescription ?? string.Empty;
            SelectComboValue(cboRequestedAppraisalType, data.RequestedAppraisalType);
            SelectComboValue(cboStatedPurpose, data.StatedPurpose);
            SelectComboValue(cboRequestedIntendedUse, data.RequestedIntendedUse);
            txtStatedIntendedUsers.Text = data.StatedIntendedUsers ?? string.Empty;
            txtSpecialRequestsNotes.Text = data.SpecialRequestsNotes ?? string.Empty;

            nudPreliminaryValueLow.Value = ClampDecimal(
                data.PreliminaryValueLow ?? 0m,
                nudPreliminaryValueLow.Minimum,
                nudPreliminaryValueLow.Maximum);

            nudPreliminaryValueHigh.Value = ClampDecimal(
                data.PreliminaryValueHigh ?? 0m,
                nudPreliminaryValueHigh.Minimum,
                nudPreliminaryValueHigh.Maximum);

            SelectComboValue(cboCurrency, data.Currency ?? "USD");
            txtPreliminaryValueNotes.Text = data.PreliminaryValueNotes ?? string.Empty;

            SelectedClientId = data.ClientId;
            txtClient.Text = data.ClientDisplayName ?? string.Empty;
            SelectComboValue(cboRelationshipToClient, data.RelationshipToClient);
            txtPrimaryContact.Text = data.PrimaryContact ?? string.Empty;
            txtPhone.Text = data.Phone ?? string.Empty;
            txtEmail.Text = data.Email ?? string.Empty;
            chkAddToContacts.Checked = data.AddToContacts;

            if (data.NeededBy.HasValue)
            {
                dtpNeededBy.Checked = true;
                dtpNeededBy.Value = data.NeededBy.Value.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                dtpNeededBy.Checked = false;
                dtpNeededBy.Value = DateTime.Today;
            }

            SelectComboValue(cboFlexibility, data.Flexibility);
            SelectComboValue(cboPreferredInspectionWindow, data.PreferredInspectionWindow);
            txtTimingNotes.Text = data.TimingNotes ?? string.Empty;

            SelectComboValue(cboSubjectLocation, data.SubjectLocation);
            txtAddress.Text = data.Address ?? string.Empty;
            txtAccessAvailability.Text = data.AccessAvailability ?? string.Empty;
            SelectComboValue(cboWillVehicleBeDriven, data.WillVehicleBeDriven);
            SelectComboValue(cboTransportRequired, data.TransportRequired);

            SelectComboValue(cboReferredBy, data.ReferredBy);
            txtReferralContact.Text = data.ReferralContact ?? string.Empty;
            txtReferralOther.Text = data.ReferralOther ?? string.Empty;
        }
        finally
        {
            _suppressDirtyTracking = false;
        }

        ValidateValueRange();
        SetDirty(false);
    }

    public RequestInformationTabData ReadValue()
    {
        return new RequestInformationTabData
        {
            RequestPurposeDescription = NullIfWhiteSpace(txtRequestPurposeDescription.Text),
            RequestedAppraisalType = SelectedText(cboRequestedAppraisalType),
            StatedPurpose = SelectedText(cboStatedPurpose),
            RequestedIntendedUse = SelectedText(cboRequestedIntendedUse),
            StatedIntendedUsers = NullIfWhiteSpace(txtStatedIntendedUsers.Text),
            SpecialRequestsNotes = NullIfWhiteSpace(txtSpecialRequestsNotes.Text),

            PreliminaryValueLow = nudPreliminaryValueLow.Value > 0m
                ? nudPreliminaryValueLow.Value
                : null,
            PreliminaryValueHigh = nudPreliminaryValueHigh.Value > 0m
                ? nudPreliminaryValueHigh.Value
                : null,
            Currency = SelectedText(cboCurrency),
            PreliminaryValueNotes = NullIfWhiteSpace(txtPreliminaryValueNotes.Text),

            ClientId = SelectedClientId,
            ClientDisplayName = NullIfWhiteSpace(txtClient.Text),
            RelationshipToClient = SelectedText(cboRelationshipToClient),
            PrimaryContact = NullIfWhiteSpace(txtPrimaryContact.Text),
            Phone = NullIfWhiteSpace(txtPhone.Text),
            Email = NullIfWhiteSpace(txtEmail.Text),
            AddToContacts = chkAddToContacts.Checked,

            NeededBy = dtpNeededBy.Checked
                ? DateOnly.FromDateTime(dtpNeededBy.Value)
                : null,
            Flexibility = SelectedText(cboFlexibility),
            PreferredInspectionWindow = SelectedText(cboPreferredInspectionWindow),
            TimingNotes = NullIfWhiteSpace(txtTimingNotes.Text),

            SubjectLocation = SelectedText(cboSubjectLocation),
            Address = NullIfWhiteSpace(txtAddress.Text),
            AccessAvailability = NullIfWhiteSpace(txtAccessAvailability.Text),
            WillVehicleBeDriven = SelectedText(cboWillVehicleBeDriven),
            TransportRequired = SelectedText(cboTransportRequired),

            ReferredBy = SelectedText(cboReferredBy),
            ReferralContact = NullIfWhiteSpace(txtReferralContact.Text),
            ReferralOther = NullIfWhiteSpace(txtReferralOther.Text)
        };
    }

    /// <summary>
    /// Applies state-driven editability. Converted requests are historically preserved,
    /// so the parent form should call this with false after conversion.
    /// </summary>
    public void SetEditable(bool editable)
    {
        foreach (Control control in EnumerateControls(this))
        {
            switch (control)
            {
                case TextBoxBase textBox:
                    textBox.ReadOnly = !editable;
                    break;

                case ComboBox comboBox:
                    comboBox.Enabled = editable;
                    break;

                case NumericUpDown numeric:
                    numeric.ReadOnly = !editable;
                    numeric.Enabled = editable;
                    break;

                case DateTimePicker datePicker:
                    datePicker.Enabled = editable;
                    break;

                case CheckBox checkBox:
                    checkBox.Enabled = editable;
                    break;
            }
        }

        btnBrowseClient.Enabled = editable;

        // Read-only text boxes retain normal readable background.
        foreach (var textBox in EnumerateControls(this).OfType<TextBoxBase>())
            textBox.BackColor = NexusTheme.Surface;
    }

    /// <summary>
    /// Allows a client-lookup dialog owned by the parent form to set the resolved client.
    /// </summary>
    public void SetClient(Guid? clientId, string? displayName)
    {
        SelectedClientId = clientId;
        txtClient.Text = displayName ?? string.Empty;
        MarkDirty();
    }

    /// <summary>
    /// Performs UI-level validation only. API/domain validation remains authoritative.
    /// </summary>
    public bool ValidateTab(out IReadOnlyList<string> errors)
    {
        var list = new List<string>();

        if (string.IsNullOrWhiteSpace(txtRequestPurposeDescription.Text))
            list.Add("Request Purpose / Description is required.");

        if (string.IsNullOrWhiteSpace(SelectedText(cboRequestedAppraisalType)))
            list.Add("Appraisal Type (Requested) is required.");

        if (string.IsNullOrWhiteSpace(SelectedText(cboStatedPurpose)))
            list.Add("Purpose (Stated) is required.");

        if (nudPreliminaryValueLow.Value > 0m &&
            nudPreliminaryValueHigh.Value > 0m &&
            nudPreliminaryValueHigh.Value < nudPreliminaryValueLow.Value)
        {
            list.Add("Preliminary high value cannot be lower than preliminary low value.");
        }

        if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
            !LooksLikeEmail(txtEmail.Text))
        {
            list.Add("Requester email address does not appear to be valid.");
        }

        errors = list;
        return list.Count == 0;
    }

    public void AcceptChanges() => SetDirty(false);

    private void SeedLookupValues()
    {
        AddItems(cboRequestedAppraisalType,
            "",
            "Fair Market Value",
            "Insurance / Replacement Value",
            "Market Value",
            "Estate / Date-of-Death",
            "Charitable Contribution",
            "Litigation / Equitable Distribution",
            "Advisory / Other");

        AddItems(cboStatedPurpose,
            "",
            "Insurance",
            "Estate Planning",
            "Estate Settlement",
            "Equitable Distribution",
            "Financing",
            "Litigation",
            "Acquisition",
            "Disposition",
            "Charitable Contribution",
            "Tax-Related",
            "Collection Management",
            "Advisory",
            "Other");

        AddItems(cboRequestedIntendedUse,
            "",
            "Insurance",
            "Estate Planning",
            "Estate Settlement",
            "Equitable Distribution",
            "Financing",
            "Litigation",
            "Acquisition",
            "Disposition",
            "Charitable Contribution",
            "Tax-Related",
            "Collection Management",
            "Advisory",
            "Other");

        AddItems(cboCurrency, "USD", "CAD", "EUR", "GBP");

        AddItems(cboRelationshipToClient,
            "",
            "Self",
            "Owner",
            "Family Member",
            "Personal Representative",
            "Executor / Administrator",
            "Attorney",
            "Trustee",
            "Agent",
            "Insurer",
            "Other");

        AddItems(cboFlexibility,
            "",
            "Flexible",
            "Somewhat Flexible",
            "Fixed Date",
            "Urgent");

        AddItems(cboPreferredInspectionWindow,
            "",
            "Weekdays",
            "Weekends",
            "Morning",
            "Afternoon",
            "Evening",
            "By Appointment",
            "Undetermined");

        AddItems(cboSubjectLocation,
            "",
            "Client Residence",
            "Client Business",
            "Storage Facility",
            "Dealer / Shop",
            "Museum / Collection",
            "Auction / Venue",
            "Other");

        AddItems(cboWillVehicleBeDriven, "", "Yes", "No", "Unknown");
        AddItems(cboTransportRequired, "", "Yes", "No", "Unknown");

        AddItems(cboReferredBy,
            "",
            "Existing Client",
            "Dealer",
            "Collector Club",
            "Attorney",
            "Insurance Professional",
            "Financial / Estate Professional",
            "Web Search",
            "Social Media",
            "Event / Show",
            "Other");

        cboCurrency.SelectedItem = "USD";
    }

    private void ApplyNexusTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;
        ForeColor = NexusTheme.Text;
        Font = NexusTheme.BodyFont;

        foreach (var group in EnumerateControls(this).OfType<GroupBox>())
        {
            group.BackColor = NexusTheme.Surface;
            group.ForeColor = NexusTheme.Primary;
            group.Font = NexusTheme.LabelFont;
        }

        foreach (var label in EnumerateControls(this).OfType<Label>())
        {
            label.ForeColor = NexusTheme.Text;
            label.Font = NexusTheme.LabelFont;
        }

        foreach (var textBox in EnumerateControls(this).OfType<TextBoxBase>())
        {
            textBox.BackColor = NexusTheme.Surface;
            textBox.ForeColor = NexusTheme.Text;
            textBox.Font = NexusTheme.BodyFont;
        }

        foreach (var combo in EnumerateControls(this).OfType<ComboBox>())
        {
            combo.BackColor = NexusTheme.Surface;
            combo.ForeColor = NexusTheme.Text;
            combo.Font = NexusTheme.BodyFont;
        }

        foreach (var numeric in EnumerateControls(this).OfType<NumericUpDown>())
        {
            numeric.BackColor = NexusTheme.Surface;
            numeric.ForeColor = NexusTheme.Text;
            numeric.Font = NexusTheme.BodyFont;
        }

        btnBrowseClient.BackColor = NexusTheme.Surface;
        btnBrowseClient.ForeColor = NexusTheme.Primary;
        btnBrowseClient.FlatStyle = FlatStyle.Flat;
        btnBrowseClient.FlatAppearance.BorderColor = Color.Silver;

        lblProvisionalNotice.ForeColor = NexusTheme.TextMuted;
        lblProvisionalNotice.Font = NexusTheme.MetaFont;
    }

    private void WireDirtyTracking()
    {
        foreach (Control control in EnumerateControls(this))
        {
            switch (control)
            {
                case TextBoxBase textBox:
                    textBox.TextChanged += (_, _) => MarkDirty();
                    break;
                case ComboBox comboBox:
                    comboBox.SelectedIndexChanged += (_, _) => MarkDirty();
                    break;
                case NumericUpDown numeric:
                    numeric.ValueChanged += (_, _) => MarkDirty();
                    break;
                case DateTimePicker datePicker:
                    datePicker.ValueChanged += (_, _) => MarkDirty();
                    break;
                case CheckBox checkBox:
                    checkBox.CheckedChanged += (_, _) => MarkDirty();
                    break;
            }
        }

        nudPreliminaryValueLow.ValueChanged += (_, _) => ValidateValueRange();
        nudPreliminaryValueHigh.ValueChanged += (_, _) => ValidateValueRange();

        btnBrowseClient.Click += (_, _) => BrowseClientRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ValidateValueRange()
    {
        var invalid =
            nudPreliminaryValueLow.Value > 0m &&
            nudPreliminaryValueHigh.Value > 0m &&
            nudPreliminaryValueHigh.Value < nudPreliminaryValueLow.Value;

        errorProvider.SetError(
            nudPreliminaryValueHigh,
            invalid ? "High value must be greater than or equal to low value." : string.Empty);
    }

    private void MarkDirty()
    {
        if (_suppressDirtyTracking)
            return;

        SetDirty(true);
    }

    private void SetDirty(bool value)
    {
        if (_isDirty == value)
            return;

        _isDirty = value;
        DirtyStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private static IEnumerable<Control> EnumerateControls(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;

            foreach (var nested in EnumerateControls(child))
                yield return nested;
        }
    }

    private static void AddItems(ComboBox combo, params string[] values)
    {
        combo.BeginUpdate();
        try
        {
            combo.Items.Clear();
            combo.Items.AddRange(values);
        }
        finally
        {
            combo.EndUpdate();
        }
    }

    private static void SelectComboValue(ComboBox combo, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            combo.SelectedIndex = combo.Items.Count > 0 ? 0 : -1;
            return;
        }

        var index = combo.FindStringExact(value);
        if (index >= 0)
        {
            combo.SelectedIndex = index;
            return;
        }

        combo.Items.Add(value);
        combo.SelectedItem = value;
    }

    private static string? SelectedText(ComboBox combo)
    {
        var value = combo.SelectedItem?.ToString();
        return NullIfWhiteSpace(value);
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal ClampDecimal(decimal value, decimal min, decimal max) =>
        Math.Min(max, Math.Max(min, value));

    private static bool LooksLikeEmail(string value)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(value.Trim());
            return string.Equals(address.Address, value.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}

public sealed record RequestInformationTabData
{
    public string? RequestPurposeDescription { get; init; }
    public string? RequestedAppraisalType { get; init; }
    public string? StatedPurpose { get; init; }
    public string? RequestedIntendedUse { get; init; }
    public string? StatedIntendedUsers { get; init; }
    public string? SpecialRequestsNotes { get; init; }

    public decimal? PreliminaryValueLow { get; init; }
    public decimal? PreliminaryValueHigh { get; init; }
    public string? Currency { get; init; }
    public string? PreliminaryValueNotes { get; init; }

    public Guid? ClientId { get; init; }
    public string? ClientDisplayName { get; init; }
    public string? RelationshipToClient { get; init; }
    public string? PrimaryContact { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public bool AddToContacts { get; init; }

    public DateOnly? NeededBy { get; init; }
    public string? Flexibility { get; init; }
    public string? PreferredInspectionWindow { get; init; }
    public string? TimingNotes { get; init; }

    public string? SubjectLocation { get; init; }
    public string? Address { get; init; }
    public string? AccessAvailability { get; init; }
    public string? WillVehicleBeDriven { get; init; }
    public string? TransportRequired { get; init; }

    public string? ReferredBy { get; init; }
    public string? ReferralContact { get; init; }
    public string? ReferralOther { get; init; }
}
