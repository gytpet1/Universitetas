using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//Pajamos.Pirmoje failo eilutėje nurodyta įvedimo data, o tolesnėse eilutėse nurodyta
//prenumeratoriaus pavardė, adresas, laikotarpio pradžia(nurodyta sveiku skaičiumi 1..12), laikotarpio
//ilgis, leidinio kodas, leidinių kiekis.Kitame faile duota tokia informacija apie leidinius: kodas,
//pavadinimas, leidėjo pavadinimas, vieno mėnesio kaina.Suskaičiuoti kiekvienam leidėjui nurodyto
//mėnesio (įvedama klaviatūra) pajamas.Atspausdinkite leidėjų pajamas, surikiuotas pagal dydį ir
//leidėjų pavadinimus, nurodant ir leidėjų leidinius su jų atneštomis pajamomis.Leidėjų pavadinimai
//neturi kartotis.

namespace Konsolinis
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Prenumeruotojas> prenumeruotojai = new List<Prenumeruotojas>();
            List<Leidinys> leidiniai = new List<Leidinys>();

            SkaitytiPrenumeratorius("D1.txt", prenumeruotojai);
            SkaitytiLeidinius("D2.txt", leidiniai);

            SpausdintiDuomenis<Prenumeruotojas>(prenumeruotojai, "Prenumeruotojai", 117);
            SpausdintiDuomenis<Leidinys>(leidiniai, "Leidiniai", 84);

            List<Leidejas> leidejai = SudarytiLeidejuSarasa(leidiniai);
            PapildytiLeidejuSarasa(leidejai, prenumeruotojai);

            Console.WriteLine("Nurodykite mėnesį:");
            int menesis = int.Parse(Console.ReadLine()); // Try
            SpausdintiPajamasTamTikraMenesi(leidejai, menesis);
            SpausdintiPajamas(leidejai);
            Console.ReadLine();
        }

        // Duomenų nuskaitymo metodai
        static void SkaitytiPrenumeratorius(string file, List<Prenumeruotojas> prenumeruotojai)
        {
            string line;
            using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding(1257)))
            {
                line = reader.ReadLine();
                while(null != (line = reader.ReadLine()))
                {
                    string[] parts = line.Split(';');
                    Prenumeruotojas p = new Prenumeruotojas(parts[0], parts[1], int.Parse(parts[2]),
                        int.Parse(parts[3]), parts[4], int.Parse(parts[5]));
                    prenumeruotojai.Add(p);
                }
            }
        }

        static void SkaitytiLeidinius(string file, List<Leidinys> leidiniai)
        {
            string line;
            using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding(1257)))
            {
                while (null != (line = reader.ReadLine()))
                {
                    string[] parts = line.Split(';');
                    Leidinys l = new Leidinys(parts[0], parts[1], parts[2], double.Parse(parts[3]));
                    leidiniai.Add(l);
                }
            }
        }

        // Duomenų spausdinimo metodas
        static void SpausdintiDuomenis<Type>(List<Type> sarasas, string antraste, int lentelesDydis)
        {
            Console.WriteLine(antraste);
            Console.WriteLine(new string('-', lentelesDydis));
            foreach (Type t in sarasas)
                Console.WriteLine("|" + t + "|");
            Console.WriteLine(new string('-', lentelesDydis));
            Console.WriteLine("\n");
        }

        // Sudaromas leidejų sąrašas, pridedant leidinius
        static List<Leidejas> SudarytiLeidejuSarasa(List<Leidinys> leidiniai)
        {
            List<Leidejas> leidejai = new List<Leidejas>(); // Leidėjų sąrašas
            List<string> pavadinimai = new List<string>(); // Laikinas sąrašas, pagal kuri tikrinama ar prideti i sąrašą

            foreach(Leidinys l in leidiniai)
            {
                if (!pavadinimai.Contains(l.Leidejas))
                {
                    pavadinimai.Add(l.Leidejas);
                    Leidejas leidejas = new Leidejas(l.Leidejas);
                    leidejas.Leidiniai.Add(l);
                    leidejai.Add(leidejas);
                }
                else // Jei leidėjas yra sąraše
                {
                    foreach(Leidejas leidejas in leidejai) // Einama per esamą leidėjų sąrašą
                    {
                        if (leidejas.Pavadinimas == l.Leidejas) // Ir pridedamas naujas leidinys
                            leidejas.Leidiniai.Add(l);
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

        static void SpausdintiPajamasTamTikraMenesi(List<Leidejas> leidejai, int menesis)
        {
            Console.WriteLine(menesis + " mėnesio leidėjų pajamos:");
            Console.WriteLine(new string('-', 44));
            foreach (Leidejas leidejas in leidejai)
            {
                Console.WriteLine("|Leidėjas: {0,-15} | Pajamos: {1,5}|", 
                    leidejas.Pavadinimas, leidejas.PajamosTamTikraMenesi(menesis));
            }
            Console.WriteLine(new string('-', 44));
            Console.WriteLine("\n");
        }

        static void SpausdintiPajamas(List<Leidejas> leidejai)
        {
            Console.WriteLine("Visų mėnesių leidėjų pajamos:");
            Console.WriteLine(new string('-', 44));
            foreach (Leidejas leidejas in leidejai)
            {
                Console.WriteLine("|Leidėjas: {0,-15} | Pajamos: {1,5}|",
                    leidejas.Pavadinimas, leidejas.Pajamos());
            }
            Console.WriteLine(new string('-', 44));
        }
    }
}
