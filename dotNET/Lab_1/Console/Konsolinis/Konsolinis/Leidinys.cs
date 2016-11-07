using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsolinis
{
    public class Leidinys : IComparable<Leidinys>, IEquatable<Leidinys>
    {
        public string LeidinioKodas { get; set; }
        public string Pavadinimas { get; set; }
        public string Leidejas { get; set; }
        public double Kaina { get; set; }
        public double Pajamos { get; set; }

        public Leidinys()
        {
        }

        public Leidinys(string leidinioKodas, string pavadinimas, string leidejas, 
            double kaina)
        {
            LeidinioKodas = leidinioKodas;
            Pavadinimas = pavadinimas;
            Leidejas = leidejas;
            Kaina = kaina;
        }

        public override string ToString()
        {
            return string.Format("{0,-15} | Leidėjas: {1,-15} | Leidinio kodas: {2,-5} | Kaina: {3,5}", Pavadinimas, Leidejas, LeidinioKodas, Kaina);
        }

        public int CompareTo(Leidinys other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Leidinys other)
        {
            throw new NotImplementedException();
        }
    }
}
