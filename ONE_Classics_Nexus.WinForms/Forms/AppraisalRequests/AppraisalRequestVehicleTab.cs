using System.ComponentModel;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public sealed class AppraisalRequestVehicleTab : UserControl
{
    private readonly TableLayoutPanel _rootLayout = new();

    private readonly GroupBox _grpIdentification = new();
    private readonly TableLayoutPanel _identificationLayout = new();

    private readonly Label _lblYear = new();
    private readonly NumericUpDown _nudYear = new();
    private readonly Label _lblMake = new();
    private readonly TextBox _txtMake = new();
    private readonly Label _lblModel = new();
    private readonly TextBox _txtModel = new();

    private readonly Label _lblVehicleType = new();
    private readonly ComboBox _cboVehicleType = new();
    private readonly Label _lblVin = new();
    private readonly TextBox _txtVin = new();
    private readonly Label _lblEngine = new();
    private readonly TextBox _txtEngine = new();
    private readonly Label _lblTransmission = new();
    private readonly TextBox _txtTransmission = new();
    private readonly Label _lblMileage = new();
    private readonly NumericUpDown _nudMileage = new();
    private readonly Label _lblOdometerUnit = new();
    private readonly ComboBox _cboOdometerUnit = new();
    private readonly Label _lblOdometerStatus = new();
    private readonly ComboBox _cboOdometerStatus = new();
    private readonly Label _lblExteriorColor = new();
    private readonly TextBox _txtExteriorColor = new();
    private readonly Label _lblInteriorColor = new();
    private readonly TextBox _txtInteriorColor = new();
    private readonly Label _lblTitleStatus = new();
    private readonly ComboBox _cboTitleStatus = new();

    private readonly GroupBox _grpCondition = new();
    private readonly GroupBox _grpKnownHistory = new();

    private readonly TextBox _txtCondition = new();
    private readonly TextBox _txtKnownHistory = new();

    private bool _suppressDirtyTracking;
    private bool _isDirty;

    public AppraisalRequestVehicleTab()
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
    /// Updates the vehicle type dropdown values with display text in the format "{id} {type_name}".
    /// </summary>
    public void SetVehicleTypeOptions(IEnumerable<VehicleTypeOption> vehicleTypes)
    {
        ArgumentNullException.ThrowIfNull(vehicleTypes);

        _suppressDirtyTracking = true;
        try
        {
            var previousSelection = _cboVehicleType.SelectedItem as VehicleTypeOption;

            _cboVehicleType.Items.Clear();
            foreach (var vehicleType in vehicleTypes)
                _cboVehicleType.Items.Add(vehicleType);

            if (previousSelection is null)
            {
                _cboVehicleType.SelectedIndex = -1;
                return;
            }

            for (var i = 0; i < _cboVehicleType.Items.Count; i++)
            {
                if (_cboVehicleType.Items[i] is VehicleTypeOption option &&
                    option.VehicleTypeId == previousSelection.VehicleTypeId)
                {
                    _cboVehicleType.SelectedIndex = i;
                    return;
                }
            }

            _cboVehicleType.SelectedIndex = -1;
        }
        finally
        {
            _suppressDirtyTracking = false;
        }
    }

    /// <summary>
    /// Applies read-only/editable state for Vehicle tab entry fields.
    /// </summary>
    public void SetEditable(bool editable)
    {
        _nudYear.ReadOnly = !editable;
        _nudYear.Enabled = editable;
        _txtMake.ReadOnly = !editable;
        _txtModel.ReadOnly = !editable;
        _cboVehicleType.Enabled = editable;
        _txtVin.ReadOnly = !editable;
        _txtEngine.ReadOnly = !editable;
        _txtTransmission.ReadOnly = !editable;
        _nudMileage.ReadOnly = !editable;
        _nudMileage.Enabled = editable;
        _cboOdometerUnit.Enabled = editable;
        _cboOdometerStatus.Enabled = editable;
        _txtExteriorColor.ReadOnly = !editable;
        _txtInteriorColor.ReadOnly = !editable;
        _cboTitleStatus.Enabled = editable;

        _txtCondition.ReadOnly = !editable;
        _txtKnownHistory.ReadOnly = !editable;

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

        ConfigureIdentificationSection();

        ConfigureSection(_grpCondition, _txtCondition,
            "Condition",
            "Capture observed condition themes, deficiencies, and visible wear indicators.");

        ConfigureSection(_grpKnownHistory, _txtKnownHistory,
            "Known History",
            "Capture ownership timeline cues, incidents, provenance, and material history notes.");

        _rootLayout.Controls.Add(_grpIdentification, 0, 0);
        _rootLayout.Controls.Add(_grpCondition, 1, 0);
        _rootLayout.Controls.Add(_grpKnownHistory, 0, 1);
        _rootLayout.SetColumnSpan(_grpKnownHistory, 2);

        Controls.Add(_rootLayout);
    }

    private void ConfigureIdentificationSection()
    {
        ConfigureGroup(_grpIdentification, "Preliminary Vehicle Identification");

        _identificationLayout.Dock = DockStyle.Fill;
        _identificationLayout.ColumnCount = 4;
        _identificationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        _identificationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _identificationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        _identificationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _identificationLayout.RowCount = 6;
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));
        _identificationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));

        ConfigureFieldLabel(_lblYear, "Year *");
        _nudYear.Minimum = 1890;
        _nudYear.Maximum = DateTime.UtcNow.Year;
        _nudYear.Value = 1890;
        ConfigureNumeric(_nudYear);

        ConfigureFieldLabel(_lblMake, "Make *");
        ConfigureTextField(_txtMake, "Required");

        ConfigureFieldLabel(_lblModel, "Model *");
        ConfigureTextField(_txtModel, "Required");

        ConfigureFieldLabel(_lblVehicleType, "Vehicle Type");
        ConfigureCombo(_cboVehicleType);

        ConfigureFieldLabel(_lblVin, "VIN");
        ConfigureTextField(_txtVin, "Optional");

        ConfigureFieldLabel(_lblEngine, "Engine");
        ConfigureTextField(_txtEngine, "Optional");

        ConfigureFieldLabel(_lblTransmission, "Transmission");
        ConfigureTextField(_txtTransmission, "Optional");

        ConfigureFieldLabel(_lblMileage, "Mileage");
        _nudMileage.Minimum = 0;
        _nudMileage.Maximum = 9_999_999;
        ConfigureNumeric(_nudMileage);

        ConfigureFieldLabel(_lblOdometerUnit, "Odometer Unit");
        ConfigureCombo(_cboOdometerUnit);
        _cboOdometerUnit.Items.Add("miles");
        _cboOdometerUnit.Items.Add("kilometers");

        ConfigureFieldLabel(_lblOdometerStatus, "Odometer Status");
        ConfigureCombo(_cboOdometerStatus);
        _cboOdometerStatus.Items.Add("Actual");
        _cboOdometerStatus.Items.Add("Exceeds Mechanical Limits");
        _cboOdometerStatus.Items.Add("Not Actual");
        _cboOdometerStatus.Items.Add("TMU");
        _cboOdometerStatus.Items.Add("Replaced");
        _cboOdometerStatus.Items.Add("Unknown");

        ConfigureFieldLabel(_lblExteriorColor, "Exterior Color");
        ConfigureTextField(_txtExteriorColor, "Optional");

        ConfigureFieldLabel(_lblInteriorColor, "Interior Color");
        ConfigureTextField(_txtInteriorColor, "Optional");

        ConfigureFieldLabel(_lblTitleStatus, "Title Status");
        ConfigureCombo(_cboTitleStatus);
        _cboTitleStatus.Items.Add("Clean");
        _cboTitleStatus.Items.Add("Salvage");
        _cboTitleStatus.Items.Add("Rebuilt");
        _cboTitleStatus.Items.Add("Flood");
        _cboTitleStatus.Items.Add("Bonded");
        _cboTitleStatus.Items.Add("Parts Only");
        _cboTitleStatus.Items.Add("NoTitle");
        _cboTitleStatus.Items.Add("Bill Of Sale");
        _cboTitleStatus.Items.Add("Foreign Registration");
        _cboTitleStatus.Items.Add("Unknown");

        _identificationLayout.Controls.Add(_lblYear, 0, 0);
        _identificationLayout.Controls.Add(_nudYear, 1, 0);
        _identificationLayout.Controls.Add(_lblMake, 2, 0);
        _identificationLayout.Controls.Add(_txtMake, 3, 0);

        _identificationLayout.Controls.Add(_lblModel, 0, 1);
        _identificationLayout.Controls.Add(_txtModel, 1, 1);
        _identificationLayout.Controls.Add(_lblVehicleType, 2, 1);
        _identificationLayout.Controls.Add(_cboVehicleType, 3, 1);

        _identificationLayout.Controls.Add(_lblVin, 0, 2);
        _identificationLayout.Controls.Add(_txtVin, 1, 2);
        _identificationLayout.Controls.Add(_lblEngine, 2, 2);
        _identificationLayout.Controls.Add(_txtEngine, 3, 2);

        _identificationLayout.Controls.Add(_lblTransmission, 0, 3);
        _identificationLayout.Controls.Add(_txtTransmission, 1, 3);
        _identificationLayout.Controls.Add(_lblMileage, 2, 3);
        _identificationLayout.Controls.Add(_nudMileage, 3, 3);

        _identificationLayout.Controls.Add(_lblOdometerUnit, 0, 4);
        _identificationLayout.Controls.Add(_cboOdometerUnit, 1, 4);
        _identificationLayout.Controls.Add(_lblOdometerStatus, 2, 4);
        _identificationLayout.Controls.Add(_cboOdometerStatus, 3, 4);

        _identificationLayout.Controls.Add(_lblExteriorColor, 0, 5);
        _identificationLayout.Controls.Add(_txtExteriorColor, 1, 5);
        _identificationLayout.Controls.Add(_lblInteriorColor, 2, 5);
        _identificationLayout.Controls.Add(_txtInteriorColor, 3, 5);

        _grpIdentification.Controls.Add(_identificationLayout);

        var titleStatusHost = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 34,
            ColumnCount = 2,
            Margin = new Padding(0),
            Padding = new Padding(0)
        };

        titleStatusHost.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        titleStatusHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        titleStatusHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        titleStatusHost.Controls.Add(_lblTitleStatus, 0, 0);
        titleStatusHost.Controls.Add(_cboTitleStatus, 1, 0);

        _grpIdentification.Controls.Add(titleStatusHost);
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

        foreach (var combo in EnumerateCombos())
        {
            combo.BackColor = NexusTheme.Surface;
            combo.ForeColor = NexusTheme.TextStrong;
            combo.Font = NexusTheme.BodyFont;
        }

        foreach (var numeric in EnumerateNumerics())
        {
            numeric.BackColor = NexusTheme.Surface;
            numeric.ForeColor = NexusTheme.TextStrong;
            numeric.Font = NexusTheme.BodyFont;
        }
    }

    private void WireDirtyTracking()
    {
        foreach (var textBox in EnumerateTextBoxes())
            textBox.TextChanged += (_, _) => MarkDirty();

        foreach (var combo in EnumerateCombos())
            combo.SelectedIndexChanged += (_, _) => MarkDirty();

        foreach (var numeric in EnumerateNumerics())
            numeric.ValueChanged += (_, _) => MarkDirty();
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

    private static void ConfigureGroup(GroupBox group, string title)
    {
        group.Dock = DockStyle.Fill;
        group.Text = title;
        group.Padding = new Padding(10, 18, 10, 10);
        group.Margin = new Padding(4);
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.AutoSize = false;
        label.TextAlign = ContentAlignment.MiddleLeft;
        label.Margin = new Padding(3);
    }

    private static void ConfigureTextField(TextBox textBox, string placeholder)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Margin = new Padding(3, 4, 10, 4);
        textBox.PlaceholderText = placeholder;
    }

    private static void ConfigureCombo(ComboBox combo)
    {
        combo.Dock = DockStyle.Fill;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.IntegralHeight = false;
        combo.Margin = new Padding(3, 4, 10, 4);
    }

    private static void ConfigureNumeric(NumericUpDown numeric)
    {
        numeric.Dock = DockStyle.Fill;
        numeric.Margin = new Padding(3, 4, 10, 4);
        numeric.ThousandsSeparator = true;
    }

    private IEnumerable<GroupBox> EnumerateGroups()
    {
        yield return _grpIdentification;
        yield return _grpCondition;
        yield return _grpKnownHistory;
    }

    private IEnumerable<TextBox> EnumerateTextBoxes()
    {
        yield return _txtMake;
        yield return _txtModel;
        yield return _txtVin;
        yield return _txtEngine;
        yield return _txtTransmission;
        yield return _txtExteriorColor;
        yield return _txtInteriorColor;
        yield return _txtCondition;
        yield return _txtKnownHistory;
    }

    private IEnumerable<ComboBox> EnumerateCombos()
    {
        yield return _cboVehicleType;
        yield return _cboOdometerUnit;
        yield return _cboOdometerStatus;
        yield return _cboTitleStatus;
    }

    private IEnumerable<NumericUpDown> EnumerateNumerics()
    {
        yield return _nudYear;
        yield return _nudMileage;
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

public sealed record VehicleTypeOption(int VehicleTypeId, string TypeName)
{
    public override string ToString() => $"{VehicleTypeId} {TypeName}";
}
