using RaspberryPi.Userland;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenGlToMatrix
{
    internal sealed class ScopedElement : IDisposable
    {
        private readonly Element m_e;
        private readonly BcmHost m_host;

        public Element E => this.m_e;

        private ScopedElement(BcmHost host, Element e)
        {
            this.m_e = e;
            this.m_host = host;
        }

        public void Dispose()
        {
            var update = this.m_host.Dispman.UpdateStart(0);
            this.m_host.Dispman.ElementRemove(update, this.m_e);
            update.SubmitSync();
        }

        public static ScopedElement Create(BcmHost host, Display display, Resource resource, Rectangle destRect, Rectangle srcRect)
        {
            var update = host.Dispman.UpdateStart(0);
            Element element = host.Dispman.ElementAdd(update, display, 0, destRect, resource, srcRect, Protection.None, null, null, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);
            update.SubmitSync();
            return new ScopedElement(host, element);
        }
    }
}
