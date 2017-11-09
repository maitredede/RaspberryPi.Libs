using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
	/// Used in GL.TexParameter, GL.TexParameterI and 5 other functions
	/// </summary>
	public enum TextureParameterName
    {
        /// <summary>
        /// Original was GL_TEXTURE_BORDER_COLOR = 0x1004
        /// </summary>
        TextureBorderColor = 4100,
        /// <summary>
        /// Original was GL_TEXTURE_MAG_FILTER = 0x2800
        /// </summary>
        TextureMagFilter = 10240,
        /// <summary>
        /// Original was GL_TEXTURE_MIN_FILTER = 0x2801
        /// </summary>
        TextureMinFilter,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_S = 0x2802
        /// </summary>
        TextureWrapS,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_T = 0x2803
        /// </summary>
        TextureWrapT,
        /// <summary>
        /// Original was GL_TEXTURE_PRIORITY = 0x8066
        /// </summary>
        TexturePriority = 32870,
        /// <summary>
        /// Original was GL_TEXTURE_PRIORITY_EXT = 0x8066
        /// </summary>
        TexturePriorityExt = 32870,
        /// <summary>
        /// Original was GL_TEXTURE_DEPTH = 0x8071
        /// </summary>
        TextureDepth = 32881,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_R = 0x8072
        /// </summary>
        TextureWrapR,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_R_EXT = 0x8072
        /// </summary>
        TextureWrapRExt = 32882,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_R_OES = 0x8072
        /// </summary>
        TextureWrapROes = 32882,
        /// <summary>
        /// Original was GL_DETAIL_TEXTURE_LEVEL_SGIS = 0x809A
        /// </summary>
        DetailTextureLevelSgis = 32922,
        /// <summary>
        /// Original was GL_DETAIL_TEXTURE_MODE_SGIS = 0x809B
        /// </summary>
        DetailTextureModeSgis,
        /// <summary>
        /// Original was GL_SHADOW_AMBIENT_SGIX = 0x80BF
        /// </summary>
        ShadowAmbientSgix = 32959,
        /// <summary>
        /// Original was GL_TEXTURE_COMPARE_FAIL_VALUE = 0x80BF
        /// </summary>
        TextureCompareFailValue = 32959,
        /// <summary>
        /// Original was GL_DUAL_TEXTURE_SELECT_SGIS = 0x8124
        /// </summary>
        DualTextureSelectSgis = 33060,
        /// <summary>
        /// Original was GL_QUAD_TEXTURE_SELECT_SGIS = 0x8125
        /// </summary>
        QuadTextureSelectSgis,
        /// <summary>
        /// Original was GL_CLAMP_TO_BORDER = 0x812D
        /// </summary>
        ClampToBorder = 33069,
        /// <summary>
        /// Original was GL_CLAMP_TO_EDGE = 0x812F
        /// </summary>
        ClampToEdge = 33071,
        /// <summary>
        /// Original was GL_TEXTURE_WRAP_Q_SGIS = 0x8137
        /// </summary>
        TextureWrapQSgis = 33079,
        /// <summary>
        /// Original was GL_TEXTURE_MIN_LOD = 0x813A
        /// </summary>
        TextureMinLod = 33082,
        /// <summary>
        /// Original was GL_TEXTURE_MAX_LOD = 0x813B
        /// </summary>
        TextureMaxLod,
        /// <summary>
        /// Original was GL_TEXTURE_BASE_LEVEL = 0x813C
        /// </summary>
        TextureBaseLevel,
        /// <summary>
        /// Original was GL_TEXTURE_MAX_LEVEL = 0x813D
        /// </summary>
        TextureMaxLevel,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_CENTER_SGIX = 0x8171
        /// </summary>
        TextureClipmapCenterSgix = 33137,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_FRAME_SGIX = 0x8172
        /// </summary>
        TextureClipmapFrameSgix,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_OFFSET_SGIX = 0x8173
        /// </summary>
        TextureClipmapOffsetSgix,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_VIRTUAL_DEPTH_SGIX = 0x8174
        /// </summary>
        TextureClipmapVirtualDepthSgix,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_LOD_OFFSET_SGIX = 0x8175
        /// </summary>
        TextureClipmapLodOffsetSgix,
        /// <summary>
        /// Original was GL_TEXTURE_CLIPMAP_DEPTH_SGIX = 0x8176
        /// </summary>
        TextureClipmapDepthSgix,
        /// <summary>
        /// Original was GL_POST_TEXTURE_FILTER_BIAS_SGIX = 0x8179
        /// </summary>
        PostTextureFilterBiasSgix = 33145,
        /// <summary>
        /// Original was GL_POST_TEXTURE_FILTER_SCALE_SGIX = 0x817A
        /// </summary>
        PostTextureFilterScaleSgix,
        /// <summary>
        /// Original was GL_TEXTURE_LOD_BIAS_S_SGIX = 0x818E
        /// </summary>
        TextureLodBiasSSgix = 33166,
        /// <summary>
        /// Original was GL_TEXTURE_LOD_BIAS_T_SGIX = 0x818F
        /// </summary>
        TextureLodBiasTSgix,
        /// <summary>
        /// Original was GL_TEXTURE_LOD_BIAS_R_SGIX = 0x8190
        /// </summary>
        TextureLodBiasRSgix,
        /// <summary>
        /// Original was GL_GENERATE_MIPMAP = 0x8191
        /// </summary>
        GenerateMipmap,
        /// <summary>
        /// Original was GL_GENERATE_MIPMAP_SGIS = 0x8191
        /// </summary>
        GenerateMipmapSgis = 33169,
        /// <summary>
        /// Original was GL_TEXTURE_COMPARE_SGIX = 0x819A
        /// </summary>
        TextureCompareSgix = 33178,
        /// <summary>
        /// Original was GL_TEXTURE_MAX_CLAMP_S_SGIX = 0x8369
        /// </summary>
        TextureMaxClampSSgix = 33641,
        /// <summary>
        /// Original was GL_TEXTURE_MAX_CLAMP_T_SGIX = 0x836A
        /// </summary>
        TextureMaxClampTSgix,
        /// <summary>
        /// Original was GL_TEXTURE_MAX_CLAMP_R_SGIX = 0x836B
        /// </summary>
        TextureMaxClampRSgix,
        /// <summary>
        /// Original was GL_TEXTURE_LOD_BIAS = 0x8501
        /// </summary>
        TextureLodBias = 34049,
        /// <summary>
        /// Original was GL_DEPTH_TEXTURE_MODE = 0x884B
        /// </summary>
        DepthTextureMode = 34891,
        /// <summary>
        /// Original was GL_TEXTURE_COMPARE_MODE = 0x884C
        /// </summary>
        TextureCompareMode,
        /// <summary>
        /// Original was GL_TEXTURE_COMPARE_FUNC = 0x884D
        /// </summary>
        TextureCompareFunc,
        /// <summary>
        /// Original was GL_TEXTURE_SWIZZLE_R = 0x8E42
        /// </summary>
        TextureSwizzleR = 36418,
        /// <summary>
        /// Original was GL_TEXTURE_SWIZZLE_G = 0x8E43
        /// </summary>
        TextureSwizzleG,
        /// <summary>
        /// Original was GL_TEXTURE_SWIZZLE_B = 0x8E44
        /// </summary>
        TextureSwizzleB,
        /// <summary>
        /// Original was GL_TEXTURE_SWIZZLE_A = 0x8E45
        /// </summary>
        TextureSwizzleA,
        /// <summary>
        /// Original was GL_TEXTURE_SWIZZLE_RGBA = 0x8E46
        /// </summary>
        TextureSwizzleRgba,
        /// <summary>
        /// Original was GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA
        /// </summary>
        DepthStencilTextureMode = 37098,
        /// <summary>
        /// Original was GL_TEXTURE_TILING_EXT = 0x9580
        /// </summary>
        TextureTilingExt = 38272
    }
}
