namespace WinFormDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chromeWebBrowser1 = new Sashulin.ChromeWebBrowser();
            this.SuspendLayout();
            // 
            // chromeWebBrowser1
            // 
            this.chromeWebBrowser1.BackColor = System.Drawing.Color.White;
            this.chromeWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromeWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.chromeWebBrowser1.Name = "chromeWebBrowser1";
            this.chromeWebBrowser1.Size = new System.Drawing.Size(600, 600);
            this.chromeWebBrowser1.TabIndex = 0;
            this.chromeWebBrowser1.BrowserTitleChange += new Sashulin.TitleChangeEventHandler(this.chromeWebBrowser1_BrowserTitleChange);
            this.chromeWebBrowser1.BrowserUrlChange += new Sashulin.UrlChangeEventHandler(this.chromeWebBrowser1_BrowserUrlChange);
            this.chromeWebBrowser1.BrowserNewWindow += new Sashulin.NewWindowEventHandler(this.chromeWebBrowser1_BrowserNewWindow);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.chromeWebBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading - HMCLHelper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sashulin.ChromeWebBrowser chromeWebBrowser1;

    }
}

