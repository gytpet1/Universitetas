using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    /// <summary>
    /// Aprašo teksto fragmentą, jo ilgį bei į kurias teksto eilutes jis patenka.
    /// </summary>
    class TekstoFragmentas
    {
        public string Tekstas { get; set; }
        public int Ilgis { get; private set; }
        public List<int> Eilutes { get; set; }

        public TekstoFragmentas()
        {

        }

        public TekstoFragmentas(string tekstas)
        {
            Eilutes = new List<int>();
            Ilgis = 0;
            Tekstas = tekstas;
        }

        /// <summary>
        /// Skaičiuoja šio teksto fragmento ilgį.
        /// </summary>
        public void SkaiciuotiIlgi()
        {
            Ilgis = Tekstas.Length;
        }
    }
}
