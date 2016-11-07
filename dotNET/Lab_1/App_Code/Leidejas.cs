using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Leidinių konteinerinė klasė (kartu su prenuomeruotojų duomenimis)
/// </summary>
public class Leidejas : IComparable<Leidejas>
{
    public string Pavadinimas { get; set; }
    public List<Leidinys> Leidiniai { get; set; }
    public List<Prenumeruotojas> Prenumeruotojai { get; set; }

    public Leidejas()
    {
        Leidiniai = new List<Leidinys>();
        Prenumeruotojai = new List<Prenumeruotojas>();
    }

    public Leidejas(string pavadinimas)
    {
        Pavadinimas = pavadinimas;
        Leidiniai = new List<Leidinys>();
        Prenumeruotojai = new List<Prenumeruotojas>();
    }

    public double PajamosTamTikraMenesi(int menesis)
    {
        double suma = 0;

        foreach (Prenumeruotojas p in Prenumeruotojai)
            foreach (int men in p.Menesiai())
                if (men == menesis)
                    foreach (Leidinys l in Leidiniai)
                        if (l.LeidinioKodas == p.LeidinioKodas)
                            suma += l.Kaina * p.Kiekis;
        return suma;
    }

    public double Pajamos()
    {
        double suma = 0; // Leidėjo suma

        foreach (Leidinys l in Leidiniai)
        {
            double sumaLeidinio = 0;
            foreach (Prenumeruotojas p in Prenumeruotojai)
            {
                if (l.LeidinioKodas == p.LeidinioKodas)
                {
                    sumaLeidinio += l.Kaina * p.Kiekis * p.LaikotarpioIlgis;
                    suma += l.Kaina * p.Kiekis * p.LaikotarpioIlgis;
                }
            }
            l.Pajamos = sumaLeidinio;
        }
        return suma;
    }
    public double LeidinioPajamos()
    {
        double sumaLeidinio = 0; // Leidėjo suma

        foreach (Leidinys l in Leidiniai)
        {
            foreach (Prenumeruotojas p in Prenumeruotojai)
            {
                if (l.LeidinioKodas == p.LeidinioKodas)
                {
                    sumaLeidinio += l.Kaina * p.Kiekis * p.LaikotarpioIlgis;
                }
            }
            l.Pajamos = sumaLeidinio;
        }
        return sumaLeidinio;
    }

    // Palyginimo metodas (IComparable realizavimui)
    public int CompareTo(Leidejas other)
    {
        if (other == null)
            return 1;
        if (Pavadinimas.CompareTo(other.Pavadinimas) != 0)
            return Pavadinimas.CompareTo(other.Pavadinimas);
        else
            return Pajamos().CompareTo(other.Pajamos());
    }
}