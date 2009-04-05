using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace Opo.Net.Mime
{
    public static class MimeUtilities
    {
        public static DateTime ParseRfc2822Date(string date)
        {
            date = date.Replace("bst", "+0100");
            date = date.Replace("gmt", "-0000");
            date = date.Replace("edt", "-0400");
            date = date.Replace("est", "-0500");
            date = date.Replace("cdt", "-0500");
            date = date.Replace("cst", "-0600");
            date = date.Replace("mdt", "-0600");
            date = date.Replace("mst", "-0700");
            date = date.Replace("pdt", "-0700");
            date = date.Replace("pst", "-0800");

            DateTime parsedDateTime = DateTime.MinValue;

            Regex r = new Regex(@"(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(?<DateTime>\d{1,2} (?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) \d{4} \d{2}\:\d{2}(?:\:\d{2})?)(?: (?<TimeZone>[\+-]\d{4}))?", RegexOptions.IgnoreCase);
            Match m = r.Match(date);
            if (m.Success)
            {
                string dateTime = m.Groups["DateTime"].Value.TrimStart('0');
                parsedDateTime = DateTime.ParseExact(dateTime, new string[] { "d MMM yyyy HH:mm", "d MMM yyyy HH:mm:ss", "d MMM yyyy hh:mm", "d MMM yyyy hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None);

                string timeZone = m.Groups["TimeZone"].Value;
                if (timeZone.Length == 5)
                {
                    int hour = Int32.Parse(timeZone.Substring(0, 3));
                    int minute = Int32.Parse(timeZone.Substring(3));
                    TimeSpan offset = new TimeSpan(hour, minute, 0);
                    parsedDateTime = new DateTimeOffset(parsedDateTime, offset).UtcDateTime;
                }
            }
            return parsedDateTime;
        }
        public static string ToRfc2822Date(this DateTime date)
        {
            //TODO: Implement conversion to RFC2822 Date (offset)
            TimeSpan offset = TimeZoneInfo.Local.BaseUtcOffset;
            Debug.WriteLine(offset.ToString());
            //DateTimeOffset dateTimeOffset = new DateTimeOffset(date, offset);
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss +") + offset.Hours.ToString().PadRight('0') + "00";
        }
    }
}
