using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinicManagementRepo
{
    public class doctor
    {
        public int doctorId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string specialization { get; set; }
        public TimeSpan visitStartTime { get; set; }
        public TimeSpan visitEndTime { get; set; }
        public doctor() { }
        public doctor(string firstName, string lastName, string gender, string specialization, TimeSpan visitStartTime, TimeSpan visitEndTime)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.specialization = specialization;
            this.visitStartTime = visitStartTime;
            this.visitEndTime = visitEndTime;
        }
    }
}
