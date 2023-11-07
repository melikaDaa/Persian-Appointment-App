using AppointmentApp.Data;
using AppointmentApp.Models;
using AppointmentApp.Models.ViewModels;
using AppointmentApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext db;

        public AppointmentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddUpdate(AppointmentViewModel model)
        {
          
            if (model != null && model.Id > 0)
            {
                //update
                var appointment = db.Appointments.FirstOrDefault(x => x.Id == model.Id);
                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = model.StartDate;
                appointment.EndDate = model.EndDate;
                appointment.Duration = model.Duration;
                appointment.DoctorId = model.DoctorId;
                appointment.PatientId = model.PatientId;
                appointment.IsDoctorApproved = false;
                appointment.AdminId = model.AdminId;
                return 1;
            }
            else
            {
              //  cretae
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId


                };
                db.Appointments.Add(appointment);
                await db.SaveChangesAsync();
                return 2;
            }
        }
        public List<DoctorViewModel> GetDoctorList()
        {
           var doctor=(from user in db.Users
                       join useroles in db.UserRoles  on user.Id equals useroles.UserId
                       join roles in db.Roles.Where(x=>x.Name==Helper.Doctor)on useroles.RoleId equals roles.Id
                       select new DoctorViewModel
                       {
                           Id= user.Id,
                           Name=user.Name,
                       }
                       ).ToList();  
            return doctor;
        }

        public List<PatientViewModel> GetPatientList()
        {
            var patient = (from user in db.Users
                          join useroles in db.UserRoles on user.Id equals useroles.UserId
                          join roles in db.Roles.Where(x => x.Name == Helper.Patient) on useroles.RoleId equals roles.Id
                          select new PatientViewModel
                          {
                              Id = user.Id,
                              Name = user.Name,
                          }
                         ).ToList();
            return patient;
        }
        public List<AppointmentViewModel> DoctorsEventsById(string doctorId)
        {
           
            return db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
        public List<AppointmentViewModel> PatientsEventsById(string patientId)
        {
            return db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }

        public AppointmentViewModel GetById(int id)
        {
            return db.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
            }).SingleOrDefault();
        }

        public async Task<int> Delete(int id)
        {
            var appointment = db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                db.Appointments.Remove(appointment);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ConirmEvent(int id)
        {
            var appointment = db.Appointments.FirstOrDefault(x => x.Id == id);
            if(appointment != null)
            {
                appointment.IsDoctorApproved=true;
                return await db.SaveChangesAsync();
            }
            return 0;
        }
    }
}
