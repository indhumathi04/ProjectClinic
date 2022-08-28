using clinicManagementRepo;
using NUnit.Framework;

namespace clinicProjectTest
{
    public class Tests
    {
        public static clinicRepo clinic ;
        [SetUp]
        public void Setup()
        {
            clinic =   new clinicRepo();
    }

        [Test]
        public void loginTestTrue()
        {
            bool login = clinic.login("staff01", "pass@staff");
            TestContext.WriteLine(login);
            Assert.IsTrue(login);
        }

        [Test]
        public void loginTestFalse()
        {
            Assert.Throws<Exception>(() => clinic.login("staff","pass"));
        }

        [Test]
        public void addPatientTest()
        {
            try
            {
                clinic.addPatient(new patient("Indhu", "Mathi", "female", 22, DateOnly.Parse("04 / 11 / 2000")));
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}