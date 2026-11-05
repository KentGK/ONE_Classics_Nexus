using System.ComponentModel;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public sealed class AppraisalRequestPartyIdentificationTab : UserControl
{
    private readonly TableLayoutPanel _rootLayout = new();
    private readonly Label _lblNotice = new();
    private readonly TableLayoutPanel _cardsLayout = new();
    private readonly GroupBox _grpClient = new();
    private readonly GroupBox _grpClientContact = new();
    private readonly GroupBox _grpIntendedUse = new();
    private readonly GroupBox _grpIntendedUsers = new();
    private readonly GroupBox _grpGuidance = new();
    private readonly GroupBox _grpValidation = new();

    private readonly Label _lblClientSummary = new();
    private readonly Button _btnSelectClient = new();
    private readonly Button _btnChangeClient = new();
    private readonly Button _btnOpenClient = new();

    private readonly ComboBox _cboPrimaryContact = new();
    private readonly Label _lblContactDetails = new();
    private readonly Button _btnAddContact = new();
    private readonly Button _btnEditContact = new();

    private readonly ComboBox _cboIntendedUse = new();
    private readonly TextBox _txtCustomNarrative = new();

    private readonly DataGridView _gridIntendedUsers = new();
    private readonly FlowLayoutPanel _usersCommandLayout = new();
    private readonly Button _btnAddClientAsUser = new();
    private readonly Button _btnClientNotNow = new();
    private readonly Button _btnClientNo = new();
    private readonly Button _btnAddUser = new();
    private readonly Button _btnEditUser = new();
    private readonly Button _btnRemoveUser = new();

    private readonly ListView _lvValidation = new();
    private readonly ColumnHeader _colSeverity = new() { Text = "Severity", Width = 90 };
    private readonly ColumnHeader _colCode = new() { Text = "Code", Width = 95 };
    private readonly ColumnHeader _colField = new() { Text = "Field", Width = 160 };
    private readonly ColumnHeader _colMessage = new() { Text = "Message", Width = 560 };
    private readonly Label _lblValidationBadge = new();
    private readonly Button _btnValidate = new();

    private bool _suppressEvents;
    private bool _isDirty;

    public AppraisalRequestPartyIdentificationTab()
    {
        BuildUi();
        ApplyTheme();
        SetDirty(false);
    }

    public event EventHandler? DirtyStateChanged;
    public event EventHandler? SelectClientRequested;
    public event EventHandler? ChangeClientRequested;
    public event EventHandler? OpenClientRequested;
    public event EventHandler? AddContactRequested;
    public event EventHandler? EditContactRequested;
    public event EventHandler? AddClientAsUserRequested;
    public event EventHandler? ClientNotNowRequested;
    public event EventHandler? ClientNoRequested;
    public event EventHandler? AddIntendedUserRequested;
    public event EventHandler? EditIntendedUserRequested;
    public event EventHandler? RemoveIntendedUserRequested;
    public event EventHandler? ValidateRequested;
    public event EventHandler<Guid?>? PrimaryClientContactChanged;
    public event EventHandler<Guid?>? ProposedIntendedUseChanged;

    public bool IsDirty => _isDirty;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Guid? SelectedPrimaryContactId
    {
        get => _cboPrimaryContact.SelectedItem is ContactOption option ? option.ContactId : null;
        set
        {
            if (!value.HasValue)
            {
                _cboPrimaryContact.SelectedIndex = -1;
                return;
            }

            for (var i = 0; i < _cboPrimaryContact.Items.Count; i++)
            {
                if (_cboPrimaryContact.Items[i] is ContactOption option && option.ContactId == value.Value)
                {
                    _cboPrimaryContact.SelectedIndex = i;
                    return;
                }
            }

            _cboPrimaryContact.SelectedIndex = -1;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Guid? SelectedIntendedUseId
    {
        get => _cboIntendedUse.SelectedItem is IntendedUseOption option ? option.IntendedUseId : null;
        set
        {
            if (!value.HasValue)
            {
                _cboIntendedUse.SelectedIndex = -1;
                return;
            }

            for (var i = 0; i < _cboIntendedUse.Items.Count; i++)
            {
                if (_cboIntendedUse.Items[i] is IntendedUseOption option && option.IntendedUseId == value.Value)
                {
                    _cboIntendedUse.SelectedIndex = i;
                    return;
                }
            }

            _cboIntendedUse.SelectedIndex = -1;
        }
    }

    public string? CustomNarrative => string.IsNullOrWhiteSpace(_txtCustomNarrative.Text) ? null : _txtCustomNarrative.Text.Trim();

    public void Bind(PartyIdentificationResponse? workspace)
    {
        _suppressEvents = true;
        try
        {
            if (workspace is null)
            {
                _lblClientSummary.Text = "No client selected.";
                _lblContactDetails.Text = "No contact selected.";
                _lblValidationBadge.Text = "Validation: not run";
                _txtCustomNarrative.Text = string.Empty;
                _cboPrimaryContact.Items.Clear();
                _cboIntendedUse.Items.Clear();
                _gridIntendedUsers.Rows.Clear();
                _lvValidation.Items.Clear();
                return;
            }

            _lblClientSummary.Text = workspace.Client is null
                ? "No client selected."
                : $"{workspace.Client.DisplayName} ({workspace.Client.ClientNumber})";

            _lblContactDetails.Text = workspace.PrimaryClientContact is null
                ? "No primary contact selected."
                : $"{workspace.PrimaryClientContact.ContactName} • {workspace.PrimaryClientContact.Email ?? "No email"} • {workspace.PrimaryClientContact.Phone ?? "No phone"}";

            SelectedPrimaryContactId = workspace.PrimaryClientContactId;
            SelectedIntendedUseId = workspace.IntendedUseId;
            _txtCustomNarrative.Text = workspace.ProposedIntendedUse?.CustomNarrative ?? string.Empty;

            _gridIntendedUsers.Rows.Clear();
            foreach (var user in workspace.ProposedIntendedUsers)
            {
                var rowIndex = _gridIntendedUsers.Rows.Add(
                    user.IsClient ? "Yes" : "No",
                    user.DisplayName,
                    user.OrganizationName ?? string.Empty,
                    user.RelationshipToClient,
                    user.UseRestrictions ?? string.Empty,
                    user.IsActive ? "Active" : "Inactive");

                _gridIntendedUsers.Rows[rowIndex].Tag = user;
            }

            BindValidation(workspace.Validation);
        }
        finally
        {
            _suppressEvents = false;
        }

        SetDirty(false);
    }

    public void SetContactOptions(IEnumerable<ClientContactSnapshot> contacts)
    {
        ArgumentNullException.ThrowIfNull(contacts);

        _suppressEvents = true;
        try
        {
            _cboPrimaryContact.Items.Clear();
            foreach (var contact in contacts)
                _cboPrimaryContact.Items.Add(new ContactOption(contact.ClientContactId, contact.ContactName));
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    public void SetIntendedUseOptions(IEnumerable<IntendedUseSnapshot> intendedUses)
    {
        ArgumentNullException.ThrowIfNull(intendedUses);

        _suppressEvents = true;
        try
        {
            _cboIntendedUse.Items.Clear();
            foreach (var use in intendedUses)
                _cboIntendedUse.Items.Add(new IntendedUseOption(use.IntendedUseId, use.UseCategoryName, use.IntendedUseName));
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    public void SetEditable(bool editable)
    {
        _btnSelectClient.Enabled = editable;
        _btnChangeClient.Enabled = editable;
        _btnAddContact.Enabled = editable;
        _btnEditContact.Enabled = editable;
        _btnAddClientAsUser.Enabled = editable;
        _btnClientNotNow.Enabled = editable;
        _btnClientNo.Enabled = editable;
        _btnAddUser.Enabled = editable;
        _btnEditUser.Enabled = editable;
        _btnRemoveUser.Enabled = editable;
        _btnValidate.Enabled = editable;
        _cboPrimaryContact.Enabled = editable;
        _cboIntendedUse.Enabled = editable;
        _txtCustomNarrative.ReadOnly = !editable;
    }

    public bool ValidateTab(out IReadOnlyList<string> errors)
    {
        var list = new List<string>();

        if (string.IsNullOrWhiteSpace(_lblClientSummary.Text) || _lblClientSummary.Text.Contains("No client selected", StringComparison.OrdinalIgnoreCase))
            list.Add("Client is required.");

        if (SelectedIntendedUseId is null)
            list.Add("Proposed intended use is required.");

        if (_gridIntendedUsers.Rows.Count == 0)
            list.Add("At least one proposed intended user is required.");

        errors = list;
        return list.Count == 0;
    }

    public void AcceptChanges() => SetDirty(false);

    public void SetValidationSummary(ValidationSummary summary)
    {
        ArgumentNullException.ThrowIfNull(summary);
        BindValidation(summary);
    }

    public bool TryGetSelectedProposedIntendedUser(out ProposedIntendedUserSnapshot? user)
    {
        if (_gridIntendedUsers.SelectedRows.Count == 1 &&
            _gridIntendedUsers.SelectedRows[0].Tag is ProposedIntendedUserSnapshot selected)
        {
            user = selected;
            return true;
        }

        user = null;
        return false;
    }

    private void BuildUi()
    {
        SuspendLayout();

        _rootLayout.ColumnCount = 1;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.RowCount = 2;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.Padding = Padding.Empty;

        _lblNotice.Dock = DockStyle.Fill;
        _lblNotice.Padding = new Padding(10, 0, 10, 0);
        _lblNotice.TextAlign = ContentAlignment.MiddleLeft;
        _lblNotice.Text = "Client/intended-user entries are proposed request facts and require formal Appraisal review before becoming authoritative.";

        _cardsLayout.ColumnCount = 2;
        _cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        _cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        _cardsLayout.RowCount = 4;
        _cardsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
        _cardsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        _cardsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _cardsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
        _cardsLayout.Dock = DockStyle.Fill;
        _cardsLayout.Padding = new Padding(8);

        ConfigureGroup(_grpClient, "Client");
        ConfigureGroup(_grpClientContact, "Primary Contact");
        ConfigureGroup(_grpIntendedUse, "Proposed Intended Use");
        ConfigureGroup(_grpIntendedUsers, "Proposed Intended Users");
        ConfigureGroup(_grpGuidance, "Workflow Guidance");
        ConfigureGroup(_grpValidation, "Validation Results");

        BuildClientCard();
        BuildContactCard();
        BuildIntendedUseCard();
        BuildIntendedUsersCard();
        BuildGuidanceCard();
        BuildValidationCard();

        _cardsLayout.Controls.Add(_grpClient, 0, 0);
        _cardsLayout.Controls.Add(_grpClientContact, 0, 1);
        _cardsLayout.Controls.Add(_grpIntendedUse, 1, 0);
        _cardsLayout.SetRowSpan(_grpIntendedUse, 2);
        _cardsLayout.Controls.Add(_grpIntendedUsers, 0, 2);
        _cardsLayout.SetColumnSpan(_grpIntendedUsers, 2);
        _cardsLayout.Controls.Add(_grpGuidance, 1, 3);
        _cardsLayout.Controls.Add(_grpValidation, 0, 3);

        _rootLayout.Controls.Add(_lblNotice, 0, 0);
        _rootLayout.Controls.Add(_cardsLayout, 0, 1);

        Controls.Add(_rootLayout);
        Dock = DockStyle.Fill;
        Padding = Padding.Empty;

        ResumeLayout(false);
    }

    private void BuildClientCard()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            RowCount = 2
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        _lblClientSummary.Dock = DockStyle.Fill;
        _lblClientSummary.Padding = new Padding(6);
        _lblClientSummary.Text = "No client selected.";

        var buttonLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        ConfigureActionButton(_btnSelectClient, "Select Client");
        ConfigureActionButton(_btnChangeClient, "Change Client");
        ConfigureActionButton(_btnOpenClient, "View Client");

        _btnSelectClient.Click += (_, _) => SelectClientRequested?.Invoke(this, EventArgs.Empty);
        _btnChangeClient.Click += (_, _) => ChangeClientRequested?.Invoke(this, EventArgs.Empty);
        _btnOpenClient.Click += (_, _) => OpenClientRequested?.Invoke(this, EventArgs.Empty);

        buttonLayout.Controls.Add(_btnSelectClient);
        buttonLayout.Controls.Add(_btnChangeClient);
        buttonLayout.Controls.Add(_btnOpenClient);

        layout.Controls.Add(_lblClientSummary, 0, 0);
        layout.Controls.Add(buttonLayout, 0, 1);
        _grpClient.Controls.Add(layout);
    }

    private void BuildContactCard()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            RowCount = 3
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        var lblContact = new Label
        {
            Text = "Primary Contact",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(3)
        };

        _cboPrimaryContact.Dock = DockStyle.Fill;
        _cboPrimaryContact.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboPrimaryContact.SelectedIndexChanged += (_, _) =>
        {
            MarkDirty();
            if (!_suppressEvents)
                PrimaryClientContactChanged?.Invoke(this, SelectedPrimaryContactId);
        };

        _lblContactDetails.Dock = DockStyle.Fill;
        _lblContactDetails.Text = "No contact selected.";
        _lblContactDetails.Padding = new Padding(6);

        var buttonLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        ConfigureActionButton(_btnAddContact, "Add Contact");
        ConfigureActionButton(_btnEditContact, "Edit Contact");

        _btnAddContact.Click += (_, _) => AddContactRequested?.Invoke(this, EventArgs.Empty);
        _btnEditContact.Click += (_, _) => EditContactRequested?.Invoke(this, EventArgs.Empty);

        buttonLayout.Controls.Add(_btnAddContact);
        buttonLayout.Controls.Add(_btnEditContact);

        layout.Controls.Add(lblContact, 0, 0);
        layout.Controls.Add(_cboPrimaryContact, 1, 0);
        layout.Controls.Add(_lblContactDetails, 0, 1);
        layout.SetColumnSpan(_lblContactDetails, 2);
        layout.Controls.Add(buttonLayout, 0, 2);
        layout.SetColumnSpan(buttonLayout, 2);

        _grpClientContact.Controls.Add(layout);
    }

    private void BuildIntendedUseCard()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            RowCount = 4
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var lblUse = new Label
        {
            Text = "Definition",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _cboIntendedUse.Dock = DockStyle.Fill;
        _cboIntendedUse.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboIntendedUse.SelectedIndexChanged += (_, _) =>
        {
            MarkDirty();
            if (!_suppressEvents)
                ProposedIntendedUseChanged?.Invoke(this, SelectedIntendedUseId);
        };

        var lblNarrative = new Label
        {
            Text = "Custom Narrative",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _txtCustomNarrative.Dock = DockStyle.Fill;
        _txtCustomNarrative.Multiline = true;
        _txtCustomNarrative.ScrollBars = ScrollBars.Vertical;
        _txtCustomNarrative.TextChanged += (_, _) => MarkDirty();

        layout.Controls.Add(lblUse, 0, 0);
        layout.Controls.Add(_cboIntendedUse, 0, 1);
        layout.Controls.Add(lblNarrative, 0, 2);
        layout.Controls.Add(_txtCustomNarrative, 0, 3);

        _grpIntendedUse.Controls.Add(layout);
    }

    private void BuildIntendedUsersCard()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            RowCount = 2
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _usersCommandLayout.Dock = DockStyle.Fill;
        _usersCommandLayout.FlowDirection = FlowDirection.LeftToRight;
        _usersCommandLayout.WrapContents = false;

        ConfigureActionButton(_btnAddClientAsUser, "Add Client As User");
        ConfigureActionButton(_btnClientNotNow, "Not Now");
        ConfigureActionButton(_btnClientNo, "Client Excluded");
        ConfigureActionButton(_btnAddUser, "Add User");
        ConfigureActionButton(_btnEditUser, "Edit User");
        ConfigureActionButton(_btnRemoveUser, "Remove User");

        _btnAddClientAsUser.Click += (_, _) => AddClientAsUserRequested?.Invoke(this, EventArgs.Empty);
        _btnClientNotNow.Click += (_, _) => ClientNotNowRequested?.Invoke(this, EventArgs.Empty);
        _btnClientNo.Click += (_, _) => ClientNoRequested?.Invoke(this, EventArgs.Empty);
        _btnAddUser.Click += (_, _) => AddIntendedUserRequested?.Invoke(this, EventArgs.Empty);
        _btnEditUser.Click += (_, _) => EditIntendedUserRequested?.Invoke(this, EventArgs.Empty);
        _btnRemoveUser.Click += (_, _) => RemoveIntendedUserRequested?.Invoke(this, EventArgs.Empty);

        _usersCommandLayout.Controls.Add(_btnAddClientAsUser);
        _usersCommandLayout.Controls.Add(_btnClientNotNow);
        _usersCommandLayout.Controls.Add(_btnClientNo);
        _usersCommandLayout.Controls.Add(_btnAddUser);
        _usersCommandLayout.Controls.Add(_btnEditUser);
        _usersCommandLayout.Controls.Add(_btnRemoveUser);

        _gridIntendedUsers.Dock = DockStyle.Fill;
        _gridIntendedUsers.AllowUserToAddRows = false;
        _gridIntendedUsers.AllowUserToDeleteRows = false;
        _gridIntendedUsers.AllowUserToResizeRows = false;
        _gridIntendedUsers.RowHeadersVisible = false;
        _gridIntendedUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridIntendedUsers.MultiSelect = false;
        _gridIntendedUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridIntendedUsers.ReadOnly = true;
        _gridIntendedUsers.Columns.Add("IsClient", "Client");
        _gridIntendedUsers.Columns.Add("DisplayName", "Name");
        _gridIntendedUsers.Columns.Add("OrganizationName", "Organization");
        _gridIntendedUsers.Columns.Add("RelationshipToClient", "Relationship");
        _gridIntendedUsers.Columns.Add("UseRestrictions", "Restrictions");
        _gridIntendedUsers.Columns.Add("State", "Status");

        layout.Controls.Add(_usersCommandLayout, 0, 0);
        layout.Controls.Add(_gridIntendedUsers, 0, 1);
        _grpIntendedUsers.Controls.Add(layout);
    }

    private void BuildGuidanceCard()
    {
        var guidance = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Client is the engaging party. Intended users are parties expected to rely on the appraisal report. Recipients are not automatically intended users.",
            Padding = new Padding(6)
        };

        _grpGuidance.Controls.Add(guidance);
    }

    private void BuildValidationCard()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
            RowCount = 2
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _lblValidationBadge.Dock = DockStyle.Fill;
        _lblValidationBadge.TextAlign = ContentAlignment.MiddleLeft;
        _lblValidationBadge.Text = "Validation: not run";

        _btnValidate.Text = "Validate";
        _btnValidate.Dock = DockStyle.Fill;
        _btnValidate.Click += (_, _) => ValidateRequested?.Invoke(this, EventArgs.Empty);

        _lvValidation.Dock = DockStyle.Fill;
        _lvValidation.FullRowSelect = true;
        _lvValidation.HideSelection = false;
        _lvValidation.MultiSelect = false;
        _lvValidation.View = View.Details;
        _lvValidation.Columns.AddRange([_colSeverity, _colCode, _colField, _colMessage]);

        layout.Controls.Add(_lblValidationBadge, 0, 0);
        layout.Controls.Add(_btnValidate, 1, 0);
        layout.Controls.Add(_lvValidation, 0, 1);
        layout.SetColumnSpan(_lvValidation, 2);

        _grpValidation.Controls.Add(layout);
    }

    private void BindValidation(ValidationSummary validation)
    {
        _lvValidation.BeginUpdate();
        try
        {
            _lvValidation.Items.Clear();
            foreach (var issue in validation.Issues)
            {
                var item = new ListViewItem(issue.Severity.ToUpperInvariant());
                item.SubItems.Add(issue.Code);
                item.SubItems.Add(issue.Field);
                item.SubItems.Add(issue.Message);
                _lvValidation.Items.Add(item);
            }
        }
        finally
        {
            _lvValidation.EndUpdate();
        }

        _lblValidationBadge.Text = validation.IsValid
            ? "Validation: ready"
            : $"Validation: {validation.BlockingIssueCount} blocker(s), {validation.WarningCount} warning(s)";
    }

    private void MarkDirty()
    {
        if (_suppressEvents)
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

    private void ApplyTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;
        ForeColor = NexusTheme.TextStrong;

        foreach (var group in Controls.OfType<Control>().SelectMany(EnumerateControls).OfType<GroupBox>())
        {
            group.BackColor = NexusTheme.Surface;
            group.ForeColor = NexusTheme.Primary;
            group.Font = NexusTheme.LabelFont;
        }

        foreach (var textBox in Controls.OfType<Control>().SelectMany(EnumerateControls).OfType<TextBox>())
        {
            textBox.BackColor = NexusTheme.Surface;
            textBox.ForeColor = NexusTheme.TextStrong;
            textBox.Font = NexusTheme.BodyFont;
        }

        foreach (var combo in Controls.OfType<Control>().SelectMany(EnumerateControls).OfType<ComboBox>())
        {
            combo.BackColor = NexusTheme.Surface;
            combo.ForeColor = NexusTheme.TextStrong;
            combo.Font = NexusTheme.BodyFont;
        }

        _lblNotice.ForeColor = NexusTheme.TextMuted;
        _lblNotice.Font = NexusTheme.MetaFont;

        foreach (var button in Controls.OfType<Control>().SelectMany(EnumerateControls).OfType<Button>())
        {
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = NexusTheme.Surface;
            button.ForeColor = NexusTheme.Primary;
            button.FlatAppearance.BorderColor = Color.Silver;
            button.Cursor = Cursors.Hand;
        }

        _btnValidate.BackColor = NexusTheme.Primary;
        _btnValidate.ForeColor = NexusTheme.White;

        _gridIntendedUsers.BackgroundColor = NexusTheme.Surface;
        _gridIntendedUsers.DefaultCellStyle.BackColor = NexusTheme.Surface;
        _gridIntendedUsers.DefaultCellStyle.ForeColor = NexusTheme.TextStrong;
    }

    private static void ConfigureGroup(GroupBox group, string text)
    {
        group.Dock = DockStyle.Fill;
        group.Text = text;
        group.Padding = new Padding(10, 18, 10, 10);
        group.Margin = new Padding(0, 0, 0, 8);
    }

    private static void ConfigureActionButton(Button button, string text)
    {
        button.Text = text;
        button.AutoSize = true;
        button.MinimumSize = new Size(84, 30);
        button.Margin = new Padding(0, 0, 6, 0);
        button.AccessibleName = text;
        button.AccessibleDescription = text;
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

    private sealed record ContactOption(Guid ContactId, string DisplayName)
    {
        public override string ToString() => DisplayName;
    }

    private sealed record IntendedUseOption(Guid IntendedUseId, string Category, string Name)
    {
        public override string ToString() => $"{Category}: {Name}";
    }
}
