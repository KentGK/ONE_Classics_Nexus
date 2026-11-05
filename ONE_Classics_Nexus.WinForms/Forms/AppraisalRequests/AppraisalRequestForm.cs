using ONE_Classics_Nexus.WinForms.Theming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Services.Api;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

/// <summary>
/// Main Appraisal Request workspace form.
///
/// This form owns the intake-shell experience:
/// - record command bar
/// - request summary/header
/// - request status card
/// - linked-record card
/// - quick notes
/// - attachments
/// - seven-tab intake workspace
///
/// AppraisalRequest remains a pre-appraisal intake aggregate. The parent form
/// must not treat client/requester statements as approved Standard 7 facts.
/// </summary>
public partial class AppraisalRequestForm : Form
{
    private bool _suppressDirtyTracking;
    private bool _isDirty;

    private AppraisalRequestInformationTab? _requestInformationControl;
    private AppraisalRequestPartyIdentificationTab? _partyIdentificationControl;
    private AppraisalRequestVehicleTab? _vehicleControl;
    private AppraisalRequestNotesAttachmentsTab? _notesAttachmentsControl;
    private readonly AppraisalRequestPartyIdentificationController? _partyIdentificationController;

    public AppraisalRequestForm()
        : this(null)
    {
    }

    public AppraisalRequestForm(AppraisalRequestApiClient? appraisalRequestApiClient)
    {
        InitializeComponent();
        ApplyNexusTheme();
        SeedLookups();
        InitializeTabHosts();

        if (appraisalRequestApiClient is not null)
            _partyIdentificationController = new AppraisalRequestPartyIdentificationController(this, appraisalRequestApiClient);

        WireUiEvents();
        SetDirty(false);
    }

    #region Public events

    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? SaveAndCloseRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? RefreshRequested;
    public event EventHandler? PrintRequested;
    public event EventHandler? CloseRequested;

    public event EventHandler? ClientLinkRequested;
    public event EventHandler? VehicleLinkRequested;
    public event EventHandler? AppraisalLinkRequested;

    public event EventHandler? SaveQuickNoteRequested;
    public event EventHandler? AddAttachmentRequested;
    public event EventHandler<AppraisalRequestAttachmentEventArgs>? AttachmentOpenRequested;
    public event EventHandler<AppraisalRequestAttachmentEventArgs>? AttachmentRemoveRequested;

    public event EventHandler? DirtyStateChanged;

    #endregion

    #region Public state

    public bool IsDirty => _isDirty;

