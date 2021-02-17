﻿//
// This file manually written from:
//     cef/include/internal/cef_types_win.h.
//
namespace Cef3.Interop
{
    using System;
    using System.Runtime.InteropServices;

    internal struct cef_window_info_t
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_window_info_t_windows
    {
        // Standard parameters required by CreateWindowEx()
        public uint ex_style;
        public cef_string_t window_name;
        public uint style;
        public int x;
        public int y;
        public int width;
        public int height;
        public IntPtr parent_window;
        public IntPtr menu;

        // If window rendering is disabled no browser window will be created. Set
        // |parent_window| to be used for identifying monitor info
        // (MonitorFromWindow). If |parent_window| is not provided the main screen
        // monitor will be used.
        public int window_rendering_disabled;

        // Set to true to enable transparent painting.
        public int transparent_painting;

        // Handle for the new browser window.
        public IntPtr window;

        #region Alloc & Free
        private static int _sizeof;

        static cef_window_info_t_windows()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_window_info_t_windows));
        }

        public static cef_window_info_t_windows* Alloc()
        {
            var ptr = (cef_window_info_t_windows*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_window_info_t_windows();
            return ptr;
        }

        public static void Free(cef_window_info_t_windows* ptr)
        {
            if (ptr != null)
            {
                libcef.string_clear(&ptr->window_name);
                Marshal.FreeHGlobal((IntPtr)ptr);
            }
        }
        #endregion
    }

    internal unsafe struct cef_window_info_t_linux
    {
        public IntPtr parent_widget;
        public bool_t window_rendering_disabled;
        public bool_t transparent_painting;
        public IntPtr widget;

        #region Alloc & Free
        private static int _sizeof;

        static cef_window_info_t_linux()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_window_info_t_linux));
        }

        public static cef_window_info_t_linux* Alloc()
        {
            var ptr = (cef_window_info_t_linux*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_window_info_t_linux();
            return ptr;
        }

        public static void Free(cef_window_info_t_linux* ptr)
        {
            if (ptr != null)
            {
                Marshal.FreeHGlobal((IntPtr)ptr);
            }
        }
        #endregion
    }

    internal unsafe struct cef_window_info_t_mac
    {
        public cef_string_t window_name;
        public int x;
        public int y;
        public int width;
        public int height;
        public int hidden;

        // NSView pointer for the parent view.
        public IntPtr parent_view;

        public bool_t window_rendering_disabled;
        public bool_t transparent_painting;

        // NSView pointer for the new browser view.
        public IntPtr view;

        #region Alloc & Free
        private static int _sizeof;

        static cef_window_info_t_mac()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_window_info_t_mac));
        }

        public static cef_window_info_t_mac* Alloc()
        {
            var ptr = (cef_window_info_t_mac*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_window_info_t_mac();
            return ptr;
        }

        public static void Free(cef_window_info_t_mac* ptr)
        {
            if (ptr != null)
            {
                libcef.string_clear(&ptr->window_name);
                Marshal.FreeHGlobal((IntPtr)ptr);
            }
        }
        #endregion
    }

}
