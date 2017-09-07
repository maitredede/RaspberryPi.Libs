using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    internal static class DispmanxNativeMethods
    {
        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispmanx_display_open", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr DisplayOpen(uint device);

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispmanx_update_start", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr UpdateStart(int priority);

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispmanx_update_submit_sync", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int UpdateSubmitSync(IntPtr update);

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispmanx_element_add", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr ElementAdd(IntPtr update, IntPtr display, int layer, out VC_RECT_T dest_rect, IntPtr src, out VC_RECT_T src_rect, uint protection, IntPtr alpha, IntPtr clamp, int transform);

        public static readonly int VERSION = 1;

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispman_init", CallingConvention = CallingConvention.Cdecl)]
        //[Obsolete("Use 'vc_vchi_dispmanx_init' ")]
        //public static extern int Init();

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_vchi_dispmanx_init", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void VchiInit(VCHI_INSTANCE_T initialise_instance,
        //    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex =2)]
        //    out VCHI_CONNECTION_T[] connections, uint num_connections);

        //[DllImport(BcmHost.LIB, EntryPoint = "vc_dispmanx_stop", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void Stop();

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_display_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_DISPLAY_HANDLE_T DisplayOpen(DISPMAN_ID device);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_display_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T DisplayClose(DISPMANX_DISPLAY_HANDLE_T display);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_display_get_info", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T DisplayGetInfo(DISPMANX_DISPLAY_HANDLE_T display, out DISPMANX_MODEINFO_T pinfo);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_resource_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_RESOURCE_HANDLE_T ResourceCreate(VC_IMAGE_TYPE_T type, uint width, uint height, out IntPtr imagePtr);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_resource_delete", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T ResourceDelete(DISPMANX_RESOURCE_HANDLE_T handle);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_resource_write_data", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T ResourceWriteData(DISPMANX_RESOURCE_HANDLE_T res, VC_IMAGE_TYPE_T src_type, int src_pitch, IntPtr src_address, ref VC_RECT_T rect);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_resource_write_data_handle", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T ResourceWriteDataHandle(DISPMANX_RESOURCE_HANDLE_T res, VC_IMAGE_TYPE_T src_type, int src_pitch, VCHI_MEM_HANDLE_T handle, uint offset, ref VC_RECT_T rect);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_resource_read_data", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T ResourceReadData(DISPMANX_RESOURCE_HANDLE_T handle, ref VC_RECT_T p_rect, IntPtr dst_address, uint dst_pitch);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_update_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_UPDATE_HANDLE_T UpdateStart(int priority);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_update_submit", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T UpdateSubmit(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_CALLBACK_FUNC_T cb_func, IntPtr cb_arg);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_update_submit_sync", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T UpdateSubmitSync(DISPMANX_UPDATE_HANDLE_T update);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_element_add", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_ELEMENT_HANDLE_T ElementAdd(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_DISPLAY_HANDLE_T display, int layer, ref VC_RECT_T dest_rect, DISPMANX_RESOURCE_HANDLE_T src, ref VC_RECT_T src_rect, DISPMANX_PROTECTION_T protection, ref VC_DISPMANX_ALPHA_T alpha, ref DISPMANX_CLAMP_T clamp, DISPMANX_TRANSFORM_T transform);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_element_remove", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T ElementRemove(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element);

        [DllImport(BcmHostNativeMethods.LIB, EntryPoint = "vc_dispmanx_snapshot", CallingConvention = CallingConvention.Cdecl)]
        public static extern DISPMANX_STATUS_T Snapshot(DISPMANX_DISPLAY_HANDLE_T display, DISPMANX_RESOURCE_HANDLE_T snapshot_resource, DISPMANX_TRANSFORM_T transform);
    }
}
