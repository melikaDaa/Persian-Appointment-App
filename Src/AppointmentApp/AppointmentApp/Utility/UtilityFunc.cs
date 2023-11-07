using System.Globalization;
using System;

namespace AppointmentApp.Utility
{
    public class UtilityFunc
    {
        public static string ConvertDateTimeToShamsi(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return String.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
        }
    }
}
