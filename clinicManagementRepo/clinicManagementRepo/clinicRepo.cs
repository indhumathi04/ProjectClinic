using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinicManagementRepo
{
    public class clinicRepo
    {
        public bool login(string staffUserName, string password)
        {
            bool loginSuccess = clinicSqlClient.isAccountValid(staffUserName, password);
            if (loginSuccess)
            {
                return true;
            }
            else
            {
                throw new Exception("Invalid login credentials... Enter correct username and password");
            }
        }

        public int addPatient(patient p)
        {
            int id = clinicSqlClient.addPatientData(p);
            if(id == 0)
            {
                throw new Exception("Error, Patient not added. Try again later");
            }
            else
            {
                return id;
            }
        }

        public List<doctor> getDoctorDetails()
        {
            List<doctor> doctorDetails = clinicSqlClient.getDoctorData();
            if(doctorDetails != null) 
            {
                return doctorDetails;
            }
            else
            {
                throw new Exception("No doctor details available  ");
            }
            
        }
        public List<doctor> getDoctorDetailsOnSpecialization(string specialization)
        {
            List<doctor> docList = clinicSqlClient.getDoctorOnSpecialization(specialization);
            if (docList.Count != 0 || docList== null)
            {
                return docList;
            }
            else
            {
                Console.WriteLine(docList.Count);
                throw new Exception("Doctor data not found for given specialization");
            }
        }
        public int bookAppointment(appointment appointment)
        {
            List<appointment> appoint = clinicSqlClient.getAppointmentData(appointment.patientId, appointment.visitDate);
            int appointmentId = clinicSqlClient.addAppointmentData(appointment);
            if(appointmentId == 0)
            {
                throw new Exception("Error, Appointment not booked. Try Again later");
            }
            else
            {
                return appointmentId;
            }
        }

        public void cancelAppointment(int appointmentId)
        {
            if (clinicSqlClient.checkAppointmentExists(appointmentId))
            {
                clinicSqlClient.cancelAppointment(appointmentId);
                Console.WriteLine("Your appointment is cancelled successfully!.");
            }
            else
            {
                throw new Exception("Invalid appointment ID");
            }
        }

        public List<appointment> getBookedAppointments(int id,DateOnly visitDate)
        {
            var appointment = clinicSqlClient.getAppointmentData(id, visitDate);
            if(appointment != null)
            {
                return appointment;
            }
            else
            {
                throw new Exception("No appointments available for given patient ID and visit date. ");
            }
        }

        public List<appointment> existingAppointmentForDoc(int docId, DateOnly visitDate)
        {
            List<appointment> existingBookedAppointment = clinicSqlClient.getExistingAppointmentDataForDoc(docId, visitDate);
            return existingBookedAppointment;
        }

        public List<string> getSpecialization()
        {
            return clinicSqlClient.getSpecialization();
        }
        public bool checkPatientExists(int id)
        {
            if (clinicSqlClient.checkPatient(id))
            {
                return true;
            }
            else
            {
                throw new Exception("Invalid Patient ID");
            }
        }
    }
}
