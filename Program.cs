using System;
using System.Collections.Generic;
using System.IO;

namespace grepThis
{
    static class MainClass
    {
        static string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string sl = Path.DirectorySeparatorChar.ToString();
        static string nl = Environment.NewLine;
        //settings
        static bool dontPrint;
        static List<string> dirs = new List<string>();
        static List<string> chosenDirs = new List<string>();
        static bool[] _checked;
        //search
        static string query;
        static string matches;
        static List<string> matchingFiles;
        static bool suspended;
        public static void Main(string[] args)
        {
            settings();
            chooseDirs();
            printControls();
            while (true)
            {
                matchingFiles = new List<string>();
                print("grep this: ");
                query = Console.ReadLine();
                matches = $"{root}{sl}matches.TXT";
                if (dontPrint)
                    File.AppendAllText(matches, $"grep this: {query}\n");
                if (String.IsNullOrEmpty(query))
                    try { throw new ArgumentException("the string to find may not be empty", "query"); }
                    catch (Exception e) { print(e.ToString() + "\n"); continue; }
                foreach (string directory in chosenDirs)
                    foreach (string file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
                    {
                        if (file == matches || file == $"{root}{sl}settings.py")
                            continue;
                        top:
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo cki = Console.ReadKey();
                            if (cki.Key == ConsoleKey.Escape)
                                break;
                            else if (cki.Key == ConsoleKey.Backspace)
                                suspended = true;
                            else if (cki.Key == ConsoleKey.Spacebar)
                                suspended = false;
                            else if (cki.Key == ConsoleKey.Insert || cki.Key == ConsoleKey.F2)
                                dontPrint = !dontPrint;
                        }
                        while (suspended)
                        {
                            System.Threading.Thread.Sleep(80);
                            goto top;
                        }
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                            if (lines[i].ToUpper().Contains(query.ToUpper()))
                            {
                                printMatch(file, directory, i, lines[i], query);
                                if (dontPrint)
                                    break;
                            }
                    }
                if (dontPrint)
                {
					foreach (string s in matchingFiles)
						println(s.Replace(root, ""));
                    println();          
                }
                else
                    println($"\rfound: {matchingFiles.Count} matching files!!");
                File.AppendAllText(matches, $"\nfound: {matchingFiles.Count} matching files!\n\n");

                println("\n");
            }
        }

        static void printMatch(string filename, string directory, int lineNumber, string line, string query)
        {
            if (!matchingFiles.Contains(filename))
            {
                string str = $"{filename}::Ln{lineNumber}";
                matchingFiles.Add(filename);
                File.AppendAllText(matches, $"{str}\n");
            }

            if (!dontPrint)
            {
                print(filename.Replace(directory, ""), ConsoleColor.DarkMagenta);
                print(":", ConsoleColor.Blue);
                print(lineNumber.ToString(), ConsoleColor.Green);
                print(":", ConsoleColor.Blue);

                List<int> insts = AllIndexesOf(line.ToUpper(), query.ToUpper());

                print(line.Substring(0, insts[0]));
                print(line.Substring(insts[0], query.Length), ConsoleColor.DarkRed);
                for (int i = 0; i < insts.Count; i++)
                    try
                    {
                        print(line.Substring(insts[i] + query.Length, insts[i + 1] - insts[i] - query.Length));
                        print(line.Substring(insts[i + 1], query.Length), ConsoleColor.DarkRed);
                    }
                    catch
                    {
                        print(line.Substring(insts[i] + query.Length));
                        break; //new
                    }

                println();
            }
            else
				print($"\rfound: {matchingFiles.Count} matching files");
            
        } 

        public static List<int> AllIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        
        static void print(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        static void println(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        
        static void settings()
        {
        	string st = "";
        	foreach (string s in File.ReadAllLines($"{root}{sl}settings.py"))
        	{
        		st = s.Replace("\t", "").Trim();
                if (st.StartsWith("[dontPrint]"))
                    dontPrint = Convert.ToBoolean(st.Replace("[dontPrint]", ""));
                else if (st.StartsWith("[dir]") && Directory.Exists(st.Replace("[dir]", "").Replace("$root", root)))
                    dirs.Add(st.Replace("[dir]", "").Replace("$root", root));
        	}
        }

        static void chooseDirs()
        {
            _checked = new bool[dirs.Count];
            for (int i = 0; i < _checked.Length; i++)
                _checked[i] = true;
            if (dirs.Count == 0)
            {
                print("ERROR: ", ConsoleColor.DarkRed);
                println("no directories configured, add lines to settings.py: [dir] <dir_path>");
                println("press any key to exit...");
                Console.ReadKey();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else if (dirs.Count == 1)
            {
				chosenDirs.Add(dirs[0]);
                return;        
            }

            int max = 0;
            foreach (string s in dirs)
                if (Directory.Exists(s) && s.Length > max)
                    max = s.Length;
            string[] pad = new string[dirs.Count];
            for (int i = 0; i < dirs.Count; i++)
                    for (int j = max; j > dirs[i].Length; j--)
                        pad[i] += " ";

            while (true)
            {
                for (int i = 0; i < dirs.Count; i++)
                {
                    string str = _checked[i] ? "x" : " ";
                    println($"#{i + 1}: {dirs[i]}{pad[i]}[{str}]");
                }
                print("\nMake a selection, press enter to continue: ");
                string s = Console.ReadLine();
                if (s == "")
                    break;
                int j = -1;
                try { j = Convert.ToInt32(s); }
                catch (Exception e)
                {
                    print($"ERROR: ", ConsoleColor.DarkRed);
                    println(e.Message);
                    println();
                    continue;
                }
                if (j > 0 && j <= dirs.Count)
                    _checked[j - 1] = !_checked[j - 1];
                else
                {
                    print($"ERROR: ", ConsoleColor.DarkRed);
                    println($"value must be between 1 and {dirs.Count}");
                }
                println("\nNew choices:");
            }
            for (int i = 0; i < dirs.Count; i++)
                if (_checked[i])
                    chosenDirs.Add(dirs[i]);
        }

        static void printControls()
        {
            println("Welcome to grep for windows, by Shane");
            println($"{nl}##############{nl}## CONTROLS ##{nl}##############");
            println("## Backspace = Pause");
            println("## Spacebar  = Resume");
            println("## Escape  = Abort");
            println("## Insert  = Toggle print (it always saves a list of matches, limited to the first line number match of that file) [F2 == alt control]");
            println($"## It is case insensitive without much option for regex, it searches as is.{nl}");
        }
    }
}