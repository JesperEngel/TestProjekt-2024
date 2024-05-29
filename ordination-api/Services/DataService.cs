using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using shared.Model;
using static shared.Util;
using Data;

namespace Service;

public class DataService
{
    private OrdinationContext db { get; }

    public DataService(OrdinationContext db) 
    {
        this.db = db;
    }

    /// <summary>
    /// Seeder noget nyt data i databasen, hvis det er nødvendigt.
    /// </summary>
    public void SeedData()
    {

        // Patients
        Patient[] patients = new Patient[5];
        patients[0] = db.Patienter.FirstOrDefault()!;

        if (patients[0] == null)
        {
            patients[0] = new Patient("250951-0515", "Luke Skywalker", 89.4);
            patients[1] = new Patient("130742-1153", "Han Solo", 83.2);
            patients[2] = new Patient("211053-0512", "Leia Organa", 63.4);
            patients[3] = new Patient("020414-1523", "Obi-Wan Kenobi", 59.9);
            patients[4] = new Patient("190481-1235", "Anakin Skywalker", 87.7);

            db.Patienter.Add(patients[0]);
            db.Patienter.Add(patients[1]);
            db.Patienter.Add(patients[2]);
            db.Patienter.Add(patients[3]);
            db.Patienter.Add(patients[4]);
            db.SaveChanges();
        }

        Laegemiddel[] laegemiddler = new Laegemiddel[5];
        laegemiddler[0] = db.Laegemiddler.FirstOrDefault()!;
        if (laegemiddler[0] == null)
        {
            laegemiddler[0] = new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk");
            laegemiddler[1] = new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml");
            laegemiddler[2] = new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk");
            laegemiddler[3] = new Laegemiddel("Methotrexat", 0.01, 0.015, 0.02, "Styk");
            laegemiddler[4] = new Laegemiddel("Prednisolon", 0.1, 0.15, 0.2, "Styk");

            db.Laegemiddler.Add(laegemiddler[0]);
            db.Laegemiddler.Add(laegemiddler[1]);
            db.Laegemiddler.Add(laegemiddler[2]);
            db.Laegemiddler.Add(laegemiddler[3]);
            db.Laegemiddler.Add(laegemiddler[4]);

            db.SaveChanges();
        }

        Ordination[] ordinationer = new Ordination[6];
        ordinationer[0] = db.Ordinationer.FirstOrDefault()!;
        if (ordinationer[0] == null) 
        {
            Laegemiddel[] lm = db.Laegemiddler.ToArray();
            Patient[] p = db.Patienter.ToArray();

            ordinationer[0] = new PN(new DateTime(2024, 1, 1), new DateTime(2024, 1, 12), 123, lm[1]);
            ordinationer[1] = new PN(new DateTime(2024, 2, 12), new DateTime(2024, 2, 14), 3, lm[0]);
            ordinationer[2] = new PN(new DateTime(2024, 1, 20), new DateTime(2024, 1, 25), 5, lm[2]);
            ordinationer[3] = new PN(new DateTime(2024, 1, 1), new DateTime(2024, 1, 12), 123, lm[1]);
            ordinationer[4] = new DagligFast(new DateTime(2024, 1, 10), new DateTime(2024, 1, 12), lm[1], 2, 0, 1, 0);
            ordinationer[5] = new DagligSkæv(new DateTime(2024, 1, 23), new DateTime(2024, 1, 24), lm[2]);

            ((DagligSkæv) ordinationer[5]).doser = new Dosis[] { 
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)        
            }.ToList();
            

            db.Ordinationer.Add(ordinationer[0]);
            db.Ordinationer.Add(ordinationer[1]);
            db.Ordinationer.Add(ordinationer[2]);
            db.Ordinationer.Add(ordinationer[3]);
            db.Ordinationer.Add(ordinationer[4]);
            db.Ordinationer.Add(ordinationer[5]);

            db.SaveChanges();

            p[0].ordinationer.Add(ordinationer[0]);
            p[0].ordinationer.Add(ordinationer[1]);
            p[2].ordinationer.Add(ordinationer[2]);
            p[3].ordinationer.Add(ordinationer[3]);
            p[1].ordinationer.Add(ordinationer[4]);
            p[1].ordinationer.Add(ordinationer[5]);

            db.SaveChanges();
        }
    }

    
    public List<PN> GetPNs() 
    {
        return db.PNs.Include(o => o.laegemiddel).Include(o => o.dates).ToList();
    }

    public List<DagligFast> GetDagligFaste()
    {
        return db.DagligFaste
            .Include(o => o.laegemiddel)
            .Include(o => o.MorgenDosis)
            .Include(o => o.MiddagDosis)
            .Include(o => o.AftenDosis)            
            .Include(o => o.NatDosis)            
            .ToList();
    }

    public List<DagligSkæv> GetDagligSkæve() 
    {
        return db.DagligSkæve
            .Include(o => o.laegemiddel)
            .Include(o => o.doser)
            .ToList();
    }

    public List<Patient> GetPatienter()
    {
        return db.Patienter.Include(p => p.ordinationer).ToList();
    }

    public List<Laegemiddel> GetLaegemidler()
    {
        return db.Laegemiddler.ToList();
    }

    public PN OpretPN(int patientId, int laegemiddelId, double antal, DateTime startDato, DateTime slutDato)
    {
        // TODO: Implement!
        Patient patient = db.Patienter.Find(patientId);
        Laegemiddel laegemiddel = db.Laegemiddler.Find(laegemiddelId);
        PN nyPN = new PN(startDato.Date, slutDato.Date, antal, laegemiddel);

        db.Ordinationer.Add(nyPN);
        patient.ordinationer.Add(nyPN);

        db.SaveChanges();

        return nyPN;
    }

    public DagligFast OpretDagligFast(int patientId, int laegemiddelId,
        double antalMorgen, double antalMiddag, double antalAften, double antalNat,
        DateTime startDato, DateTime slutDato)
    {
        // TODO: Implement!
        Patient patient = db.Patienter.Find(patientId);
        Laegemiddel laegemiddel = db.Laegemiddler.Find(laegemiddelId);
        DagligFast nyDagligFast = new DagligFast(startDato.Date, slutDato.Date, laegemiddel, antalMorgen, antalMiddag, antalAften, antalNat);

        db.Ordinationer.Add(nyDagligFast);
        patient.ordinationer.Add(nyDagligFast);

        db.SaveChanges();

        return nyDagligFast!;
    }

    public DagligSkæv OpretDagligSkaev(int patientId, int laegemiddelId, Dosis[] doser, DateTime startDato, DateTime slutDato)
    {
        // TODO: Implement!
        Patient patient = db.Patienter.Find(patientId);
        Laegemiddel laegemiddel = db.Laegemiddler.Find(laegemiddelId);
        DagligSkæv nyDagligSkæv = new DagligSkæv(startDato.Date, slutDato.Date, laegemiddel, doser);

        db.Ordinationer.Add(nyDagligSkæv);
        patient.ordinationer.Add(nyDagligSkæv);

        db.SaveChanges();

        return nyDagligSkæv!;
    }

    public string AnvendOrdination(int id, Dato dato)
    {
        // TODO: Implement!
        PN ordination = db.PNs.Find(id);

        if (ordination == null)
        {
            return "Ordination ikke fundet";
        }
        else if (ordination.givDosis(dato))
        {
            ordination.dates.Add(dato);
            db.SaveChanges();
            return "Ordination anvendt!";
        }
        else
        {
            return "Dato ikke accepteret!!";
        }
    }

    /// <summary>
    /// Den anbefalede dosis for den pågældende patient, per døgn, hvor der skal tages hensyn til
    /// patientens vægt. Enheden afhænger af lægemidlet. Patient og lægemiddel må ikke være null.
    /// </summary>
    /// <param name="patient"></param>
    /// <param name="laegemiddel"></param>
    /// <returns></returns>
    public double GetAnbefaletDosisPerDøgn(int patientId, int laegemiddelId)
    {
        // TODO: Implement!
        Patient patient = db.Patienter.Find(patientId);
        Laegemiddel laegemiddel = db.Laegemiddler.Find(laegemiddelId);

        if (patient.vaegt < 25)
        {
            return patient.vaegt * laegemiddel.enhedPrKgPrDoegnLet;
        }
        else if (patient.vaegt >= 25 && patient.vaegt <= 120)
        {
            return patient.vaegt * laegemiddel.enhedPrKgPrDoegnNormal;
        }
        else
        {
            return patient.vaegt * laegemiddel.enhedPrKgPrDoegnTung;
        }
    }

}