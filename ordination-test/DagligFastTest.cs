namespace ordination_test;

using shared.Model;

[TestClass]
public class DagligFastTest
{

    [TestMethod]
    public void DoegnDosisTest()
    {
        // Valid        
        DagligFast tc1 = new DagligFast(new DateTime(2024, 1, 10), new DateTime(2024, 1, 12), new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 1, 0, 0, 0);

        double doegnDosis_tc1 = tc1.doegnDosis();

        Assert.AreEqual(1, doegnDosis_tc1);

        DagligFast tc2 = new DagligFast(new DateTime(2024, 1, 10), new DateTime(2024, 1, 12), new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), 0, 0, 0, 0);

        double doegnDosis_tc2 = tc2.doegnDosis();

        Assert.AreEqual(0, doegnDosis_tc2);


        // Invalid
        DagligFast tc3 = new DagligFast(new DateTime(2024, 1, 10), new DateTime(2024, 1, 12), new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"), -1, 1, 1, 1);

        double doegnDosis_tc3 = tc3.doegnDosis();

        Assert.AreEqual(-1, doegnDosis_tc3);
    }
}