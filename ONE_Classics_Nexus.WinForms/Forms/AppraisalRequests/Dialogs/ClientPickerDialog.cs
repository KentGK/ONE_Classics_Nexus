using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.WinForms.Services.Api;
using ONE_Classics_Nexus.WinForms.Theming;
using ClientEntity = ONE_Classics_Nexus.Domain.Entities.Clients;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests.Dialogs;

public sealed class ClientPickerDialog : Form
{
    private readonly AppraisalRequestApiClient _apiClient;
    private readonly TextBox _txtClientId = new();
    private readonly Label _lblClientSummary = new();
    private readonly ComboBox _cboPrimaryContact = new();
    private readonly Button _btnLoad = new();
    private readonly Button _btnOk = new();

    public ClientPickerDialog(AppraisalRequestApiClient apiClient, Guid? existingClientId)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

        BuildUi();

        if (existingClientId.HasValue && existingClientId.Value != Guid.Empty)
            _txtClientId.Text = existingClientId.Value.ToString();
    }

    public Guid? SelectedClientId { get; private set; }

    public Guid? SelectedPrimaryContactId =>
        _cboPrimaryContact.SelectedItem is ClientContactOption option ? option.ContactId : null;

    private void BuildUi()
    {
        Text = "Select Client";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Width = 760;
        Height = 340;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 5,
            Padding = new Padding(14)
        };

        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

        var lblClientId = new Label
        {
            Text = "Client Id",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _txtClientId.Dock = DockStyle.Fill;

        _btnLoad.Text = "Load";
        _btnLoad.Dock = DockStyle.Fill;
        _btnLoad.Click += BtnLoad_Click;

        var lblContact = new Label
        {
            Text = "Primary Contact",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _cboPrimaryContact.Dock = DockStyle.Fill;
        _cboPrimaryContact.DropDownStyle = ComboBoxStyle.DropDownList;

        _lblClientSummary.Dock = DockStyle.Fill;
        _lblClientSummary.Padding = new Padding(6);
        _lblClientSummary.BackColor = NexusTheme.SurfaceAlt;
        _lblClientSummary.Text = "Load a client to review summary and available contacts.";

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false
        };

        var btnCancel = new Button { Text = "Cancel", Width = 94, DialogResult = DialogResult.Cancel };
        _btnOk.Text = "Select";
        _btnOk.Width = 94;
        _btnOk.Enabled = false;
        _btnOk.DialogResult = DialogResult.OK;
        _btnOk.Click += (_, _) => SelectedClientId = ParseClientId();

        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(_btnOk);

        root.Controls.Add(lblClientId, 0, 0);
        root.Controls.Add(_txtClientId, 1, 0);
        root.Controls.Add(_btnLoad, 2, 0);

        root.Controls.Add(lblContact, 0, 1);
        root.Controls.Add(_cboPrimaryContact, 1, 1);
        root.SetColumnSpan(_cboPrimaryContact, 2);

        root.Controls.Add(_lblClientSummary, 0, 2);
        root.SetColumnSpan(_lblClientSummary, 3);

        root.Controls.Add(buttonPanel, 2, 4);

        Controls.Add(root);

        AcceptButton = _btnOk;
        CancelButton = btnCancel;
    }

    private async void BtnLoad_Click(object? sender, EventArgs e)
    {
        var clientId = ParseClientId();
        if (!clientId.HasValue)
        {
            MessageBox.Show(this, "Enter a valid client id.", "Select Client", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var client = await _apiClient.GetClientAsync(clientId.Value).ConfigureAwait(true);
            if (client is null)
            {
                MessageBox.Show(this, "Client not found.", "Select Client", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedClientId = client.ClientId;
            BindClientSummary(client);

            var contacts = await _apiClient.GetClientContactsAsync(client.ClientId).ConfigureAwait(true);
            BindContacts(contacts);

            _btnOk.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Select Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BindContacts(IReadOnlyList<ClientContact> contacts)
    {
        _cboPrimaryContact.BeginUpdate();
        try
        {
            _cboPrimaryContact.Items.Clear();
            _cboPrimaryContact.Items.Add(new ClientContactOption(null, "(None)"));

            foreach (var contact in contacts.Where(c => c.IsActive))
            {
                var name = BuildContactName(contact);
                _cboPrimaryContact.Items.Add(new ClientContactOption(contact.ClientContactId, name));
            }

            var primary = contacts.FirstOrDefault(c => c.IsActive && c.IsPrimary);
            if (primary is not null)
            {
                for (var i = 0; i < _cboPrimaryContact.Items.Count; i++)
                {
                    if (_cboPrimaryContact.Items[i] is ClientContactOption option && option.ContactId == primary.ClientContactId)
                    {
                        _cboPrimaryContact.SelectedIndex = i;
                        return;
                    }
                }
            }

            _cboPrimaryContact.SelectedIndex = 0;
        }
        finally
        {
            _cboPrimaryContact.EndUpdate();
        }
    }

    private void BindClientSummary(ClientEntity client)
    {
        var displayName = BuildClientName(client);
        _lblClientSummary.Text = $"{displayName}{Environment.NewLine}Type: {client.ClientTypeCode}   Status: {client.StatusCode}{Environment.NewLine}Email: {client.Email}   Phone: {client.Phone}";
    }

    private Guid? ParseClientId() =>
        Guid.TryParse(_txtClientId.Text.Trim(), out var clientId) ? clientId : null;

    private static string BuildClientName(ClientEntity client)
    {
        if (!string.IsNullOrWhiteSpace(client.DisplayName))
            return client.DisplayName;

        if (!string.IsNullOrWhiteSpace(client.OrganizationName))
            return client.OrganizationName;

        return $"{client.FirstName} {client.LastName}".Trim();
    }

    private static string BuildContactName(ClientContact contact)
    {
        var fullName = $"{contact.FirstName} {contact.LastName}".Trim();
        if (!string.IsNullOrWhiteSpace(fullName))
            return fullName;

        return contact.Email ?? contact.Phone ?? contact.ClientContactId.ToString();
    }

    private sealed record ClientContactOption(Guid? ContactId, string DisplayName)
    {
        public override string ToString() => DisplayName;
    }
}
