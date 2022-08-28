using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace clinicManagementRepo
{
    public class patient
    {
        public int patientId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public DateOnly dob { get; set; }
        public patient() { }
        public patient(string firstName, string lastName, string gender, int age, DateOnly dob)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.age = age;
            this.dob = dob;
        }


    }
}
