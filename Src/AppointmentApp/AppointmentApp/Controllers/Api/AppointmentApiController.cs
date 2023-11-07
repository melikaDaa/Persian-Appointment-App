using AppointmentApp.Models.ViewModels;
using AppointmentApp.Services;
using AppointmentApp.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentApp.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            this.appointmentService = appointmentService;
            this.httpContextAccessor = httpContextAccessor;
            loginUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentViewModel data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = appointmentService.AddUpdate(data).Result;
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch (System.Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponse<List<AppointmentViewModel>> commonResponse = new CommonResponse<List<AppointmentViewModel>>();
            try
            {
                if (role == Helper.Patient)
                {
                    commonResponse.dataenum = appointmentService.PatientsEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    commonResponse.dataenum = appointmentService.DoctorsEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else
                {
                    commonResponse.dataenum = appointmentService.DoctorsEventsById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentViewModel> commonResponse = new CommonResponse<AppointmentViewModel>();
            try
            {
                commonResponse.dataenum = appointmentService.GetById(id);
                commonResponse.status = Helper.success_code;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = await appointmentService.Delete(id);
                commonResponse.message = commonResponse.status == 1 ? Helper.appointmentDeleted : Helper.somethingWentWrong;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public async Task<IActionResult> ConfirmEvent(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var result = appointmentService.ConirmEvent(id).Result;
                if (result > 0)
                {
                    commonResponse.status = Helper.success_code;
                    commonResponse.message = Helper.meetingConfirm;
                }
                else
                {
                    commonResponse.status = Helper.failure_code;
                    commonResponse.message = Helper.meetingConfirmError;
                }

            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
