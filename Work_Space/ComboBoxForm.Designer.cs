namespace WorkSpaceApp
{
    partial class ComboBoxForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBoxCars;
        private System.Windows.Forms.ComboBox comboFilter;
        private System.Windows.Forms.ComboBox comboValidated;
        private System.Windows.Forms.ComboBox comboKeyboard;
        private System.Windows.Forms.ComboBox comboCustom;
        private System.Windows.Forms.ComboBox comboEngines;
        private ComboGridDropdown comboGridDropdown;
        private ComboGridSearchBar comboGridSearchBar;

        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Label labelCar;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label labelValidated;
        private System.Windows.Forms.Label labelKeyboard;
        private System.Windows.Forms.Label labelCustom;
        private System.Windows.Forms.Label labelEngine;
        private System.Windows.Forms.Label labelGridDropdown;
        private System.Windows.Forms.Label labelGridSearchBar;

        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBoxCars = new System.Windows.Forms.ComboBox();
            this.comboFilter = new System.Windows.Forms.ComboBox();
            this.comboValidated = new System.Windows.Forms.ComboBox();
            this.comboKeyboard = new System.Windows.Forms.ComboBox();
            this.comboCustom = new System.Windows.Forms.ComboBox();
            this.comboEngines = new System.Windows.Forms.ComboBox();
            this.comboGridDropdown = new ComboGridDropdown();
            this.comboGridSearchBar = new ComboGridSearchBar();
            this.labelColor = new System.Windows.Forms.Label();
            this.labelCar = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.labelValidated = new System.Windows.Forms.Label();
            this.labelKeyboard = new System.Windows.Forms.Label();
            this.labelCustom = new System.Windows.Forms.Label();
            this.labelEngine = new System.Windows.Forms.Label();
            this.labelGridDropdown = new System.Windows.Forms.Label();
            this.labelGridSearchBar = new System.Windows.Forms.Label();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(30, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(200, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // comboBoxCars
            // 
            this.comboBoxCars.Location = new System.Drawing.Point(30, 80);
            this.comboBoxCars.Name = "comboBoxCars";
            this.comboBoxCars.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCars.TabIndex = 1;
            // 
            // comboFilter
            // 
            this.comboFilter.Location = new System.Drawing.Point(30, 130);
            this.comboFilter.Name = "comboFilter";
            this.comboFilter.Size = new System.Drawing.Size(200, 21);
            this.comboFilter.TabIndex = 2;
            // 
            // comboValidated
            // 
            this.comboValidated.Location = new System.Drawing.Point(30, 180);
            this.comboValidated.Name = "comboValidated";
            this.comboValidated.Size = new System.Drawing.Size(200, 21);
            this.comboValidated.TabIndex = 3;
            // 
            // comboKeyboard
            // 
            this.comboKeyboard.Location = new System.Drawing.Point(30, 230);
            this.comboKeyboard.Name = "comboKeyboard";
            this.comboKeyboard.Size = new System.Drawing.Size(200, 21);
            this.comboKeyboard.TabIndex = 4;
            // 
            // comboCustom
            // 
            this.comboCustom.Location = new System.Drawing.Point(30, 280);
            this.comboCustom.Name = "comboCustom";
            this.comboCustom.Size = new System.Drawing.Size(200, 21);
            this.comboCustom.TabIndex = 5;
            // 
            // comboEngines
            // 
            this.comboEngines.Location = new System.Drawing.Point(30, 330);
            this.comboEngines.Name = "comboEngines";
            this.comboEngines.Size = new System.Drawing.Size(200, 21);
            this.comboEngines.TabIndex = 6;
            // 
            // comboGridDropdown
            // 
            this.comboGridDropdown.Location = new System.Drawing.Point(30, 382);
            this.comboGridDropdown.Name = "comboGridDropdown";
            this.comboGridDropdown.Size = new System.Drawing.Size(200, 21);
            this.comboGridDropdown.TabIndex = 16;
            // 
            // comboGridSearchBar
            // 
            this.comboGridSearchBar.Location = new System.Drawing.Point(30, 429);
            this.comboGridSearchBar.Name = "comboGridSearchBar";
            this.comboGridSearchBar.Size = new System.Drawing.Size(200, 21);
            this.comboGridSearchBar.TabIndex = 17;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(250, 30);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(300, 24);
            this.labelColor.TabIndex = 7;
            this.labelColor.Text = "Selected Color:";
            // 
            // labelCar
            // 
            this.labelCar.Location = new System.Drawing.Point(250, 80);
            this.labelCar.Name = "labelCar";
            this.labelCar.Size = new System.Drawing.Size(300, 24);
            this.labelCar.TabIndex = 8;
            this.labelCar.Text = "Selected Car:";
            // 
            // labelFilter
            // 
            this.labelFilter.Location = new System.Drawing.Point(250, 130);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(300, 24);
            this.labelFilter.TabIndex = 9;
            this.labelFilter.Text = "Dynamic Filter:";
            // 
            // labelValidated
            // 
            this.labelValidated.Location = new System.Drawing.Point(250, 180);
            this.labelValidated.Name = "labelValidated";
            this.labelValidated.Size = new System.Drawing.Size(300, 24);
            this.labelValidated.TabIndex = 10;
            this.labelValidated.Text = "Validated Input:";
            // 
            // labelKeyboard
            // 
            this.labelKeyboard.Location = new System.Drawing.Point(250, 230);
            this.labelKeyboard.Name = "labelKeyboard";
            this.labelKeyboard.Size = new System.Drawing.Size(300, 24);
            this.labelKeyboard.TabIndex = 11;
            this.labelKeyboard.Text = "Keyboard Navigation:";
            // 
            // labelCustom
            // 
            this.labelCustom.Location = new System.Drawing.Point(250, 280);
            this.labelCustom.Name = "labelCustom";
            this.labelCustom.Size = new System.Drawing.Size(300, 24);
            this.labelCustom.TabIndex = 12;
            this.labelCustom.Text = "Owner-Draw ComboBox:";
            // 
            // labelEngine
            // 
            this.labelEngine.Location = new System.Drawing.Point(250, 330);
            this.labelEngine.Name = "labelEngine";
            this.labelEngine.Size = new System.Drawing.Size(300, 24);
            this.labelEngine.TabIndex = 13;
            this.labelEngine.Text = "Engine Option:";
            // 
            // labelGridDropdown
            // 
            this.labelGridDropdown.Location = new System.Drawing.Point(250, 379);
            this.labelGridDropdown.Name = "labelGridDropdown";
            this.labelGridDropdown.Size = new System.Drawing.Size(300, 24);
            this.labelGridDropdown.TabIndex = 18;
            this.labelGridDropdown.Text = "Grid Dropdown Car:";
            // 
            // labelGridSearchBar
            // 
            this.labelGridSearchBar.Location = new System.Drawing.Point(250, 429);
            this.labelGridSearchBar.Name = "labelGridSearchBar";
            this.labelGridSearchBar.Size = new System.Drawing.Size(300, 24);
            this.labelGridSearchBar.TabIndex = 19;
            this.labelGridSearchBar.Text = "Grid Search Engine:";
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(30, 496);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(90, 30);
            this.buttonOpen.TabIndex = 14;
            this.buttonOpen.Text = "Open";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(140, 496);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 30);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "Close";
            // 
            // ComboBoxForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 560);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.comboBoxCars);
            this.Controls.Add(this.comboFilter);
            this.Controls.Add(this.comboValidated);
            this.Controls.Add(this.comboKeyboard);
            this.Controls.Add(this.comboCustom);
            this.Controls.Add(this.comboEngines);
            this.Controls.Add(this.comboGridDropdown);
            this.Controls.Add(this.comboGridSearchBar);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelCar);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.labelValidated);
            this.Controls.Add(this.labelKeyboard);
            this.Controls.Add(this.labelCustom);
            this.Controls.Add(this.labelEngine);
            this.Controls.Add(this.labelGridDropdown);
            this.Controls.Add(this.labelGridSearchBar);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.buttonClose);
            this.Name = "ComboBoxForm";
            this.Text = "ComboBox Demo Form";
            this.ResumeLayout(false);

        }
    }
}