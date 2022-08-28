using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;

namespace clinicManagementRepo
{
    public class clinicSqlClient
    {
        public static SqlConnection con;
        public static SqlCommand cmd;
        private static SqlConnection getcon()
        {
            con = new SqlConnection("Data Source=.;Initial Catalog=clinicDB;Integrated Security=true");
            con.Open();
            return con;
        }

        public static bool isAccountValid(string userName, string password)
        {
            con = getcon();
            cmd = new SqlCommand("select count(*) from staff where staffUserName = \'" + userName + "\' and staffPassword = \'" + password + "\'");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
            {
                return true;
            }
            return false;
        }

        public static int addPatientData(patient P)
        {
            con = getcon();
            cmd = new SqlCommand("insert into patient(firstName,lastName,sex,age,dob) values(@firstName,@lastName,@sex,@age,@dob)");
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@firstName", P.firstName);
            cmd.Parameters.AddWithValue("@lastName", P.lastName);
            cmd.Parameters.AddWithValue("@sex", P.gender);
            cmd.Parameters.AddWithValue("@age", P.age);
            cmd.Parameters.AddWithValue("@dob", P.dob.ToString()); ;
            cmd.ExecuteNonQuery();
            
            cmd = new SqlCommand("SELECT IDENT_CURRENT('patient')");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return (Convert.ToInt32(dr[0]));
            }
            return 0;

        }

        public static List<doctor> getDoctorData()
        {
            con=getcon();
            cmd = new SqlCommand("select * from doctor");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            List<doctor> doctorList = new List<doctor>();
            while (dr.Read())
            {
                doctor doctor = new doctor();
                doctor.doctorId = Convert.ToInt32(dr["doctorId"]);
                doctor.firstName = (string)dr["firstName"];
                doctor.lastName = (string)dr["lastName"];
                doctor.gender = (string)dr["sex"];
                doctor.specialization = (string)dr["specialization"];
                doctor.visitStartTime = (TimeSpan)dr["visitStartTime"];
                doctor.visitEndTime = (TimeSpan)dr["visitEndTime"];
                doctorList.Add(doctor); 
            }
            return doctorList;
        }

        public static List<doctor> getDoctorOnSpecialization(string specialization)
        {
            con = getcon();
            cmd = new SqlCommand("select * from doctor where specialization = \'"+specialization+"\'");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            List<doctor> doctorList = new List<doctor>();
            while (dr.Read())
            {
                doctor doctor = new doctor();
                doctor.doctorId = Convert.ToInt32(dr["doctorId"]);
                doctor.firstName = (string)dr["firstName"];
                doctor.lastName = (string)dr["lastName"];
                doctor.gender = (string)dr["sex"];
                doctor.specialization = (string)dr["specialization"];
                doctor.visitStartTime = (TimeSpan)dr["visitStartTime"];
                doctor.visitEndTime = (TimeSpan)dr["visitEndTime"];
                doctorList.Add(doctor);
            }
            return doctorList;
        }

        public static int addAppointmentData(appointment A)
        {
            con = getcon();
            cmd = new SqlCommand("insert into appointments(patientId,specialization,doctorId,visitDate,appointmentTime,appointmentStatus) values(@patientId,@specialization,@doctorId,@visitDate,@appointmentTime,@appointmentStatus)");
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@patientId", A.patientId);
            cmd.Parameters.AddWithValue("@specialization", A.specialization);
            cmd.Parameters.AddWithValue("@doctorId", A.doctorId);
            cmd.Parameters.AddWithValue("@visitDate", (A.visitDate).ToString());
            cmd.Parameters.AddWithValue("@appointmentTime", A.appointmentTime);
            cmd.Parameters.AddWithValue("@appointmentStatus", "booked");
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT IDENT_CURRENT('appointments')");

            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return (Convert.ToInt32(dr[0]));
            }
            return 0;
        }

        public static List<appointment> getAppointmentData(int patientId,DateOnly visitDate)
        {
            List<appointment> appointmentList = new List<appointment>();
            con = getcon();
            cmd = new SqlCommand("select * from appointments where patientId = @patientId and visitDate = @visitDate");
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@patientId", patientId);
            cmd.Parameters.AddWithValue("@visitDate", visitDate.ToString());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                appointment appointment = new appointment();
                appointment.appointmentId = Convert.ToInt32(dr["appointmentId"]);
                appointment.patientId = Convert.ToInt32(dr["patientId"]);
                appointment.specialization = (string)dr["specialization"];
                appointment.doctorId = Convert.ToInt32(dr["doctorId"]);
                appointment.visitDate = DateOnly.Parse(DateTime.Parse(dr["visitDate"].ToString()).ToString("dd/MM/yyyy"));
                appointment.appointmentTime = TimeSpan.Parse(dr["appointmentTime"].ToString());
                appointmentList.Add(appointment);
            }
            return appointmentList;
        }

        public static List<appointment> getExistingAppointmentDataForDoc(int docId, DateOnly visitDate)
        {
            /*ateTime dt = visitDate.ToDateTime(TimeOnly.Parse("10:00 PM"));*/
            //DateTime dt = DateTime.ParseExact(visitDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //Console.WriteLine(dt.ToString("yyyy/MM/dd"));
            //String source = visitDate.ToString();
            //String date = String.Join("-", source.Split('/').Reverse());
            //Console.WriteLine(date);
            List<appointment> appointmentList = new List<appointment>();
            con = getcon();
            cmd = new SqlCommand("select * from appointments where doctorId = @doctorId and visitDate = @date and appointmentStatus=\'booked\'");
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@date", visitDate.ToString());
            cmd.Parameters.AddWithValue("@doctorId", docId);
            //Console.WriteLine(dt.ToLongDateString());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                appointment appointment = new appointment();
                appointment.appointmentId = Convert.ToInt32(dr["appointmentId"]);
                appointment.patientId = Convert.ToInt32(dr["patientId"]);
                appointment.specialization = (string)dr["specialization"];
                appointment.doctorId = Convert.ToInt32(dr["doctorId"]);
                appointment.visitDate = DateOnly.Parse(DateTime.Parse(dr["visitDate"].ToString()).ToString("dd/MM/yyyy"));
                appointment.appointmentTime = TimeSpan.Parse(dr["appointmentTime"].ToString());
                appointmentList.Add(appointment);
            }
            return appointmentList;
        }

        public static List<string> getSpecialization()
        {
            List<string> specializationList = new List<string>();
            con = getcon();
            cmd = new SqlCommand("select distinct specialization from doctor");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                specializationList.Add((string)dr[0]);
            }
            return specializationList;
        }
        public static void cancelAppointment(int appointmentId)
        {
            con = getcon();
            cmd = new SqlCommand("update appointments set appointmentStatus = \'cancelled\' where appointmentId = "+appointmentId);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
        }

        public static bool checkAppointmentExists(int appointmentId)
        {
            con = getcon();
            cmd = new SqlCommand("select count(*) from appointments where appointmentId = " + appointmentId + " and appointmentStatus = \'booked\'");
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read() && Convert.ToInt32(dr[0]) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
