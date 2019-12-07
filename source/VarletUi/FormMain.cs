﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Variety;

namespace VarletUi
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitiateWindow();
            CheckAvailablePHP();

            var cf = Config.Load();
            cf.PhpVersion = Globals.DefaultPhpVersion;
            cf.InstallHttpService = true;
            cf.InstalMailhogService = true;
            cf.Save(Globals.ConfigFileName());

            var httpSvcName = Globals.ServiceNameHttp;
            if (Services.IsServiceInstalled(httpSvcName)) {
                pictStatusHttpd.BackColor = Color.OrangeRed;
                if (Services.IsServiceRunning(httpSvcName))
                {
                    pictStatusHttpd.BackColor = Color.Green;
                    btnServices.Text = "&Stop Services";
                    comboPhpVersion.Enabled = false;
                    lblReloadHttpd.Enabled = true;
                    lblLogfileHttpd.Enabled = true;
                    lblReloadSmtp.Enabled = true;
                    lblLogfileSmtp.Enabled = true;
                } else {
                    pictStatusHttpd.BackColor = Color.OrangeRed;
                    btnServices.Text = "&Start Services";
                    comboPhpVersion.Enabled = true;
                }
            } else {
                pictStatusHttpd.BackColor = Color.SlateGray;
            }

            var smtpSvcName = Globals.ServiceNameSmtp;
            if (Services.IsServiceInstalled(smtpSvcName)) {
                pictStatusSmtp.BackColor = Color.OrangeRed;
                if (Services.IsServiceRunning(smtpSvcName))
                {
                    pictStatusSmtp.BackColor = Color.Green;
                    btnServices.Text = "&Stop Services";
                    lblReloadHttpd.Enabled = true;
                    lblLogfileHttpd.Enabled = true;
                    lblReloadSmtp.Enabled = true;
                    lblLogfileSmtp.Enabled = true;
                } else {
                    pictStatusSmtp.BackColor = Color.OrangeRed;
                    btnServices.Text = "&Start Services";
                }
            } else {
                pictStatusSmtp.BackColor = Color.SlateGray;
                lblReloadHttpd.Enabled = false;
                lblLogfileHttpd.Enabled = false;
                lblReloadSmtp.Enabled = false;
                lblLogfileSmtp.Enabled = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            (new TrayContext()).ShowTrayIconNotification();
            Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            (new TrayContext()).ExitApplication();
        }

        private void InitiateWindow()
        {
            Text = Application.ProductName + " v" + Globals.Version;

            Activate();
            BringToFront();
            Focus();
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            // do something
        }

        public void btnTerminal_Click(object sender, EventArgs e)
        {
            var wwwDir = Common.GetAppPath() + @"\www";
            try
            {
                if (Directory.Exists(Common.DirProgramFiles(@"\PowerShell"))) {
                    var proc = new Process {StartInfo =
                    {
                        FileName = "pwsh.exe",
                        Arguments = "-NoLogo -WorkingDirectory " + wwwDir,
                        UseShellExecute = true
                    }};
                    proc.Start();
                } else  {
                    var proc = new Process {StartInfo =
                    {
                        FileName = "cmd.exe",
                        Arguments = "/k \"cd /d " + wwwDir + "\"",
                        UseShellExecute = true
                    }};
                    proc.Start();
                }
            } catch (FormatException) {
                // do something here
            }
        }

        private void CheckAvailablePHP()
        {
            var pkgPhp = Common.GetAppPath() + @"\pkg\php";

            try
            {
                if (!Directory.Exists(pkgPhp)) return;
                foreach (var t in Directory.GetDirectories(pkgPhp))  {
                    comboPhpVersion.Items.Add(Path.GetFileName(t));
                }
                comboPhpVersion.SelectedIndex = comboPhpVersion.FindStringExact(Globals.DefaultPhpVersion);
            }
            catch (FormatException)
            {
                // do something here
            }
        }

        private void lblAbout_Click(object sender, EventArgs e)
        {
            Common.OpenUrl("https://github.com/riipandi/varlet");
        }

        private void lblHostFile_Click(object sender, EventArgs e)
        {
            try {
                var file = Environment.SystemDirectory + @"\drivers\etc\hosts";
                Common.OpenWithNotepad(file, true);
            } catch (FormatException) {
                // do something here
            }
        }

        public void lblSettings_Click(object sender, EventArgs e)
        {
            new FormSettings().ShowDialog();
        }
    }
}
