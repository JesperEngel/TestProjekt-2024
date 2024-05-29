namespace ordination_test;

using shared.Model;

[TestClass]
public class PatientTest
{

    [TestMethod]
    public void PatientHasName()
    {
        string cpr = "010735-1234";
        string navn = "Darth Vader";
        double vægt = 89;

        Patient patient = new Patient(cpr, navn, vægt);
        Assert.AreEqual(navn, patient.navn);
    }

}