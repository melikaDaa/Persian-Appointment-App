using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AppointmentApp.Utility
{
    public class Helper
    {
        public const string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        public static string appointmentAdded = "رزرو با موفقیت اضافه شد";
        public static string appointmentUpdated = "رزرو با موفقیت ویرایش شد.";
        public static string appointmentDeleted = "رزرو با موفقیت حذف شد";
        public static string appointmentExists = "قرار ملاقات برای تاریخ و زمان انتخاب شده از قبل وجود دارد.";
        public static string appointmentNotExists = "رزرو وجود ندارد";

        public static string meetingConfirm = "جلسه با موفقیت تایید شد.";
        public static string meetingConfirmError = "مشکل در جلسه ";

        public static string appointmentAddError = "مشکلی رخ داده دوباره تلاش کنید";
        public static string appointmentUpdatError = "مشکلی رخ داده دوباره تلاش کنید";
        public static string somethingWentWrong = "مشکلی رخ داده دوباره تلاش کنید";
        public static int success_code = 1;
        public static int failure_code = 0;

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Admin, Text=Helper.Admin},
                    new SelectListItem{Value=Helper.Doctor, Text=Helper.Doctor}
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Patient, Text=Helper.Patient}
                };
            }
        }


        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " ساعت" });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " ساعت 30 دقیقه" });
                minute = minute + 30;
            }
            return duration;
        }
    }
}
