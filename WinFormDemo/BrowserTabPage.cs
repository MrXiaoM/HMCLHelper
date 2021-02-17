using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sashulin;

namespace WinFormDemo
{
    public partial class BrowserTabPage : TabPage
    {
        public ChromeWebBrowser browser;

        public BrowserTabPage(string URL,bool IsGoHome = false)
        {
            browser = new ChromeWebBrowser();
            InitializeComponent();
            this.browser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.TabIndex = 0;

            CSharpBrowserSettings settings = new CSharpBrowserSettings();
            //settings.UserAgent = "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 4 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
            settings.CachePath = @"C:\temp\caches";
            browser.Initialize(settings);
            this.Controls.Add(browser);
            browser.Validate();
            browser.Dock = DockStyle.Fill;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
