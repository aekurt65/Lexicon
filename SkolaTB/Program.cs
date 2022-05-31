using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SkolaTB {
    class Program {
        static void Main(string[] args) {
            Workers workers = new Workers();
            InputHelpers ih = new InputHelpers();

            while (true) {
                Console.WriteLine();
                int number = ih.readInt("Ange ett tal 1-16, eller 0 för att avsluta, 99 för hjälp");

                workers.writeHeader(number);

                string fName = String.Format("f{0}", number);
                MethodInfo fn = typeof(Workers).GetMethod(fName);
                if(fn == null) {
                    Console.Write("Funktionen {0} finns inte, försök med ett annat tal", number);
                    continue;
                }

                fn.Invoke(workers, new object[] { });
            }
        }

        private class Workers {
            private Random rnd = new Random();
            private InputHelpers ih = new InputHelpers();
            private ArrayHelpers ah = new ArrayHelpers();
            private TextRepo t = new TextRepo();

            private readonly Dictionary<int, string> help = new Dictionary<int, string> {
                {0, "Hejdå!" },
                {1, "Heja världen!" },
                {2, "Vem är du?" },
                {3, "Färg-TV" },
                {4, "Dagens datum" },
                {5, "5 myror är fler än.." },
                {6, "Gissa talet" },
                {7, "Spara text" },
                {8, "Hämta sparad text" },
                {9, "Matte B" },
                {10, "Multiplikationstabell 1-10" },
                {11, "Sortera får ej användas" },
                {12, "Palindrom" },
                {13, "2 siffror blir många" },
                {14, "Ännu sorterare" },
                {15, "Summera tal" },
                {16, "Ett klassiskt spel" },
                {99, "Hjälptext" },
            };

            public void writeHeader(int ix) {
                if (help.ContainsKey(ix)) {
                    Console.WriteLine();
                    Console.WriteLine(help[ix]);
                    Console.WriteLine(new string('=', help[ix].Length));
                    Console.WriteLine();
                }
            }

            public void f99() {
                Console.WriteLine();
                foreach (int key in help.Keys) {
                    Console.WriteLine("{0}\t{1}", key, help[key]);
                }
                Console.WriteLine();
            }

            /// <summary>
            /// Exit the program
            /// </summary>
            public void f0() {
                System.Threading.Thread.Sleep(700);
                Environment.Exit(0);
            }

            /// <summary>
            /// 1. Funktion som skriver ut ”Hello World” i konsolen
            /// </summary>
            public void f1() {
                Console.WriteLine("Hello World!");
            }

            /// <summary>
            /// 2. Funktion som tar in input från användaren 
            /// (Förnamn, Efternamn, Ålder)
            /// och sedan skriver ut dessa i konsolen
            /// </summary>
            public void f2() {
                string txtPrename = ih.readString("Ange Förnamn");
                string txtSurname = ih.readString("Ange Efternamn");
                int intAge = ih.readInt("Ange Ålder (år som helt tal)");

                var sb = new StringBuilder();
                sb.AppendFormat("Förnamn:   {0}\n", txtPrename);
                sb.AppendFormat("Efternamn: {0}\n", txtSurname);
                sb.AppendFormat("Ålder:     {0} år\n", intAge);
                string message = String.Format("Dina uppgifter är:\n{0}", sb.ToString());

                Console.WriteLine();
                Console.WriteLine(message);
            }

            private bool isCustomColor = false;

            /// <summary>
            /// 3. Funktion som ändrar färgen på texten i konsolen (och ändrar tillbaka
            /// om man använder funktionen igen
            /// </summary>
            public void f3() {
                if (isCustomColor) {
                    Console.ResetColor();
                    isCustomColor = false;
                    Console.WriteLine("Tråkigt att du inte gillade färgen, men då ändrar vi tillbaka!");
                } else {
                    // There are 16 possible colors, however
                    // some colors are boring or give unreadable text, so we
                    // choose from a subset of decent colors
                    ConsoleColor[] decentColors = new ConsoleColor[] {
                        ConsoleColor.DarkBlue,
                        ConsoleColor.DarkGreen,
                        ConsoleColor.DarkCyan,
                        ConsoleColor.DarkRed,
                        ConsoleColor.DarkYellow,
                        ConsoleColor.DarkGray,
                        ConsoleColor.Blue,
                        ConsoleColor.Green,
                        ConsoleColor.Cyan,
                        ConsoleColor.Red
                    };
                    int colorIndex = rnd.Next(decentColors.Length);
                    Console.ForegroundColor = decentColors[colorIndex];
                    isCustomColor = true;
                    Console.WriteLine("Hoppas du gillar den nya färgen ))");
                }
            }

            /// <summary>
            /// 4. Funktion för att skriva ut dagens datum
            /// </summary>
            public void f4() {
                DateTime d = DateTime.Now;
                string msg = String.Format("Dagens datum är {0:yyyy-MM-dd}", d);
                Console.WriteLine(msg);
            }

            /// <summary>
            /// 5. Funktion som tar två input värden, sedan skriver ut vilket av dem som är störst.
            /// </summary>
            public void f5() {
                Console.WriteLine("Ange 2 tal");
                int n1 = ih.readInt("Tal 1");
                int n2 = ih.readInt("Tal 2");
                Console.WriteLine("Det största talet är {0}", Math.Max(n1, n2));
            }

            /// <summary>
            /// 6. Funktion som genererar att slumpmässigt tal mellan 1 och 100.
            /// Användaren ska sedan gissa talet. Gissar användaren rätt så 
            /// ska ett meddelande säga detta, samt hur många försök det tog. 
            /// Gissar användaren fel ska ett meddelande visas som informerar 
            /// ifall talet var för stort eller för litet
            /// </summary>
            /// <param name="arg"></param>
            public void f6() {
                int num = rnd.Next(1, 101);
                int nGuesses = 0;
                while (true) {
                    nGuesses += 1;
                    int n = ih.readInt("Gissa ett tal mellan 1 och 100 (eller ange 0 för att ge upp");
                    if (n == 0)
                        return;
                    switch (Math.Sign(n - num)) {
                        case -1:
                            Console.WriteLine("Det sökta talet är STÖRRE");
                            break;
                        case 1:
                            Console.WriteLine("Det sökta talet är MINDRE");
                            break;
                        case 0:
                            Console.WriteLine("==YAY!!== du hittade rätt svar på bara {0} försök!", nGuesses);
                            Console.WriteLine();
                            break;
                    }
                }
            }

            /// <summary>
            /// 7. Funktion där användaren skriver in en textrad, som sedan sparas i en fil på hårddisken
            /// </summary>
            public void f7() {
                while(true) {
                    string str = ih.readString("Skriv nån text (ENTER för att avbryta)");
                    if(str == "") {
                        return;
                    }
                    t.saveText(str);
                    Console.WriteLine("Det finns nu {0} sparade texter", t.Count);
                }
            }

            /// <summary>
            /// 8. Funktion där en fil läses in från hårddisken
            /// (sist sparade filen från uppgift 7)
            /// </summary>
            public void f8() {
                try {
                    string str = t.readLastSavedText();
                    Console.WriteLine(str);
                    Console.WriteLine();
                    Console.WriteLine("Det finns nu {0} sparade texter kvar", t.Count);
                }
                catch (TextRepo.NoTextsException) {
                    Console.WriteLine("Det finns inga sparade texter, anropa funktion 7 för att spara en text");
                    return;
                }
            }

            /// <summary>
            /// 9. Funktion där användaren skickar in ett decimaltal 
            /// och får tillbaka roten, kvadraten och
            /// upphöjt till 10
            /// </summary>
            public void f9() {
                double d = ih.readDbl("Ange ett tal");
                StringBuilder sb = new StringBuilder();

                string sqrt = String.Format("{0}{1}", Math.Sqrt(Math.Abs(d)), d < 0 ? " i" : "");
                sb.AppendFormat("Roten ur {0} är:        {1}\n", d, sqrt);
                sb.AppendFormat("Kvadraten för {0} är:   {1}\n", d, Math.Pow(d, 2));
                sb.AppendFormat("{0} upphöjt till 10 är: {1}\n", d, Math.Pow(d, 10));
                Console.WriteLine(sb.ToString());
            }

            /// <summary>
            /// 10. Funktion där programmet skriver ut en multiplikationstabell
            /// </summary>
            public void f10() {
                const int min = 1;
                const int max = 10;
                const int size = max - min + 1;

                for (int i = 0; i < size; i++) {
                    int[] row = new int[size];
                    for (int j = 0; j < size; j++) {
                        row[j] = (min + i) * (min + j);
                    }
                    Console.WriteLine(string.Join("\t", row));
                }
            }

            /// <summary>
            /// 11. Funktion som skapar två arrayer. Den första fylls med 
            /// slumpmässiga tal. Den andra fylls med 
            /// talen från den första i stigande ordning.
            /// Array.Sort() får EJ användas.
            /// </summary>
            public void f11() {
                int n = 20;
                int[] ar = new int[n];
                for (int i = 0; i < n; i++) {
                    ar[i] = rnd.Next(1000);
                }
                Console.WriteLine("ORG   : {0}", ah.ToString(ar));
                Console.WriteLine("SORTED: {0}", ah.ToString(ah.Sort(ar)));
            }

            /// <summary>
            /// 12. Funktion som tar en input från användaren och kontrollerar ifall det är en palindrom 
            /// </summary>
            public void f12() {
                while (true) {
                    string str = ih.readString("Skriv ett ord (blankt för att avbryta)");
                    if (str == "")
                        break;
                    str = str.ToLower();
                    string reversed = string.Join("", str.Reverse());
                    Console.WriteLine("{0} {1} ett palindrom", str, str == reversed ? "ÄR" : "ÄR INTE");
                    Console.WriteLine();
                }
            }

            /// <summary>
            /// 13. Funktion som tar två inputs från användaren och skriver sedan ut alla
            /// siffror som är mellan de två inputsen.
            /// </summary>
            public void f13() {
                Console.WriteLine("Ange två heltal (inte mer än 20 emellan): ");
                while(true) {
                    int a = ih.readInt("Tal 1");
                    int b = ih.readInt("Tal 2");
                    if (Math.Abs(a - b) > 20) {
                        Console.WriteLine("Talen får int skilja sig med mer än 20, försök igen: ");
                        continue;
                    }
                    int[] ar = Enumerable.Range(Math.Min(a, b), Math.Abs(a - b) + 1).ToArray();
                    Console.WriteLine(string.Join(", ", ar));
                    break;
                }
            }

            /// <summary>
            /// 14. Funktion där användaren skickar in ett antal värden 
            /// (komma-separerade siffror) som sedan sorteras 
            /// och skrivs ut efter udda och jämna värden.
            /// </summary>
            public void f14() {
                while (true) {
                    int[] ar = ih.readIntList("Ange några kommaseparerade heltal (tomt för att avbryta)");
                    if (ar.Length == 0)
                        return;
                    ar = ah.Sort(ar);
                    int[] arOdds = ar.Where(i => i % 2 == 1).ToArray();
                    int[] arEvens = ar.Where(i => i % 2 == 0).ToArray();
                    Console.WriteLine("Udda tal:  {0}", ah.ToString(arOdds));
                    Console.WriteLine("Jämna tal: {0}", ah.ToString(arEvens));
                    Console.WriteLine();
                }
            }

            /// <summary>
            /// 15. Funktion där användaren skriver in ett antal värden(komma-separerade siffor)
            /// som sedan adderas och skrivs ut.
            /// </summary>
            public void f15() {
                while(true) {
                    int[] ar = ih.readIntList("Ange några kommaseparerade heltal (tomt för att avbryta)");
                    if (ar.Length == 0)
                        return;
                    Console.WriteLine("Summan är {0}", ar.Sum());
                    Console.WriteLine();
                }
            }

            /// <summary>
            /// 16. Funktion där användaren ska ange namnet på sin karaktär och namnet på
            /// en motståndare.
            /// Funktionen lägger sedan till slumpmässiga värden för Hälsa, Styrka och Tur,
            /// som sparas i en instans av en klass.
            /// Slutligen utses en vinnare, som helt enkelt är den som har störst summa
            /// av Hälsa, Styrka och Tur
            /// </summary>
            public void f16() {
                while (true) {
                    Console.WriteLine("Ange spelarnas namn");
                    Console.WriteLine("-------------------");
                    string name1 = ih.readString("Din spelare");
                    string name2 = ih.readString("Motspelaren");

                    if (name1 == "" || name2 == "")
                        return;
                    Console.WriteLine();

                    Player p1 = new Player(name1);
                    Player p2 = new Player(name2);

                    Console.WriteLine("Din spelare");
                    Console.WriteLine("-----------");
                    Console.WriteLine(p1.ToString());

                    Console.WriteLine("Motspelaren");
                    Console.WriteLine("-----------");
                    Console.WriteLine(p2.ToString());

                    int p1Value = p1.getTotal();
                    int p2Value = p2.getTotal();

                    switch (Math.Sign(p1Value - p2Value)) {
                        case 1:
                            Console.WriteLine("{0} vinner med {1} - {2}!", p1.name, p1Value, p2Value);
                            break;
                        case 0:
                            Console.WriteLine("Oavgjort {0} - {1}!", p1Value, p2Value);
                            break;
                        case -1:
                            Console.WriteLine("{0} vinner med {1} - {2}!", p2.name, p2Value, p1Value);
                            break;
                    }
                    Console.WriteLine();
                }
            }

            private class Player {
                static Random rnd = new Random();

                public string name;
                public int health;
                public int luck;
                public int strength;

                public Player(string name) {
                    this.name = name;
                    this.health = rnd.Next(100);
                    this.luck = rnd.Next(100);
                    this.strength = rnd.Next(100);
                }

                public override string ToString() {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Namn:   {0}\n", this.name);
                    sb.AppendFormat("Hälsa:  {0}\n", this.health);
                    sb.AppendFormat("Styrka: {0}\n", this.strength);
                    sb.AppendFormat("Tur:    {0}\n", this.luck);
                    return sb.ToString();
                }

                public int getTotal() {
                    return this.health + this.luck + this.strength;
                }
            }
        }

        /// <summary>
        /// Class InputHelpers, Read values from console with some checks
        /// </summary>
        private class InputHelpers {
            public string readString(string arg) {
                Console.Write("{0}: ", arg);
                string str = Console.ReadLine();
                if (str == null) {
                    throw new Exception("No input to read");
                }
                return str.Trim();
            }

            public int readInt(string arg) {
                while (true) {
                    string str = readString(arg);
                    bool isNumber = int.TryParse(str, out int number);
                    if (isNumber)
                        return number;
                    Console.WriteLine("Oops, det där var inte ett heltal, försök igen");
                }
            }

            public int[] readIntList(string arg) {
                while (true) {
                    string str = readString(arg);
                    string[] arStr = str.Split(',');
                    List<int> ret = new List<int>();
                    List<string> invalidNumbers = new List<string>();
                    foreach (string strInt in arStr) {
                        if (string.IsNullOrEmpty(strInt))
                            continue;
                        if (!int.TryParse(strInt.Trim(), out int value)) {
                            invalidNumbers.Add(strInt.Trim());
                        }
                        ret.Add(value);
                    }
                    if (invalidNumbers.Count > 0) {
                        Console.WriteLine("Ogiltiga heltal i listan, bl.a. {0}, försök igen", invalidNumbers[0]);
                        continue;
                    }
                    return ret.ToArray();
                }
            }

            public double readDbl(string arg) {
                while (true) {
                    string str = readString(arg);
                    bool isNumber = double.TryParse(str, out double number);
                    if (isNumber)
                        return number;
                    Console.WriteLine("Oops, det där var inte ett tal, försök igen");
                }
            }
        }

        private class ArrayHelpers {
            public string ToString<T>(T[] lst) {
                return String.Join(", ", lst);
            }

            /// <summary>
            /// Returns a sorted copy of the array. The original array is not changed
            /// </summary>
            /// <param name="ar"></param>
            /// <returns>A sorted copy of the array</returns>
            public int[] Sort(int[] ar) {
                int[] copy = (int[])ar.Clone();
                return Sort(copy, 0, copy.Length);
            }

            private int[] Sort(int[] ar, int start, int length) {
                if (length == 2) {
                    if (ar[start + 1] < ar[start]) {
                        SwitchItems(ar, start, start + 1);
                    }
                } else if (length > 2) {
                    int half = length / 2;
                    Sort(ar, start, half);
                    Sort(ar, start + half, length - half);
                    Merge(ar, start, start + half, start + length);
                }
                return ar;
            }

            private int[] Merge(int[] ar, int start1, int start2, int end) {
                int size = end - start1;
                int[] temp = new int[size];
                int i = start1;
                int j = start2;
                for (int iTemp = 0; iTemp < size; iTemp++) {
                    if (j >= end || i < start2 && ar[i] < ar[j]) {
                        temp[iTemp] = ar[i++];
                    } else {
                        temp[iTemp] = ar[j++];
                    }
                }
                for (int iTemp = 0; iTemp < size; iTemp++) {
                    ar[start1 + iTemp] = temp[iTemp];
                }
                return ar;
            }

            private T[] SwitchItems<T>(T[] ar, int a, int b) {
                T temp = ar[a];
                ar[a] = ar[b];
                ar[b] = temp;
                return ar;
            }
        }

        /// <summary>
        /// A small text repositary,
        /// Added texts are saved in a file in the TEMP directory
        /// When a text is retrieved, the last added text is read from it's
        /// file, and the file is deleted
        /// </summary>
        private class TextRepo {
            public class NoTextsException : Exception { }
            List<string> tempFiles = new List<string>();

            ~TextRepo() {
                while (tempFiles.Count > 0) {
                    string fileName = tempFiles[0];
                    File.Delete(fileName);
                    Console.WriteLine("Fil {0} borttagen", fileName);
                    tempFiles.RemoveAt(0);
                }
            }

            public int Count { get { return tempFiles.Count; } }

            public void saveText(string str) {
                string tempFileName = getTempFileName();
                File.WriteAllText(tempFileName, str);
                tempFiles.Add(tempFileName);
            }

            public string readLastSavedText() {
                string tempFileName;
                do {
                    if (tempFiles.Count == 0) {
                        throw new NoTextsException();
                    }
                    tempFileName = tempFiles.Last();
                    tempFiles.RemoveAt(tempFiles.Count - 1);
                } while (!System.IO.File.Exists(tempFileName));
                return File.ReadAllText(tempFileName);
            }

            private string getTempFileName() {
                return System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt";
            }
        }
    }
}
