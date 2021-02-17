namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;

    /// <summary>
    /// Implement this interface to handle events related to geolocation permission
    /// requests. The methods of this class will be called on the browser process IO
    /// thread.
    /// </summary>
    public abstract unsafe partial class CefGeolocationHandler
    {
        private void on_request_geolocation_permission(cef_geolocation_handler_t* self, cef_browser_t* browser, cef_string_t* requesting_url, int request_id, cef_geolocation_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_requesting_url = cef_string_t.ToString(requesting_url);
            var m_callback = CefGeolocationCallback.FromNative(callback);

            OnRequestGeolocationPermission(m_browser, m_requesting_url, request_id, m_callback);
        }

        /// <summary>
        /// Called when a page requests permission to access geolocation information.
        /// |requesting_url| is the URL requesting permission and |request_id| is the
        /// unique ID for the permission request. Call CefGeolocationCallback::Continue
        /// to allow or deny the permission request.
        /// </summary>
        protected abstract void OnRequestGeolocationPermission(CefBrowser browser, string requestingUrl, int requestId, CefGeolocationCallback callback);


        private void on_cancel_geolocation_permission(cef_geolocation_handler_t* self, cef_browser_t* browser, cef_string_t* requesting_url, int request_id)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_requesting_url = cef_string_t.ToString(requesting_url);

            OnCancelGeolocationPermission(m_browser, m_requesting_url, request_id);
        }

        /// <summary>
        /// Called when a geolocation access request is canceled. |requesting_url| is
        /// the URL that originally requested permission and |request_id| is the unique
        /// ID for the permission request.
        /// </summary>
        protected abstract void OnCancelGeolocationPermission(CefBrowser browser, string requesting_url, int request_id);
    }
}
