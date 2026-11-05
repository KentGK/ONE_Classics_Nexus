using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public class ComboBoxOrchestrator
    {
        private readonly ComboBoxController _controller;
        private readonly ComboBoxService _service;

        // Internal registry of all ComboBoxes managed by the orchestrator
        private readonly Dictionary<string, ComboBox> _registry;

        public ComboBoxOrchestrator()
        {
            _controller = new ComboBoxController();
            _service = new ComboBoxService();
            _registry = new Dictionary<string, ComboBox>();
        }

        // ---------------------------------------------------------
        //  REGISTER COMBOBOXES (central registry)
        // ---------------------------------------------------------
        public void Register(string key, ComboBox combo)
        {
            if (!_registry.ContainsKey(key))
                _registry.Add(key, combo);
        }

        public ComboBox Get(string key)
        {
            return _registry.TryGetValue(key, out var combo) ? combo : null;
        }

        // ---------------------------------------------------------
        //  INITIALIZE FULL WORKFLOW
        // ---------------------------------------------------------
        public void InitializeWorkflow(
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
            // Register all ComboBoxes
            Register("colors", comboColors);
            Register("cars", comboCars);
            Register("filter", comboFilter);
            Register("validated", comboValidated);
            Register("keyboard", comboKeyboard);
            Register("custom", comboCustom);
            Register("engines", comboEngines);

            // Delegate initialization to the controller
            _controller.Initialize(
                comboColors,
                comboCars,
                comboFilter,
                comboValidated,
                comboKeyboard,
                comboCustom,
                comboEngines,
                buttonOpen,
                buttonClose,
                labelColor,
                labelCar,
                labelEngine);
        }

        // ---------------------------------------------------------
        //  GLOBAL RESET (all ComboBoxes)
        // ---------------------------------------------------------
        public void ResetAll()
        {
            foreach (var combo in _registry.Values)
                combo.SelectedIndex = -1;
        }

        // ---------------------------------------------------------
        //  GLOBAL DROPDOWN CONTROL
        // ---------------------------------------------------------
        public void OpenAll()
        {
            foreach (var combo in _registry.Values)
                combo.DroppedDown = true;
        }

        public void CloseAll()
        {
            foreach (var combo in _registry.Values)
                combo.DroppedDown = false;
        }

        // ---------------------------------------------------------
        //  APPLY OWNER-DRAW TO ALL COMBOBOXES
        // ---------------------------------------------------------
        public void ApplyOwnerDrawToAll()
        {
            foreach (var combo in _registry.Values)
                _service.EnableOwnerDraw(combo);
        }

        // ---------------------------------------------------------
        //  APPLY VALIDATION TO ALL COMBOBOXES
        // ---------------------------------------------------------
        public void ApplyValidationToAll()
        {
            foreach (var combo in _registry.Values)
                _service.EnableValidation(combo);
        }

        // ---------------------------------------------------------
        //  APPLY KEYBOARD NAVIGATION TO ALL COMBOBOXES
        // ---------------------------------------------------------
        public void ApplyKeyboardNavigationToAll()
        {
            foreach (var combo in _registry.Values)
                _service.EnableKeyboardNavigation(combo);
        }

        // ---------------------------------------------------------
        //  APPLY CONTEXT MENUS TO ALL COMBOBOXES
        // ---------------------------------------------------------
        public void ApplyContextMenusToAll()
        {
            foreach (var combo in _registry.Values)
                _service.AttachContextMenu(combo);
        }

        // ---------------------------------------------------------
        //  RELOAD DATA FOR A SPECIFIC COMBOBOX
        // ---------------------------------------------------------
        public void Reload(string key, IEnumerable<string> items)
        {
            var combo = Get(key);
            if (combo != null)
                _service.BindItems(combo, items);
        }

        // ---------------------------------------------------------
        //  RELOAD OBJECT DATA FOR A SPECIFIC COMBOBOX
        // ---------------------------------------------------------
        public void ReloadObjects<T>(string key, IEnumerable<T> data, string display, string value)
        {
            var combo = Get(key);
            if (combo != null)
                _service.BindObjects(combo, data.Cast<object>(), display, value);
        }
    }
}
