using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public class ComboBoxesDemoForm : Form
    {
        public ComboBoxesDemoForm()
        {
            this.Text = "All ComboBoxes Demo";
            this.Width = 400;
            this.Height = 300;

            var combo1 = new ComboGridDropdown { Location = new Point(20, 20), Width = 300 };
            var combo2 = new ComboGridDropdown { Location = new Point(20, 60), Width = 300 };
            var combo3 = new ComboGridDropdown { Location = new Point(20, 100), Width = 300 };

            combo1.Bind(new[] { "Alpha", "Beta", "Gamma" },
                ("Name", (string s) => s));

            combo2.Bind(new[] { 1, 2, 3 },
                ("Value", (int i) => i));

            combo3.Bind(new[] { "Red", "Green", "Blue" },
                ("Color", (string s) => s));

            this.Controls.Add(combo1);
            this.Controls.Add(combo2);
            this.Controls.Add(combo3);
        }
    }
}