using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal enum MMAL_STATUS_T
    {
        MMAL_SUCCESS = 0,                 /**< Success */
        MMAL_ENOMEM,                      /**< Out of memory */
        MMAL_ENOSPC,                      /**< Out of resources (other than memory) */
        MMAL_EINVAL,                      /**< Argument is invalid */
        MMAL_ENOSYS,                      /**< Function not implemented */
        MMAL_ENOENT,                      /**< No such file or directory */
        MMAL_ENXIO,                       /**< No such device or address */
        MMAL_EIO,                         /**< I/O error */
        MMAL_ESPIPE,                      /**< Illegal seek */
        MMAL_ECORRUPT,                    /**< Data is corrupt \attention FIXME: not POSIX */
        MMAL_ENOTREADY,                   /**< Component is not ready \attention FIXME: not POSIX */
        MMAL_ECONFIG,                     /**< Component is not configured \attention FIXME: not POSIX */
        MMAL_EISCONN,                     /**< Port is already connected */
        MMAL_ENOTCONN,                    /**< Port is disconnected */
        MMAL_EAGAIN,                      /**< Resource temporarily unavailable. Try again later*/
        MMAL_EFAULT,                      /**< Bad address */
        /* Do not add new codes here unless they match something from POSIX */
        MMAL_STATUS_MAX = 0x7FFFFFFF      /**< Force to 32 bit */
    }
}
