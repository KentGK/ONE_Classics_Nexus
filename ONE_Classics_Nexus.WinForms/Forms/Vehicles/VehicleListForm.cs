using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.WinForms.Services;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.Vehicles;

public sealed class VehicleListForm : Form
{
    private readonly IVehicleApiService _vehicleApiService;

    private readonly TextBox _txtSearch;
    private readonly TextBox _txtMake;
    private readonly TextBox _txtModel;
    private readonly ComboBox _cboHasRedFlags;
    private readonly Button _btnSearch;
    private readonly Button _btnClear;
    private readonly Button _btnRefresh;
    private readonly Button _btnOpen;
    private readonly DataGridView _grid;
    private readonly BindingSource _bindingSource;
    private readonly Label _lblStatus;

    private List<VehicleResponse> _allVehicles = new();

    public event EventHandler<Guid>? VehicleOpenRequested;

    public VehicleListForm(IVehicleApiService vehicleApiService)
    {
        ArgumentNullException.ThrowIfNull(vehicleApiService);

        _vehicleApiService = vehicleApiService;

        Text = "Vehicle List";
        Name = nameof(VehicleListForm);
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;
        Dock = DockStyle.Fill;

        _txtSearch = new TextBox();
        _txtMake = new TextBox();
        _txtModel = new TextBox();
        _cboHasRedFlags = new ComboBox();
        _btnSearch = new Button();
        _btnClear = new Button();
        _btnRefresh = new Button();
        _btnOpen = new Button();
        _grid = new DataGridView();
        _bindingSource = new BindingSource();
        _lblStatus = new Label();

        BuildLayout();
        ConfigureGrid();
        ConfigureToolbar();

        Shown += async (_, _) => await RefreshListAsync();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = NexusTheme.SurfaceAlt,
            Padding = new Padding(16)
        };

        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 10,
            AutoSize = true,
            BackColor = NexusTheme.Surface,
            Padding = new Padding(12),
            Margin = new Padding(0, 0, 0, 8)
        };

        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        var lblSearch = new Label { Text = "Search", AutoSize = true, Anchor = AnchorStyles.Left };
        var lblMake = new Label { Text = "Make", AutoSize = true, Anchor = AnchorStyles.Left };
        var lblModel = new Label { Text = "Model", AutoSize = true, Anchor = AnchorStyles.Left };

        _txtSearch.Dock = DockStyle.Fill;
        _txtMake.Dock = DockStyle.Fill;
        _txtModel.Dock = DockStyle.Fill;

        _cboHasRedFlags.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboHasRedFlags.Items.AddRange(new object[] { "All", "Red Flags", "No Red Flags" });
        _cboHasRedFlags.SelectedIndex = 0;
        _cboHasRedFlags.Width = 120;

        _btnSearch.Text = "Search";
        _btnClear.Text = "Clear";
        _btnRefresh.Text = "Refresh";
        _btnOpen.Text = "Open";

        _btnSearch.Width = 90;
        _btnClear.Width = 90;
        _btnRefresh.Width = 90;
        _btnOpen.Width = 90;

        toolbar.Controls.Add(lblSearch, 0, 0);
        toolbar.Controls.Add(_txtSearch, 1, 0);
        toolbar.Controls.Add(lblMake, 2, 0);
        toolbar.Controls.Add(_txtMake, 3, 0);
        toolbar.Controls.Add(lblModel, 4, 0);
        toolbar.Controls.Add(_txtModel, 5, 0);
        toolbar.Controls.Add(_cboHasRedFlags, 6, 0);
        toolbar.Controls.Add(_btnSearch, 7, 0);
        toolbar.Controls.Add(_btnClear, 8, 0);
        toolbar.Controls.Add(_btnRefresh, 9, 0);

        var gridContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = NexusTheme.Surface,
            Padding = new Padding(8)
        };

        _grid.Dock = DockStyle.Fill;
        gridContainer.Controls.Add(_grid);

        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            ColumnCount = 2,
            AutoSize = true,
            BackColor = NexusTheme.Surface,
            Padding = new Padding(12),
            Margin = new Padding(0, 8, 0, 0)
        };

        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _lblStatus.AutoSize = true;
        _lblStatus.Anchor = AnchorStyles.Left;
        _lblStatus.Text = "Ready";

        footer.Controls.Add(_lblStatus, 0, 0);
        footer.Controls.Add(_btnOpen, 1, 0);

        root.Controls.Add(toolbar, 0, 0);
        root.Controls.Add(gridContainer, 0, 1);
        root.Controls.Add(footer, 0, 2);

        Controls.Add(root);
    }

    private void ConfigureToolbar()
    {
        _btnSearch.Click += (_, _) => ApplyFilters();
        _btnClear.Click += (_, _) =>
        {
            _txtSearch.Clear();
            _txtMake.Clear();
            _txtModel.Clear();
            _cboHasRedFlags.SelectedIndex = 0;
            ApplyFilters();
        };

        _btnRefresh.Click += async (_, _) => await RefreshListAsync();
        _btnOpen.Click += (_, _) => OpenSelectedVehicle();

        _txtSearch.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyFilters();
                e.Handled = true;
            }
        };

        _txtMake.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyFilters();
                e.Handled = true;
            }
        };

        _txtModel.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyFilters();
                e.Handled = true;
            }
        };
    }

    private void ConfigureGrid()
    {
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.MultiSelect = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        _grid.Columns.Add(CreateTextColumn("Year", nameof(VehicleGridItem.Year), 70));
        _grid.Columns.Add(CreateTextColumn("Make", nameof(VehicleGridItem.Make), 120));
        _grid.Columns.Add(CreateTextColumn("Model", nameof(VehicleGridItem.Model), 120));
        _grid.Columns.Add(CreateTextColumn("Series / Trim", nameof(VehicleGridItem.SeriesTrim), 180));
        _grid.Columns.Add(CreateTextColumn("VIN / Chassis", nameof(VehicleGridItem.VinOrChassis), 220));
        _grid.Columns.Add(CreateTextColumn("Vehicle Type", nameof(VehicleGridItem.VehicleTypeId), 180));
        _grid.Columns.Add(CreateTextColumn("Title Status", nameof(VehicleGridItem.TitleStatus), 110));
        _grid.Columns.Add(CreateTextColumn("Condition", nameof(VehicleGridItem.ConditionSummary), 90));
        _grid.Columns.Add(CreateTextColumn("Red Flag", nameof(VehicleGridItem.RedFlagStatus), 90));
        _grid.Columns.Add(CreateTextColumn("Last Updated", nameof(VehicleGridItem.UpdatedAtUtc), 150));

        _grid.DataSource = _bindingSource;
        _grid.DoubleClick += (_, _) => OpenSelectedVehicle();
    }

    private static DataGridViewTextBoxColumn CreateTextColumn(string headerText, string propertyName, int width)
    {
        return new DataGridViewTextBoxColumn
        {
            HeaderText = headerText,
            DataPropertyName = propertyName,
            Width = width,
            SortMode = DataGridViewColumnSortMode.Automatic,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        };
    }

    private async Task RefreshListAsync()
    {
        try
        {
            SetBusy(true, "Loading vehicles...");
            var items = await _vehicleApiService.ListAsync();
            _allVehicles = items.OrderByDescending(x => x.UpdatedAtUtc).ToList();
            ApplyFilters();
            SetBusy(false, $"Loaded {_allVehicles.Count} vehicles.");
        }
        catch (Exception ex)
        {
            SetBusy(false, "Vehicle load failed.");
            MessageBox.Show(this, ex.Message, "Vehicle List", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyFilters()
    {
        var search = _txtSearch.Text.Trim();
        var make = _txtMake.Text.Trim();
        var model = _txtModel.Text.Trim();
        var redFlags = _cboHasRedFlags.SelectedIndex;

        var query = _allVehicles.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v =>
                Contains(v.Vin, search)
                || Contains(v.Make, search)
                || Contains(v.Model, search)
                || Contains(v.Trim, search)
                || Contains(v.SeriesName, search)
                || Contains(v.BodyStyle, search));
        }

        if (!string.IsNullOrWhiteSpace(make))
        {
            query = query.Where(v => Contains(v.Make, make));
        }

        if (!string.IsNullOrWhiteSpace(model))
        {
            query = query.Where(v => Contains(v.Model, model));
        }

        if (redFlags == 1)
        {
            query = query.Where(v => v.HasRedFlags);
        }
        else if (redFlags == 2)
        {
            query = query.Where(v => !v.HasRedFlags);
        }

        var rows = query
            .Select(v => new VehicleGridItem(v))
            .OrderByDescending(v => v.UpdatedAtUtc)
            .ToList();

        _bindingSource.DataSource = rows;
        _lblStatus.Text = $"{rows.Count} vehicle(s) shown.";
        _btnOpen.Enabled = rows.Count > 0;
    }

    private void OpenSelectedVehicle()
    {
        if (_bindingSource.Current is not VehicleGridItem item)
        {
            return;
        }

        VehicleOpenRequested?.Invoke(this, item.VehicleId);
    }

    private static bool Contains(string? source, string value)
    {
        return source?.Contains(value, StringComparison.OrdinalIgnoreCase) == true;
    }

    private void SetBusy(bool isBusy, string status)
    {
        Cursor = isBusy ? Cursors.WaitCursor : Cursors.Default;
        _btnSearch.Enabled = !isBusy;
        _btnClear.Enabled = !isBusy;
        _btnRefresh.Enabled = !isBusy;
        _btnOpen.Enabled = !isBusy && _bindingSource.Count > 0;
        _lblStatus.Text = status;
    }

    private sealed class VehicleGridItem
    {
        public Guid VehicleId { get; }
        public int Year { get; }
        public string Make { get; }
        public string Model { get; }
        public string SeriesTrim { get; }
        public string VinOrChassis { get; }
        public string VehicleTypeId { get; } = string.Empty;
        public string TitleStatus { get; }
        public string ConditionSummary { get; }
        public string RedFlagStatus { get; }
        public string UpdatedAtUtc { get; }

        public VehicleGridItem(VehicleResponse vehicle)
        {
            VehicleId = vehicle.VehicleId;
            Year = vehicle.Year;
            Make = vehicle.Make;
            Model = vehicle.Model;
            SeriesTrim = $"{vehicle.SeriesName} {vehicle.Trim}".Trim();
            VinOrChassis = !string.IsNullOrWhiteSpace(vehicle.Vin) ? vehicle.Vin : vehicle.ChassisNumber ?? string.Empty;
            VehicleTypeId = vehicle.VehicleTypeId?.ToString() ?? string.Empty;
            TitleStatus = vehicle.TitleStatus ?? string.Empty;
            ConditionSummary = vehicle.OverallConditionScore?.ToString("0.0") ?? "N/A";
            RedFlagStatus = vehicle.HasRedFlags ? "Red Flags" : "No Red Flags";
            UpdatedAtUtc = vehicle.UpdatedAtUtc.ToString("g");
        }
    }
}
