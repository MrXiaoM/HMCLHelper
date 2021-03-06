//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;
    
    // Role: PROXY
    public sealed unsafe partial class CefWebPluginInfo : IDisposable
    {
        internal static CefWebPluginInfo FromNative(cef_web_plugin_info_t* ptr)
        {
            return new CefWebPluginInfo(ptr);
        }
        
        internal static CefWebPluginInfo FromNativeOrNull(cef_web_plugin_info_t* ptr)
        {
            if (ptr == null) return null;
            return new CefWebPluginInfo(ptr);
        }
        
        private cef_web_plugin_info_t* _self;
        
        private CefWebPluginInfo(cef_web_plugin_info_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");
            _self = ptr;
        }
        
        ~CefWebPluginInfo()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
        }
        
        public void Dispose()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
            GC.SuppressFinalize(this);
        }
        
        internal int AddRef()
        {
            return cef_web_plugin_info_t.add_ref(_self);
        }
        
        internal int Release()
        {
            return cef_web_plugin_info_t.release(_self);
        }
        
        internal int RefCt
        {
            get { return cef_web_plugin_info_t.get_refct(_self); }
        }
        
        internal cef_web_plugin_info_t* ToNative()
        {
            AddRef();
            return _self;
        }
    }
}