    public Guid? AppraisalRequestId { get; private set; }
    public long RowVersion { get; private set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AppraisalRequestInformationTab RequestInformationTab =>
        _requestInformationControl
        ?? throw new InvalidOperationException("Request Information tab has not been initialized.");

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AppraisalRequestPartyIdentificationTab PartyIdentificationTab =>
        _partyIdentificationControl
        ?? throw new InvalidOperationException("Party Identification tab has not been initialized.");

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AppraisalRequestVehicleTab VehicleTab =>
        _vehicleControl
        ?? throw new InvalidOperationException("Vehicle tab has not been initialized.");

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AppraisalRequestNotesAttachmentsTab NotesAttachmentsTab =>
        _notesAttachmentsControl
        ?? throw new InvalidOperationException("Notes and Attachments tab has not been initialized.");

    [Browsable(false)]
    public TabPage SubjectInformationTabPage => tabSubjectInformation;

    [Browsable(false)]
    public TabPage RequesterContactTabPage => tabRequesterContact;

    [Browsable(false)]
    public TabPage ServiceTimingTabPage => tabServiceTiming;

    [Browsable(false)]
    public TabPage IntakeDetailsTabPage => tabIntakeDetails;

    [Browsable(false)]
    public TabPage ReviewDispositionTabPage => tabReviewDisposition;

    [Browsable(false)]
    public TabPage HistoryTabPage => tabHistory;

    #endregion

    #region Binding

    public void Bind(AppraisalRequestFormData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _suppressDirtyTracking = true;
        try
        {
            AppraisalRequestId = data.AppraisalRequestId;
            RowVersion = data.RowVersion;

            if (AppraisalRequestId.HasValue && _partyIdentificationController is not null)
                _ = LoadPartyIdentificationAsync(AppraisalRequestId.Value);

            txtRequestNumber.Text = data.RequestNumber ?? string.Empty;
            txtDateCreated.Text = FormatDate(data.CreatedAtLocal);
            txtCreatedBy.Text = data.CreatedBy ?? string.Empty;

            SelectComboValue(cboStatus, data.Status);
            SelectComboValue(cboPriority, data.Priority ?? "Normal");
            SelectComboValue(cboSource, data.Source);
            txtLastUpdated.Text = FormatDateTime(data.UpdatedAtLocal);

            SetStatusSummary(
                data.Status,
                data.Priority,
                data.QualificationStatus,
                data.Disposition);

            SetLinkedRecords(
                data.ClientDisplayName,
                data.ClientId,
                data.VehicleDisplayName,
                data.VehicleId,
                data.AppraisalDisplayName,
                data.ConvertedAppraisalId);

            txtQuickNote.Text = string.Empty;

            SetAttachments(data.Attachments);

            lblRecordIdValue.Text = data.AppraisalRequestId?.ToString() ?? "Not yet saved";
            lblFooterCreatedByValue.Text = data.CreatedBy ?? "—";
            lblFooterCreatedAtValue.Text = FormatDateTime(data.CreatedAtLocal);
            lblFooterUpdatedByValue.Text = data.UpdatedBy ?? "—";
            lblFooterUpdatedAtValue.Text = FormatDateTime(data.UpdatedAtLocal);

            RequestInformationTab.Bind(data.RequestInformation ?? new RequestInformationTabData());
            PartyIdentificationTab.Bind(data.PartyIdentification);

            SetConvertedLock(
                string.Equals(data.Status, "Converted", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            _suppressDirtyTracking = false;
        }

        SetDirty(false);
    }

    public AppraisalRequestHeaderData ReadHeader()
    {
        return new AppraisalRequestHeaderData
        {
            AppraisalRequestId = AppraisalRequestId,
            RequestNumber = NullIfWhiteSpace(txtRequestNumber.Text),
            Status = SelectedText(cboStatus),
            Priority = SelectedText(cboPriority),
            Source = SelectedText(cboSource),
            RowVersion = RowVersion
        };
    }

    private async Task LoadPartyIdentificationAsync(Guid appraisalRequestId)
    {
        if (_partyIdentificationController is null)
            return;

        try
        {
            await _partyIdentificationController.LoadAsync(appraisalRequestId).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            if (IsDisposed)
                return;

            MessageBox.Show(
                this,
                $"Unable to load party identification.\n\n{ex.Message}",
                "Appraisal Request",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    public string? ReadQuickNote() => NullIfWhiteSpace(txtQuickNote.Text);

    public void ClearQuickNote()
    {
        _suppressDirtyTracking = true;
        try
        {
            txtQuickNote.Clear();
        }
        finally
        {
            _suppressDirtyTracking = false;
        }

        btnSaveQuickNote.Enabled = false;
    }

    public void AcceptChanges()
    {
        RequestInformationTab.AcceptChanges();
        PartyIdentificationTab.AcceptChanges();
        VehicleTab.AcceptChanges();
        NotesAttachmentsTab.AcceptChanges();
        SetDirty(false);
    }

    #endregion

    #region Status / linked records / attachments

    public void SetStatusSummary(
        string? status,
        string? priority,
        string? qualification,
        string? disposition)
    {
        lblStatusValue.Text = DisplayOrDash(status);
        lblPriorityValue.Text = DisplayOrDash(priority);
        lblQualificationValue.Text = DisplayOrDash(qualification);
        lblDispositionValue.Text = DisplayOrDash(disposition);

        pnlStatusBadge.BackColor = GetStatusBadgeColor(status);
        lblStatusValue.ForeColor = GetStatusBadgeTextColor(status);
    }

    public void SetLinkedRecords(
        string? clientDisplayName,
        Guid? clientId,
        string? vehicleDisplayName,
        Guid? vehicleId,
        string? appraisalDisplayName,
        Guid? appraisalId)
    {
        lnkClient.Text = !string.IsNullOrWhiteSpace(clientDisplayName)
            ? clientDisplayName
            : clientId.HasValue ? clientId.Value.ToString() : "Not Linked";

        lnkVehicle.Text = !string.IsNullOrWhiteSpace(vehicleDisplayName)
            ? vehicleDisplayName
            : vehicleId.HasValue ? vehicleId.Value.ToString() : "Not Linked";

        lnkAppraisal.Text = !string.IsNullOrWhiteSpace(appraisalDisplayName)
            ? appraisalDisplayName
            : appraisalId.HasValue ? appraisalId.Value.ToString() : "Not Created";

        lnkClient.Enabled = clientId.HasValue || !string.IsNullOrWhiteSpace(clientDisplayName);
        lnkVehicle.Enabled = vehicleId.HasValue || !string.IsNullOrWhiteSpace(vehicleDisplayName);
        lnkAppraisal.Enabled = appraisalId.HasValue || !string.IsNullOrWhiteSpace(appraisalDisplayName);
    }

    public void SetAttachments(IEnumerable<AppraisalRequestAttachmentListItem>? attachments)
    {
        lvAttachments.BeginUpdate();
        try
        {
            lvAttachments.Items.Clear();

            foreach (var attachment in attachments ?? Enumerable.Empty<AppraisalRequestAttachmentListItem>())
            {
                var item = new ListViewItem(attachment.FileName)
                {
                    Tag = attachment
                };

                item.SubItems.Add(attachment.DisplaySize ?? string.Empty);
                item.SubItems.Add(
                    attachment.AddedAtLocal.HasValue
                        ? attachment.AddedAtLocal.Value.ToString("g")
                        : string.Empty);

                lvAttachments.Items.Add(item);
            }
        }
        finally
        {
            lvAttachments.EndUpdate();
        }

        UpdateAttachmentCaption();
    }

    public void AddAttachment(AppraisalRequestAttachmentListItem attachment)
    {
        var current = lvAttachments.Items
            .Cast<ListViewItem>()
            .Select(x => x.Tag)
            .OfType<AppraisalRequestAttachmentListItem>()
            .ToList();

        current.Add(attachment);
        SetAttachments(current);
    }

    public void RemoveAttachment(Guid attachmentId)
    {
        var match = lvAttachments.Items
            .Cast<ListViewItem>()
            .FirstOrDefault(x =>
                x.Tag is AppraisalRequestAttachmentListItem a &&
                a.AttachmentId == attachmentId);

        if (match != null)
            lvAttachments.Items.Remove(match);

        UpdateAttachmentCaption();
    }

    #endregion

    #region Tab hosting

    /// <summary>
    /// Replaces the default Subject Information placeholder with a production control.
    /// </summary>
    public void SetSubjectInformationControl(Control control) =>
        SetTabContent(tabSubjectInformation, control);

    public void SetRequesterContactControl(Control control) =>
        SetTabContent(tabRequesterContact, control);

    public void SetServiceTimingControl(Control control) =>
        SetTabContent(tabServiceTiming, control);

    public void SetIntakeDetailsControl(Control control) =>
        SetTabContent(tabIntakeDetails, control);

    public void SetReviewDispositionControl(Control control) =>
        SetTabContent(tabReviewDisposition, control);

    public void SetHistoryControl(Control control) =>
        SetTabContent(tabHistory, control);

    private void InitializeTabHosts()
    {
        _requestInformationControl = new AppraisalRequestInformationTab
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty
        };

        _requestInformationControl.DirtyStateChanged += (_, _) =>
        {
            if (_requestInformationControl.IsDirty)
                MarkDirty();
        };

        SetTabContent(tabRequestInformation, _requestInformationControl);

        _partyIdentificationControl = new AppraisalRequestPartyIdentificationTab
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty
        };

        _partyIdentificationControl.DirtyStateChanged += (_, _) =>
        {
            if (_partyIdentificationControl.IsDirty)
                MarkDirty();
        };

        SetTabContent(tabRequesterContact, _partyIdentificationControl);

        _vehicleControl = new AppraisalRequestVehicleTab
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty
        };

        _vehicleControl.DirtyStateChanged += (_, _) =>
        {
            if (_vehicleControl.IsDirty)
                MarkDirty();
        };

        SetTabContent(tabSubjectInformation, _vehicleControl);

        _notesAttachmentsControl = new AppraisalRequestNotesAttachmentsTab
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty
        };

        _notesAttachmentsControl.DirtyStateChanged += (_, _) =>
        {
            if (_notesAttachmentsControl.IsDirty)
                MarkDirty();
        };

        SetTabContent(tabServiceTiming, _notesAttachmentsControl);

        BuildPlaceholder(
            tabIntakeDetails,
            "Review",
            "Validation status, workflow checks, and review notes before submission or lifecycle transitions.");

        BuildPlaceholder(
            tabReviewDisposition,
            "Disposition",
            "Qualification outcomes, assignment updates, and acceptance/decline/cancel/withdraw actions.");

        BuildPlaceholder(
            tabHistory,
            "History",
            "Chronological request events, status transitions, notes, attachments, assignments, audit events, and conversion history.");
    }

    private void SetTabContent(TabPage tabPage, Control control)
    {
        ArgumentNullException.ThrowIfNull(control);

        tabPage.SuspendLayout();
        try
        {
            tabPage.Controls.Clear();
            control.Dock = DockStyle.Fill;
            control.Margin = Padding.Empty;
            tabPage.Controls.Add(control);
        }
        finally
        {
            tabPage.ResumeLayout();
        }
    }

    private static void BuildPlaceholder(TabPage page, string title, string description)
    {
        var host = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(28)
        };

        var titleLabel = new Label
        {
            AutoSize = true,
            Text = title,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(14, 40, 65),
            Location = new Point(28, 28)
        };

        var descriptionLabel = new Label
        {
            AutoSize = false,
            Text = description,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(95, 105, 115),
            Location = new Point(30, 68),
            Size = new Size(760, 52)
        };

        var noteLabel = new Label
        {
            AutoSize = true,
            Text = "Host surface ready for its dedicated WinForms control.",
            Font = new Font("Segoe UI", 9F, FontStyle.Italic),
            ForeColor = Color.FromArgb(125, 130, 138),
            Location = new Point(30, 130)
        };

        host.Controls.Add(titleLabel);
        host.Controls.Add(descriptionLabel);
        host.Controls.Add(noteLabel);

        page.Controls.Clear();
        page.Controls.Add(host);
    }

    #endregion

    #region Editability / workflow lock

    public void SetConvertedLock(bool converted)
    {
        // Request material facts become historically preserved after conversion.
        var editable = !converted;

        // Status remains enabled so the supported Converted -> Closed transition
        // can still be executed through the parent/controller workflow.
        cboStatus.Enabled = true;
        cboPriority.Enabled = editable;
        cboSource.Enabled = editable;

        RequestInformationTab.SetEditable(editable);
        PartyIdentificationTab.SetEditable(editable);
        VehicleTab.SetEditable(editable);
        NotesAttachmentsTab.SetEditable(editable);

        btnDelete.Enabled = editable;
        btnAddAttachment.Enabled = editable;
        btnSaveQuickNote.Enabled = editable && !string.IsNullOrWhiteSpace(txtQuickNote.Text);

        lblEditMode.Text = converted ? "Locked • Converted" : "Edit Mode";
        lblEditMode.ForeColor = converted ? Color.FromArgb(150, 75, 0) : NexusTheme.TextMuted;
    }

    public void SetEditModeText(string text)
    {
        lblEditMode.Text = string.IsNullOrWhiteSpace(text) ? "Edit Mode" : text.Trim();
    }

    #endregion

    #region Validation

    public bool ValidateForm(out IReadOnlyList<string> errors)
    {
        var list = new List<string>();

        if (string.IsNullOrWhiteSpace(SelectedText(cboStatus)))
            list.Add("Request Status is required.");

        if (!RequestInformationTab.ValidateTab(out var requestTabErrors))
            list.AddRange(requestTabErrors);

        if (!PartyIdentificationTab.ValidateTab(out var partyErrors))
            list.AddRange(partyErrors);

        errors = list;
        return list.Count == 0;
    }

    #endregion

    #region Theme

    private void ApplyNexusTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;
        ForeColor = NexusTheme.TextStrong;
        Font = NexusTheme.BodyFont;

        pnlCommandBar.BackColor = NexusTheme.Surface;
        pnlHeader.BackColor = NexusTheme.Surface;
        pnlRightSidebar.BackColor = NexusTheme.SurfaceAlt;
        pnlFooter.BackColor = NexusTheme.Surface;

        lblFormTitle.ForeColor = NexusTheme.TextStrong;
        lblFormTitle.Font = NexusTheme.H2Font;

        foreach (var group in EnumerateControls(this).OfType<GroupBox>())
        {
            group.BackColor = NexusTheme.Surface;
            group.ForeColor = NexusTheme.Primary;
            group.Font = NexusTheme.LabelFont;
        }

        foreach (var label in EnumerateControls(this).OfType<Label>())
        {
            if (ReferenceEquals(label, lblStatusValue))
                continue;

            label.ForeColor = NexusTheme.TextStrong;
        }

        foreach (var textBox in EnumerateControls(this).OfType<TextBoxBase>())
        {
            textBox.BackColor = NexusTheme.Surface;
            textBox.ForeColor = NexusTheme.TextStrong;
            textBox.Font = NexusTheme.BodyFont;
        }

        foreach (var combo in EnumerateControls(this).OfType<ComboBox>())
        {
            combo.BackColor = NexusTheme.Surface;
            combo.ForeColor = NexusTheme.TextStrong;
            combo.Font = NexusTheme.BodyFont;
        }

        ConfigureCommandButton(btnNew, primary: false);
        ConfigureCommandButton(btnSave, primary: true);
        ConfigureCommandButton(btnSaveClose, primary: false);
        ConfigureCommandButton(btnDelete, primary: false);
        ConfigureCommandButton(btnRefresh, primary: false);
        ConfigureCommandButton(btnPrint, primary: false);
        ConfigureCommandButton(btnClose, primary: false);

        ConfigureSidebarButton(btnSaveQuickNote, primary: true);
        ConfigureSidebarButton(btnAddAttachment, primary: false);
        ConfigureSidebarButton(btnOpenAttachment, primary: false);
        ConfigureSidebarButton(btnRemoveAttachment, primary: false);

        lnkClient.LinkColor = NexusTheme.Primary;
        lnkVehicle.LinkColor = NexusTheme.Primary;
        lnkAppraisal.LinkColor = NexusTheme.Primary;

        tabMain.Font = NexusTheme.LabelFont;
    }

