using System;

namespace DotNet.Utilities
{
    public class PHPTIMETOCSHAPTIME
    {
       #region  Unix ʱ���ת��
       /// <summary>
       /// DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
       /// </summary>
       /// <param name="time"> DateTimeʱ���ʽ</param>
       /// <returns>Unixʱ�����ʽ</returns>
       public static int ConvertDateTimeInt(System.DateTime time)
       {
           System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
           return (int)(time - startTime).TotalSeconds;
       }
      /// <summary>
        /// ʱ���תΪC#��ʽʱ��
        /// </summary>
        /// <param name="timeStamp">Unixʱ�����ʽ</param>
        /// <returns>C#��ʽʱ��</returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

#endregion
    }
}
