using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sashulin;
using Sashulin.Core;
using Sashulin.common;
using System.Text.RegularExpressions;
using System.IO;
namespace WinFormDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            onTitleChangeEvent += onTitleChange;
            onLoginEvent += onLogin;

            string url = "https://login.live.com/oauth20_authorize.srf" +
                    "?client_id=00000000402b5328" +
                    "&response_type=code" +
                    "&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL" +
                    "&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf";
            CSharpBrowserSettings settings = new CSharpBrowserSettings();
            //settings.UserAgent = "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 4 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
            settings.CachePath = Application.StartupPath + "\\caches";
            settings.DefaultUrl = url;
            this.chromeWebBrowser1.Initialize(settings);
            this.chromeWebBrowser1.SetPopupMenuVisible(false);

            this.chromeWebBrowser1.Validate();
        }


        public string ShowMessage(string msg)
        {
            MessageBox.Show(msg);
            return "return : Js call";
        }

        private void showmsg()
        {
            MessageBox.Show("element listener");
        }

        private void chromeWebBrowser1_BrowserUrlChange(object sender, UrlChangeEventArgs e)
        {
            Regex regex = new Regex("^https://login\\.live\\.com/oauth20_desktop\\.srf\\?code=(.*?)&lc=(.*?)$");
            if (regex.IsMatch(e.Url))
            {
                String s = e.Url;
                String code = s.Substring(s.IndexOf('=') + 1, s.IndexOf('&') - (s.IndexOf('=') + 1));
                this.Invoke(onLoginEvent, code);
            }
        }

        private void chromeWebBrowser1_BrowserTitleChange(object sender, TitleEventArgs e)
        {
            this.Invoke(onTitleChangeEvent, e.Title);
        }

        delegate void OnTitleChange(string title);

        event OnTitleChange onTitleChangeEvent;

        delegate void OnLogin(string title);

        event OnLogin onLoginEvent;

        private void onTitleChange(string title)
        {
            this.Text = title + " - HMCLHelper";
        }

        private void onLogin(string code)
        {
            DialogTextBox dialog = new DialogTextBox(code);
            dialog.ShowDialog();
            Application.Exit();
        }

        private void chromeWebBrowser1_BrowserNewWindow(object sender, NewWindowEventArgs e)
        {
            chromeWebBrowser1.OpenUrl(e.NewUrl);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