    private static void ConfigureCommandButton(Button button, bool primary)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = primary ? 0 : 1;
        button.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 218);
        button.BackColor = primary ? NexusTheme.Primary : NexusTheme.Surface;
        button.ForeColor = primary ? NexusTheme.White : NexusTheme.TextStrong;
        button.Cursor = Cursors.Hand;
    }

    private static void ConfigureSidebarButton(Button button, bool primary)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = primary ? 0 : 1;
        button.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 218);
        button.BackColor = primary ? NexusTheme.Primary : NexusTheme.Surface;
        button.ForeColor = primary ? Color.White : NexusTheme.Primary;
        button.Cursor = Cursors.Hand;
    }

    #endregion

    #region Lookup setup

    private void SeedLookups()
    {
        AddItems(cboStatus,
            "Draft",
            "Submitted",
            "UnderReview",
            "NeedsInformation",
            "Qualified",
            "Accepted",
            "Declined",
            "Cancelled",
            "Withdrawn",
            "Converted",
            "Closed");

        AddItems(cboPriority,
            "Low",
            "Normal",
            "High",
            "Urgent");

        AddItems(cboSource,
            "",
            "Phone Call",
            "Email",
            "Website",
            "Walk-In",
            "Referral",
            "Existing Client",
            "Event / Show",
            "Dealer",
            "Professional Referral",
            "Other");

        cboStatus.SelectedItem = "Draft";
        cboPriority.SelectedItem = "Normal";
    }

    #endregion

    #region Event wiring

    private void WireUiEvents()
    {
        btnNew.Click += (_, _) => NewRequested?.Invoke(this, EventArgs.Empty);
        btnSave.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        btnSaveClose.Click += (_, _) => SaveAndCloseRequested?.Invoke(this, EventArgs.Empty);
        btnDelete.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        btnPrint.Click += (_, _) => PrintRequested?.Invoke(this, EventArgs.Empty);
        btnClose.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);

        lnkClient.LinkClicked += (_, _) => ClientLinkRequested?.Invoke(this, EventArgs.Empty);
        lnkVehicle.LinkClicked += (_, _) => VehicleLinkRequested?.Invoke(this, EventArgs.Empty);
        lnkAppraisal.LinkClicked += (_, _) => AppraisalLinkRequested?.Invoke(this, EventArgs.Empty);

        txtQuickNote.TextChanged += (_, _) =>
        {
            btnSaveQuickNote.Enabled = !string.IsNullOrWhiteSpace(txtQuickNote.Text);
        };

        btnSaveQuickNote.Click += (_, _) =>
            SaveQuickNoteRequested?.Invoke(this, EventArgs.Empty);

        btnAddAttachment.Click += (_, _) =>
            AddAttachmentRequested?.Invoke(this, EventArgs.Empty);

        btnOpenAttachment.Click += (_, _) => RaiseSelectedAttachment(AttachmentOpenRequested);
        btnRemoveAttachment.Click += (_, _) => RaiseSelectedAttachment(AttachmentRemoveRequested);

        lvAttachments.DoubleClick += (_, _) => RaiseSelectedAttachment(AttachmentOpenRequested);
        lvAttachments.SelectedIndexChanged += (_, _) =>
        {
            var selected = lvAttachments.SelectedItems.Count == 1;
            btnOpenAttachment.Enabled = selected;
            btnRemoveAttachment.Enabled = selected;
        };

        cboStatus.SelectedIndexChanged += (_, _) =>
        {
            if (_suppressDirtyTracking)
                return;

            lblStatusValue.Text = DisplayOrDash(SelectedText(cboStatus));
            pnlStatusBadge.BackColor = GetStatusBadgeColor(SelectedText(cboStatus));
            lblStatusValue.ForeColor = GetStatusBadgeTextColor(SelectedText(cboStatus));
            MarkDirty();
        };

        cboPriority.SelectedIndexChanged += (_, _) =>
        {
            if (_suppressDirtyTracking)
                return;

            lblPriorityValue.Text = DisplayOrDash(SelectedText(cboPriority));
            MarkDirty();
        };

        cboSource.SelectedIndexChanged += (_, _) => MarkDirty();

        FormClosing += (_, e) =>
        {
            // The shell/controller owns unsaved-change prompting.
            // The form intentionally does not make persistence decisions itself.
        };

        FormClosed += (_, _) => _partyIdentificationController?.Dispose();
    }

    /// <summary>
    /// Defers splitter sizing until after first show so panel dimensions are available.
    /// </summary>
    private void AppraisalRequestForm_Shown(object? sender, EventArgs e)
    {
        BeginInvoke((MethodInvoker)ApplyWorkspaceSplitterLayout);
    }

    private void SplitWorkspace_SizeChanged(object? sender, EventArgs e)
    {
        ApplyWorkspaceSplitterLayout();
    }

    private void RaiseSelectedAttachment(
        EventHandler<AppraisalRequestAttachmentEventArgs>? handler)
    {
        if (handler == null || lvAttachments.SelectedItems.Count != 1)
            return;

        if (lvAttachments.SelectedItems[0].Tag is not AppraisalRequestAttachmentListItem attachment)
            return;

        handler.Invoke(this, new AppraisalRequestAttachmentEventArgs(attachment));
    }

    #endregion

    #region Dirty state

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
        lblEditMode.Text = value ? "Edit Mode • Unsaved" : "Edit Mode";
        DirtyStateChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Helpers

    private void UpdateAttachmentCaption()
    {
        grpAttachments.Text = $"Attachments ({lvAttachments.Items.Count})";
    }

    private static string DisplayOrDash(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();

    private static string FormatDate(DateTime? value) =>
        value.HasValue ? value.Value.ToString("MM/dd/yyyy") : string.Empty;

    private static string FormatDateTime(DateTime? value) =>
        value.HasValue ? value.Value.ToString("MM/dd/yyyy h:mm tt") : string.Empty;

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? SelectedText(ComboBox combo) =>
        NullIfWhiteSpace(combo.SelectedItem?.ToString());

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

    private static IEnumerable<Control> EnumerateControls(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;

            foreach (var nested in EnumerateControls(child))
                yield return nested;
        }
    }

    private static Color GetStatusBadgeColor(string? status) =>
        status?.Trim().ToUpperInvariant() switch
        {
            "CONVERTED" => Color.FromArgb(224, 242, 232),
            "ACCEPTED" => Color.FromArgb(224, 242, 232),
            "QUALIFIED" => Color.FromArgb(228, 241, 255),
            "UNDERREVIEW" => Color.FromArgb(255, 244, 214),
            "NEEDSINFORMATION" => Color.FromArgb(255, 244, 214),
            "DECLINED" => Color.FromArgb(250, 226, 226),
            "CANCELLED" => Color.FromArgb(239, 239, 239),
            "WITHDRAWN" => Color.FromArgb(239, 239, 239),
            _ => Color.FromArgb(226, 237, 252)
        };

    private static Color GetStatusBadgeTextColor(string? status) =>
        status?.Trim().ToUpperInvariant() switch
        {
            "CONVERTED" => Color.FromArgb(28, 107, 63),
            "ACCEPTED" => Color.FromArgb(28, 107, 63),
            "DECLINED" => Color.FromArgb(157, 50, 50),
            _ => Color.FromArgb(14, 40, 65)
        };

    #endregion
}

