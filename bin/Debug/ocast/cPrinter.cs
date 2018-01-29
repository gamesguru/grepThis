using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using static ocast.pReader;
using static ocast.Program;
namespace ocast
{
    public class cPrinter
    {
        public static void println(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void print(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void println(string text) => Console.WriteLine(text);
        public static void println() => Console.WriteLine("");
        public static void print(string text) => Console.Write(text);


        public static bool busy = false; //limits one device to update the console box at once
        public static void updateStatusBlob(device d)
        {
            while (busy)
                Thread.Sleep(120);
            busy = true;
            string bar = "<";
            int prog = d.statusBlob_cycleCount / cycleMax;
            double _prog = (double)d.statusBlob_cycleCount / cycleMax;
            _prog = Math.Round(_prog, 2) * 100;
            string __prog = $"{_prog}%";
            for (int i = 0; i < 3 - _prog.ToString().Length; i++)
                __prog = $" {__prog}";

            for (int i = 0; i < 25; i++)
                if (i < _prog / 4)
                    bar += "=";
                else
                    bar += " ";
            string __lmsg = 25 > d.statusBlob__lmsg.Length ? d.statusBlob__lmsg : d.statusBlob__lmsg.Substring(0, 25);
            int oldL = __lmsg.Length;
            for (int i = 0; i < 25 - oldL; i++)
                __lmsg += " ";

            int anrn = d.statusBlob_anrCount.ToString().Length;
            string anrCnt = "";
            if (anrn == 1)
                anrCnt = $" {d.statusBlob_anrCount}";
            else if (anrn == 2)
                anrCnt = d.statusBlob_anrCount.ToString();
            else if (anrn > 2)
                anrCnt = "99+";
            bar += $"> [{__prog}] [ANRs:{anrCnt}]";
            bar += $" ...{__lmsg}... {d.manu} {d.model}";
            statusBlobs[d.statusBlob_index] = bar;
            
            Console.SetCursorPosition(0, Console.CursorTop - (statusBlobs.Length));
            print(string.Join("\n", statusBlobs) + "\nPress BackSpace to Suspend, SpaceBar to Resume...");
            
            //TODO: color-code
            //Console.SetCursorPosition(0, Console.CursorTop - (statusBlobs.Length))
            // for (int i=0;i<statusBlobs.Length;i++)
            //     println(statusBlobs[i]); //work here
            // print("Press Backspace to Suspend, Spacebar to Resume...");
            busy = false;
        }



        public static void logCrash(device d, string hmsg, string dmsg)
        {
            d.statusBlob__lmsg = $"log:{hmsg}";
            updateStatusBlob(d);
            string file;
            string image;
            int pn = 0;
            while (File.Exists(file = $"{root}{slash}logs{slash}{hmsg.Replace("@", "-").Replace(" ", "_")}_{d.udid}_{pn}.TXT"))
                pn++;
            image = $"{root}{slash}logs{slash}{hmsg.Replace("@", "-").Replace(" ", "_")}_{d.udid}_{pn}.PNG";
            List<string> output = new List<string>();
            adb("screencap /sdcard/screen.png", d);
            output.Add($"{d.manu} {d.model} -- {d.udid} -- {DateTime.Now}");
            output.Add(dmsg);
            output.Add("###########\n## dmesg ##\n###########");
            output.AddRange(adb("shell dmseg", d));
            output.Add("##########\n## kmsg ##\n##########");
            output.AddRange(adb("shell cat /proc/kmsg", d));
            output.AddRange(adb("shell cat /proc/last_kmsg", d));
            output.Add("########\n## PS ##\n########");
            output.AddRange(adb("shell ps", d));
            output.Add("############\n## LOGCAT ##\n############");
            output.AddRange(adb("shell logcat -d", d));
            adb("shell logcat -c", d);
            output.Add("############\n## TRACES ##\n############");
            output.AddRange(adb("shell cat data/anr/traces.txt", d));
            Directory.CreateDirectory($"{root}{slash}logs");
            File.WriteAllLines(file, output);
            if (d.model.Contains("OCTP_"))
                adb($"pull /mnt/sdcard/screen.png {image}");
            else
                adb($"pull /sdcard/screen.png {image}");
            recover(d);
        }
    }
}