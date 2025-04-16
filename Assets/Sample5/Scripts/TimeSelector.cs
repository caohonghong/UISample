using UnityEngine.Serialization;

namespace Game.Logic
{
    using UnityEngine;
    using TMPro;
    using System;
    using System.Collections.Generic;

    public class TimeSelector : MonoBehaviour
    {
        public TMP_Dropdown dateDropdown;
        public TMP_Dropdown hourDropdown;
        public TMP_Dropdown minuteDropdown;
        public TextMeshProUGUI txtTime;
        private long _cdEndTimeStamp = 1698231180; // 示例，以实际例子替换
        private long _selectedTime;
        private long earliestTimeTimestamp;
        private DateTime earliestTime;

        public long SelectedTime => _selectedTime;
        
        public long _currentTimeStamp;//当前时间戳（需要赋值）


        private void Awake()
        {
            // Add listeners to dropdowns
            dateDropdown.onValueChanged.AddListener(delegate
            {
                UpdateHourDropdown();
                UpdateMinuteDropdown();
                UpdateSelectedTime();
            });
            hourDropdown.onValueChanged.AddListener(delegate
            {
                UpdateMinuteDropdown();
                UpdateSelectedTime();
            });
            minuteDropdown.onValueChanged.AddListener(delegate { UpdateSelectedTime(); });
        }

        public void InitTimeSelector(long cdEndTimeStamp)
        {
            _cdEndTimeStamp = cdEndTimeStamp;
            earliestTimeTimestamp = GetEarliestTime();
            earliestTime = DateTimeOffset.FromUnixTimeSeconds(earliestTimeTimestamp).UtcDateTime;

            InitializeDateDropdown();
            UpdateHourDropdown();
            UpdateMinuteDropdown();
            SetToDefaultTime();
        }

        long GetEarliestTime()
        {
            // 获取当前时间的UTC时间戳

            long currentTimeStamp = _currentTimeStamp;

            // 确定基准时间戳
            DateTime baseTime;

            if (currentTimeStamp < _cdEndTimeStamp)
            {
                baseTime = DateTimeOffset.FromUnixTimeSeconds(_cdEndTimeStamp).UtcDateTime;
            }
            else
            {
                baseTime = DateTimeOffset.FromUnixTimeSeconds(currentTimeStamp).UtcDateTime;
            }

            // 调整基准时间到下一个5的倍数的分钟
            int minutes = baseTime.Minute;
            int adjustment = (minutes % 5 == 0) ? 0 : 5 - (minutes % 5);
            DateTime earliestReservationTime = baseTime.AddMinutes(adjustment);

            // 转换最早预约时间为UTC时间戳
            long earliestReservationTimeStamp = ((DateTimeOffset)earliestReservationTime).ToUnixTimeSeconds();
            return earliestReservationTimeStamp;
        }

        private void InitializeDateDropdown()
        {
            dateDropdown.ClearOptions();
            DateTime nextDay = earliestTime.Date.AddDays(1);

            List<string> dateOptions = new List<string>
            {
                earliestTime.ToString("MM/dd"),
                nextDay.ToString("MM/dd")
            };

            dateDropdown.AddOptions(dateOptions);
        }

        private void UpdateHourDropdown()
        {
            hourDropdown.ClearOptions();
            DateTime selectedDate = DateTime.ParseExact(dateDropdown.options[dateDropdown.value].text, "MM/dd", null);

            List<string> hourOptions = new List<string>();
            int startHour = (selectedDate.Date == earliestTime.Date) ? earliestTime.Hour : 0;

            for (int hour = startHour; hour <= 23; hour++)
            {
                hourOptions.Add(hour.ToString("D2"));
            }

            hourDropdown.AddOptions(hourOptions);

            // Reset hour to the first value after updating options
            if (hourOptions.Count > 0)
            {
                hourDropdown.value = 0;
            }
        }

        private void UpdateMinuteDropdown()
        {
            minuteDropdown.ClearOptions();
            DateTime selectedDate = DateTime.ParseExact(dateDropdown.options[dateDropdown.value].text, "MM/dd", null);
            int selectedHour = int.Parse(hourDropdown.options[hourDropdown.value].text);

            List<string> minuteOptions = new List<string>();
            int startMinute = (selectedDate.Date == earliestTime.Date && selectedHour == earliestTime.Hour)
                ? earliestTime.Minute
                : 0;
            int endMinute = 55;

            for (int minute = startMinute; minute <= endMinute; minute += 5)
            {
                minuteOptions.Add(minute.ToString("D2"));
            }

            minuteDropdown.AddOptions(minuteOptions);

            // Reset minute to the first value after updating options
            if (minuteOptions.Count > 0)
            {
                minuteDropdown.value = 0;
            }
        }

        private void SetToDefaultTime()
        {
            DateTime now = DateTime.UtcNow;
            DateTime referenceTime = now >= earliestTime ? now : earliestTime;

            int minute = referenceTime.Minute;
            minute = (minute % 5 == 0) ? minute : (minute / 5 + 1) * 5;
            if (minute >= 60)
            {
                referenceTime = referenceTime.AddHours(1);
                minute = 0;
            }

            DateTime defaultTime = new DateTime(referenceTime.Year, referenceTime.Month, referenceTime.Day,
                referenceTime.Hour, minute, 0, DateTimeKind.Utc);

            DateTime maxBookableTime = earliestTime.Date.AddDays(1).AddHours(23).AddMinutes(55);
            if (defaultTime < earliestTime)
                defaultTime = earliestTime;
            if (defaultTime > maxBookableTime)
                defaultTime = maxBookableTime;

            dateDropdown.value = dateDropdown.options.FindIndex(option => option.text == defaultTime.ToString("MM/dd"));
            hourDropdown.value =
                hourDropdown.options.FindIndex(option => option.text == defaultTime.Hour.ToString("D2"));
            minuteDropdown.value =
                minuteDropdown.options.FindIndex(option => option.text == defaultTime.Minute.ToString("D2"));

            UpdateSelectedTime(); // Initial update to display the default time
        }

        private void UpdateSelectedTime()
        {
            string selectedDateStr = dateDropdown.options[dateDropdown.value].text;
            int selectedHour = int.Parse(hourDropdown.options[hourDropdown.value].text);
            int selectedMinute = int.Parse(minuteDropdown.options[minuteDropdown.value].text);

            DateTime selectedDate = DateTime.ParseExact(selectedDateStr, "MM/dd", null);
            DateTime utcSelectedTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                selectedHour, selectedMinute, 0, DateTimeKind.Utc);

            _selectedTime = ((DateTimeOffset)utcSelectedTime).ToUnixTimeSeconds();
            // 转换为本地时间并显示
            DateTime localTime = utcSelectedTime.ToLocalTime();
            txtTime.text = "localTime : "+(string.Format("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}",
                localTime.Year, localTime.Month, localTime.Day, localTime.Hour, localTime.Minute)); 
        }
        //添加额外的时间方法
        
    }
}