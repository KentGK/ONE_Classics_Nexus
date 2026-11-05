using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public class ComboBoxService
    {
        // ---------------------------------------------------------
        //  SIMPLE BINDING (string list)
        // ---------------------------------------------------------
        public void BindItems(ComboBox combo, IEnumerable<string> items)
        {
            combo.Items.Clear();
            combo.Items.AddRange(items.ToArray());
        }

        // ---------------------------------------------------------
        //  OBJECT BINDING (DisplayMember / ValueMember)
        // ---------------------------------------------------------
        public void BindObjects<T>(
            ComboBox combo,
            IEnumerable<T> data,
            string displayMember,
            string valueMember)
        {
            combo.DataSource = data.ToList();
            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
        }

        // ---------------------------------------------------------
        //  AUTOCOMPLETE ENABLEMENT
        // ---------------------------------------------------------
        public void EnableAutoComplete(ComboBox combo)
        {
            combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        // ---------------------------------------------------------
        //  DYNAMIC FILTERING (live search)
        // ---------------------------------------------------------
        public void EnableDynamicFilter(ComboBox combo, IEnumerable<string> items)
        {
            var allItems = items.ToList();

            combo.TextChanged += (s, e) =>
            {
                string filter = combo.Text.ToLower();
                combo.Items.Clear();

                foreach (var item in allItems)
                    if (item.ToLower().Contains(filter))
                        combo.Items.Add(item);

                combo.DroppedDown = true;
                combo.SelectionStart = combo.Text.Length;
                combo.SelectionLength = 0;
            };
        }

        // ---------------------------------------------------------
        //  VALIDATION (must match list)
        // ---------------------------------------------------------
        public void EnableValidation(ComboBox combo)
        {
            combo.Validating += (s, e) =>
            {
                if (!combo.Items.Contains(combo.Text))
                {
                    MessageBox.Show("Please select a valid item from the list.");
                    e.Cancel = true;
                }
            };
        }

        // ---------------------------------------------------------
        //  KEYBOARD ENHANCEMENTS
        // ---------------------------------------------------------
        public void EnableKeyboardNavigation(ComboBox combo)
        {
            combo.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    MessageBox.Show($"Selected: {combo.Text}");
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    combo.DroppedDown = false;
                }
            };
        }

        // ---------------------------------------------------------
        //  OWNER-DRAW (custom rendering)
        // ---------------------------------------------------------
        public void EnableOwnerDraw(ComboBox combo)
        {
            combo.DrawMode = DrawMode.OwnerDrawFixed;

            combo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();
                string text = combo.Items[e.Index].ToString();

                Brush brush = ((e.State & DrawItemState.Selected) != 0)
                    ? Brushes.DarkOrange
                    : Brushes.Black;

                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
                e.DrawFocusRectangle();
            };
        }

        // ---------------------------------------------------------
        //  MULTI-COLUMN RENDERING (two-column simulated grid)
        // ---------------------------------------------------------
        public void EnableMultiColumn<T>(
            ComboBox combo,
            IEnumerable<T> data,
            Func<T, string> col1,
            Func<T, string> col2)
        {
            combo.DrawMode = DrawMode.OwnerDrawFixed;

            var list = data.ToList();
            combo.Items.Clear();
            combo.Items.AddRange(list.Cast<object>().ToArray());

            combo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                var item = (T)combo.Items[e.Index];
                e.DrawBackground();

                e.Graphics.DrawString(col1(item), e.Font, Brushes.Black, new PointF(e.Bounds.Left + 5, e.Bounds.Top));
                e.Graphics.DrawString(col2(item), e.Font, Brushes.Gray, new PointF(e.Bounds.Left + 150, e.Bounds.Top));

                e.DrawFocusRectangle();
            };
        }

        // ---------------------------------------------------------
        //  CONTEXT MENU (Clear / Reset)
        // ---------------------------------------------------------
        public void AttachContextMenu(ComboBox combo)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Clear", null, (s, e) => combo.Text = "");
            menu.Items.Add("Reset", null, (s, e) => combo.SelectedIndex = -1);

            combo.ContextMenuStrip = menu;
        }

        // ---------------------------------------------------------
        //  PROGRAMMATIC DROPDOWN CONTROL
        // ---------------------------------------------------------
        public void OpenDropdown(ComboBox combo) => combo.DroppedDown = true;
        public void CloseDropdown(ComboBox combo) => combo.DroppedDown = false;

        // ---------------------------------------------------------
        //  COMBOBOX RESET
        // ---------------------------------------------------------
        public void Reset(ComboBox combo)
        {
            combo.SelectedIndex = -1;
            combo.Text = "";
        }
    }
}
