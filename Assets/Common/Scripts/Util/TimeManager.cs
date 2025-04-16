using System;

namespace Common.Scripts.Util
{
    public class TimeManager
    {
        public static readonly int ONE_DAY_HOUR = 24;

        public static readonly double D_ONE_HOUR_SECOND = 3600.0;
        public static readonly double D_ONE_DAY_SECOND = D_ONE_HOUR_SECOND * ONE_DAY_HOUR;

        public static readonly long L_ONE_HOUR_SECOND = 3600;
        public static readonly long L_ONE_DAY_SECOND = L_ONE_HOUR_SECOND * ONE_DAY_HOUR;

        public static readonly int ONE_YEAR_DAY = 365;
        public static readonly long L_ONE_YEAR_SECOND = L_ONE_DAY_SECOND * ONE_YEAR_DAY;
        public static readonly long L_ONE_YEAR_MILLISECOND = L_ONE_YEAR_SECOND * 1000;
        public static string FormatTimestampMilliSec(long timestampInMilliseconds)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampInMilliseconds).DateTime;
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// UTC时间戳转换为本时区/UTC时区的显示时间
        /// </summary>
        /// <param name="timestampUtc">utc0的时间戳</param>
        /// <param name="isLocal">是否为本地，默认否 (即为UTC)</param>
        /// <param name="timeFormat">时间格式，默认yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static string FormatTimestamp(long timestampUtc, bool isLocal = false,
            string timeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            DateTimeOffset dateTimeOffsetUtc = DateTimeOffset.FromUnixTimeSeconds(timestampUtc);
            TimeZoneInfo localTimeZone = TimeZoneInfo.Utc;
            if (isLocal)
            {
                localTimeZone = TimeZoneInfo.Local;
            }

            DateTimeOffset localDateTimeOffset = TimeZoneInfo.ConvertTime(dateTimeOffsetUtc, localTimeZone);
            return localDateTimeOffset.ToString(timeFormat);
        }
        // a-b
        public int GetIntervalDays(long timeSecondA, long timeSecondB)
        {
            var day1 = (long)Math.Floor(timeSecondA / D_ONE_DAY_SECOND);
            var day2 = (long)Math.Floor(timeSecondB / D_ONE_DAY_SECOND);
            return (int)(day1 - day2);
        }

        public static long ConvertToTimestamp(string dateTimeStr)
        {
            // 定义日期格式
            string format = "yyyy-M-d-H-m-s"; // 注意月份和日期单个数字的处理

            // 使用 DateTime.ParseExact 解析日期字符串
            DateTime dateTime = DateTime.ParseExact(dateTimeStr, format, null);

            // 转换为 Unix 时间戳
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.Zero);
            long unixTimestamp = dateTimeOffset.ToUnixTimeSeconds();

            return unixTimestamp;
        }
        
           public int CalculateDaysDifference(long timestampA, long timestampB)
        {
            // 将时间戳转换为 DateTime 对象
            DateTime dateTimeA = DateTimeOffset.FromUnixTimeSeconds(timestampA).UtcDateTime;
            DateTime dateTimeB = DateTimeOffset.FromUnixTimeSeconds(timestampB).UtcDateTime;

            // 计算差异
            TimeSpan difference = dateTimeB - dateTimeA;

            // 返回天数
            return (int)difference.TotalDays;
        }

        /*/// <summary>
        /// 详细时间参数，d:h  h:m m:s
        /// </summary>
        public string SetTimeFormat(long time, long formatKey = -1)
        {
            long days = (long)(time / 3600L / 24L);
            long hours = (long)(time / 3600L - days * 24L);
            long minutes = (long)(time - (hours * 3600L) - (days * 86400L)) / 60L;
            long seconds = (long)(time - (hours * 3600L) - (minutes * 60L) - (days * 86400L));

            if (days == 0 && hours == 0 && minutes == 0 && seconds == 0)
            {
                return "";
            }

            if (days > 0)
            {
                return XLang.GetFormat(formatKey != -1 ? formatKey : 500002, days.ToString("00"), hours.ToString("00"));
            }
            else if (hours > 0 && days == 0)
            {
                return XLang.GetFormat(formatKey != -1 ? formatKey : 500003, hours.ToString("00"),
                    minutes.ToString("00"));
            }
            else
            {
                return XLang.GetFormat(formatKey != -1 ? formatKey : 500004, minutes.ToString("00"),
                    seconds.ToString("00"));
            }
        }

        /// <summary>
        /// 详细时间参数 d:h:m:s   h:m:s  
        /// </summary>
        public string SetTimeFormat1(long time, long formatKey = -1)
        {
            long days = (long)(time / 3600L / 24L);
            long hours = (long)(time / 3600L - days * 24L);
            long minutes = (long)(time - (hours * 3600L) - (days * 86400L)) / 60L;
            long seconds = (long)(time - (hours * 3600L) - (minutes * 60L) - (days * 86400L));

            if (days == 0 && hours == 0 && minutes == 0 && seconds == 0)
            {
                return "";
            }

            if (days > 0)
            {
                return XLang.GetFormat(formatKey != -1 ? formatKey : 500108, days.ToString("00"), hours.ToString("00"),
                    minutes.ToString("00"), seconds.ToString("00"));
            }
            else
            {
                return XLang.GetFormat(formatKey != -1 ? formatKey : 500001, hours.ToString("00"),
                    minutes.ToString("00"), seconds.ToString("00"));
            }
        }*/
    }
}