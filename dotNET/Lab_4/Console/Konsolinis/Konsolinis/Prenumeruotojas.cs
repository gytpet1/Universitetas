using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsolinis
{
    public class Prenumeruotojas
    {
        public string Pavarde { get; set; }
        public string Adresas { get; set; }
        public int LaikotarpioPr { get; set; }
        public int LaikotarpioIlgis { get; set; }
        public string LeidinioKodas { get; set; }
        public int Kiekis { get; set; }

        public Prenumeruotojas()
        {
        }

        public Prenumeruotojas(string pavarde, string adresas, int laikotarpioPr,
            int laikotarpioIlgis, string leidinioKodas, int kiekis)
        {
            Pavarde = pavarde;
            Adresas = adresas;
            LaikotarpioPr = laikotarpioPr;
            LaikotarpioIlgis = laikotarpioIlgis;
            LeidinioKodas = leidinioKodas;
            Kiekis = kiekis;
        }

        // Metodas sudaro sąrašą prenumeravimo mėnesių
        public List<int> Menesiai()
        {
            List<int> men = new List<int>();
            for (int i = 0; i < LaikotarpioIlgis; i++)
            {
                int menesis = 0;
                if (men.Contains(12)) // Pereina i kitus metus
                    menesis = LaikotarpioPr + i - 12;
                else
                    menesis = LaikotarpioPr + i;
                men.Add(menesis);
            }
            return men;
        }

        public override string ToString()
        {
            return string.Format("{0,-15} | {1,-20} | Užsakyta: {2, -2}mėn. | Prenumeravimo trukmė: {3, -2}"
                + "| Leidinio kodas: {4,-5} | {5}vnt.", Pavarde, Adresas, 
                LaikotarpioPr, LaikotarpioIlgis, LeidinioKodas, Kiekis);
        }
    }
}
