using AppointmentApp.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentApp.Services
{
    public interface IAppointmentService
    {
        public List<DoctorViewModel> GetDoctorList();
        public List<PatientViewModel> GetPatientList();
        public Task<int> AddUpdate(AppointmentViewModel model);
        public List<AppointmentViewModel> DoctorsEventsById(string doctorId);
        public List<AppointmentViewModel> PatientsEventsById(string patientId);

        public AppointmentViewModel GetById(int id);
        public Task<int> Delete(int id);
        public Task<int> ConirmEvent(int id);
    }
}
