using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using RaspberryPi.Native;
using System.Drawing;

namespace RaspberryPi
{
    public sealed class Resource : IDisposable
    {
        private readonly DISPMANX_RESOURCE_HANDLE_T m_handle;
        private readonly VC_IMAGE_TYPE_T m_type;
        private readonly IntPtr m_imagePtr;

        internal DISPMANX_RESOURCE_HANDLE_T Handle { get { return this.m_handle; } }

        internal Resource(DISPMANX_RESOURCE_HANDLE_T handle, VC_IMAGE_TYPE_T type, IntPtr imagePtr)
        {
            this.m_handle = handle;
            this.m_type = type;
            this.m_imagePtr = imagePtr;
        }

        public void WriteData(int pitch, byte[] data, Rectangle rect)
        {
            this.WriteData(this.m_type, pitch, data, rect);
        }

        public void WriteData(VC_IMAGE_TYPE_T type, int pitch, byte[] data, Rectangle rect)
        {
            DISPMANX_STATUS_T ret;
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                VC_RECT_T vcrect = new VC_RECT_T
                {
                    x = rect.X,
                    y = rect.Y,
                    width = rect.Width,
                    height = rect.Height
                };
                ret = Native.DispmanxNativeMethods.ResourceWriteData(this.m_handle, type, pitch, dataHandle.AddrOfPinnedObject(), ref vcrect);
                if (ret != DISPMANX_STATUS_T.SUCCESS)
                    throw new DispmanException($"Operation failed : {nameof(Native.DispmanxNativeMethods.ResourceWriteData)} returned {ret}");
            }
            finally
            {
                dataHandle.Free();
            }
        }

        public void Dispose()
        {
            Native.DispmanxNativeMethods.ResourceDelete(this.m_handle);
        }

        public void ReadData(Rectangle rect, byte[] image, int dest_pitch)
        {
            GCHandle imageHandle = GCHandle.Alloc(image, GCHandleType.Pinned);
            try
            {
                VC_RECT_T vcRect = new VC_RECT_T(rect);
                IntPtr dst_address = imageHandle.AddrOfPinnedObject();
                var ret = DispmanxNativeMethods.ResourceReadData(this.m_handle, ref vcRect, dst_address, (uint)dest_pitch);
                if (ret != DISPMANX_STATUS_T.SUCCESS)
                    throw new DispmanException($"Operation failed : {nameof(Native.DispmanxNativeMethods.ResourceReadData)} returned {ret}");
            }
            finally
            {
                imageHandle.Free();
            }
        }
    }
}
