using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public class ComboGridDropdown : UserControl
    {
        private ComboBox _combo;
        private Panel _dropPanel;
        private DataGridView _grid;
        private bool _isDropped;

        private List<object> _rawData = new List<object>();
        private List<(string Header, Func<object, object> Selector)> _columns;

        public ComboGridDropdown()
        {
            InitializeComponents();
            WireEvents();
        }

        // ---------------------------------------------------------
        //  INITIALIZE UI COMPONENTS
        // ---------------------------------------------------------
        private void InitializeComponents()
        {
            _combo = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _dropPanel = new Panel
            {
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 200,
                Width = this.Width,
                BackColor = Color.White
            };

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            _dropPanel.Controls.Add(_grid);

            this.Controls.Add(_dropPanel);
            this.Controls.Add(_combo);
        }

        // ---------------------------------------------------------
        //  WIRE EVENTS
        // ---------------------------------------------------------
        private void WireEvents()
        {
            _combo.Click += (s, e) => ToggleDropdown();
            _combo.KeyDown += Combo_KeyDown;

            _grid.CellClick += (s, e) => SelectRow(e.RowIndex);

            this.LostFocus += (s, e) => HideDropdown();
            _grid.LostFocus += (s, e) => HideDropdown();
        }

        // ---------------------------------------------------------
        //  PUBLIC API — BINDING
        // ---------------------------------------------------------
        public void Bind<T>(IEnumerable<T> data, params (string Header, Func<T, object> Selector)[] columns)
        {
            _rawData = data.Cast<object>().ToList();
            _columns = columns
                .Select(c => (c.Header, new Func<object, object>(o => c.Selector((T)o))))
                .ToList();

            BuildGrid();
            BuildCombo();
        }

        private void BuildGrid()
        {
            _grid.Columns.Clear();
            foreach (var col in _columns)
                _grid.Columns.Add(col.Header, col.Header);

            _grid.Rows.Clear();
            foreach (var item in _rawData)
                _grid.Rows.Add(_columns.Select(c => c.Selector(item)).ToArray());
        }

        private void BuildCombo()
        {
            _combo.Items.Clear();
            foreach (var item in _rawData)
                _combo.Items.Add(_columns[0].Selector(item).ToString());
        }

        // ---------------------------------------------------------
        //  DROPDOWN CONTROL
        // ---------------------------------------------------------
        private void ToggleDropdown()
        {
            _isDropped = !_isDropped;
            _dropPanel.Visible = _isDropped;

            if (_isDropped)
            {
                _dropPanel.BringToFront();
                _dropPanel.Location = new Point(0, _combo.Height);
                _grid.Focus();
            }
        }

        private void HideDropdown()
        {
            if (_isDropped)
            {
                _isDropped = false;
                _dropPanel.Visible = false;
            }
        }

        // ---------------------------------------------------------
        //  SELECTION HANDLING
        // ---------------------------------------------------------
        private void SelectRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _grid.Rows.Count)
                return;

            var row = _grid.Rows[rowIndex];
            _combo.Text = row.Cells[0].Value?.ToString();
            HideDropdown();
        }

        // ---------------------------------------------------------
        //  KEYBOARD NAVIGATION
        // ---------------------------------------------------------
        private void Combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ToggleDropdown();
                _grid.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideDropdown();
            }
        }

        // ---------------------------------------------------------
        //  EXPOSE SELECTED VALUE
        // ---------------------------------------------------------
        public string SelectedText => _combo.Text;

        public object SelectedRow
        {
            get
            {
                if (_grid.SelectedRows.Count > 0)
                    return _grid.SelectedRows[0].DataBoundItem;
                return null;
            }
        }
    }
}