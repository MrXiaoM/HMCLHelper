namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;

    // TODO: use intptr-sized implementations ?

    /// <summary>
    /// Class representing a binary value. Can be used on any process and thread.
    /// </summary>
    public sealed unsafe partial class CefBinaryValue
    {
        /// <summary>
        /// Creates a new object that is not owned by any other object. The specified
        /// |data| will be copied.
        /// </summary>
        public static CefBinaryValue Create(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            fixed (byte* data_ptr = data)
            {
                var value = cef_binary_value_t.create(data_ptr, (UIntPtr)data.LongLength);
                return CefBinaryValue.FromNative(value);
            }
        }

        /// <summary>
        /// Returns true if this object is valid. Do not call any other methods if this
        /// method returns false.
        /// </summary>
        public bool IsValid
        {
            get { return cef_binary_value_t.is_valid(_self) != 0; }
        }

        /// <summary>
        /// Returns true if this object is currently owned by another object.
        /// </summary>
        public bool IsOwned
        {
            get { return cef_binary_value_t.is_owned(_self) != 0; }
        }

        /// <summary>
        /// Returns a copy of this object. The data in this object will also be copied.
        /// </summary>
        public CefBinaryValue Copy()
        {
            var value = cef_binary_value_t.copy(_self);
            return CefBinaryValue.FromNative(value);
        }

        /// <summary>
        /// Returns the data size.
        /// </summary>
        public long Size
        {
            get { return (long)cef_binary_value_t.get_size(_self); }
        }

        /// <summary>
        /// Read up to |buffer_size| number of bytes into |buffer|. Reading begins at
        /// the specified byte |data_offset|. Returns the number of bytes read.
        /// </summary>
        public long GetData(byte[] buffer, long bufferSize, long dataOffset)
        {
            if (buffer.LongLength < dataOffset + bufferSize) throw new ArgumentOutOfRangeException("dataOffset");

            fixed (byte* buffer_ptr = buffer)
            {
                return (long)cef_binary_value_t.get_data(_self, buffer_ptr, (UIntPtr)bufferSize, (UIntPtr)dataOffset);
            }

            // FIXME: CefBinaryValue.GetData - allow work with buffer / etc
        }

        public byte[] ToArray()
        {
            var value = new byte[Size];
            var readed = GetData(value, value.Length, 0);
            if (readed != value.Length) throw new InvalidOperationException();
            return value;
        }
    }
}
