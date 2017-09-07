using RaspberryPi.MMAL.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public enum MMALParameterId
    {
        #region Common
        MMAL_PARAMETER_UNUSED                  /**< Never a valid parameter ID */
         = NativeMethods.MMAL_PARAMETER_GROUP_COMMON,
        MMAL_PARAMETER_SUPPORTED_ENCODINGS,    /**< Takes a MMAL_PARAMETER_ENCODING_T */
        MMAL_PARAMETER_URI,                    /**< Takes a MMAL_PARAMETER_URI_T */
        MMAL_PARAMETER_CHANGE_EVENT_REQUEST,   /**< Takes a MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T */
        MMAL_PARAMETER_ZERO_COPY,              /**< Takes a MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_BUFFER_REQUIREMENTS,    /**< Takes a MMAL_PARAMETER_BUFFER_REQUIREMENTS_T */
        MMAL_PARAMETER_STATISTICS,             /**< Takes a MMAL_PARAMETER_STATISTICS_T */
        MMAL_PARAMETER_CORE_STATISTICS,        /**< Takes a MMAL_PARAMETER_CORE_STATISTICS_T */
        MMAL_PARAMETER_MEM_USAGE,              /**< Takes a MMAL_PARAMETER_MEM_USAGE_T */
        MMAL_PARAMETER_BUFFER_FLAG_FILTER,     /**< Takes a MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_SEEK,                   /**< Takes a MMAL_PARAMETER_SEEK_T */
        MMAL_PARAMETER_POWERMON_ENABLE,        /**< Takes a MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_LOGGING,                /**< Takes a MMAL_PARAMETER_LOGGING_T */
        MMAL_PARAMETER_SYSTEM_TIME,            /**< Takes a MMAL_PARAMETER_UINT64_T */
        MMAL_PARAMETER_NO_IMAGE_PADDING,       /**< Takes a MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_LOCKSTEP_ENABLE,         /**< Takes a MMAL_PARAMETER_BOOLEAN_T */
        #endregion

        #region Camera
        /* 0 */
        MMAL_PARAMETER_THUMBNAIL_CONFIGURATION    /**< Takes a @ref MMAL_PARAMETER_THUMBNAIL_CONFIG_T */
              = NativeMethods.MMAL_PARAMETER_GROUP_CAMERA,
        MMAL_PARAMETER_CAPTURE_QUALITY,           /**< Unused? */
        MMAL_PARAMETER_ROTATION,                  /**< Takes a @ref MMAL_PARAMETER_INT32_T */
        MMAL_PARAMETER_EXIF_DISABLE,              /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_EXIF,                      /**< Takes a @ref MMAL_PARAMETER_EXIF_T */
        MMAL_PARAMETER_AWB_MODE,                  /**< Takes a @ref MMAL_PARAM_AWBMODE_T */
        MMAL_PARAMETER_IMAGE_EFFECT,              /**< Takes a @ref MMAL_PARAMETER_IMAGEFX_T */
        MMAL_PARAMETER_COLOUR_EFFECT,             /**< Takes a @ref MMAL_PARAMETER_COLOURFX_T */
        MMAL_PARAMETER_FLICKER_AVOID,             /**< Takes a @ref MMAL_PARAMETER_FLICKERAVOID_T */
        MMAL_PARAMETER_FLASH,                     /**< Takes a @ref MMAL_PARAMETER_FLASH_T */
        MMAL_PARAMETER_REDEYE,                    /**< Takes a @ref MMAL_PARAMETER_REDEYE_T */
        MMAL_PARAMETER_FOCUS,                     /**< Takes a @ref MMAL_PARAMETER_FOCUS_T */
        MMAL_PARAMETER_FOCAL_LENGTHS,             /**< Unused? */
        MMAL_PARAMETER_EXPOSURE_COMP,             /**< Takes a @ref MMAL_PARAMETER_INT32_T or MMAL_PARAMETER_RATIONAL_T */
        MMAL_PARAMETER_ZOOM,                      /**< Takes a @ref MMAL_PARAMETER_SCALEFACTOR_T */
        MMAL_PARAMETER_MIRROR,                    /**< Takes a @ref MMAL_PARAMETER_MIRROR_T */

        /* 0x10 */
        MMAL_PARAMETER_CAMERA_NUM,                /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_CAPTURE,                   /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_EXPOSURE_MODE,             /**< Takes a @ref MMAL_PARAMETER_EXPOSUREMODE_T */
        MMAL_PARAMETER_EXP_METERING_MODE,         /**< Takes a @ref MMAL_PARAMETER_EXPOSUREMETERINGMODE_T */
        MMAL_PARAMETER_FOCUS_STATUS,              /**< Takes a @ref MMAL_PARAMETER_FOCUS_STATUS_T */
        MMAL_PARAMETER_CAMERA_CONFIG,             /**< Takes a @ref MMAL_PARAMETER_CAMERA_CONFIG_T */
        MMAL_PARAMETER_CAPTURE_STATUS,            /**< Takes a @ref MMAL_PARAMETER_CAPTURE_STATUS_T */
        MMAL_PARAMETER_FACE_TRACK,                /**< Takes a @ref MMAL_PARAMETER_FACE_TRACK_T */
        MMAL_PARAMETER_DRAW_BOX_FACES_AND_FOCUS,  /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_JPEG_Q_FACTOR,             /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_FRAME_RATE,                /**< Takes a @ref MMAL_PARAMETER_FRAME_RATE_T */
        MMAL_PARAMETER_USE_STC,                   /**< Takes a @ref MMAL_PARAMETER_CAMERA_STC_MODE_T */
        MMAL_PARAMETER_CAMERA_INFO,               /**< Takes a @ref MMAL_PARAMETER_CAMERA_INFO_T */
        MMAL_PARAMETER_VIDEO_STABILISATION,       /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_FACE_TRACK_RESULTS,        /**< Takes a @ref MMAL_PARAMETER_FACE_TRACK_RESULTS_T */
        MMAL_PARAMETER_ENABLE_RAW_CAPTURE,        /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */

        /* 0x20 */
        MMAL_PARAMETER_DPF_FILE,                  /**< Takes a @ref MMAL_PARAMETER_URI_T */
        MMAL_PARAMETER_ENABLE_DPF_FILE,           /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_DPF_FAIL_IS_FATAL,         /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_CAPTURE_MODE,              /**< Takes a @ref MMAL_PARAMETER_CAPTUREMODE_T */
        MMAL_PARAMETER_FOCUS_REGIONS,             /**< Takes a @ref MMAL_PARAMETER_FOCUS_REGIONS_T */
        MMAL_PARAMETER_INPUT_CROP,                /**< Takes a @ref MMAL_PARAMETER_INPUT_CROP_T */
        MMAL_PARAMETER_SENSOR_INFORMATION,        /**< Takes a @ref MMAL_PARAMETER_SENSOR_INFORMATION_T */
        MMAL_PARAMETER_FLASH_SELECT,              /**< Takes a @ref MMAL_PARAMETER_FLASH_SELECT_T */
        MMAL_PARAMETER_FIELD_OF_VIEW,             /**< Takes a @ref MMAL_PARAMETER_FIELD_OF_VIEW_T */
        MMAL_PARAMETER_HIGH_DYNAMIC_RANGE,        /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_DYNAMIC_RANGE_COMPRESSION, /**< Takes a @ref MMAL_PARAMETER_DRC_T */
        MMAL_PARAMETER_ALGORITHM_CONTROL,         /**< Takes a @ref MMAL_PARAMETER_ALGORITHM_CONTROL_T */
        MMAL_PARAMETER_SHARPNESS,                 /**< Takes a @ref MMAL_PARAMETER_RATIONAL_T */
        MMAL_PARAMETER_CONTRAST,                  /**< Takes a @ref MMAL_PARAMETER_RATIONAL_T */
        MMAL_PARAMETER_BRIGHTNESS,                /**< Takes a @ref MMAL_PARAMETER_RATIONAL_T */
        MMAL_PARAMETER_SATURATION,                /**< Takes a @ref MMAL_PARAMETER_RATIONAL_T */

        /* 0x30 */
        MMAL_PARAMETER_ISO,                       /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_ANTISHAKE,                 /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_IMAGE_EFFECT_PARAMETERS,   /**< Takes a @ref MMAL_PARAMETER_IMAGEFX_PARAMETERS_T */
        MMAL_PARAMETER_CAMERA_BURST_CAPTURE,      /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_CAMERA_MIN_ISO,            /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_CAMERA_USE_CASE,           /**< Takes a @ref MMAL_PARAMETER_CAMERA_USE_CASE_T */
        MMAL_PARAMETER_CAPTURE_STATS_PASS,        /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_CAMERA_CUSTOM_SENSOR_CONFIG, /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_ENABLE_REGISTER_FILE,      /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_REGISTER_FAIL_IS_FATAL,    /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_CONFIGFILE_REGISTERS,      /**< Takes a @ref MMAL_PARAMETER_CONFIGFILE_T */
        MMAL_PARAMETER_CONFIGFILE_CHUNK_REGISTERS,/**< Takes a @ref MMAL_PARAMETER_CONFIGFILE_CHUNK_T */
        MMAL_PARAMETER_JPEG_ATTACH_LOG,           /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_ZERO_SHUTTER_LAG,          /**< Takes a @ref MMAL_PARAMETER_ZEROSHUTTERLAG_T */
        MMAL_PARAMETER_FPS_RANGE,                 /**< Takes a @ref MMAL_PARAMETER_FPS_RANGE_T */
        MMAL_PARAMETER_CAPTURE_EXPOSURE_COMP,     /**< Takes a @ref MMAL_PARAMETER_INT32_T */

        /* 0x40 */
        MMAL_PARAMETER_SW_SHARPEN_DISABLE,        /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_FLASH_REQUIRED,            /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_SW_SATURATION_DISABLE,     /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_SHUTTER_SPEED,             /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_CUSTOM_AWB_GAINS,          /**< Takes a @ref MMAL_PARAMETER_AWB_GAINS_T */
        MMAL_PARAMETER_CAMERA_SETTINGS,           /**< Takes a @ref MMAL_PARAMETER_CAMERA_SETTINGS_T */
        MMAL_PARAMETER_PRIVACY_INDICATOR,         /**< Takes a @ref MMAL_PARAMETER_PRIVACY_INDICATOR_T */
        MMAL_PARAMETER_VIDEO_DENOISE,             /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_STILLS_DENOISE,            /**< Takes a @ref MMAL_PARAMETER_BOOLEAN_T */
        MMAL_PARAMETER_ANNOTATE,                  /**< Takes a @ref MMAL_PARAMETER_CAMERA_ANNOTATE_T */
        MMAL_PARAMETER_STEREOSCOPIC_MODE,         /**< Takes a @ref MMAL_PARAMETER_STEREOSCOPIC_MODE_T */
        MMAL_PARAMETER_CAMERA_INTERFACE,          /**< Takes a @ref MMAL_PARAMETER_CAMERA_INTERFACE_T */
        MMAL_PARAMETER_CAMERA_CLOCKING_MODE,      /**< Takes a @ref MMAL_PARAMETER_CAMERA_CLOCKING_MODE_T */
        MMAL_PARAMETER_CAMERA_RX_CONFIG,          /**< Takes a @ref MMAL_PARAMETER_CAMERA_RX_CONFIG_T */
        MMAL_PARAMETER_CAMERA_RX_TIMING,          /**< Takes a @ref MMAL_PARAMETER_CAMERA_RX_TIMING_T */
        MMAL_PARAMETER_DPF_CONFIG,                /**< Takes a @ref MMAL_PARAMETER_UINT32_T */

        /* 0x50 */
        MMAL_PARAMETER_JPEG_RESTART_INTERVAL,     /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_CAMERA_ISP_BLOCK_OVERRIDE, /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_LENS_SHADING_OVERRIDE,     /**< Takes a @ref MMAL_PARAMETER_LENS_SHADING_T */
        MMAL_PARAMETER_BLACK_LEVEL,               /**< Takes a @ref MMAL_PARAMETER_UINT32_T */
        MMAL_PARAMETER_RESIZE_PARAMS,             /**< Takes a @ref MMAL_PARAMETER_RESIZE_T */
        MMAL_PARAMETER_CROP,                      /**< Takes a @ref MMAL_PARAMETER_CROP_T */
        MMAL_PARAMETER_OUTPUT_SHIFT,              /**< Takes a @ref MMAL_PARAMETER_INT32_T */
        MMAL_PARAMETER_CCM_SHIFT,                 /**< Takes a @ref MMAL_PARAMETER_INT32_T */
        MMAL_PARAMETER_CUSTOM_CCM,                /**< Takes a @ref MMAL_PARAMETER_CUSTOM_CCM_T */
        #endregion
    }
}
