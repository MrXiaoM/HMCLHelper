﻿//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_file_dialog_mode_t.
//
namespace Cef3
{
    /// <summary>
    /// Supported file dialog modes.
    /// </summary>
    public enum CefFileDialogMode
    {
        /// <summary>
        /// Requires that the file exists before allowing the user to pick it.
        /// </summary>
        Open = 0,
        
        /// <summary>
        /// Like Open, but allows picking multiple files to open.
        /// </summary>
        OpenMultiple,
        
        /// <summary>
        /// Allows picking a nonexistent file, and prompts to overwrite if the file
        /// already exists.
        /// </summary>
        Save,
    }
}
