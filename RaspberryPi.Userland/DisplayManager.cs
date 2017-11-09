using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using NativeMethods = RaspberryPi.Userland.Interop.DispmanxNativeMethods;

namespace RaspberryPi.Userland
{
    public sealed class DisplayManager
    {
        internal DisplayManager()
        {
        }

        /// <summary>
        /// Opens a display on the given device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Display DisplayOpen(Screen device)
        {
            DISPMANX_DISPLAY_HANDLE_T handle = NativeMethods.DisplayOpen((DISPMAN_ID)(int)device);
            if (handle.Handle == IntPtr.Zero)
                return null;
            return new Display(handle);
        }

        /// <summary>
        /// Opens an offscreen display
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public Display DisplayOpenOffscreen(Resource dest, DISPMANX_TRANSFORM_T orientation)
        {
            DISPMANX_DISPLAY_HANDLE_T handle = NativeMethods.DisplayOpenOffscreen(dest.Handle, orientation);
            if (handle.Handle == IntPtr.Zero)
                return null;
            return new Display(handle);
        }

        /// <summary>
        /// Create a new resource
        /// </summary>
        /// <param name="type"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Resource CreateResource(VC_IMAGE_TYPE_T type, int width, int height)
        {
            DISPMANX_RESOURCE_HANDLE_T handle = NativeMethods.ResourceCreate(type, (uint)width, (uint)height, out IntPtr imagePtr);
            if (handle.Handle == IntPtr.Zero)
            {
                return null;
            }
            return new Resource(handle, type, imagePtr);
        }

        /// <summary>
        /// Start a new update
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public Update UpdateStart(int priority)
        {
            DISPMANX_UPDATE_HANDLE_T handle = NativeMethods.UpdateStart(priority);
            if (handle.Handle == IntPtr.Zero)
                return null;
            return new Update(handle);
        }

        public Element ElementAdd(Update update, Display display, int layer, Rectangle dest_rect, Resource resource, Rectangle src_rect, Protection protection, Alpha alpha, Clamp clamp, DISPMANX_TRANSFORM_T transform)
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update));
            if (display == null)
                throw new ArgumentNullException(nameof(display));
            if (protection == null)
                throw new ArgumentNullException(nameof(protection));

            VC_RECT_T d = dest_rect;
            VC_RECT_T s = src_rect;

            VC_DISPMANX_ALPHA_T nativeAlpha = alpha?.Native;
            DISPMANX_CLAMP_T nativeClamp = clamp?.Native;
            DISPMANX_RESOURCE_HANDLE_T resHandle = resource?.Handle ?? DISPMANX_RESOURCE_HANDLE_T.Null;

            DISPMANX_ELEMENT_HANDLE_T handle = NativeMethods.ElementAdd(update.Handle, display.Handle, layer, ref d, resHandle, ref s, protection.Handle, ref nativeAlpha, ref nativeClamp, transform);
            if (handle.Handle == IntPtr.Zero)
                return null;
            return new Element(handle);
        }

        public bool ElementRemove(Update update, Element element)
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update));
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var status = NativeMethods.ElementRemove(update.Handle, element.Handle);
            return status == DISPMANX_STATUS_T.SUCCESS;
        }
    }
}
