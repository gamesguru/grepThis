using System;
using System.Collections.Generic;
using System.IO;

namespace grepThis
{
    static class MainClass
    {
        static string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string slash = Path.DirectorySeparatorChar.ToString();
        static bool dontPrint;
        static string query;
        static string filQuery = "matches.TXT";
        static List<string> matchingFiles;
        static bool suspended;
        public static void Main(string[] args)
        {
            settings();
            while (true)
            {                
                matchingFiles = new List<string>();
                print("grep this: ");
                query = Console.ReadLine();
                filQuery = $"{root}{slash}matches.TXT";
                //filQuery = "";
                if (dontPrint)
                    File.AppendAllText(filQuery, $"grep this: {query}\n");
                //for (int i = 0; i < query.Length; i++)
                //if (Char.IsLetterOrDigit(query[i]))
                //    filQuery += query[i];
                //else
                //filQuery += "";
                if (String.IsNullOrEmpty(query))
                    try { throw new ArgumentException("the string to find may not be empty", "query"); }
                    catch (Exception e) { print(e.ToString() + "\n"); continue; }
                foreach (string file in Directory.GetFiles(root, "*.*", SearchOption.AllDirectories))
                {
                top:
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo cki = Console.ReadKey();
                        if (cki.Modifiers == ConsoleModifiers.Shift && cki.Key == ConsoleKey.Q)
                            break;
                        else if (cki.Key == ConsoleKey.Backspace)
                            suspended = true;
                        else if (cki.Key == ConsoleKey.Spacebar)
                            suspended = false;
                        else if (cki.Modifiers == ConsoleModifiers.Shift && cki.Key == ConsoleKey.P)
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
	    					printMatch(file, i, lines[i], query);
                            if (dontPrint)
                                break;
                        }
                }
                println($"\rfound: {matchingFiles.Count} matching files!!");
                File.AppendAllText(filQuery, $"\nfound: {matchingFiles.Count} matching files!\n\n");

                println("\n");
            }
        }

        private static void printMatch(string filename, int lineNumber, string line, string query)
        {
            if (filename == filQuery || filename == "settings.py")
                return;

            if (!matchingFiles.Contains(filename))
            {
                string str = $"{filename}::Ln{lineNumber}";
                matchingFiles.Add(filename);
                File.AppendAllText(filQuery, $"{str}\n");
            }

            if (!dontPrint)
            {
                print(filename.Replace(root, ""), ConsoleColor.DarkMagenta);
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
        
        private static void print(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        private static void println(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        
        static void settings()
        {
        	string st = "";
        	foreach (string s in File.ReadAllLines($"{root}{slash}settings.py"))
        	{
        		st = s.Replace("\t", "").Trim();
        		if (st.StartsWith("[dontPrint]"))
        			dontPrint= Convert.ToBoolean(st.Replace("[dontPrint]", ""));
        	}
        }
    }
}
