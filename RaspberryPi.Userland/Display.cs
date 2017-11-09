using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland
{
    public sealed class Display : IDisposable
    {
        private readonly Lazy<DISPMANX_MODEINFO_T> m_infosLoader;
        private readonly DISPMANX_DISPLAY_HANDLE_T m_handle;

        internal DISPMANX_DISPLAY_HANDLE_T Handle { get { return this.m_handle; } }
        public IntPtr Ptr => this.m_handle.Handle;

        internal Display(DISPMANX_DISPLAY_HANDLE_T handle)
        {
            this.m_handle = handle;

            this.m_infosLoader = new Lazy<DISPMANX_MODEINFO_T>(this.GetInfos);
        }

        private DISPMANX_MODEINFO_T GetInfos()
        {
            var ret = DispmanxNativeMethods.DisplayGetInfo(this.m_handle, out DISPMANX_MODEINFO_T infos);
            if (ret != DISPMANX_STATUS_T.SUCCESS)
                throw new DispmanException($"Operation failed : {nameof(DispmanxNativeMethods.DisplayGetInfo)}");
            return infos;
        }

        public int Width { get { return this.m_infosLoader.Value.width; } }
        public int Height { get { return this.m_infosLoader.Value.height; } }
        public Screen DisplayNum { get { return (Screen)this.m_infosLoader.Value.display_num; } }

        public void Dispose()
        {
            DispmanxNativeMethods.DisplayClose(this.m_handle);
        }

        public void Snapshot(Resource resource, DISPMANX_TRANSFORM_T transform)
        {
            var ret = DispmanxNativeMethods.Snapshot(this.m_handle, resource.Handle, transform);
            if (ret != DISPMANX_STATUS_T.SUCCESS)
                throw new DispmanException($"Operation failed : {nameof(DispmanxNativeMethods.Snapshot)}");
        }
    }
}
