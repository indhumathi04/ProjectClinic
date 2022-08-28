using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinicManagementRepo
{
    public class appointment
    {
        public int appointmentId { get; set; }
        public int patientId { get; set; }
        public string specialization { get; set; }
        public int doctorId { get; set; }
        public DateOnly visitDate { get; set; }
        public TimeSpan appointmentTime { get; set; }
        public string appointmentStatus { get; set; }
        public appointment()
        {

        }
        public appointment( int patientId, string specialization, int doctorId, DateOnly visitDate, TimeSpan appointmentTime, string appointmentStatus)
        {
            this.patientId = patientId;
            this.specialization = specialization;
            this.doctorId = doctorId;
            this.visitDate = visitDate;
            this.appointmentTime = appointmentTime;
            this.appointmentStatus = appointmentStatus;
        }
    }
}
