using RaspberryPi.LibFreefare.Interop;
using RaspberryPi.LibNFC;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    /// <summary>
    /// Base class for freefare tags
    /// </summary>
    public abstract class FreefareTag : IDisposable
    {
        private readonly IntPtr m_ptr;
        private readonly bool m_dispose;
        private readonly NfcDevice m_device;
        private readonly NfcTarget m_target;
        private TagType? m_type;
        private string m_name;
        private string m_uid;

        internal IntPtr Handle => this.m_ptr;
        internal NfcDevice Device => this.m_device;

        internal FreefareTag(IntPtr ptr, bool dispose, NfcDevice device, NfcTarget target)
        {
            this.m_ptr = ptr;
            this.m_dispose = dispose;
            this.m_device = device;
            this.m_target = target;
        }

        internal static FreefareTag Build(IntPtr ptrTag, bool dispose, NfcDevice device, NfcTarget target)
        {
            TagType type = NativeMethods.freefare_get_tag_type(ptrTag);
            Console.WriteLine("  type={0}", type);

            FreefareTag tag;
            switch (type)
            {
                case TagType.NTAG_21x:
                    tag = new NTAG21xTag(ptrTag, dispose, device, target);
                    Console.WriteLine("  subtype={0}", ((NTAG21xTag)tag).SubType);
                    break;
                case TagType.MIFARE_CLASSIC_1K:
                    tag = new MifareClassic1kTag(ptrTag, dispose, device, target);
                    break;
                case TagType.MIFARE_CLASSIC_4K:
                    tag = new MifareClassic4kTag(ptrTag, dispose, device, target);
                    break;
                //TODO other tag types
                default:
                    tag = new GenericTag(ptrTag, dispose, device, target);
                    break;
            }
            return tag;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.
                if (this.m_dispose)
                {
                    NativeMethods.freefare_free_tag(this.m_ptr);
                }

                disposedValue = true;
            }
        }

        ~FreefareTag()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        void IDisposable.Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Get the tag type
        /// </summary>
        public TagType Type
        {
            get
            {
                if (!this.m_type.HasValue)
                {
                    this.m_type = NativeMethods.freefare_get_tag_type(this.m_ptr);
                }
                return this.m_type.Value;
            }
        }

        /// <summary>
        /// Get the tag friendly name
        /// </summary>
        public string Name
        {
            get
            {
                if (this.m_name == null)
                {
                    IntPtr namePtr = NativeMethods.freefare_get_tag_friendly_name(this.m_ptr);
                    this.m_name = Marshal.PtrToStringAnsi(namePtr);
                }
                return this.m_name;
            }
        }

        /// <summary>
        /// Get the tag UID
        /// </summary>
        public string UID
        {
            get
            {
                if (this.m_uid == null)
                {
                    this.m_uid = NativeMethods.freefare_get_tag_uid(this.m_ptr);
                }
                return this.m_uid;
            }
        }

        internal string LastError()
        {
            IntPtr ptr = NativeMethods.freefare_strerror(this.m_ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        private static Func<FreefareTag, bool> GetIsPresent;

        private static bool FreeFareIsPresent(FreefareTag tag)
        {
            return NativeMethods.freefare_selected_tag_is_present(tag.m_device.DangerousGetHandle());
        }

        private static bool FreeFareIsPresentAlternative(FreefareTag tag)
        {
            throw new NotImplementedException();
            //if (tag.m_target != null)
            //{
            //    return tag.m_target.IsPresent();
            //}
            //IntPtr ptrArr = NativeMethods.freefare_get_tags(tag.m_device.DangerousGetHandle());
            //if (ptrArr == IntPtr.Zero)
            //    throw new FreefareException("Error enumerating tags: " + tag.m_device.LastError);

            //IntPtr ptr;
            //int i = 0;
            //while ((ptr = Marshal.ReadIntPtr(ptrArr, IntPtr.Size * i++)) != IntPtr.Zero)
            //{
            //    if (ptr == tag.m_ptr)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public bool IsPresent()
        {
            if (GetIsPresent == null)
            {
                lock (typeof(FreefareTag))
                {
                    if (GetIsPresent == null)
                    {
                        try
                        {
                            bool value = FreeFareIsPresent(this);
                            GetIsPresent = FreeFareIsPresent;
                            return false;
                        }
                        catch (EntryPointNotFoundException)
                        {
                            GetIsPresent = FreeFareIsPresentAlternative;
                        }
                    }
                }
            }
            return GetIsPresent(this);

            ////return NativeMethods.freefare_selected_tag_is_present(this.m_device.Handle);
            //IntPtr ptrDevice = Marshal.ReadIntPtr(this.m_ptr, 0);
            //var info = Marshal.PtrToStructure<LibNFC.Interop.nfc_iso14443a_info>(this.m_ptr + IntPtr.Size);

            //Console.WriteLine("Values of {0}", info.GetType().Name);
            //foreach (FieldInfo f in info.GetType().GetFields())
            //{
            //    Console.WriteLine("{0}={1}", f.Name, f.GetValue(info));
            //}
            //Console.WriteLine("---");
            //return false;

            //NfcError error = LibNFC.Interop.NativeMethods.initiator_target_is_present(ptrDevice, ptrTarget);
            //Console.WriteLine("isPresent={0}", error);
            //switch (error)
            //{
            //    case NfcError.Success:
            //        return true;
            //    case NfcError.NFC_ENOTSUCHDEV:
            //        return false;
            //    default:
            //        NfcException.Raise(error);
            //        return false;
            //}
        }
    }
}
