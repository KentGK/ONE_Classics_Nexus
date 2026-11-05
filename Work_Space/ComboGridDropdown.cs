using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public partial class ComboGridDropdown : UserControl
    {
        private bool _isDropped;

        private List<object> _rawData = new List<object>();
        private List<(string Header, Func<object, object> Selector)> _columns = new List<(string Header, Func<object, object> Selector)>();

        // ADDED: expose an event so consumers (e.g., ComboBoxForm) can react to selection changes.
        public event EventHandler SelectedIndexChanged;

        public ComboGridDropdown()
        {
            InitializeComponents();
            WireEvents();
        }

        // ---------------------------------------------------------
        //  WIRE EVENTS
        // ---------------------------------------------------------
        private void WireEvents()
        {
            _combo.Click += (s, e) => ToggleDropdown();
            _combo.KeyDown += Combo_KeyDown;

            _grid.CellClick += (s, e) => SelectRow(e.RowIndex, closeDropdown: true);
            _grid.KeyDown += Grid_KeyDown;

            // ADDED: react to any row selection change (mouse, arrow keys, or programmatic),
            // not just an explicit click or Enter confirmation. This keeps the combo text
            // and SelectedIndexChanged event in sync with whatever row is currently highlighted.
            _grid.SelectionChanged += (s, e) => SelectRow(_grid.CurrentCell?.RowIndex ?? -1, closeDropdown: false);

            // Instead of closing immediately on LostFocus (which fires as soon as
            // focus moves from _combo to _grid while opening the dropdown),
            // defer the check until after the focus change has fully settled.
            _combo.Leave += (s, e) => ScheduleHideIfFocusLeftControl();
            _grid.Leave += (s, e) => ScheduleHideIfFocusLeftControl();
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

                if (_grid.Rows.Count > 0)
                {
                    _grid.ClearSelection();
                    _grid.Rows[0].Selected = true;
                    _grid.CurrentCell = _grid.Rows[0].Cells[0];
                }

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

        // Focus moves between _combo and _grid (both inside this control) while the
        // dropdown is open/opening. We only want to close the dropdown when focus
        // truly leaves both controls, so we check on the next message loop tick,
        // after the new control has actually received focus.
        private void ScheduleHideIfFocusLeftControl()
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                if (!_combo.Focused && !_grid.Focused)
                    HideDropdown();
            }));
        }

        // ---------------------------------------------------------
        //  SELECTION HANDLING
        // ---------------------------------------------------------
        // UPDATED: added closeDropdown parameter so row-highlight changes (SelectionChanged)
        // can update the display without prematurely closing the dropdown, while an explicit
        // click or Enter confirmation still closes it as before.
        private void SelectRow(int rowIndex, bool closeDropdown)
        {
            if (rowIndex < 0 || rowIndex >= _grid.Rows.Count)
                return;

            var row = _grid.Rows[rowIndex];
            _combo.Text = row.Cells[0].Value?.ToString();

            if (closeDropdown)
                HideDropdown();

            // ADDED: raise the event so subscribers are notified of the new selection.
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        // ---------------------------------------------------------
        //  KEYBOARD NAVIGATION
        // ---------------------------------------------------------
        private void Combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (!_isDropped)
                    ToggleDropdown();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideDropdown();
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectRow(_grid.CurrentCell?.RowIndex ?? -1, closeDropdown: true);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideDropdown();
                _combo.Focus();
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