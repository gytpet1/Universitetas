using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public int PasirinktasMenuo = 1; // Globalus kintamsis, rodo vartotoju pasirinktą mėnesį
    const string filePathLeidiniai = (@"D:\Lab4\Data\Leidiniai\");
    const string filePathPrenumeratoriai = (@"D:\Lab4\Data\Prenumeratoriai\");
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // Iš sąrašo suranda pasirinktą mėnesį
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach(ListItem item in DropDownList1.Items)
            if (item.Selected == true)
                PasirinktasMenuo = int.Parse(item.Value);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string Rez = Server.MapPath(@"Data\Rezultatai.txt"); // Rezultatų failas

        if (File.Exists(Rez))
            File.Delete(Rez);

        List<Prenumeruotojas> prenumeruotojai = new List<Prenumeruotojas>(); // Visų prenumeruotojų sąrašas
        List<Leidinys> leidiniai = new List<Leidinys>(); // Visų leidinių sąrašas

        ReadPrenumeratorius(prenumeruotojai);
        ReadLeidinys(leidiniai);
        SpausdintiDuomenis<Prenumeruotojas>(Rez, prenumeruotojai, "Prenumeruotojai", 121);
        SpausdintiDuomenis<Leidinys>(Rez, leidiniai, "Leidiniai", 86);

        List<Leidejas> leidejai = SudarytiLeidejuSarasa(leidiniai); // Leidėjų sąrašas (įtraukiami jų leidiniai)
        PapildytiLeidejuSarasa(leidejai, prenumeruotojai); // Į leidėjų sąrašą pridedami juos prenumeravę klientai

        SpausdintiPajamasTamTikraMenesi(Rez, leidejai, PasirinktasMenuo);
        SpausdintiPajamas(Rez, "Visų mėnesių leidėjų pajamos:", leidejai);
        List<Leidejas> Surikiuoti = leidejai.OrderBy(nn => nn.PajamosTamTikraMenesi(PasirinktasMenuo)).ThenBy(nn => nn.Pavadinimas).ToList<Leidejas>();
        leidejai.Sort();
        SpausdintiPajamas(Rez, "Surikuotos visų mėnesių leidėjų pajamos:", Surikiuoti);
    }

    // Duomenų nuskaitymo metodai    
    public static void ReadLeidinys(List<Leidinys> L)
    {
        string[] filePaths = Directory.GetFiles(filePathLeidiniai, "*.txt");
        foreach (string path in filePaths)
        {
            string line = null;
            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1257)))
            {
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    string id = values[0];
                    string pavadinimas = values[1];
                    string leidejoPavadinimas = values[2];
                    double kaina = Convert.ToDouble(values[3]);
                    Leidinys temp = new Leidinys(id, pavadinimas, leidejoPavadinimas, kaina);
                    L.Add(temp);
                }
            }
        }
    }
    public static void ReadPrenumeratorius(List<Prenumeruotojas> P)
    {
        string[] filePaths = Directory.GetFiles(filePathPrenumeratoriai, "*.txt");
        foreach (string path in filePaths)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1257)))
            {
                string line = reader.ReadLine();
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    string pavarde = values[0];
                    string adresas = values[1];
                    int laikotarpioPradzia = Convert.ToInt32(values[2]);
                    int laikotarpioIlgis = Convert.ToInt32(values[3]);
                    string leidinioKodas = values[4];
                    int leidiniuKiekis = Convert.ToInt32(values[5]);
                    Prenumeruotojas temp = new Prenumeruotojas(pavarde,adresas,laikotarpioPradzia,laikotarpioIlgis,leidinioKodas, leidiniuKiekis);
                    P.Add(temp);
                }
            }
        }
    }    
    // Duomenų spausdinimo metodas
    static void SpausdintiDuomenis<Type>(string file, List<Type> sarasas, string antraste, int lentelesDydis)
    {
        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine(antraste);
            writer.WriteLine(new string('.', lentelesDydis));
            foreach (Type t in sarasas)
                writer.WriteLine("|" + t + "|");
            writer.WriteLine(new string('.', lentelesDydis));
            writer.WriteLine("\n");
        }
    }

    // Sudaromas leidejų sąrašas, pridedant leidinius
    static List<Leidejas> SudarytiLeidejuSarasa(List<Leidinys> leidiniai)
    {
        List<Leidejas> leidejai = new List<Leidejas>(); // Leidėjų sąrašas
        List<string> pavadinimai = new List<string>(); // Laikinas sąrašas, pagal kuri tikrinama ar prideti i sąrašą

        foreach (Leidinys leidinys in leidiniai)
        {
            if (!pavadinimai.Contains(leidinys.Leidejas))
            {
                pavadinimai.Add(leidinys.Leidejas);
                Leidejas leidejas = new Leidejas(leidinys.Leidejas);
                leidejas.Leidiniai.Add(leidinys);
                leidejai.Add(leidejas);
            }
            else // Jei leidėjas yra sąraše
            {
                foreach (Leidejas leidejas in leidejai) // Einama per esamą leidėjų sąrašą
                {
                    if (leidejas.Pavadinimas == leidinys.Leidejas) // Ir pridedamas naujas leidinys
                        leidejas.Leidiniai.Add(leidinys);
                }
            }
        }
        return leidejai;
    }

    // Sąrašas papildomas informacija apie prenumeruotojus
    static void PapildytiLeidejuSarasa(List<Leidejas> leidejai, List<Prenumeruotojas> prenumeruotojai)
    {
        foreach (Leidejas leidejas in leidejai) // Einama per visus leidėjus
            foreach (Leidinys leidinys in leidejas.Leidiniai) // Einama per leidėjų leidinius
                foreach (Prenumeruotojas prenumeruotojas in prenumeruotojai) // Einama per prenumeruotojų sąrašą
                    if (leidinys.LeidinioKodas == prenumeruotojas.LeidinioKodas) // Surandama kokį leidinį užsisakė
                        leidejas.Prenumeruotojai.Add(prenumeruotojas);
    }

    static void SpausdintiPajamasTamTikraMenesi(string file, List<Leidejas> leidejai, int menesis)
    {
        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine(menesis + " mėnesio leidėjų pajamos:");
            writer.WriteLine(new string('.', 29));
            writer.WriteLine("|    Leidėjas    |Uždirbo,eu|");
            foreach (Leidejas leidejas in leidejai)
            {
                writer.WriteLine("|{0,-15} | {1,9}|",
                    leidejas.Pavadinimas, leidejas.PajamosTamTikraMenesi(menesis));
            }
            writer.WriteLine(new string('.', 29));
            writer.WriteLine("\n");
        }
    }

    static void SpausdintiPajamas(string file, string antraste, List<Leidejas> leidejai)
    {
        using (StreamWriter writer = new StreamWriter(file, true))
        {
            writer.WriteLine(antraste);
            writer.WriteLine(new string('.', 38));
            foreach (Leidejas leidejas in leidejai)
            {
                writer.WriteLine("|{0,-15} | Uždirbo: {1,7}eu|", leidejas.Pavadinimas, leidejas.Pajamos());
                writer.WriteLine("|Leidiniai:                          |");
                foreach (Leidinys leidinys in leidejas.Leidiniai)
                    writer.WriteLine("|{0,-15} | Uždirbo: {1,7}eu|", leidinys.Pavadinimas, leidinys.Pajamos);
                writer.WriteLine(new string('.', 38));
            }
            writer.WriteLine("\n");
        }
    }
}