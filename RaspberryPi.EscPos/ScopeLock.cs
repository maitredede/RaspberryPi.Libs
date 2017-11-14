using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RaspberryPi.EscPos
{
    internal sealed class ScopeLock : IDisposable
    {
        private readonly SemaphoreSlim m_sem;

        internal ScopeLock(SemaphoreSlim sem)
        {
            this.m_sem = sem;
        }

        public void Dispose()
        {
            this.m_sem.Release();
        }
    }
}
