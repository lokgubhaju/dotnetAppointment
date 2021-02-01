using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TerminDoc.Models
{
    public class AppointmentViewModel
    {
        [Key]
        public int id{get; set;}
        [Required]
        [Display(Name = "Appointment Date")]
        public DateTime appointmentDate { get; set; }
        [Required]
        public string Name{ get; set; }
     
        public string Address {get; set;}
        [Display(Name = "Blood Group")]
        public string BloodGroup{get; set;}
        [Range(1, 100)]        
        public int Age{get; set;}
        public string Gender{ get; set;}
    }
}