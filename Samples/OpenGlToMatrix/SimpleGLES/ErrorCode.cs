using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
	/// Not used directly.
	/// </summary>
	public enum ErrorCode
    {
        /// <summary>
        /// Original was GL_NO_ERROR = 0
        /// </summary>
        NoError,
        /// <summary>
        /// Original was GL_INVALID_ENUM = 0x0500
        /// </summary>
        InvalidEnum = 1280,
        /// <summary>
        /// Original was GL_INVALID_VALUE = 0x0501
        /// </summary>
        InvalidValue,
        /// <summary>
        /// Original was GL_INVALID_OPERATION = 0x0502
        /// </summary>
        InvalidOperation,
        /// <summary>
        /// Original was GL_STACK_OVERFLOW = 0x0503
        /// </summary>
        StackOverflow,
        /// <summary>
        /// Original was GL_STACK_UNDERFLOW = 0x0504
        /// </summary>
        StackUnderflow,
        /// <summary>
        /// Original was GL_OUT_OF_MEMORY = 0x0505
        /// </summary>
        OutOfMemory,
        /// <summary>
        /// Original was GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506
        /// </summary>
        InvalidFramebufferOperation,
        /// <summary>
        /// Original was GL_INVALID_FRAMEBUFFER_OPERATION_EXT = 0x0506
        /// </summary>
        InvalidFramebufferOperationExt = 1286,
        /// <summary>
        /// Original was GL_INVALID_FRAMEBUFFER_OPERATION_OES = 0x0506
        /// </summary>
        InvalidFramebufferOperationOes = 1286,
        /// <summary>
        /// Original was GL_CONTEXT_LOST = 0x0507
        /// </summary>
        ContextLost,
        /// <summary>
        /// Original was GL_TABLE_TOO_LARGE = 0x8031
        /// </summary>
        TableTooLarge = 32817,
        /// <summary>
        /// Original was GL_TABLE_TOO_LARGE_EXT = 0x8031
        /// </summary>
        TableTooLargeExt = 32817,
        /// <summary>
        /// Original was GL_TEXTURE_TOO_LARGE_EXT = 0x8065
        /// </summary>
        TextureTooLargeExt = 32869
    }
}
