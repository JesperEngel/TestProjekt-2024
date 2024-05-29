namespace ordination_test;

using shared.Model;
using static shared.Util;

[TestClass]
public class DagligSkævTest
{

    [TestMethod]
    public void DoegnDosisTestTest()
    {
        // Valid
        DagligSkæv tc1 = new DagligSkæv(new DateTime(2024, 1, 23), new DateTime(2024, 1, 24), new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        tc1.doser = new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 5),
                new Dosis(CreateTimeOnly(16, 0, 0), 10)}
                .ToList();

        Assert.AreEqual(15, tc1.doegnDosis());

        DagligSkæv tc2 = new DagligSkæv(new DateTime(2024, 1, 23), new DateTime(2024, 1, 24), new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        tc2.doser = new Dosis[] {
                new Dosis(CreateTimeOnly(16, 0, 0), 0)}
                .ToList();

        Assert.AreEqual(0, tc2.doegnDosis());


        // Invalid
        DagligSkæv tc3 = new DagligSkæv(new DateTime(2024, 1, 23), new DateTime(2024, 1, 24), new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        tc3.doser = new Dosis[] {
                new Dosis(CreateTimeOnly(16, 0, 0), -1)}
                .ToList();

        Assert.AreEqual(0, tc3.doegnDosis());

        DagligSkæv tc4 = new DagligSkæv(new DateTime(2024, 1, 23), new DateTime(2024, 1, 24), new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        tc4.doser = new Dosis[] {
                new Dosis(CreateTimeOnly(16, 0, 0), -10)}
                .ToList();

        Assert.AreEqual(0, tc4.doegnDosis());
    }
}