public sealed record AppraisalRequestHeaderData
{
    public Guid? AppraisalRequestId { get; init; }
    public string? RequestNumber { get; init; }
    public string? Status { get; init; }
    public string? Priority { get; init; }
    public string? Source { get; init; }
    public long RowVersion { get; init; }
}

public sealed record AppraisalRequestFormData
{
    public Guid? AppraisalRequestId { get; init; }
    public string? RequestNumber { get; init; }

    public string? Status { get; init; }
    public string? Priority { get; init; }
    public string? Source { get; init; }

    public string? QualificationStatus { get; init; }
    public string? Disposition { get; init; }

    public Guid? ClientId { get; init; }
    public string? ClientDisplayName { get; init; }

    public Guid? VehicleId { get; init; }
    public string? VehicleDisplayName { get; init; }

    public Guid? ConvertedAppraisalId { get; init; }
    public string? AppraisalDisplayName { get; init; }

    public string? CreatedBy { get; init; }
    public string? UpdatedBy { get; init; }
    public DateTime? CreatedAtLocal { get; init; }
    public DateTime? UpdatedAtLocal { get; init; }

    public long RowVersion { get; init; }

    public RequestInformationTabData? RequestInformation { get; init; }
    public PartyIdentificationResponse? PartyIdentification { get; init; }

    public IReadOnlyList<AppraisalRequestAttachmentListItem> Attachments { get; init; }
        = Array.Empty<AppraisalRequestAttachmentListItem>();
}

public sealed record AppraisalRequestAttachmentListItem
{
    public Guid AttachmentId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string? DisplaySize { get; init; }
    public DateTime? AddedAtLocal { get; init; }
    public string? Uri { get; init; }
    public string? MediaType { get; init; }
}

public sealed class AppraisalRequestAttachmentEventArgs : EventArgs
{
    public AppraisalRequestAttachmentEventArgs(AppraisalRequestAttachmentListItem attachment)
    {
        Attachment = attachment;
    }

    public AppraisalRequestAttachmentListItem Attachment { get; }
}
