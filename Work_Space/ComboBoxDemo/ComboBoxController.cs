using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComboBoxDemo
{
    public class ComboBoxController
    {
        private readonly ComboBoxService _service;

        // Internal state (optional)
        private IEnumerable<string> _colorList;
        private IEnumerable<Car> _carList;
        private IEnumerable<EngineOption> _engineList;

        public ComboBoxController()
        {
            _service = new ComboBoxService();
        }

        // ---------------------------------------------------------
        //  INITIALIZATION ENTRY POINT
        // ---------------------------------------------------------
        public void Initialize(
            ComboBox comboColors,
            ComboBox comboCars,
            ComboBox comboFilter,
            ComboBox comboValidated,
            ComboBox comboKeyboard,
            ComboBox comboCustom,
            ComboBox comboEngines,
            Button buttonOpen,
            Button buttonClose,
            Label labelColor,
            Label labelCar,
            Label labelEngine)
        {
            LoadData();
            ConfigureCombos(
                comboColors,
                comboCars,
                comboFilter,
                comboValidated,
                comboKeyboard,
                comboCustom,
                comboEngines);
            WireEvents(
                comboColors,
                comboCars,
                comboEngines,
                buttonOpen,
                buttonClose,
                labelColor,
                labelCar,
                labelEngine);
        }

        // ---------------------------------------------------------
        //  LOAD DATA (could be DB, API, etc.)
        // ---------------------------------------------------------
        private void LoadData()
        {
            _colorList = new List<string>
            {
                "Red","Green","Blue","Yellow","Purple","Orange",
                "Black","White","Silver","Gray","Maroon","Teal"
            };

            _carList = new List<Car>
            {
                new Car { Id = 1, Name = "Camaro SS" },
                new Car { Id = 2, Name = "Corvette C3" },
                new Car { Id = 3, Name = "Chevelle 396" }
            };

            _engineList = new List<EngineOption>
            {
                new EngineOption { Id = 1, Name = "LS1 350hp", Horsepower = 350 },
                new EngineOption { Id = 2, Name = "LS3 430hp", Horsepower = 430 },
                new EngineOption { Id = 3, Name = "LT1 455hp", Horsepower = 455 }
            };
        }

        // ---------------------------------------------------------
        //  CONFIGURE COMBOBOXES
        // ---------------------------------------------------------
        private void ConfigureCombos(
            ComboBox comboColors,
            ComboBox comboCars,
            ComboBox comboFilter,
            ComboBox comboValidated,
            ComboBox comboKeyboard,
            ComboBox comboCustom,
            ComboBox comboEngines)
        {
            // Basic items
            _service.BindItems(comboColors, _colorList);

            // Cars (data-bound)
            _service.BindObjects(comboCars, _carList, "Name", "Id");

            // Dynamic filter
            _service.EnableDynamicFilter(comboFilter, _colorList);

            // Validation
            _service.BindItems(comboValidated, _colorList);
            _service.EnableValidation(comboValidated);

            // Keyboard navigation
            _service.BindItems(comboKeyboard, _colorList);
            _service.EnableKeyboardNavigation(comboKeyboard);

            // Owner-draw
            _service.BindItems(comboCustom, _colorList);
            _service.EnableOwnerDraw(comboCustom);

            // Engines (complex objects)
            _service.BindObjects(comboEngines, _engineList, "Name", "Id");
        }

        // ---------------------------------------------------------
        //  EVENT WIRING
        // ---------------------------------------------------------
        private void WireEvents(
            ComboBox comboColors,
            ComboBox comboCars,
            ComboBox comboEngines,
            Button buttonOpen,
            Button buttonClose,
            Label labelColor,
            Label labelCar,
            Label labelEngine)
        {
            comboColors.SelectedIndexChanged += (s, e) =>
            {
                labelColor.Text = $"Selected Color: {comboColors.SelectedItem}";
            };

            comboCars.SelectedIndexChanged += (s, e) =>
            {
                if (comboCars.SelectedItem is Car car)
                    labelCar.Text = $"Car: {car.Name} (ID: {car.Id})";
            };

            comboEngines.SelectedIndexChanged += (s, e) =>
            {
                if (comboEngines.SelectedItem is EngineOption engine)
                    labelEngine.Text = $"{engine.Name} — {engine.Horsepower} hp";
            };

            buttonOpen.Click += (s, e) => _service.OpenDropdown(comboColors);
            buttonClose.Click += (s, e) => _service.CloseDropdown(comboColors);
        }

        // ---------------------------------------------------------
        //  PUBLIC API (optional)
        // ---------------------------------------------------------
        public void ResetCombo(ComboBox combo) => _service.Reset(combo);
        public void Open(ComboBox combo) => _service.OpenDropdown(combo);
        public void Close(ComboBox combo) => _service.CloseDropdown(combo);
    }

    // ---------------------------------------------------------
    //  MODEL CLASSES
    // ---------------------------------------------------------
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EngineOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Horsepower { get; set; }
    }
}
