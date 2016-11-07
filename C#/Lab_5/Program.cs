using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lab5
{
    class Program
    {
        const string CFd = "Knyga.txt";
        const string CFa = "Analizė.txt";
        const string CFr = "ManoKnyga.txt";


        static void Main(string[] args)
        {
            if (File.Exists(CFa))
                File.Delete(CFa);
            if (File.Exists(CFr))
                File.Delete(CFr);

            Dictionary<int, List<string>> zodziai = new Dictionary<int, List<string>>();
            List<TekstoFragmentas> tekstoFragmentai = new List<TekstoFragmentas>();
            List<string> tZodziai = new List<string>(); //Trumpiausi zodziai
            string[] lines;
            char[] skyrikliai = { ' ', ',', '.', ';', '-', '!', '?', ':' };
            TekstoApdorojimas(skyrikliai, ref tekstoFragmentai, ref tZodziai, out lines, ref zodziai);
            IsvestiIlgiausiaGrandine(tekstoFragmentai);
            IsvestiTrumpiausiusZodzius(tZodziai, lines, skyrikliai);
            ManoKnyga(zodziai, lines, skyrikliai);
        }

        /// <summary>
        /// Nuskaito duotą tekstą ir pradeda jį apdoroti (žiūrėti į metodą ApdorotiEilute).
        /// </summary>
        /// <param name="skyrikliai">Masyvas su visais tekste naudojamais skyrikliais.</param>
        /// <param name="tekstoFragmentai">Masyvas kuriame saugomos žodžių grandinės.</param>
        /// <param name="tZodziai">Masyvas kuriame bus saugomi trumpiausi teksto žodžiai.</param>
        /// <param name="lines">Masyvas kuriame laikomas visas nuskaitytas tekstas.</param>
        /// <param name="zodziai">Mąsyvas su visų eilučių žodžiais kartu su jiems priklausiančiais skyrikliais</param>
        public static void TekstoApdorojimas(char[] skyrikliai, ref List<TekstoFragmentas> tekstoFragmentai, ref List<string> tZodziai, out string[] lines,
            ref Dictionary<int, List<string>> zodziai)
        {
            lines = File.ReadAllLines(CFd, Encoding.GetEncoding(1257));
            bool newStreak = false;
            bool streak = false;
            bool sorted = false;
            char last = ' ';
            int eilNr = 0;

            foreach (string line in lines)
            {
                eilNr++;
                ApdorotiEilute(line, ref tZodziai, skyrikliai, ref last, ref tekstoFragmentai, ref sorted, ref newStreak, eilNr, ref zodziai, ref streak);
            }

        }

        /// <summary>
        /// Nagrinėja vieną eilutę: ieško trumpiausių žodžių, ieško žodžių grandinių, saugo duomenis reikalingus tolesniems skaičiavimams.
        /// </summary>
        /// <param name="line">Nagrinėjama teksto eilutė.</param>
        /// <param name="tZodziai">Masyvas kuriame bus saugomi trumpiausi teksto žodžiai.</param>
        /// <param name="skyrikliai">Masyvas su visais tekste naudojamais skyrikliais.</param>
        /// <param name="streak">True - šiuo metu nagrinėjamas žodis yra žodžių grandinės dalis, False - priešingybė. Reikalinga kai grandinė 
        /// persikelia per kelias eilutes.</param>
        /// <param name="last">Paskutinis paskutinio eilutės žodžio simbolis, reikalinga kai grandinė persikelia į kitą eilutę.</param>
        /// <param name="tekstoFragmentai">Masyvas kuriame saugomos žodžių grandinės.</param>
        /// <param name="sorted">Tikrina ar trumpiausių žodžių masyvas jau surikiuotas.</param>
        /// <param name="newStreak">Rodo ar eilutės pabaiga - pradžia sukurs naują grandinę ar tęs jau esamą.</param>
        /// <param name="eilNr">Nurodo dabar nagrinėjamos eilutės numerį.</param>
        /// <param name="words">Mąsyvas su visų eilučių žodžiais kartu su jiems priklausiančiais skyrikliais</param>
        public static void ApdorotiEilute(string line, ref List<string> tZodziai, char[] skyrikliai, ref char last, ref List<TekstoFragmentas> tekstoFragmentai,
            ref bool sorted, ref bool newStreak, int eilNr, ref Dictionary<int, List<string>> words, ref bool streak)
        {
            string[] zodziai = line.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
            List<string> zodziaiSuSkyrikliais = new List<string>();
            int index1 = 0;
            int index2 = 0;
            string zodis1 = "";
            string zodis2 = "";
            int index3 = 0;
            int index4 = 0;

            using (StreamWriter fa = File.AppendText(CFa))
            {
                for (int i = 0; i < zodziai.Count(); i++)
                {
                    if (i != zodziai.Count() - 1)
                    {
                        index3 = line.IndexOf(zodziai[i], index4);
                        index4 = line.IndexOf(zodziai[i + 1], index3 + zodziai[i].Length);
                        zodziaiSuSkyrikliais.Add(line.Substring(index3, index4 - index3));
                    }
                    else
                    {
                        index3 = line.LastIndexOf(zodziai[i]);
                        zodziaiSuSkyrikliais.Add(line.Substring(index3, line.Length - index3));
                    }

                    if (zodziai[i].Length >= 3)                         //Ieško trumpiausių žodžių
                    {
                        if (tZodziai.Count() < 10)
                        {
                            tZodziai.Add(zodziai[i]);
                            fa.WriteLine(zodziai[i]);
                        }
                        else if (zodziai.Length < tZodziai[9].Length)
                        {
                            IterptiZodi(ref tZodziai, zodziai[i]);
                            fa.WriteLine(zodziai[i]);
                        }

                        if (tZodziai.Count() == 10 && !sorted)
                        {
                            tZodziai = RusiuotiZodzius(tZodziai);
                            sorted = true;
                        }
                    }

                    //Seka žodžių grandinę
                    zodis1 = zodziai[i].ToLower();
                    if (i != zodziai.Count() - 1)
                        zodis2 = zodziai[i + 1].ToLower();

                    if (i == 0 && newStreak && last == zodis1[0])
                    {
                        index1 = 0;
                        if (zodis1[zodziai[i].Length - 1] == zodis2[0])
                            index2 = line.IndexOf(zodziai[i + 1]) + zodziai[i + 1].Length;
                        else
                            index2 = zodziai[i].Length - 1;
                        tekstoFragmentai[tekstoFragmentai.Count() - 1].Tekstas += " " + line.Substring(index1, index2 - index1);
                        tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Add(eilNr);
                        fa.WriteLine(line.Substring(index1, index2 - index1));
                        index2 = index1;
                        streak = true;
                    }
                    else
                        if (i == 0 && !newStreak && zodis1[zodziai[i].Length - 1] == zodis2[0])
                    {
                        index1 = 0;
                        index2 = line.IndexOf(zodziai[i + 1]) + zodziai[i + 1].Length;
                        tekstoFragmentai.Add(new TekstoFragmentas(line.Substring(index1, index2 - index1)));
                        tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Add(eilNr);
                        fa.WriteLine("\n{0}", line.Substring(index1, index2 - index1));
                    }
                    else
                        if (i == zodziai.Count() - 1)
                    {
                        if (!streak)
                        {
                            tekstoFragmentai.Add(new TekstoFragmentas(line.Substring(line.LastIndexOf(zodziai[i]), line.Length - line.LastIndexOf(zodziai[i]))));
                            tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Add(eilNr);
                            fa.WriteLine(line.Substring(line.LastIndexOf(zodziai[i]), line.Length - line.LastIndexOf(zodziai[i])));
                        }
                        newStreak = true;
                        last = zodziai[i][zodziai[i].Length - 1];
                    }
                    else
                        if (zodis1[zodziai[i].Length - 1] == zodis2[0])
                    {
                        index1 = line.IndexOf(zodziai[i], index2) + zodziai[i].Length;
                        index2 = line.IndexOf(zodziai[i + 1], index1) + zodziai[i + 1].Length;
                        if (streak)
                        {
                            tekstoFragmentai[tekstoFragmentai.Count() - 1].Tekstas += line.Substring(index1, index2 - index1);
                            if (!tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Contains(eilNr))
                                tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Add(eilNr);
                            fa.WriteLine(line.Substring(index1, index2 - index1));
                        }
                        else
                        {
                            index1 -= zodziai[i].Length;
                            tekstoFragmentai.Add(new TekstoFragmentas(line.Substring(index1, index2 - index1)));
                            tekstoFragmentai[tekstoFragmentai.Count() - 1].Eilutes.Add(eilNr);
                            fa.WriteLine(line.Substring(index1, index2 - index1));
                        }
                    }
                    else
                        streak = false;
                }

            }
            words.Add(eilNr, zodziaiSuSkyrikliais);
        }

        /// <summary>
        /// Rikiuoja žodžius pagal jų ilgumą, naudojant išrinkimo rikiavimo algoritmą.
        /// </summary>
        /// <param name="tZodziai">Masyvas kuris bus rikiuojamas</param>
        /// <returns>Grąžinamas surikiuotas masyvas.</returns>
        public static List<string> RusiuotiZodzius(List<string> tZodziai)
        {
            int k;
            string papildomas;
            for (int i = 0; i < tZodziai.Count() - 1; i++)
            {
                k = i;
                for (int n = i + 1; n < tZodziai.Count(); n++)
                {
                    if (tZodziai[n].Length < tZodziai[k].Length)
                        k = n;
                }
                papildomas = tZodziai[i];
                tZodziai[i] = tZodziai[k];
                tZodziai[k] = papildomas;
            }

            return tZodziai;
        }

        /// <summary>
        /// Įterpia duotą žodį į rikiuotą masyvą.
        /// </summary>
        /// <param name="tZodziai">Rikiuotas masyvas kuriame bus įterpiamas žodis</param>
        /// <param name="iZodis">Įterpiamas žodis.</param>
        public static void IterptiZodi(ref List<string> tZodziai, string iZodis)
        {
            int i = 0;
            while (i < tZodziai.Count() && iZodis.Length > tZodziai[i].Length)
                i++;

            tZodziai.Insert(i, iZodis);
        }

        /// <summary>
        /// Į konsolę spausdina informaciją apie ilgiausią žodžių grandinę duotame tekste.
        /// </summary>
        /// <param name="tekstoFragmentai">Žodžių grandinių masyvas.</param>
        public static void IsvestiIlgiausiaGrandine(List<TekstoFragmentas> tekstoFragmentai)
        {
            int n = IlgiausiaZodziuGrandine(tekstoFragmentai);
            Console.WriteLine("Ilgiausia žodžių grandinė:\n{0}", tekstoFragmentai[n].Tekstas);
            Console.WriteLine("Ši grandinė išsidėsčiusi šiose eilutėse: ");
            foreach (int i in tekstoFragmentai[n].Eilutes)
                Console.Write("{0} ", i);
        }

        /// <summary>
        /// Žodžių grandinių masyve ieško ilgiausios žodžių grandinės indekso.
        /// </summary>
        /// <param name="tekstoFragmentai">Žodžių grandinių masyvas kuriame bus ieškoma ilgiausia.</param>
        /// <returns>Ilgiausios žodžių grandinės indeksas masyve "tekstoFragmentai"</returns>
        public static int IlgiausiaZodziuGrandine(List<TekstoFragmentas> tekstoFragmentai)
        {
            foreach (TekstoFragmentas tF in tekstoFragmentai)
                tF.SkaiciuotiIlgi();

            int n = 0;
            for (int i = 0; i < tekstoFragmentai.Count(); i++)
            {
                if (tekstoFragmentai[n].Ilgis <= tekstoFragmentai[i].Ilgis)
                    n = i;
            }

            return n;
        }

        /// <summary>
        /// Kreipiasi į metodą ieškantį žodžių pasikartojimų dažnio ir spausdina rezultatus į konsolę.
        /// </summary>
        /// <param name="tZodziai">Trumpiausi žodžiai kurių pasikartojimo dažnis yra ieškomas</param>
        /// <param name="lines">Tekstas kuriame ieškomi žodžiai</param>
        /// <param name="skyrikliai">Skyriklių masyvas</param>
        public static void IsvestiTrumpiausiusZodzius(List<string> tZodziai, string[] lines, char[] skyrikliai)
        {
            int[] zPasikartojimai = ZodziuPasikartojimai(tZodziai, lines, skyrikliai);
            Console.WriteLine("\n\nTrumpiausi žodžiai:\n" +
                              "-----------------------------------------\n" +
                              "    Žodis        Pasikartojimų skaičius  \n" +
                              "-----------------------------------------");
            for (int i = 0; i < 10; i++)
                Console.WriteLine("{0,-17}{1:d}", tZodziai[i], zPasikartojimai[i]);

            Console.WriteLine("-----------------------------------------");

        }

        /// <summary>
        /// Suskaičiuoja kiek kartų pasikartoja kiekvienas duotas žodis duotame tekste.
        /// </summary>
        /// <param name="tZodziai">Trumpiausi žodžiai kurių pasikartojimo dažnis yra ieškomas</param>
        /// <param name="lines">Tekstas kuriame ieškomi žodžiai</param>
        /// <param name="skyrikliai">Skyriklių masyvas</param>
        /// <returns>Grąžina sveikųjų skaičių masyva kurio indeksai sutampa su atitinkamų žodžių indeksais masyve "tZodziai"</returns>
        public static int[] ZodziuPasikartojimai(List<string> tZodziai, string[] lines, char[] skyrikliai)
        {
            int[] zPasikartojimai = new int[10];
            for (int i = 0; i < 10; i++)
                zPasikartojimai[i] = 0;
            tZodziai.RemoveRange(10, tZodziai.Count() - 10);

            foreach (string line in lines)
            {
                string[] words = line.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    for (int i = 0; i < tZodziai.Count(); i++)
                    {
                        if (tZodziai[i] == word)
                            zPasikartojimai[i]++;
                    }
                }
            }

            return zPasikartojimai;
        }

        /// <summary>
        /// Kreipiasi į metodus kurie sutvarko tekstą failui ManoKnyga.txt, ir spausdina rezultatus.
        /// </summary>
        /// <param name="words">Mąsyvas su visų eilučių žodžiais kartu su jiems priklausiančiais skyrikliais</param>
        /// <param name="lines">Masyvas su visais neapdorotais duomenimis</param>
        /// <param name="skyrikliai">Skyriklių masyvas</param>
        public static void ManoKnyga(Dictionary<int, List<string>> words, string[] lines, char[] skyrikliai)
        {
            List<string> eilutes = TekstoLygiavimas(words, lines, skyrikliai);
            using (StreamWriter fr = File.AppendText(CFr))
            {
                foreach (string line in eilutes)
                    fr.WriteLine(line);
            }
        }

        /// <summary>
        /// Sutvarko ir išlygiuoja tekstą
        /// </summary>
        /// <param name="words">Mąsyvas su visų eilučių žodžiais kartu su jiems priklausiančiais skyrikliais</param>
        /// <param name="lines">Masyvas su visais neapdorotais duomenimis</param>
        /// <param name="skyrikliai">Skyriklių masyvas</param>
        /// <returns>Grąžina masyva su sulygiuotomis eilutėmis</returns>
        public static List<string> TekstoLygiavimas(Dictionary<int, List<string>> words, string[] lines, char[] skyrikliai)
        {
            List<string> eilutes = new List<string>();
            List<string> zodziai = lines[0].Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries).ToList();   //Kiekvienos pozicijos ilgiausias zodis nesikartojanciais skyrikliais
            string zodis = ""; //Papildomas
            int n;  //Papildomas
            string line = "";
            List<int> pozicijos = new List<int>();

            //Suranda ilgiausią žodį kiekvienai pozicijai
            foreach (int eilute in words.Keys.ToList())
            {
                for (int i = 0; i < words[eilute].Count(); i++)
                {
                    zodis = words[eilute][i];
                    n = zodis.Length - 1;
                    while (skyrikliai.Contains(zodis[n]) && n >= 0)
                    {
                        if (zodis[n - 1] == zodis[n])
                        {
                            zodis = zodis.Remove(n, 1);
                        }
                        n--;
                    }
                    if (i <= zodziai.Count() - 1)
                    {
                        if (zodis.Length > zodziai[i].Length)
                            zodziai[i] = zodis;
                    }
                    else
                        zodziai.Add(zodis);
                }
            }

            pozicijos.Add(1);   //Pirma pozicija duota sąlygoje
            foreach (string word in zodziai)    //Skaičiuoja santykines pozicijas
            {
                line = line + word + "  ";
                pozicijos.Add(word.Length + 2);
            }

            //Eilučių formatavimas
            foreach (int eilute in words.Keys.ToList())
            {
                line = "";
                for (int i = 0; i < words[eilute].Count(); i++)
                {
                    zodis = words[eilute][i];
                    if (zodis.Length > pozicijos[i + 1] - 2)
                    {
                        n = zodis.Length - 1;
                        //Naikina nereikalingus skyriklius
                        while (skyrikliai.Contains(zodis[n]) && zodis.Length > pozicijos[i + 1] - 2)
                        {
                            if (zodis[n - 1] == zodis[n])
                            {
                                zodis = zodis.Remove(n, 1);
                            }
                            n--;
                        }
                        if (zodis.Length == pozicijos[i + 1] - 2)
                            line += zodis;
                    }
                    else
                        if (zodis.Length < pozicijos[i + 1] - 2)
                    {
                        n = pozicijos[i + 1] - zodis.Length - 2;
                        string tarpai = new string(' ', n);
                        line += zodis + tarpai;
                    }
                    else
                        if (zodis.Length == pozicijos[i + 1] - 2)
                        line += zodis;
                }
                eilutes.Add(line);
            }

            return eilutes;
        }
    }
}
