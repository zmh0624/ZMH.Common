using System.Configuration;
using System.Drawing;

namespace Ecis.Common.CommonHelper
{
    public class AppSettingCfg
    {
        public static string GetAppConfig(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        /// <summary>
        /// 是否启用自动回收物理内存
        /// </summary>
        public static bool UseAutoFlushMemory
        {
            get
            {
                var mem = GetAppConfig("UseAutoFlushMemory");
                if (string.IsNullOrEmpty(mem))
                {
                    return false;
                }
                else
                {
                    return mem.Equals(bool.TrueString);
                }
            }
        }

        /// <summary>
        /// 是否直连数据库
        /// </summary>
        public static bool UseADODirect
        {
            get
            {
                var ado = GetAppConfig("UseADODirect");
                if (string.IsNullOrEmpty(ado))
                {
                    return false;
                }
                else
                {
                    return ado.Equals(bool.TrueString);
                }
            }
        }

        /// <summary>
        /// 床卡是否启用扩展提示块
        /// </summary>
        public static bool BedCardSupperTooltip
        {
            get
            {
                return GetAppConfig("BedCardSupperTooltip").Equals(bool.TrueString);
            }
        }

        private static int _rtimeRedColor = 0;
        private static int _rtimeYellowColor = 0;
        private static int _rtimeTriage = 0;
        private static int _redColor = 0;
        private static int _orangeColor = 0;
        private static int _yellowColor = 0;
        private static int _greenColor = 0;

        /// <summary>
        /// 滞留时间红色时间(默认2) ，单位：小时
        /// </summary>
        public static int RTimeRedColor
        {
            get
            {
                if (_rtimeRedColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("RTimeRedColor"), out _rtimeRedColor))
                    {
                        _rtimeRedColor = 2;
                    }
                }
                return _rtimeRedColor;
            }
        }

        /// <summary>
        /// 滞留时间黄色时间（默认12），单位：小时
        /// </summary>
        public static int RTimeYellowColor
        {
            get
            {
                if (_rtimeYellowColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("RTimeYellowColor"), out _rtimeYellowColor))
                    {
                        _rtimeYellowColor = 12;
                    }
                }
                return _rtimeYellowColor;
            }
        }

        /// <summary>
        /// 分诊滞留时间时间（默认10），单位：分钟
        /// </summary>
        public static int RTimeTriage
        {
            get
            {
                if (_rtimeTriage == 0)
                {
                    if (!int.TryParse(GetAppConfig("RTimeTriage"), out _rtimeTriage))
                    {
                        _rtimeTriage = 10;
                    }
                }
                return _rtimeTriage;
            }
        }

        /// <summary>
        /// 一级颜色（默认红）
        /// </summary>
        public static Color FstLevelRedColor
        {
            get
            {
                if (_redColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("FstLevelColor"), out _redColor))
                    {
                        return Color.FromArgb(-65536);
                    }
                }
                return Color.FromArgb(_redColor);
            }
        }

        /// <summary>
        /// 二级颜色（默认橙）
        /// </summary>
        public static Color SecLevelOrangeColor
        {
            get
            {
                if (_orangeColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("SecLevelColor"), out _orangeColor))
                    {
                        return Color.FromArgb(-47872);
                    }
                }
                return Color.FromArgb(_orangeColor);
            }
        }

        /// <summary>
        /// 三级颜色（默认黄）
        /// </summary>
        public static Color ThdLevelYellowColor
        {
            get
            {
                if (_yellowColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("ThdLevelColor"), out _yellowColor))
                    {
                        return Color.FromArgb(-23296);
                    }
                }
                return Color.FromArgb(_yellowColor);
            }
        }

        /// <summary>
        /// 四级颜色（默认绿）
        /// </summary>
        public static Color FourLevelGreenColor
        {
            get
            {
                if (_greenColor == 0)
                {
                    if (!int.TryParse(GetAppConfig("FourLevelColor"), out _greenColor))
                    {
                        return Color.FromArgb(-16744448);
                    }
                }
                return Color.FromArgb(_greenColor);
            }
        }

        /// <summary>
        /// 已提交医嘱颜色
        /// </summary>
        public static Color SubmitColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("SubmitColor"));
            }
        }

        /// <summary>
        /// 已作废医嘱颜色
        /// </summary>
        public static Color InvalidColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("InvalidColor"));
            }
        }

        /// <summary>
        /// 已停用医嘱颜色
        /// </summary>
        public static Color StopColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("StopColor"));
            }
        }

        /// <summary>
        /// 已缴费医嘱颜色
        /// </summary>
        public static Color PayColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("PayColor"));
            }
        }

        /// <summary>
        /// 已执行医嘱颜色
        /// </summary>
        public static Color ExecuteColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("ExecuteColor"));
            }
        }

        /// <summary>
        /// 皮试阳性医嘱颜色
        /// </summary>
        public static Color PositiveColor
        {
            get
            {
                return ColorTranslator.FromHtml(GetAppConfig("PositiveColor"));
            }
        }

        public static string WristbandPrinterName
        {
            get
            {
                return GetAppConfig("WristbandPrinterName");
            }
        }

        public static string InHospitalApplicationPrinterName
        {
            get
            {
                return GetAppConfig("InHospitalApplicationPrinterName");
            }
        }

        public static string BloodApplyPrinterName
        {
            get
            {
                return GetAppConfig("BloodApplyPrinterName");
            }
        }

        public static string SeatCardPrinterName
        {
            get
            {
                return GetAppConfig("SeatCardPrinterName");
            }
        }

        /// <summary>
        /// 电子病历水印 1:启用 0:不用
        /// </summary>
        public static bool IsShowEmrBGImage
        {
            get
            {
                return GetAppConfig("IsShowEmrBGImage") == "1";
            }
        }

        /// <summary>
        /// 从配置文件中获取 时间轴 配置  1启用  0不启用
        /// </summary>
        public static bool IsShowUcTimeLine
        {
            get
            {
                return GetAppConfig("IsShowUcTimeLine") == "1";
            }
        }
    }
}