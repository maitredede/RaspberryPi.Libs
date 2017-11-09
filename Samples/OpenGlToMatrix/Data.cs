using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenGlToMatrix
{
    public sealed class Data : IDisposable
    {
        private readonly GCHandle m_quadx;
        private readonly GCHandle m_textCoords;
        private readonly byte[] tex1;
        private readonly byte[] tex2;
        private readonly byte[] tex3;
        private readonly GCHandle tex1Handle;
        private readonly GCHandle tex2Handle;
        private readonly GCHandle tex3Handle;

        public Data()
        {
            this.m_quadx = GCHandle.Alloc(quadx, GCHandleType.Pinned);
            this.m_textCoords = GCHandle.Alloc(texCoords, GCHandleType.Pinned);

            this.LoadTex(ref this.tex1, ref this.tex1Handle, "Lucca_128_128.raw");
            this.LoadTex(ref this.tex2, ref this.tex2Handle, "Gaudi_128_128.raw");
            this.LoadTex(ref this.tex3, ref this.tex3Handle, "Djenne_128_128.raw");
        }

        private void LoadTex(ref byte[] tex, ref GCHandle texHandle, string file)
        {
            int image_sz = Program.IMAGE_SIZE * Program.IMAGE_SIZE * 3;
            using (FileStream fs = File.OpenRead(file))
            using (MemoryStream ms = new MemoryStream())
            {
                fs.CopyTo(ms);

                if (ms.Length != image_sz)
                    throw new InvalidOperationException(string.Format("File size differ: expected {0} found {1}", image_sz, ms.Length));
                tex = ms.ToArray();
            }
            texHandle = GCHandle.Alloc(tex, GCHandleType.Pinned);
        }

        public IntPtr QuadX => this.m_quadx.AddrOfPinnedObject();
        public IntPtr TexCoords => this.m_textCoords.AddrOfPinnedObject();
        public IntPtr TexBuf1 => this.tex1Handle.AddrOfPinnedObject();
        public IntPtr TexBuf2 => this.tex2Handle.AddrOfPinnedObject();
        public IntPtr TexBuf3 => this.tex3Handle.AddrOfPinnedObject();

        public void Dispose()
        {
            this.m_quadx.Free();
            this.tex1Handle.Free();
            this.tex2Handle.Free();
            this.tex3Handle.Free();
        }

        private static readonly sbyte[] quadx = {
   /* FRONT */
   -10, -10,  10,
   10, -10,  10,
   -10,  10,  10,
   10,  10,  10,

   /* BACK */
   -10, -10, -10,
   -10,  10, -10,
   10, -10, -10,
   10,  10, -10,

   /* LEFT */
   -10, -10,  10,
   -10,  10,  10,
   -10, -10, -10,
   -10,  10, -10,

   /* RIGHT */
   10, -10, -10,
   10,  10, -10,
   10, -10,  10,
   10,  10,  10,

   /* TOP */
   -10,  10,  10,
   10,  10,  10,
   -10,  10, -10,
   10,  10, -10,

   /* BOTTOM */
   -10, -10,  10,
   -10, -10, -10,
   10, -10,  10,
   10, -10, -10,
};

        private static float[] texCoords = {
   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f,

   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f,

   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f,

   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f,

   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f,

   0.0f,  0.0f,
   0.0f,  1.0f,
   1.0f,  0.0f,
   1.0f,  1.0f
};
    }
}
