using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
	/// Used in GL.Disable, GL.Enable and 1 other function
	/// </summary>
	public enum EnableCap
    {
        /// <summary>
        /// Original was GL_POINT_SMOOTH = 0x0B10
        /// </summary>
        PointSmooth = 2832,
        /// <summary>
        /// Original was GL_LINE_SMOOTH = 0x0B20
        /// </summary>
        LineSmooth = 2848,
        /// <summary>
        /// Original was GL_LINE_STIPPLE = 0x0B24
        /// </summary>
        LineStipple = 2852,
        /// <summary>
        /// Original was GL_POLYGON_SMOOTH = 0x0B41
        /// </summary>
        PolygonSmooth = 2881,
        /// <summary>
        /// Original was GL_POLYGON_STIPPLE = 0x0B42
        /// </summary>
        PolygonStipple,
        /// <summary>
        /// Original was GL_CULL_FACE = 0x0B44
        /// </summary>
        CullFace = 2884,
        /// <summary>
        /// Original was GL_LIGHTING = 0x0B50
        /// </summary>
        Lighting = 2896,
        /// <summary>
        /// Original was GL_COLOR_MATERIAL = 0x0B57
        /// </summary>
        ColorMaterial = 2903,
        /// <summary>
        /// Original was GL_FOG = 0x0B60
        /// </summary>
        Fog = 2912,
        /// <summary>
        /// Original was GL_DEPTH_TEST = 0x0B71
        /// </summary>
        DepthTest = 2929,
        /// <summary>
        /// Original was GL_STENCIL_TEST = 0x0B90
        /// </summary>
        StencilTest = 2960,
        /// <summary>
        /// Original was GL_NORMALIZE = 0x0BA1
        /// </summary>
        Normalize = 2977,
        /// <summary>
        /// Original was GL_ALPHA_TEST = 0x0BC0
        /// </summary>
        AlphaTest = 3008,
        /// <summary>
        /// Original was GL_Dither = 0X0bd0
        /// </summary>
        Dither = 3024,
        /// <summary>
        /// Original was GL_Blend = 0X0be2
        /// </summary>
        Blend = 3042,
        /// <summary>
        /// Original was GL_INDEX_LOGIC_OP = 0x0BF1
        /// </summary>
        IndexLogicOp = 3057,
        /// <summary>
        /// Original was GL_COLOR_LOGIC_OP = 0x0BF2
        /// </summary>
        ColorLogicOp,
        /// <summary>
        /// Original was GL_SCISSOR_TEST = 0x0C11
        /// </summary>
        ScissorTest = 3089,
        /// <summary>
        /// Original was GL_TEXTURE_GEN_S = 0x0C60
        /// </summary>
        TextureGenS = 3168,
        /// <summary>
        /// Original was GL_TEXTURE_GEN_T = 0x0C61
        /// </summary>
        TextureGenT,
        /// <summary>
        /// Original was GL_TEXTURE_GEN_R = 0x0C62
        /// </summary>
        TextureGenR,
        /// <summary>
        /// Original was GL_TEXTURE_GEN_Q = 0x0C63
        /// </summary>
        TextureGenQ,
        /// <summary>
        /// Original was GL_AUTO_NORMAL = 0x0D80
        /// </summary>
        AutoNormal = 3456,
        /// <summary>
        /// Original was GL_MAP1_COLOR_4 = 0x0D90
        /// </summary>
        Map1Color4 = 3472,
        /// <summary>
        /// Original was GL_MAP1_INDEX = 0x0D91
        /// </summary>
        Map1Index,
        /// <summary>
        /// Original was GL_MAP1_NORMAL = 0x0D92
        /// </summary>
        Map1Normal,
        /// <summary>
        /// Original was GL_MAP1_TEXTURE_COORD_1 = 0x0D93
        /// </summary>
        Map1TextureCoord1,
        /// <summary>
        /// Original was GL_MAP1_TEXTURE_COORD_2 = 0x0D94
        /// </summary>
        Map1TextureCoord2,
        /// <summary>
        /// Original was GL_MAP1_TEXTURE_COORD_3 = 0x0D95
        /// </summary>
        Map1TextureCoord3,
        /// <summary>
        /// Original was GL_MAP1_TEXTURE_COORD_4 = 0x0D96
        /// </summary>
        Map1TextureCoord4,
        /// <summary>
        /// Original was GL_MAP1_VERTEX_3 = 0x0D97
        /// </summary>
        Map1Vertex3,
        /// <summary>
        /// Original was GL_MAP1_VERTEX_4 = 0x0D98
        /// </summary>
        Map1Vertex4,
        /// <summary>
        /// Original was GL_MAP2_COLOR_4 = 0x0DB0
        /// </summary>
        Map2Color4 = 3504,
        /// <summary>
        /// Original was GL_MAP2_INDEX = 0x0DB1
        /// </summary>
        Map2Index,
        /// <summary>
        /// Original was GL_MAP2_NORMAL = 0x0DB2
        /// </summary>
        Map2Normal,
        /// <summary>
        /// Original was GL_MAP2_TEXTURE_COORD_1 = 0x0DB3
        /// </summary>
        Map2TextureCoord1,
        /// <summary>
        /// Original was GL_MAP2_TEXTURE_COORD_2 = 0x0DB4
        /// </summary>
        Map2TextureCoord2,
        /// <summary>
        /// Original was GL_MAP2_TEXTURE_COORD_3 = 0x0DB5
        /// </summary>
        Map2TextureCoord3,
        /// <summary>
        /// Original was GL_MAP2_TEXTURE_COORD_4 = 0x0DB6
        /// </summary>
        Map2TextureCoord4,
        /// <summary>
        /// Original was GL_MAP2_VERTEX_3 = 0x0DB7
        /// </summary>
        Map2Vertex3,
        /// <summary>
        /// Original was GL_MAP2_VERTEX_4 = 0x0DB8
        /// </summary>
        Map2Vertex4,
        /// <summary>
        /// Original was GL_TEXTURE_1D = 0x0DE0
        /// </summary>
        Texture1D = 3552,
        /// <summary>
        /// Original was GL_TEXTURE_2D = 0x0DE1
        /// </summary>
        Texture2D,
        /// <summary>
        /// Original was GL_POLYGON_OFFSET_POINT = 0x2A01
        /// </summary>
        PolygonOffsetPoint = 10753,
        /// <summary>
        /// Original was GL_POLYGON_OFFSET_LINE = 0x2A02
        /// </summary>
        PolygonOffsetLine,
        /// <summary>
        /// Original was GL_CLIP_PLANE0 = 0x3000
        /// </summary>
        ClipPlane0 = 12288,
        /// <summary>
        /// Original was GL_CLIP_PLANE1 = 0x3001
        /// </summary>
        ClipPlane1,
        /// <summary>
        /// Original was GL_CLIP_PLANE2 = 0x3002
        /// </summary>
        ClipPlane2,
        /// <summary>
        /// Original was GL_CLIP_PLANE3 = 0x3003
        /// </summary>
        ClipPlane3,
        /// <summary>
        /// Original was GL_CLIP_PLANE4 = 0x3004
        /// </summary>
        ClipPlane4,
        /// <summary>
        /// Original was GL_CLIP_PLANE5 = 0x3005
        /// </summary>
        ClipPlane5,
        /// <summary>
        /// Original was GL_LIGHT0 = 0x4000
        /// </summary>
        Light0 = 16384,
        /// <summary>
        /// Original was GL_LIGHT1 = 0x4001
        /// </summary>
        Light1,
        /// <summary>
        /// Original was GL_LIGHT2 = 0x4002
        /// </summary>
        Light2,
        /// <summary>
        /// Original was GL_LIGHT3 = 0x4003
        /// </summary>
        Light3,
        /// <summary>
        /// Original was GL_LIGHT4 = 0x4004
        /// </summary>
        Light4,
        /// <summary>
        /// Original was GL_LIGHT5 = 0x4005
        /// </summary>
        Light5,
        /// <summary>
        /// Original was GL_LIGHT6 = 0x4006
        /// </summary>
        Light6,
        /// <summary>
        /// Original was GL_LIGHT7 = 0x4007
        /// </summary>
        Light7,
        /// <summary>
        /// Original was GL_CONVOLUTION_1D_EXT = 0x8010
        /// </summary>
        Convolution1DExt = 32784,
        /// <summary>
        /// Original was GL_CONVOLUTION_2D_EXT = 0x8011
        /// </summary>
        Convolution2DExt,
        /// <summary>
        /// Original was GL_SEPARABLE_2D_EXT = 0x8012
        /// </summary>
        Separable2DExt,
        /// <summary>
        /// Original was GL_HISTOGRAM_EXT = 0x8024
        /// </summary>
        HistogramExt = 32804,
        /// <summary>
        /// Original was GL_MINMAX_EXT = 0x802E
        /// </summary>
        MinmaxExt = 32814,
        /// <summary>
        /// Original was GL_POLYGON_OFFSET_FILL = 0x8037
        /// </summary>
        PolygonOffsetFill = 32823,
        /// <summary>
        /// Original was GL_RESCALE_NORMAL_EXT = 0x803A
        /// </summary>
        RescaleNormalExt = 32826,
        /// <summary>
        /// Original was GL_TEXTURE_3D_EXT = 0x806F
        /// </summary>
        Texture3DExt = 32879,
        /// <summary>
        /// Original was GL_VERTEX_ARRAY = 0x8074
        /// </summary>
        VertexArray = 32884,
        /// <summary>
        /// Original was GL_NORMAL_ARRAY = 0x8075
        /// </summary>
        NormalArray,
        /// <summary>
        /// Original was GL_COLOR_ARRAY = 0x8076
        /// </summary>
        ColorArray,
        /// <summary>
        /// Original was GL_INDEX_ARRAY = 0x8077
        /// </summary>
        IndexArray,
        /// <summary>
        /// Original was GL_TEXTURE_COORD_ARRAY = 0x8078
        /// </summary>
        TextureCoordArray,
        /// <summary>
        /// Original was GL_EDGE_FLAG_ARRAY = 0x8079
        /// </summary>
        EdgeFlagArray,
        /// <summary>
        /// Original was GL_INTERLACE_SGIX = 0x8094
        /// </summary>
        InterlaceSgix = 32916,
        /// <summary>
        /// Original was GL_MULTISAMPLE_SGIS = 0x809D
        /// </summary>
        MultisampleSgis = 32925,
        /// <summary>
        /// Original was GL_SAMPLE_ALPHA_TO_MASK_SGIS = 0x809E
        /// </summary>
        SampleAlphaToMaskSgis,
        /// <summary>
        /// Original was GL_SampleAlphaToCoverage = 0X809e
        /// </summary>
        SampleAlphaToCoverage = 32926,
        /// <summary>
        /// Original was GL_SAMPLE_ALPHA_TO_ONE_SGIS = 0x809F
        /// </summary>
        SampleAlphaToOneSgis,
        /// <summary>
        /// Original was GL_SAMPLE_MASK_SGIS = 0x80A0
        /// </summary>
        SampleMaskSgis,
        /// <summary>
        /// Original was GL_SampleCoverage = 0X80a0
        /// </summary>
        SampleCoverage = 32928,
        /// <summary>
        /// Original was GL_TEXTURE_COLOR_TABLE_SGI = 0x80BC
        /// </summary>
        TextureColorTableSgi = 32956,
        /// <summary>
        /// Original was GL_COLOR_TABLE_SGI = 0x80D0
        /// </summary>
        ColorTableSgi = 32976,
        /// <summary>
        /// Original was GL_POST_CONVOLUTION_COLOR_TABLE_SGI = 0x80D1
        /// </summary>
        PostConvolutionColorTableSgi,
        /// <summary>
        /// Original was GL_POST_COLOR_MATRIX_COLOR_TABLE_SGI = 0x80D2
        /// </summary>
        PostColorMatrixColorTableSgi,
        /// <summary>
        /// Original was GL_TEXTURE_4D_SGIS = 0x8134
        /// </summary>
        Texture4DSgis = 33076,
        /// <summary>
        /// Original was GL_PIXEL_TEX_GEN_SGIX = 0x8139
        /// </summary>
        PixelTexGenSgix = 33081,
        /// <summary>
        /// Original was GL_SPRITE_SGIX = 0x8148
        /// </summary>
        SpriteSgix = 33096,
        /// <summary>
        /// Original was GL_REFERENCE_PLANE_SGIX = 0x817D
        /// </summary>
        ReferencePlaneSgix = 33149,
        /// <summary>
        /// Original was GL_IR_INSTRUMENT1_SGIX = 0x817F
        /// </summary>
        IrInstrument1Sgix = 33151,
        /// <summary>
        /// Original was GL_CALLIGRAPHIC_FRAGMENT_SGIX = 0x8183
        /// </summary>
        CalligraphicFragmentSgix = 33155,
        /// <summary>
        /// Original was GL_FRAMEZOOM_SGIX = 0x818B
        /// </summary>
        FramezoomSgix = 33163,
        /// <summary>
        /// Original was GL_FOG_OFFSET_SGIX = 0x8198
        /// </summary>
        FogOffsetSgix = 33176,
        /// <summary>
        /// Original was GL_SHARED_TEXTURE_PALETTE_EXT = 0x81FB
        /// </summary>
        SharedTexturePaletteExt = 33275,
        /// <summary>
        /// Original was GL_ASYNC_HISTOGRAM_SGIX = 0x832C
        /// </summary>
        AsyncHistogramSgix = 33580,
        /// <summary>
        /// Original was GL_PIXEL_TEXTURE_SGIS = 0x8353
        /// </summary>
        PixelTextureSgis = 33619,
        /// <summary>
        /// Original was GL_ASYNC_TEX_IMAGE_SGIX = 0x835C
        /// </summary>
        AsyncTexImageSgix = 33628,
        /// <summary>
        /// Original was GL_ASYNC_DRAW_PIXELS_SGIX = 0x835D
        /// </summary>
        AsyncDrawPixelsSgix,
        /// <summary>
        /// Original was GL_ASYNC_READ_PIXELS_SGIX = 0x835E
        /// </summary>
        AsyncReadPixelsSgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHTING_SGIX = 0x8400
        /// </summary>
        FragmentLightingSgix = 33792,
        /// <summary>
        /// Original was GL_FRAGMENT_COLOR_MATERIAL_SGIX = 0x8401
        /// </summary>
        FragmentColorMaterialSgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT0_SGIX = 0x840C
        /// </summary>
        FragmentLight0Sgix = 33804,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT1_SGIX = 0x840D
        /// </summary>
        FragmentLight1Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT2_SGIX = 0x840E
        /// </summary>
        FragmentLight2Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT3_SGIX = 0x840F
        /// </summary>
        FragmentLight3Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT4_SGIX = 0x8410
        /// </summary>
        FragmentLight4Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT5_SGIX = 0x8411
        /// </summary>
        FragmentLight5Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT6_SGIX = 0x8412
        /// </summary>
        FragmentLight6Sgix,
        /// <summary>
        /// Original was GL_FRAGMENT_LIGHT7_SGIX = 0x8413
        /// </summary>
        FragmentLight7Sgix
    }
}
