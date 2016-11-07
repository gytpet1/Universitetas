using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsolinis
{
    class Leidejas
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
    }
}
