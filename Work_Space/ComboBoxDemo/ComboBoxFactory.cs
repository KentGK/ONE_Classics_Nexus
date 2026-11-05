using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComboBoxDemo
{
    public static class ComboBoxFactory
    {
        // ---------------------------------------------------------
        //  BASIC COMBOBOX (simple items)
        // ---------------------------------------------------------
        public static ComboBox CreateBasic(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.Items.AddRange(items.ToArray());
            return cb;
        }

        // ---------------------------------------------------------
        //  DATA-BOUND COMBOBOX (DisplayMember / ValueMember)
        // ---------------------------------------------------------
        public static ComboBox CreateBound<T>(
            IEnumerable<T> data,
            string displayMember,
            string valueMember)
        {
            var cb = CreateBase();
            cb.DataSource = data.ToList();
            cb.DisplayMember = displayMember;
            cb.ValueMember = valueMember;
            return cb;
        }

        // ---------------------------------------------------------
        //  AUTOCOMPLETE COMBOBOX
        // ---------------------------------------------------------
        public static ComboBox CreateAutoComplete(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.Items.AddRange(items.ToArray());
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            return cb;
        }

        // ---------------------------------------------------------
        //  DYNAMIC FILTER COMBOBOX
        // ---------------------------------------------------------
        public static ComboBox CreateDynamicFilter(IEnumerable<string> items)
        {
            var allItems = items.ToList();
            var cb = CreateBase();
            cb.Items.AddRange(allItems.ToArray());

            cb.TextChanged += (s, e) =>
            {
                string filter = cb.Text.ToLower();
                cb.Items.Clear();

                foreach (var item in allItems)
                    if (item.ToLower().Contains(filter))
                        cb.Items.Add(item);

                cb.DroppedDown = true;
                cb.SelectionStart = cb.Text.Length;
                cb.SelectionLength = 0;
            };

            return cb;
        }

        // ---------------------------------------------------------
        //  VALIDATED COMBOBOX (must match list)
        // ---------------------------------------------------------
        public static ComboBox CreateValidated(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.Items.AddRange(items.ToArray());

            cb.Validating += (s, e) =>
            {
                if (!cb.Items.Contains(cb.Text))
                {
                    MessageBox.Show("Please select a valid item from the list.");
                    e.Cancel = true;
                }
            };

            return cb;
        }

        // ---------------------------------------------------------
        //  KEYBOARD-ENHANCED COMBOBOX
        // ---------------------------------------------------------
        public static ComboBox CreateKeyboardEnhanced(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.Items.AddRange(items.ToArray());

            cb.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    MessageBox.Show($"Selected: {cb.Text}");
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    cb.DroppedDown = false;
                }
            };

            return cb;
        }

        // ---------------------------------------------------------
        //  OWNER-DRAW COMBOBOX (custom rendering)
        // ---------------------------------------------------------
        public static ComboBox CreateOwnerDraw(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.DrawMode = DrawMode.OwnerDrawFixed;
            cb.Items.AddRange(items.ToArray());

            cb.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();
                string text = cb.Items[e.Index].ToString();

                Brush brush = ((e.State & DrawItemState.Selected) != 0)
                    ? Brushes.DarkOrange
                    : Brushes.Black;

                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
                e.DrawFocusRectangle();
            };

            return cb;
        }

        // ---------------------------------------------------------
        //  MULTI-COLUMN COMBOBOX (simulated grid)
        // ---------------------------------------------------------
        public static ComboBox CreateMultiColumn<T>(
            IEnumerable<T> data,
            Func<T, string> col1,
            Func<T, string> col2)
        {
            var cb = CreateBase();
            cb.DrawMode = DrawMode.OwnerDrawFixed;

            var list = data.ToList();
            cb.Items.AddRange(list.Cast<object>().ToArray());

            cb.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                var item = (T)cb.Items[e.Index];
                e.DrawBackground();

                e.Graphics.DrawString(col1(item), e.Font, Brushes.Black, new PointF(e.Bounds.Left + 5, e.Bounds.Top));
                e.Graphics.DrawString(col2(item), e.Font, Brushes.Gray, new PointF(e.Bounds.Left + 150, e.Bounds.Top));

                e.DrawFocusRectangle();
            };

            return cb;
        }

        // ---------------------------------------------------------
        //  CONTEXT-MENU COMBOBOX
        // ---------------------------------------------------------
        public static ComboBox CreateWithContextMenu(IEnumerable<string> items)
        {
            var cb = CreateBase();
            cb.Items.AddRange(items.ToArray());

            var menu = new ContextMenuStrip();
            menu.Items.Add("Clear", null, (s, e) => cb.Text = "");
            menu.Items.Add("Reset", null, (s, e) => cb.SelectedIndex = -1);

            cb.ContextMenuStrip = menu;
            return cb;
        }

        // ---------------------------------------------------------
        //  BASE COMBOBOX (shared defaults)
        // ---------------------------------------------------------
        private static ComboBox CreateBase()
        {
            return new ComboBox
            {
                Size = new Size(200, 24),
                DropDownStyle = ComboBoxStyle.DropDown
            };
        }
    }
}