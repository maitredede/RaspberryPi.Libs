using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Interop
{
    public abstract class NativeObject : SafeHandle
    {
        public NativeObject(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle)
        {
            this.SetHandle(handle);
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected abstract void FreeHandle();

        protected sealed override bool ReleaseHandle()
        {
            if (this.IsInvalid)
                return false;
            this.FreeHandle();
            this.SetHandleAsInvalid();
            return true;
        }
    }
}
