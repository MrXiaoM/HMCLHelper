namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;

    /// <summary>
    /// Callback interface for asynchronous continuation of file dialog requests.
    /// </summary>
    public sealed unsafe partial class CefFileDialogCallback
    {
        /// <summary>
        /// Continue the file selection with the specified |file_paths|. This may be
        /// a single value or a list of values depending on the dialog mode. An empty
        /// value is treated the same as calling Cancel().
        /// </summary>
        public void Continue(string[] filePaths)
        {
            var n_filePaths = cef_string_list.From(filePaths);

            cef_file_dialog_callback_t.cont(_self, n_filePaths);

            libcef.string_list_free(n_filePaths);
        }

        /// <summary>
        /// Cancel the file selection.
        /// </summary>
        public void Cancel()
        {
            cef_file_dialog_callback_t.cancel(_self);
        }
    }
}
