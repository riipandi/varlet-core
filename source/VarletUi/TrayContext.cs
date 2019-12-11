﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace VarletUi
{
    public class TrayContext : ApplicationContext
    {
        #region Private Members
        private readonly NotifyIcon _trayIcon;

        #endregion

        public TrayContext()
        {
            _trayIcon = new NotifyIcon(new System.ComponentModel.Container())
            {
                Visible = true,
                Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(FormMain)).GetObject("$this.Icon"))),
                BalloonTipText = Application.ProductName + " minimized to tray.",
                Text = Application.ProductName + "v" + Application.ProductVersion,
            };
            _trayIcon.DoubleClick += new System.EventHandler(TrayIcon_DoubleClick);

            // Initiate the context menu and items
            var trayContextMenu = new ContextMenuStrip();
            _trayIcon.ContextMenuStrip = trayContextMenu;

            // Context menu item
            var trayMenuItemDisplayForm = new ToolStripMenuItem() { Text = "Open " + Application.ProductName };
            trayMenuItemDisplayForm.Click += new EventHandler(TrayMenuItemDisplayForm_Click);

            var trayMenuItemOptions = new ToolStripMenuItem() { Text = "&Preferences" };
            trayMenuItemOptions.Click += new EventHandler(TrayMenuItemOptions_Click);

            var trayMenuItemExit = new ToolStripMenuItem() { Text = "E&xit" };
            trayMenuItemExit.Click += new EventHandler(TrayMenuItemExit_Click);

            // Attach context menu item
            trayContextMenu.Items.Add(trayMenuItemDisplayForm);
            trayContextMenu.Items.Add(trayMenuItemOptions);
            trayContextMenu.Items.Add(new ToolStripSeparator());
            trayContextMenu.Items.Add(trayMenuItemExit);
        }

        private static void TrayMenuItemOptions_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMainForm();
                var fs = new FormSetting();
                foreach (Form fc in Application.OpenForms) {
                    if (fc.Name == fs.Name) fc.Dispose();
                }

                (new FormMain()).lblSetting_Click(sender, e);
            }
            catch (FormatException)  {}
        }

        public void ShowTrayIconNotification()
        {
            _trayIcon.ShowBalloonTip(3000);
            _trayIcon.Dispose();
        }

        private static void TrayIcon_DoubleClick(object Sender, EventArgs e)
        {
            ShowMainForm();
        }

        private static void TrayMenuItemDisplayForm_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        public void ExitApplication()
        {
            ExitThreadCore();
        }

        private void TrayMenuItemExit_Click(object sender, EventArgs e)
        {
            ExitThreadCore();
        }

        protected override void ExitThreadCore()
        {
            base.ExitThreadCore();
            if (MessageBox.Show("Exit Varlet Controller?", Application.ProductName, MessageBoxButtons.YesNo) !=  DialogResult.Yes) return;
            _trayIcon.Dispose();
            Application.ExitThread();
        }

        private static void ShowMainForm()
        {
            try {
                var fm = new FormMain();
                foreach (Form fc in Application.OpenForms) {
                    if (fc.Name == fm.Name) fc.Hide();
                }
                fm.Show();
                fm.Activate();
                fm.BringToFront();
                fm.Focus();
            } catch (FormatException) {}
        }
    }
}
