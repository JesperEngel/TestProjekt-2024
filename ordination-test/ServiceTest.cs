namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void AnvendOrdinationTest()
    {
        PN test = service.GetPNs().Find(x => x.OrdinationId == 1);

        // Valid
        string tc1 = service.AnvendOrdination(1, new Dato { dato = test.startDen.Date });

        Assert.AreEqual("Ordination anvendt!", tc1);
        Assert.AreEqual(1, test.dates.Count());

        string tc2 = service.AnvendOrdination(1, new Dato { dato = test.startDen.Date.AddDays(1) });

        Assert.AreEqual("Ordination anvendt!", tc2);
        Assert.AreEqual(2, test.dates.Count());

        string tc3 = service.AnvendOrdination(1, new Dato { dato = test.slutDen.Date });

        Assert.AreEqual("Ordination anvendt!", tc3);
        Assert.AreEqual(3, test.dates.Count());

        string tc4 = service.AnvendOrdination(1, new Dato { dato = test.slutDen.Date.AddDays(-1) });

        Assert.AreEqual("Ordination anvendt!", tc4);
        Assert.AreEqual(4, test.dates.Count());

        string tc5 = service.AnvendOrdination(1, new Dato { dato = test.startDen.Date.AddDays(5) });

        Assert.AreEqual("Ordination anvendt!", tc5);
        Assert.AreEqual(5, test.dates.Count());


        // Invalid
        string tc6 = service.AnvendOrdination(-1, new Dato { dato = test.startDen.Date.AddDays(5) });

        Assert.AreEqual("Ordination ikke fundet", tc6);
        Assert.AreEqual(5, test.dates.Count());

        string tc7 = service.AnvendOrdination(1, new Dato { dato = test.slutDen.Date.AddDays(10) });

        Assert.AreEqual("Dato ikke accepteret!!", tc7);
        Assert.AreEqual(5, test.dates.Count());

        string tc8 = service.AnvendOrdination(1, new Dato { dato = test.slutDen.Date.AddDays(1) });

        Assert.AreEqual("Dato ikke accepteret!!", tc8);
        Assert.AreEqual(5, test.dates.Count());

        string tc9 = service.AnvendOrdination(-1, new Dato { dato = DateTime.Now.Date });

        Assert.AreEqual("Ordination ikke fundet", tc9);
        Assert.AreEqual(5, test.dates.Count());

    }
}