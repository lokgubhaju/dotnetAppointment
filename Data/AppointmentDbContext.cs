using Microsoft.EntityFrameworkCore;
using TerminDoc.Models;
namespace TerminDoc.Data{
    public class AppointmentDbContext : DbContext{
        public AppointmentDbContext (DbContextOptions<AppointmentDbContext>options)
            :base(options)
    {   
    }
     public DbSet<AppointmentViewModel> Appointment { get; set; }

    }
}