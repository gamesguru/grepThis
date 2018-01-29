using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using static ocast.Program;
using static ocast.cPrinter;

namespace ocast
{
    public class pReader
    {
        static Process p;
        public static void adbMain(string arg)
        {
            p = new Process();
            ProcessStartInfo s = new ProcessStartInfo((Program.slash == "/") ? "/usr/lib/android-sdk/platform-tools/adb" : "c:/program files (x86)/android/android-sdk/platform-tools/adb");
            s.Arguments = arg;
            s.RedirectStandardOutput = true;
            s.UseShellExecute = false;
            s.CreateNoWindow = true;
            p = Process.Start(s);
            string line = "";
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            p.Close();
        }


        public static List<string> adb(string arg, device d = null, bool printSO = false)
        {
            if (!(d == null))
            {
                while (d._adbBusy)
                    Thread.Sleep(120);
                d._adbBusy = true;
            }
            List<string> output = new List<string>();
            string line = "";
            ProcessStartInfo s = new ProcessStartInfo((Program.slash == "/") ? "/usr/lib/android-sdk/platform-tools/adb" : "c:/program files (x86)/android/android-sdk/platform-tools/adb");
            if (d == null)
            {
                p = new Process();
                s.Arguments = arg;
            }
            else
                s.Arguments = $"-s {d.udid} {arg}";
            s.RedirectStandardOutput = true;
            s.UseShellExecute = false;
            s.CreateNoWindow = true;

            if (adbDebug)
                Console.Write($"[ADB] {arg} --> ");
            if (d == null)
                p = Process.Start(s);
            else
                d.adbProc = Process.Start(s);
            try
            {
                while (((d == null) ? line = p.StandardOutput.ReadLine() : line = d.adbProc.StandardOutput.ReadLine()) != null)
                    output.Add(line);
            }
            catch (Exception e)
            {
                d.statusBlob__lmsg = e.Message;
                updateStatusBlob(d);
                Thread.Sleep(5000);
            }

            if (printSO)
                Console.WriteLine(string.Join("\n", output));
            try { p.Close(); }
            catch { }
            try { d.adbProc.Close(); }
            catch { }
            if (d != null)
                d._adbBusy = false;
            return output;
        }

        public static void disableSystemUI(device d) => adb("shell service call activity 42 s16 com.android.systemui", d);

        public static void activateOneControl(device d) => adb($"shell am start com.lci1.one/md514e745197d12febda9505d9609e0ec28.SplashScreen", d);

        public static void unlockPhone(device d)
        {
            ///TODO: make work for all devices, LG, Google, etc
            if (!d.model.Contains("OCTP_"))
            {
                adb($"-s {d.udid} shell input keyevent 3", d); //works better than 82 or 26, *for phones
                Thread.Sleep(1300);
                adb($"-s {d.udid} shell input swipe {0.2 * d.Sx} {0.75 * d.Sy} {0.8 * d.Sx} {0.5 * d.Sy} 950", d);
            }
            else if (d.model.Contains("OCTP_"))
                adb($"-s {d.udid} shell input keyevent 26", d); //works best on OCTPs
        }

        public static void uiautomatorDump(device d)
        {
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            string s = d.udid;
            //dbg
            //d.statusBlob__lmsg = "uiautomator dump";
            //updateStatusBlob(d);
            while (!(d.dumpPath = adb($"-s {s} shell uiautomator dump", d)[0]).Contains("UI hierchary dumped to: "))
            {
                d.statusBlob__lmsg = $"adb_d:{d.dumpPath}";
                updateStatusBlob(d);
                Thread.Sleep(120);
            }
            d.dumpPath = d.dumpPath.Replace("UI hierchary dumped to: ", "").Trim();

            while (!(d.xml = adb($"-s {s} shell cat {d.dumpPath}", d)[0]).StartsWith("<?xml"))
            {
                d.statusBlob__lmsg = $"adb_x:{d.xml}";
                updateStatusBlob(d);
                Thread.Sleep(120);
            }
        }

        public static void swipeUp(device d)
        {
            //dbg
            //d.statusBlob__lmsg = "swipe up";
            //updateStatusBlob(d);
            int x = d.Sy / 2;
            int y1 = d.Sx / 4;
            int y2 = 3 * d.Sx / 4;
            adb($"shell input swipe {x} {y1} {x} {y2} 1250", d);
        }
        public static void swipeDown(device d)
        {
            //dbg
            //d.statusBlob__lmsg = "swipe down";
            //updateStatusBlob(d);
            int x = d.Sy / 2;
            int y1 = 3 * d.Sx / 4;
            int y2 = d.Sx / 4;
            adb($"shell input swipe {x} {y1} {x} {y2} 1250", d);
        }

        public static bool screenOn(device d)
        {
            foreach (string s in adb("shell dumpsys input_method", d))
                if (s.Contains("mScreenOn="))
                    return Convert.ToBoolean(s.Split(new string[] { "mScreenOn=" }, StringSplitOptions.None)[1].Trim());
                else if (s.Contains("mInteractive="))
                    return Convert.ToBoolean(s.Split(new string[] { "mInteractive=" }, StringSplitOptions.None)[1].Trim());
            return false;
        }
    }
}