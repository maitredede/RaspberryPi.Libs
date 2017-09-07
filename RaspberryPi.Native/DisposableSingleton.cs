using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi
{
    public abstract class DisposableSingleton<T> : IDisposable where T : DisposableSingleton<T>
    {
        private static DisposableSingleton<T> s_singleton;

        protected DisposableSingleton()
        {
            lock (typeof(T))
            {
                if (s_singleton != null)
                    throw new InvalidOperationException("Already instanciated somewhere and/or not properly disposed");
                s_singleton = this;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lock (typeof(T))
                    {
                        s_singleton = null;
                    }
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        ~DisposableSingleton()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
