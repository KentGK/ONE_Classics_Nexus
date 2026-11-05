using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkSpaceApp
{
    public class ComboGridDropdownHost : Form
    {
        private Control _hostedControl;
        private bool _closingFromOutside;

        // Win32 constants for drop shadow + animation
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TOPMOST = 0x00000008;
        private const int CS_DROPSHADOW = 0x00020000;

        private const int AW_BLEND = 0x00080000;
        private const int AW_ACTIVATE = 0x00020000;

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        public ComboGridDropdownHost(Control hostedControl)
        {
            _hostedControl = hostedControl;
            InitializeHost();
        }

        // ---------------------------------------------------------
        //  HOST INITIALIZATION
        // ---------------------------------------------------------
        private void InitializeHost()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;

            Controls.Add(_hostedControl);
            _hostedControl.Dock = DockStyle.Fill;

            Width = _hostedControl.Width;
            Height = _hostedControl.Height;

            Deactivate += (s, e) => CloseFromOutside();
        }

        // ---------------------------------------------------------
        //  WINDOW STYLE OVERRIDES (shadow + no activation)
        // ---------------------------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                cp.ExStyle |= WS_EX_TOOLWINDOW;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                cp.ExStyle |= WS_EX_TOPMOST;

                cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        // ---------------------------------------------------------
        //  SHOW WITH ANIMATION
        // ---------------------------------------------------------
        public void ShowDropdown(Point location)
        {
            Location = location;

            Opacity = 0;
            Show();

            AnimateWindow(Handle, 120, AW_BLEND | AW_ACTIVATE);
            Opacity = 1;
        }

        // ---------------------------------------------------------
        //  CLOSE WITH ANIMATION
        // ---------------------------------------------------------
        public void CloseDropdown()
        {
            AnimateWindow(Handle, 120, AW_BLEND);
            Close();
        }

        private void CloseFromOutside()
        {
            if (!_closingFromOutside)
            {
                _closingFromOutside = true;
                CloseDropdown();
            }
        }
    }
}
