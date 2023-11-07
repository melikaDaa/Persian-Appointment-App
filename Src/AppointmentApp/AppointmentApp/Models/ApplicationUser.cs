using Microsoft.AspNetCore.Identity;

namespace AppointmentApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
