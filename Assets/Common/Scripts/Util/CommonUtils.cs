using System;

namespace Common.Scripts.Util
{
    public static class CommonUtils
    {

        /// <summary>
        /// 过滤字符串，只允许纯数字通过，非数字部分被略过<p/>
        /// 例如："1a2a3" -> "123"
        /// </summary>
        /// <param name="input">待检测的字符串</param>
        /// <returns>只包含0-9的数字的字符串</returns>
        public static string FilterNonNumeric(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            char[] result = input.ToCharArray();
            int index = 0;
            foreach (char c in result)
            {
                if (char.IsDigit(c))
                {
                    result[index++] = c;
                }
            }

            return new string(result, 0, index);
        }


    

    }
}