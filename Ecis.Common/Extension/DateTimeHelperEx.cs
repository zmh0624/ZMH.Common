using System;

namespace ZMH.Common.Extension
{
    public static class DateTimeHelperEx
    {
        public static string GetAge(this DateTime? birth, DateTime? visit)
        {
            if (birth == null || !birth.HasValue ||
                visit == null || !visit.HasValue ||
                visit.Value < birth.Value) return "";

            #region
            //TimeSpan ts = visit.Value.Subtract(birth.Value);
            //if (ts.Days > 365) return string.Format("{0}岁", Math.Round(Decimal.Parse(ts.Days.ToString()) / 365, MidpointRounding.AwayFromZero));
            //if (ts.Days > 30) return string.Format("{0}月", ts.Days / 30);
            //if (ts.Days > 7) return string.Format("{0}周", ts.Days / 7);
            //return string.Format("{0}天", ts.Days);
            #endregion
            //*<3小时         显示分钟  如：125分钟
            //3小时<*<3天     显示小时  如：48小时
            //3天<*<3个月     显示天    如：60天
            //3个月<*<3岁     显示月    如：24个月
            //*>3岁           显示岁    如：5岁，87岁等。

            string age = string.Empty;
            try
            {
                if ((visit.Value.Year - birth.Value.Year) > 3 || ((visit.Value.Year - birth.Value.Year) == 3 && (birth.Value.Month <= visit.Value.Month)))//大于等于三岁，显示岁
                {
                    if (birth.Value.Month <= visit.Value.Month) //足月
                    {
                        age = (visit.Value.Year - birth.Value.Year).ToString() + "岁";
                    }
                    else
                    {
                        age = (visit.Value.Year - birth.Value.Year - 1).ToString() + "岁";
                    }
                }
                else if (
                    ((visit.Value.Year - birth.Value.Year) * 12 + (visit.Value.Month - birth.Value.Month)) > 3
                    ||
                    ((((visit.Value.Year - birth.Value.Year) * 12 + (visit.Value.Month - birth.Value.Month)) == 3) && (birth.Value.Day <= visit.Value.Day))
                    ) //大于等于三个月，显示月
                {
                    if (birth.Value.Day <= visit.Value.Day) //足日
                    {
                        age = ((visit.Value.Year - birth.Value.Year) * 12 + (visit.Value.Month - birth.Value.Month)).ToString() + "个月";
                    }
                    else
                    {
                        age = ((visit.Value.Year - birth.Value.Year) * 12 + (visit.Value.Month - birth.Value.Month) - 1).ToString() + "个月";
                    }
                }
                else if ((visit.Value - birth.Value).TotalDays >= 3) //大于等于三天，显示天
                {
                    age = ((int)(visit.Value - birth.Value).TotalDays).ToString() + "天";
                }
                else if ((visit.Value - birth.Value).TotalHours >= 3) //大于等于三小时，显示小时
                {
                    age = ((int)(visit.Value - birth.Value).TotalHours).ToString() + "小时";
                }
                else //小于三小时，显示分钟
                {
                    age = ((int)(visit.Value - birth.Value).TotalMinutes).ToString() + "分钟";
                }
            }
            catch { }
            return age;
        }

        public static int? GetAgeNum(this DateTime? birth, DateTime? currentDate)
        {
            if (birth == null || currentDate == null || currentDate < birth) return null;

            TimeSpan ts = currentDate.Value.Subtract(birth.Value);
            return ts.Days / 365;
        }

