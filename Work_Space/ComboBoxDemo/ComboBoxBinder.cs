using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComboBoxDemo
{
    public class ComboBoxBinder
    {
        private readonly ComboBoxService _service;

        // Declarative binding rules
        private readonly Dictionary<string, ComboBoxBindingRule> _rules;

        public ComboBoxBinder()
        {
            _service = new ComboBoxService();
            _rules = new Dictionary<string, ComboBoxBindingRule>();
        }

        // ---------------------------------------------------------
        //  REGISTER BINDING RULE
        // ---------------------------------------------------------
        public void Register(string key, ComboBox combo, ComboBoxBindingRule rule)
        {
            rule.Combo = combo;
            _rules[key] = rule;

            ApplyRule(rule);
        }

        // ---------------------------------------------------------
        //  APPLY A SINGLE RULE
        // ---------------------------------------------------------
        private void ApplyRule(ComboBoxBindingRule rule)
        {
            if (rule.Items != null)
                _service.BindItems(rule.Combo, rule.Items);

            if (rule.Objects != null)
                _service.BindObjects(rule.Combo, rule.Objects, rule.DisplayMember, rule.ValueMember);

            if (rule.EnableAutoComplete)
                _service.EnableAutoComplete(rule.Combo);

            if (rule.EnableDynamicFilter)
                _service.EnableDynamicFilter(rule.Combo, rule.Items);

            if (rule.EnableValidation)
                _service.EnableValidation(rule.Combo);

            if (rule.EnableKeyboardNavigation)
                _service.EnableKeyboardNavigation(rule.Combo);

            if (rule.EnableOwnerDraw)
                _service.EnableOwnerDraw(rule.Combo);

            if (rule.EnableContextMenu)
                _service.AttachContextMenu(rule.Combo);

            if (rule.MultiColumnObjects != null)
                _service.EnableMultiColumn(
                    rule.Combo,
                    rule.MultiColumnObjects,
                    rule.Column1Selector,
                    rule.Column2Selector);
        }

        // ---------------------------------------------------------
        //  RELOAD DATA FOR A RULE
        // ---------------------------------------------------------
        public void Reload(string key, IEnumerable<string> items)
        {
            if (_rules.TryGetValue(key, out var rule))
            {
                rule.Items = items;
                _service.BindItems(rule.Combo, items);
            }
        }

        public void ReloadObjects<T>(string key, IEnumerable<T> objects)
        {
            if (_rules.TryGetValue(key, out var rule))
            {
                rule.Objects = objects.Cast<object>().ToList();
                _service.BindObjects(rule.Combo, objects, rule.DisplayMember, rule.ValueMember);
            }
        }

        // ---------------------------------------------------------
        //  APPLY ALL RULES
        // ---------------------------------------------------------
        public void ApplyAll()
        {
            foreach (var rule in _rules.Values)
                ApplyRule(rule);
        }
    }

    // ---------------------------------------------------------
    //  DECLARATIVE BINDING RULE MODEL
    // ---------------------------------------------------------
    public class ComboBoxBindingRule
    {
        public ComboBox Combo { get; set; }

        // Simple items
        public IEnumerable<string> Items { get; set; }

        // Object binding
        public IEnumerable<object> Objects { get; set; }
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }

        // Behavior flags
        public bool EnableAutoComplete { get; set; }
        public bool EnableDynamicFilter { get; set; }
        public bool EnableValidation { get; set; }
        public bool EnableKeyboardNavigation { get; set; }
        public bool EnableOwnerDraw { get; set; }
        public bool EnableContextMenu { get; set; }

        // Multi-column binding
        public IEnumerable<object> MultiColumnObjects { get; set; }
        public Func<object, string> Column1Selector { get; set; }
        public Func<object, string> Column2Selector { get; set; }
    }
}
