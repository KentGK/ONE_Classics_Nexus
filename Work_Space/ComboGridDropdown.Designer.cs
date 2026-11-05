using System.Drawing;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public partial class ComboGridDropdown
    {
        private ComboBox _combo;
        private Panel _dropPanel;
        private DataGridView _grid;

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
    }
}