using AppointmentApp.Services;
using AppointmentApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.DoctorList = appointmentService.GetDoctorList();
            ViewBag.PatientList = appointmentService.GetPatientList();
            return View();
        }
    }
}
