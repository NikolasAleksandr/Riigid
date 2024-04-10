
class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, string> riikPealinn, pealinnRiik;
        List<string> riigid;

        // Путь к файлу
        string failinimi = @"C:\Users\Kolaleila\source\repos\Riigid\Riigid\riigid_pealinnad.txt";

        FailistToDict(failinimi, out riikPealinn, out pealinnRiik, out riigid);

        Console.WriteLine("Sisestage riigi või pealinna nimi: ");
        string kasutajaSisend = Console.ReadLine();
        string tulemus = KuvaPealinnaVoiRiik(riikPealinn, kasutajaSisend);
        Console.WriteLine(tulemus);

        Console.WriteLine("Kas soovite kontrollida oma teadmisi? (Jah/Ei): ");
        string soovTestida = Console.ReadLine();
        if (soovTestida.ToLower() == "jah")
        {
            double tulemusProtsent = TestiTeadmised(riikPealinn);
            Console.WriteLine($"Teie tulemus: {tulemusProtsent}% õigetest vastustest.");
        }
    }

    static void FailistToDict(string failinimi, out Dictionary<string, string> riikPealinn, out Dictionary<string, string> pealinnRiik, out List<string> riigid)
    {
        riikPealinn = new Dictionary<string, string>();
        pealinnRiik = new Dictionary<string, string>();
        riigid = new List<string>();

        using (StreamReader sr = new StreamReader(failinimi))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('-');
                string riik = parts[0].Trim();
                string pealinn = parts[1].Trim();
                riikPealinn[riik] = pealinn;
                pealinnRiik[pealinn] = riik;
                riigid.Add(riik);
            }
        }
    }

    static string LisaSonastikku(Dictionary<string, string> sonastik, string voti, string vaartus)
    {
        sonastik[voti] = vaartus;
        return $"{voti} on lisatud sõnastikku. Nüüd on tema väärtus {vaartus}.";
    }

    static string MuudaSonastikku(Dictionary<string, string> sonastik, string voti, string vaartus)
    {
        sonastik[voti] = vaartus;
        return $"{voti} on muudetud. Nüüd on tema väärtus {vaartus}.";
    }

    static string KuvaPealinnaVoiRiik(Dictionary<string, string> sonastik, string sisendTekst)
    {
        sisendTekst = char.ToUpper(sisendTekst[0]) + sisendTekst.Substring(1);

        if (sonastik.ContainsKey(sisendTekst))
        {
            return $"{sisendTekst} pealinn: {sonastik[sisendTekst]}";
        }

        foreach (KeyValuePair<string, string> kvp in sonastik)
        {
            if (kvp.Value == sisendTekst)
            {
                return $"Riik pealinnaga {sisendTekst}: {kvp.Key}";
            }
        }

        Console.WriteLine($"{sisendTekst} puudub sõnastikust. Kas soovite lisada? (Jah/Ei): ");
        string lisaValik = Console.ReadLine();
        if (lisaValik.ToLower() == "jah")
        {
            Console.WriteLine($"Sisestage {sisendTekst} pealinn või riik: ");
            string uusVaartus = Console.ReadLine();
            return LisaSonastikku(sonastik, sisendTekst, uusVaartus);
        }
        else
        {
            Console.WriteLine($"{sisendTekst} puudub sõnastikust. Kas soovite seda muuta? (Jah/Ei): ");
            string muudaValik = Console.ReadLine();
            if (muudaValik.ToLower() == "jah")
            {
                Console.WriteLine($"Sisestage uus väärtus {sisendTekst} pealinnale või riigile: ");
                string uusVaartus = Console.ReadLine();
                return MuudaSonastikku(sonastik, sisendTekst, uusVaartus);
            }
            else
            {
                return "Tegevus tühistatud.";
            }
        }
    }

    static bool OigeVastus(string antudVastus, string oodatavVastus)
    {
        return antudVastus.ToLower() == oodatavVastus.ToLower();
    }

    static double TestiTeadmised(Dictionary<string, string> sonastik)
    {
        int oigedVastused = 0;
        int kusimused = 0;

        Random rand = new Random();
        List<string> keys = new List<string>(sonastik.Keys);
        for (int i = 0; i < 6; i++)
        {
            string suvalineSona = keys[rand.Next(keys.Count)];
            string oigeVaartus = sonastik[suvalineSona];

            Console.WriteLine($"Mis on pealinn riigile '{suvalineSona}'? ");
            string kysimus = Console.ReadLine();

            if (OigeVastus(kysimus, oigeVaartus))
            {
                Console.WriteLine("Õige!");
                oigedVastused++;
            }
            else
            {
                Console.WriteLine("Vale!");
            }
            kusimused++;
        }

        double protsent = (double)oigedVastused / kusimused * 100;
        return protsent;
    }
}

