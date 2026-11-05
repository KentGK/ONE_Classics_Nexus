using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComboBoxDemo
{
    public partial class ComboBoxForm : Form
    {
        // Data sources
        private List<string> _allColors;
        private List<Car> _cars;
        private List<EngineOption> _engines;

        public ComboBoxForm()
        {
            InitializeComponent();
            InitializeData();
            InitializeComboBoxes();
            InitializeEvents();
            InitializeContextMenus();
        }

        // -----------------------------
        //  DATA INITIALIZATION
        // -----------------------------
        private void InitializeData()
        {
            _allColors = new List<string>
            {
                "Red","Green","Blue","Yellow","Purple","Orange",
                "Black","White","Silver","Gray","Maroon","Teal"
            };

            _cars = new List<Car>
            {
                new Car { Id = 1, Name = "Camaro SS" },
                new Car { Id = 2, Name = "Corvette C3" },
                new Car { Id = 3, Name = "Chevelle 396" }
            };

            _engines = new List<EngineOption>
            {
                new EngineOption { Id = 1, Name = "LS1 350hp", Horsepower = 350 },
                new EngineOption { Id = 2, Name = "LS3 430hp", Horsepower = 430 },
                new EngineOption { Id = 3, Name = "LT1 455hp", Horsepower = 455 }
            };
        }

        // -----------------------------
        //  COMBOBOX INITIALIZATION
        // -----------------------------
        private void InitializeComboBoxes()
        {
            // Simple items
            comboBox1.Items.AddRange(_allColors.ToArray());

            // Cars (data-bound)
            comboBoxCars.DataSource = _cars;
            comboBoxCars.DisplayMember = "Name";
            comboBoxCars.ValueMember = "Id";

            // Dynamic filter
            comboFilter.Items.AddRange(_allColors.ToArray());

            // Validated input
            comboValidated.Items.AddRange(_allColors.ToArray());

            // Keyboard navigation
            comboKeyboard.Items.AddRange(_allColors.ToArray());

            // Owner-draw
            comboCustom.DrawMode = DrawMode.OwnerDrawFixed;
            comboCustom.Items.AddRange(_allColors.ToArray());

            // Engines (complex objects)
            comboEngines.DataSource = _engines;
            comboEngines.DisplayMember = "Name";
            comboEngines.ValueMember = "Id";

            // Grid dropdown (cars, single-column click-to-select)
            comboGridDropdown.Bind(_cars,
                ("Name", c => c.Name),
                ("Id", c => c.Id));

            // Grid search bar (engines, searchable multi-column)
            comboGridSearchBar.Bind(_engines,
                ("Name", en => en.Name),
                ("Horsepower", en => en.Horsepower));
        }

        // -----------------------------
        //  EVENT WIRING
        // -----------------------------
        private void InitializeEvents()
        {
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBoxCars.SelectedIndexChanged += ComboBoxCars_SelectedIndexChanged;

            comboFilter.TextChanged += ComboFilter_TextChanged;

            comboValidated.Validating += ComboValidated_Validating;

            comboKeyboard.KeyDown += ComboKeyboard_KeyDown;

            comboCustom.DrawItem += ComboCustom_DrawItem;

            comboEngines.SelectedIndexChanged += ComboEngines_SelectedIndexChanged;

            buttonOpen.Click += (s, e) => comboBox1.DroppedDown = true;
            buttonClose.Click += (s, e) => comboBox1.DroppedDown = false;
        }

        // -----------------------------
        //  CONTEXT MENU
        // -----------------------------
        private void InitializeContextMenus()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Clear", null, (s, e) => comboBox1.Text = "");
            menu.Items.Add("Reset", null, (s, e) => comboBox1.SelectedIndex = -1);

            comboBox1.ContextMenuStrip = menu;
        }

        // -----------------------------
        //  EVENT HANDLERS
        // -----------------------------
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelColor.Text = $"Selected Color: {comboBox1.SelectedItem}";
        }

        private void ComboBoxCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCars.SelectedItem is Car car)
                labelCar.Text = $"Car: {car.Name} (ID: {car.Id})";
        }

        private void ComboFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = comboFilter.Text.ToLower();
            comboFilter.Items.Clear();

            foreach (var c in _allColors)
                if (c.ToLower().Contains(filter))
                    comboFilter.Items.Add(c);

            comboFilter.DroppedDown = true;
            comboFilter.SelectionStart = comboFilter.Text.Length;
            comboFilter.SelectionLength = 0;

            labelFilter.Text = $"Dynamic Filter: {comboFilter.Items.Count} match(es)";
        }

        private void ComboValidated_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!comboValidated.Items.Contains(comboValidated.Text))
            {
                MessageBox.Show("Please select a valid item from the list.");
                e.Cancel = true;
            }
        }

        private void ComboKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                labelKeyboard.Text = $"Keyboard Selected: {comboKeyboard.Text}";
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                comboKeyboard.DroppedDown = false;
            }
        }

        private void ComboCustom_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            string text = comboCustom.Items[e.Index].ToString();
            Brush brush = ((e.State & DrawItemState.Selected) != 0)
                ? Brushes.DarkOrange
                : Brushes.Black;

            e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            e.DrawFocusRectangle();

            labelCustom.Text = "Owner-Draw ComboBox: Custom Rendering Active";
        }

        private void ComboEngines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEngines.SelectedItem is EngineOption engine)
                labelEngine.Text = $"{engine.Name} — {engine.Horsepower} hp";
        }
    }

}