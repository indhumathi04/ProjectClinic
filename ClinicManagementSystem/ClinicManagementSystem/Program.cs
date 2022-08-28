using clinicManagementRepo;
using System.Globalization;
using System.Text.RegularExpressions;

namespace clinicManagementSystem
{
    internal class clinicManagementClient {
        public static clinicRepo clinic = new clinicRepo();
        public static void Main()
        {
            bool active = true;
            bool login = false;
            Console.WriteLine("***************************** CLINIC MANAGEMENT SYSTEM *****************************");
            while (active)
            {  
                try
                {
                    login = stafflogin();
                    if (login)
                    {
                        Console.WriteLine("You've logged in successfully!\n");
                        while (login)
                        {
                            try
                            {
                                Console.WriteLine("select from the following options...");
                                Console.WriteLine("1 - View doctors \n2 - Add patient \n3 - Schedule Appointment \n4 - Cancel Appointment \n5 - Logout ");
                                Console.Write("Enter your choice:  ");
                                int choice = Convert.ToInt32(Console.ReadLine());
                                switch (choice)
                                {
                                    case 1: viewDoctors(); continue;
                                    case 2: addPatient(); continue;
                                    case 3: scheduleAppointment(); continue;
                                    case 4: cancelAppointment(); continue;
                                    case 5: login = false; break;
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private static void addPatient()
        {
            string firstName = getName("firstname");
            string lastName = getName("lastname");
            int age = getAge();
            string gender = getGender();
            DateOnly dob = DateOnly.FromDateTime(getDob());
            patient patientObj = new patient(firstName, lastName, gender, age, dob);
            int id = clinic.addPatient(patientObj);
            Console.WriteLine("Your patient ID: "+id);
        }

        private static void cancelAppointment()
        {
            Console.WriteLine("\n****************************** CANCEL APPOINTMENT ***********************************");
            Console.Write("Enter patient ID: ");
            int id = getID();
            Console.Write("Enter date of visit. (format: yyyy/mm/dd)");
            DateOnly visitDate = DateOnly.Parse(Console.ReadLine());
            List<appointment> appointments = clinic.getBookedAppointments(id, visitDate);
            foreach(appointment appointment in appointments)
            {
                Console.WriteLine("Appointment ID :"+ appointment.appointmentId +"  Specialization: "+appointment.specialization+"  Doctor ID: "+appointment.doctorId+" Visitdate :"+ appointment.visitDate+" Time: "+appointment.appointmentTime+ " Status: "+ appointment.appointmentStatus);
            }
            Console.Write("Enter the appointment ID to be cancelled:  ");
            int appId = getID();
            clinic.cancelAppointment(appId);
        }
        private static void scheduleAppointment()
        {
            Console.WriteLine("\n******************************* SCHEDULE APPOINTMENT *******************************");
            Console.Write("Enter patient ID :  ");
            int id = getID();
            clinic.checkPatientExists(id);
            Console.WriteLine("\nChoose specialization from the following options");
            List<string> specialization = clinic.getSpecialization();
            for(int i = 0; i < specialization.Count; i++)
            {
                Console.WriteLine(i+1 + " - " + specialization[i]);
            }
            Console.Write("Enter your choice of specialization:  ");
            int c = getChoice(specialization.Count);
            string specializationChoice = specialization[c - 1];
            List<doctor> doc = clinic.getDoctorDetailsOnSpecialization(specializationChoice);
            Console.WriteLine("\nDoctors available on specialization " + specializationChoice + " are: \n");
            foreach(doctor doctor in doc)
            {
                Console.WriteLine("Doctor ID: " + doctor.doctorId + " Doctor Name: " + doctor.firstName + " " + doctor.lastName + " ");
            }
            Console.WriteLine("Enter doctor ID of the doctor chosen: ");
            int docId = getID();
            doctor d = doc.Where(x => x.doctorId == docId).FirstOrDefault();
            Console.Write("Enter visit date (format: yyyy/mm/dd) :  ");
            DateOnly visitDate = DateOnly.Parse(Console.ReadLine());
          
            Console.WriteLine("\nAvailable slots :  ");

            List<TimeSpan> existingAppointmentsForDoc =  (from app in clinic.existingAppointmentForDoc(d.doctorId, visitDate) select app.appointmentTime).ToList();
            int slotNo = 0;
            for (TimeSpan i = d.visitStartTime; i < d.visitEndTime; i = i.Add(new TimeSpan(1,0,0)),slotNo++)
            {
                if (!existingAppointmentsForDoc.Contains(i))
                {
                    Console.WriteLine(slotNo+1 + " - " + i +" status: available");
                }
                else
                {
                    Console.WriteLine(slotNo+1 + " - " + i + " status: booked");
                }
            }
            Console.Write("Enter your choice of slot :  "); 
            int slotChoice = getChoice(slotNo);
            appointment bookNewAppointment = new appointment(id,specializationChoice,d.doctorId,visitDate,d.visitStartTime.Add(new TimeSpan(slotChoice-1,0,0)),"booked");
            int appId= clinic.bookAppointment(bookNewAppointment);
            Console.WriteLine("Your appointment ID  : " + appId +"\n");
        }

        static bool stafflogin()
        {
            Console.WriteLine("\n*********************************** LOGIN PAGE *************************************");
            Console.Write("Enter your username : ");
            string userName= Console.ReadLine();
            Console.Write("Enter your password : ");
            string password= Console.ReadLine();
            return clinic.login(userName, password);
        }
        static string getName(string firstOrLast)
        {

            Console.Write("Enter the patient's "+firstOrLast+":  ");
            var name = Console.ReadLine();
            var regexItem = new Regex("^[a-zA-Z ]*$");

            if (regexItem.IsMatch(name))
            {
                return name;
            }
            else
            {
                throw new Exception("Name should not contain symbols or numbers. Enter valid name. ");
            }

        }
        static DateTime getDob()
        {
            Console.Write("Enter the patient's Date of Birth:  ");
            DateTime dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            int res = DateTime.Compare(dob, DateTime.Now);
            int res1 = DateTime.Compare(DateTime.Now.AddYears(-120), dob);
            if (res <= 0 && res1 < 0)
            {
                return dob;
            }
            else
            {
                throw new Exception("Enter valid Date of Birth");
            }

        }
        static string getGender()
        {
            Console.Write("Enter the gender of the patient:  ");
            string gender = Console.ReadLine();
            if ( gender.ToLower() == "female"|| gender.ToLower()=="f")
            {
                return "female";
            }
            else if (gender.ToLower() == "male"|| gender.ToLower()=="m")
            {
                return "male";
            }
            else if (gender.ToLower() == "other")
            {
                return "other";
            }
            else
            {
                throw new Exception("Invalid gender. Enter valid option");
            }
        }
        static int getAge()
        {
            Console.Write("Enter the age of patient:  ");
            int Age;
            bool isInt = int.TryParse(Console.ReadLine(),out Age);
            if (isInt)
            {
                if (Age > 0 && Age <= 120)
                {
                    return Age;
                }
                else
                {
                    throw new Exception("Invalid Age. Enter number between 0 and 120");
                }
            }
            else
            {
                throw new Exception("Invalid Age. Enter only numbers");
            }
        }

        static int getID()
        {
            int id;
            bool isInt = int.TryParse(Console.ReadLine(), out id);
            if (isInt)
            {
                return id;
            }
            else
            {
                throw new Exception("Invalid ID. Enter only numbers");
            }
        }

        static int getChoice(int count)
        {
            int choice;
            bool isInt = int.TryParse(Console.ReadLine(), out choice);
            if (isInt)
            {
                return choice;
            }
            else
            {
                throw new Exception("Invalid choice. Enter valid choice between 1 - " + count);
            }
        }
        static void viewDoctors()
        {
            List<doctor> doc = clinic.getDoctorDetails();
            foreach(doctor doctor in doc)
            {
                Console.WriteLine("Doctor ID:  " + doctor.doctorId+ ", Doctor's name: "+ doctor.firstName+" "+doctor.lastName +", Gender: "+doctor.gender + ", Specialization: "+ doctor.specialization +"\n" );
            }

        }
    }
}