namespace ordination_test;

using shared.Model;

[TestClass]
public class OrdinationTest
{

    [TestMethod]
    public void AntalDageTest()
    {
        // Valid
        PN tc1 = new PN(new DateTime(2024, 11, 20), new DateTime(2024, 11, 23), 123, new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"));

        int antalDage_tc1 = tc1.antalDage();

        Assert.AreEqual(3, antalDage_tc1);

        PN tc2 = new PN(new DateTime(2024, 11, 20), new DateTime(2024, 11, 20), 123, new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"));

        int antalDage_tc2 = tc2.antalDage();

        Assert.AreEqual(0, antalDage_tc2);


        // Invalid
        PN tc3 = new PN(new DateTime(2024, 11, 23), new DateTime(2024, 11, 20), 123, new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml"));

        int antalDage_tc3 = tc3.antalDage();

        Assert.AreEqual(-1, antalDage_tc3);
    }
}