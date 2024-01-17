using System.Globalization;

namespace NafathAPI.Extensions;
public static class DateExtensions
    {
    public static string GetHijriDate ( this DateTime date )
        {
        DateTimeFormatInfo HijriDTFI;
        HijriDTFI = new CultureInfo ( "ar-SA" , false ).DateTimeFormat;
        HijriDTFI.Calendar = new UmAlQuraCalendar ( );
        return date.Date.ToString ( "yyyy-MM-dd" , HijriDTFI );
        }

    public static DateTime GetGregDate ( this string dateString )
        {
        return DateTime.ParseExact ( dateString , "dd-MM-yyyy" , CultureInfo.InvariantCulture );
        }
    }
