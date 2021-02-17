namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;

    /// <summary>
    /// Callback interface for CefBrowserHost::RunFileDialog. The methods of this
    /// class will be called on the browser process UI thread.
    /// </summary>
    public abstract unsafe partial class CefRunFileDialogCallback
    {
        private void cont(cef_run_file_dialog_callback_t* self, cef_browser_host_t* browser_host, cef_string_list* file_paths)
        {
            CheckSelf(self);

            var mBrowserHost = CefBrowserHost.FromNative(browser_host);
            var mFilePaths = cef_string_list.ToArray(file_paths);

            OnFileDialogDismissed(mBrowserHost, mFilePaths);
        }

        /// <summary>
        /// Called asynchronously after the file dialog is dismissed. If the selection
        /// was successful |file_paths| will be a single value or a list of values
        /// depending on the dialog mode. If the selection was cancelled |file_paths|
        /// will be empty.
        /// </summary>
        protected abstract void OnFileDialogDismissed(CefBrowserHost browserHost, string[] filePaths);
    }
}
