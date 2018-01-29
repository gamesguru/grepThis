using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace grepThis
{
    static class MainClass
    {
        static string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static void Main(string[] args)
        {
            while (true)
            {
                print("grep this: ");
                string query = Console.ReadLine();
                if (String.IsNullOrEmpty(query))
                    try { throw new ArgumentException("the string to find may not be empty", "query"); }
                    catch (Exception e) { print(e.ToString() + "\n"); continue; }
                foreach (string file in Directory.GetFiles(root, "*.*", SearchOption.AllDirectories))
                {
                    string[] lines = File.ReadAllLines(file);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].ToUpper().Contains(query.ToUpper()))
                                printMatch(file, i, lines[i], query);
                    }
                }
                println("\n");
            }
        }

        private static void printMatch(string filename, int lineNumber, string line, string query)
        {
            print(filename.Replace(root, ""), ConsoleColor.DarkMagenta);
            print(":", ConsoleColor.Blue);
            print(lineNumber.ToString(), ConsoleColor.Green);
            print(":", ConsoleColor.Blue);

            List<int> insts = AllIndexesOf(line.ToUpper(), query.ToUpper());
            print(line.Substring(0, insts[0]));
            print(line.Substring(insts[0], query.Length), ConsoleColor.DarkRed);
                       
            for (int i = 0; i < insts.Count; i++)
            {
                try
                {
                    print(line.Substring(insts[i] + query.Length, insts[i + 1] - insts[i] - query.Length));
                    print(line.Substring(insts[i + 1], query.Length), ConsoleColor.DarkRed);
                }
                catch
                {
                    print(line.Substring(insts[i] + query.Length));
                }
            }
            println();
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
    }
}
