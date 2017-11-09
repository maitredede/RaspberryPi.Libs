using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
    /// Used in GL.Hint
    /// </summary>
    public enum HintTarget
    {
        /// <summary>
        /// Original was GL_PERSPECTIVE_CORRECTION_HINT = 0x0C50
        /// </summary>
        PerspectiveCorrectionHint = 3152,
        /// <summary>
        /// Original was GL_POINT_SMOOTH_HINT = 0x0C51
        /// </summary>
        PointSmoothHint,
        /// <summary>
        /// Original was GL_LINE_SMOOTH_HINT = 0x0C52
        /// </summary>
        LineSmoothHint,
        /// <summary>
        /// Original was GL_POLYGON_SMOOTH_HINT = 0x0C53
        /// </summary>
        PolygonSmoothHint,
        /// <summary>
        /// Original was GL_FOG_HINT = 0x0C54
        /// </summary>
        FogHint,
        /// <summary>
        /// Original was GL_PREFER_DOUBLEBUFFER_HINT_PGI = 0x1A1F8
        /// </summary>
        PreferDoublebufferHintPgi = 107000,
        /// <summary>
        /// Original was GL_CONSERVE_MEMORY_HINT_PGI = 0x1A1FD
        /// </summary>
        ConserveMemoryHintPgi = 107005,
        /// <summary>
        /// Original was GL_RECLAIM_MEMORY_HINT_PGI = 0x1A1FE
        /// </summary>
        ReclaimMemoryHintPgi,
        /// <summary>
        /// Original was GL_NATIVE_GRAPHICS_BEGIN_HINT_PGI = 0x1A203
        /// </summary>
        NativeGraphicsBeginHintPgi = 107011,
        /// <summary>
        /// Original was GL_NATIVE_GRAPHICS_END_HINT_PGI = 0x1A204
        /// </summary>
        NativeGraphicsEndHintPgi,
        /// <summary>
        /// Original was GL_ALWAYS_FAST_HINT_PGI = 0x1A20C
        /// </summary>
        AlwaysFastHintPgi = 107020,
        /// <summary>
        /// Original was GL_ALWAYS_SOFT_HINT_PGI = 0x1A20D
        /// </summary>
        AlwaysSoftHintPgi,
        /// <summary>
        /// Original was GL_ALLOW_DRAW_OBJ_HINT_PGI = 0x1A20E
        /// </summary>
        AllowDrawObjHintPgi,
        /// <summary>
        /// Original was GL_ALLOW_DRAW_WIN_HINT_PGI = 0x1A20F
        /// </summary>
        AllowDrawWinHintPgi,
        /// <summary>
        /// Original was GL_ALLOW_DRAW_FRG_HINT_PGI = 0x1A210
        /// </summary>
        AllowDrawFrgHintPgi,
        /// <summary>
        /// Original was GL_ALLOW_DRAW_MEM_HINT_PGI = 0x1A211
        /// </summary>
        AllowDrawMemHintPgi,
        /// <summary>
        /// Original was GL_STRICT_DEPTHFUNC_HINT_PGI = 0x1A216
        /// </summary>
        StrictDepthfuncHintPgi = 107030,
        /// <summary>
        /// Original was GL_STRICT_LIGHTING_HINT_PGI = 0x1A217
        /// </summary>
        StrictLightingHintPgi,
        /// <summary>
        /// Original was GL_STRICT_SCISSOR_HINT_PGI = 0x1A218
        /// </summary>
        StrictScissorHintPgi,
        /// <summary>
        /// Original was GL_FULL_STIPPLE_HINT_PGI = 0x1A219
        /// </summary>
        FullStippleHintPgi,
        /// <summary>
        /// Original was GL_CLIP_NEAR_HINT_PGI = 0x1A220
        /// </summary>
        ClipNearHintPgi = 107040,
        /// <summary>
        /// Original was GL_CLIP_FAR_HINT_PGI = 0x1A221
        /// </summary>
        ClipFarHintPgi,
        /// <summary>
        /// Original was GL_WIDE_LINE_HINT_PGI = 0x1A222
        /// </summary>
        WideLineHintPgi,
        /// <summary>
        /// Original was GL_BACK_NORMALS_HINT_PGI = 0x1A223
        /// </summary>
        BackNormalsHintPgi,
        /// <summary>
        /// Original was GL_VERTEX_DATA_HINT_PGI = 0x1A22A
        /// </summary>
        VertexDataHintPgi = 107050,
        /// <summary>
        /// Original was GL_VERTEX_CONSISTENT_HINT_PGI = 0x1A22B
        /// </summary>
        VertexConsistentHintPgi,
        /// <summary>
        /// Original was GL_MATERIAL_SIDE_HINT_PGI = 0x1A22C
        /// </summary>
        MaterialSideHintPgi,
        /// <summary>
        /// Original was GL_MAX_VERTEX_HINT_PGI = 0x1A22D
        /// </summary>
        MaxVertexHintPgi,
        /// <summary>
        /// Original was GL_PACK_CMYK_HINT_EXT = 0x800E
        /// </summary>
        PackCmykHintExt = 32782,
        /// <summary>
        /// Original was GL_UNPACK_CMYK_HINT_EXT = 0x800F
        /// </summary>
        UnpackCmykHintExt,
        /// <summary>
        /// Original was GL_PHONG_HINT_WIN = 0x80EB
        /// </summary>
        PhongHintWin = 33003,
        /// <summary>
        /// Original was GL_CLIP_VOLUME_CLIPPING_HINT_EXT = 0x80F0
        /// </summary>
        ClipVolumeClippingHintExt = 33008,
        /// <summary>
        /// Original was GL_TEXTURE_MULTI_BUFFER_HINT_SGIX = 0x812E
        /// </summary>
        TextureMultiBufferHintSgix = 33070,
        /// <summary>
        /// Original was GL_GENERATE_MIPMAP_HINT = 0x8192
        /// </summary>
        GenerateMipmapHint = 33170,
        /// <summary>
        /// Original was GL_GENERATE_MIPMAP_HINT_SGIS = 0x8192
        /// </summary>
        GenerateMipmapHintSgis = 33170,
        /// <summary>
        /// Original was GL_PROGRAM_BINARY_RETRIEVABLE_HINT = 0x8257
        /// </summary>
        ProgramBinaryRetrievableHint = 33367,
        /// <summary>
        /// Original was GL_CONVOLUTION_HINT_SGIX = 0x8316
        /// </summary>
        ConvolutionHintSgix = 33558,
        /// <summary>
        /// Original was GL_SCALEBIAS_HINT_SGIX = 0x8322
        /// </summary>
        ScalebiasHintSgix = 33570,
        /// <summary>
        /// Original was GL_LINE_QUALITY_HINT_SGIX = 0x835B
        /// </summary>
        LineQualityHintSgix = 33627,
        /// <summary>
        /// Original was GL_VERTEX_PRECLIP_SGIX = 0x83EE
        /// </summary>
        VertexPreclipSgix = 33774,
        /// <summary>
        /// Original was GL_VERTEX_PRECLIP_HINT_SGIX = 0x83EF
        /// </summary>
        VertexPreclipHintSgix,
        /// <summary>
        /// Original was GL_TEXTURE_COMPRESSION_HINT = 0x84EF
        /// </summary>
        TextureCompressionHint = 34031,
        /// <summary>
        /// Original was GL_TEXTURE_COMPRESSION_HINT_ARB = 0x84EF
        /// </summary>
        TextureCompressionHintArb = 34031,
        /// <summary>
        /// Original was GL_VERTEX_ARRAY_STORAGE_HINT_APPLE = 0x851F
        /// </summary>
        VertexArrayStorageHintApple = 34079,
        /// <summary>
        /// Original was GL_MULTISAMPLE_FILTER_HINT_NV = 0x8534
        /// </summary>
        MultisampleFilterHintNv = 34100,
        /// <summary>
        /// Original was GL_TRANSFORM_HINT_APPLE = 0x85B1
        /// </summary>
        TransformHintApple = 34225,
        /// <summary>
        /// Original was GL_TEXTURE_STORAGE_HINT_APPLE = 0x85BC
        /// </summary>
        TextureStorageHintApple = 34236,
        /// <summary>
        /// Original was GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B
        /// </summary>
        FragmentShaderDerivativeHint = 35723,
        /// <summary>
        /// Original was GL_FRAGMENT_SHADER_DERIVATIVE_HINT_ARB = 0x8B8B
        /// </summary>
        FragmentShaderDerivativeHintArb = 35723,
        /// <summary>
        /// Original was GL_FRAGMENT_SHADER_DERIVATIVE_HINT_OES = 0x8B8B
        /// </summary>
        FragmentShaderDerivativeHintOes = 35723,
        /// <summary>
        /// Original was GL_BINNING_CONTROL_HINT_QCOM = 0x8FB0
        /// </summary>
        BinningControlHintQcom = 36784
    }
}