        public static string GetAge(this DateTime birth, DateTime visit)
        {
            if (birth == null || visit == null || visit < birth) return "";

            #region
            //TimeSpan ts = visit.Subtract(birth);
            //if (ts.Days > 365) return string.Format("{0}岁", Math.Round(Decimal.Parse(ts.Days.ToString()) / 365, MidpointRounding.AwayFromZero));
            //if (ts.Days > 30) return string.Format("{0}月", ts.Days / 30);
            //if (ts.Days > 7) return string.Format("{0}周", ts.Days / 7);
            //return string.Format("{0}天", ts.Days);
            #endregion

            //*<3小时         显示分钟  如：125分钟
            //3小时<*<3天     显示小时  如：48小时
            //3天<*<3个月     显示天    如：60天
            //3个月<*<3岁     显示月    如：24个月
            //*>3岁           显示岁    如：5岁，87岁等。

            string age = string.Empty;
            try
            {
                if ((visit.Year - birth.Year) > 3 || ((visit.Year - birth.Year) == 3 && (birth.Month <= visit.Month)))//大于等于三岁，显示岁
                {
                    if (birth.Month <= visit.Month) //足月
                    {
                        age = (visit.Year - birth.Year).ToString() + "岁";
                    }
                    else
                    {
                        age = (visit.Year - birth.Year - 1).ToString() + "岁";
                    }
                }
                else if (
                    ((visit.Year - birth.Year) * 12 + (visit.Month - birth.Month)) > 3
                    ||
                    ((((visit.Year - birth.Year) * 12 + (visit.Month - birth.Month)) == 3) && (birth.Day <= visit.Day))
                    ) //大于等于三个月，显示月
                {
                    if (birth.Day <= visit.Day) //足日
                    {
                        age = ((visit.Year - birth.Year) * 12 + (visit.Month - birth.Month)).ToString() + "个月";
                    }
                    else
                    {
                        age = ((visit.Year - birth.Year) * 12 + (visit.Month - birth.Month) - 1).ToString() + "个月";
                    }
                }
                else if ((visit - birth).TotalDays >= 3) //大于等于三天，显示天
                {
                    age = ((int)(visit - birth).TotalDays).ToString() + "天";
                }
                else if ((visit - birth).TotalHours >= 3) //大于等于三小时，显示小时
                {
                    age = ((int)(visit - birth).TotalHours).ToString() + "小时";
                }
                else //小于三小时，显示分钟
                {
                    age = ((int)(visit - birth).TotalMinutes).ToString() + "分钟";
                }
            }
            catch { }
            return age;
        }

        /// <summary>
        /// 年月周日小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeFormatHour(this DateTime time,DateTime now)
        {
            TimeSpan ts = now.Subtract(time);
            if (ts.Days > 365) return string.Format("{0}{1}", ts.Days / 365, Properties.Resources.Year);
            if (ts.Days > 30) return string.Format("{0}{1}", ts.Days / 30, Properties.Resources.Month);
            if (ts.Days > 7) return string.Format("{0}{1}", ts.Days / 7, Properties.Resources.Week);
            if (ts.Days > 1) return string.Format("{0}{1}", ts.Days, Properties.Resources.Day);

            return string.Format("{1}{0}", Properties.Resources.Hour, ts.Hours);
        }

        /// <summary>
        /// 小时分钟/日小时分钟
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeFormatHourMin(this DateTime? time,DateTime now)
        {
            if (time == null)
                return "";
            TimeSpan ts = now - Convert.ToDateTime(time);

            var str = "";
            if (ts.Days > 0)
            {
                str = string.Format("{4}{5}{2}{0}{3}{1}",
                    Properties.Resources.Hour, Properties.Resources.Min, ts.Hours, ts.Minutes, ts.Days, Properties.Resources.Day);
            }
            else
            {
                str = string.Format("{2}{0}{3}{1}", Properties.Resources.Hour, Properties.Resources.Min, ts.Hours, ts.Minutes);
            }
            return str;
        }

        /// <summary>
        /// 时间格式化：1900-01-01
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 时间格式化：1900-01-01 00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateHHmmString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 时间格式化：00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToTimeHHmmString(this DateTime time)
        {
            return time.ToString("HH:mm");
        }

        /// <summary>
        /// 时间格式化：1900-01-01 00:00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToPerformanceTime(this TimeSpan timeSpan)
        {
            return string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        /// <summary>
        /// 时间格式化：2015-01-01 00：00：00
        /// </summary>
        /// <param name="startDt"></param>
        /// <returns></returns>
        public static DateTime FormatWithMinHours(this DateTime startDt)
        {
            string t = startDt.ToDateString() + " 00:00:00";
            return DateTime.Parse(t);
        }

        /// <summary>
        /// 时间格式化：2015-01-01 23：59：59
        /// </summary>
        /// <param name="startDt"></param>
        /// <returns></returns>
        public static DateTime FormatWithMaxHours(this DateTime endDt)
        {
            string t = endDt.ToDateString() + " 23:59:59";
            return DateTime.Parse(t);
        }

        /// <summary>
        /// Str转DateTime
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string datetime)
        {
            if (string.IsNullOrEmpty(datetime))
            {
                return null;
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParse(datetime, out dt))
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}