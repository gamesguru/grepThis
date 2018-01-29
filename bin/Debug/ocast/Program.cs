using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using static ocast.uiViewer;
using static ocast.pReader;
using static ocast.testLogic;
using static ocast.cPrinter;

namespace ocast
{
    public class Program
    {
        public class device
        {
            public string udid;
            public int port;
            public string manu;
            public string model;
            public string droidVer;
            public string build;
            public string version;
            public int Sx;                      //phone dimensions
            public int Sy;
            public string dumpPath;
            public string xml;
            public ConcurrentBag<obj> objs;
            public string[] initialGridItems;
            public int backCoordX, backCoordY;  //saves time from having ui dump each time
            public bool _adbBusy = false;
            public Process adbProc;
            //printing status
            public string _last = "";
            public int statusBlob_index;
            public int statusBlob_cycleCount = 0;
            public string statusBlob__lmsg = "";
            public int statusBlob_anrCount = 0;
        }

        public static ConcurrentBag<device> devices = new ConcurrentBag<device>();
        public static string[] statusBlobs;
        private static ConsoleKeyInfo kci;

        public static string slash = Path.DirectorySeparatorChar.ToString();
        public static string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string settingsIni = $"{root}{slash}settings.ini";

        public static bool adbDebug = false;
        public static string adbLoc = "";
        public static int cycleMax = 25; //add support to change this at runtime
        public static string wifi = "";
        public static bool freshInstall = false;
        public static bool demo = false;

        static void Main(string[] args)
        {
            string[] sets = File.ReadAllLines(settingsIni);
            foreach (string s in sets)
            {
                if (s.StartsWith("#"))
                    continue;
                string st = s.Split('#')[0].Trim();
                if (st.StartsWith("[adbDebug]"))
                    adbDebug = Convert.ToBoolean(st.Replace("[adbDebug]", "").Replace("\t", ""));
                else if (st.StartsWith("[adbLoc]") && File.Exists(st.Replace("[adbLoc]", "").Replace("\t", ""))) //test on windows
                    adbLoc = st.Replace("[adbLoc]", "").Replace("\t", "");
                else if (st.StartsWith("[cycleMax]"))
                    cycleMax = Convert.ToInt32(st.Replace("[cycleMax]", "").Replace("\t", ""));
                else if (st.StartsWith("[freshInstall]"))
                    freshInstall = Convert.ToBoolean(st.Replace("[freshInstall]", "").Replace("\t", ""));
                else if (st.StartsWith("[demo]"))
                    demo = Convert.ToBoolean(st.Replace("[demo]", "").Replace("\t", ""));
                else if (st.StartsWith("[wifi]") && File.Exists(st.Replace("[wifi]", "").Replace("\t", ""))) //test on windows
                    wifi = st.Replace("[wifi]", "").Replace("\t", "");
            }

            #region check devices
            ConcurrentBag<string> serials;
            List<string> lines;
            while (true)
            {
                lines = adb("devices");
                serials = new ConcurrentBag<string>();
                for (int i = 0; i < lines.Count; i++)
                    if (lines[i].Contains("\tdevice"))
                        serials.Add(lines[i].Replace("\tdevice", ""));


                if (serials.Count > 0)
                    break;
                print("\rno device found! ", ConsoleColor.DarkRed);
                println("press F2 for raw output, another key to quit or wait to retry");

                //waits for 3 seconds for user to press key
                int j = 0;
                kci = new ConsoleKeyInfo();
                while (j < 30 && kci == new ConsoleKeyInfo())
                {
                    j++;
                    if (j % 5 == 0)
                        Console.Write(".", ConsoleColor.DarkRed);
                    if (Console.KeyAvailable) //checks without waiting, similar to Process.StandardOutput.Peek()
                        kci = Console.ReadKey();
                    Thread.Sleep(100);
                }


                if (kci != new ConsoleKeyInfo() && kci.Key == ConsoleKey.F2)
                {
                    println();
                    List<string> rawOutput = adb("devices");
                    for (int i = 0; i < rawOutput.Count; i++)
                        if (i == 0)
                            println(rawOutput[i], ConsoleColor.DarkCyan);
                        else
                            println(rawOutput[i]);
                }
                else if (kci != new ConsoleKeyInfo())
                    return;
            }
            #endregion

            #region prep devices
            println("List of devices/emulators attached", ConsoleColor.DarkCyan);
            //int port = 5038;
            for (int i = 0; i < serials.Count; i++)
            {
                device d = new device();
                d.udid = serials.ToArray()[i];
                d.statusBlob_index = i;
                //d.port = port++;
                lines = adb($"shell getprop", d);
                for (int k = 0; k < lines.Count; k++)
                    if (lines[k].Contains("ro.product.manufacturer"))
                        d.manu = lines[k].Split(':')[1].Replace("[", "").Replace("]", "").Trim();
                    else if (lines[k].Contains("ro.build.version.release"))
                        d.droidVer = lines[k].Split(':')[1].Replace("[", "").Replace("]", "").Trim();
                    else if (lines[k].Contains("ro.product.model"))
                        d.model = lines[k].Split(':')[1].Replace("[", "").Replace("]", "").Trim();

                lines = adb($"shell dumpsys package com.lci1.one", d);
                for (int k = 0; k < lines.Count; k++)
                    if (lines[k].Trim().StartsWith("versionName="))        //versionName=2.4
                        d.version = lines[k].Trim().Replace("versionName=", "");
                    else if (lines[k].Trim().StartsWith("versionCode="))   //versionCode=502 targetSdk=23
                        d.build = lines[k].Trim().Split(' ')[0].Replace("versionCode=", "");

                print(">>> ");
                print($"{d.manu} {d.model} \t", ConsoleColor.Green);
                print($"({d.droidVer}) [v{d.version} ");
                print($"({d.build})", ConsoleColor.DarkBlue);
                println("]");
                devices.Add(d);
            }
            statusBlobs = new string[devices.Count];
            #endregion

            string pad = "\n";
            for (int i = 0; i < devices.Count; i++)
                pad += "\n";
            print($"\nbeginning the main test... ");
            print($"{devices.Count} devices", ConsoleColor.DarkCyan);
            print("... ");
            print($"{cycleMax} cycles", ConsoleColor.DarkCyan);
            print($"...{pad}");
            t = new Thread(() => DoPar());
            t.Start();
            Console.TreatControlCAsInput = true;
            while ((kci = Console.ReadKey()) != null)
                if (!suspended && kci.Key == ConsoleKey.Backspace)
                    suspended = true;
                else if (suspended && kci.Key == ConsoleKey.Spacebar)
                    suspended = false;
        }


        static Thread t;
        private static void DoPar() => Parallel.ForEach(devices, d =>
        {
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            updateStatusBlob(d);
            d.statusBlob__lmsg = "grabbing screen size";
            string raw = adb($"shell wm size", d)[0];
            string size = raw.Split(':')[1].Trim();

            d.Sx = Convert.ToInt16(size.Split('x')[0]);
            d.Sy = Convert.ToInt16(size.Split('x')[1]);

            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "unlocking phone";
            updateStatusBlob(d);

            //dbg
            //if (!screenOn(d))
            //{
            //    d.statusBlob__lmsg = "SCREEN OFF";
            //    updateStatusBlob(d);
            //    Thread.Sleep(4000);
            //}
            //else
            //{
            //    d.statusBlob__lmsg = "SCREEN ON";
            //    updateStatusBlob(d);
            //    Thread.Sleep(4000);
            //}
            if (!d.model.Contains("OCTP_") || !screenOn(d))
                unlockPhone(d);

            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "launching onecontrol";
            updateStatusBlob(d);
            activateOneControl(d);

            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "waiting 4sec";
            updateStatusBlob(d);
            Thread.Sleep(4000);

            if (freshInstall)
                firstTimeSetup(d);

            if (d.model.Contains("OCTP_"))
            {
                if (suspended)
                    suspend(d);
                while (suspended)
                    Thread.Sleep(200);
                d.statusBlob__lmsg = "disabling system ui";
                updateStatusBlob(d);
                disableSystemUI(d);
                Thread.Sleep(2500);
            }     //disable systemUI
            else if (!d.model.Contains("OCTP_") && !demo)
            {
                if (suspended)
                    suspend(d);
                while (suspended)
                    Thread.Sleep(200);
                d.statusBlob__lmsg = "entering pin";
                updateStatusBlob(d);
                pin(d);
                Thread.Sleep(3500);
            }                               //enter pin

            d.initialGridItems = grabInitialGridItems(d);

            //main loop
            while (d.statusBlob_cycleCount++ < cycleMax)
                foreach (string st in d.initialGridItems)
                {
                    if (suspended)
                        suspend(d);
                    while (suspended)
                        Thread.Sleep(200);
                    //TODO: remove this after we get it working
                    logCrash(devices.ToArray()[0], $"test_crash{d.statusBlob_cycleCount}", "this is a test message describing the nature of the crash");
                    runTest(st, d);
                }
            d.statusBlob_cycleCount--;
            d.statusBlob__lmsg = "COMPLETE!!";
            updateStatusBlob(d);
        });



        public static bool panic(device d)
        {
            bool _panic = true;
            foreach (obj o in d.objs)
                try
                {
                    if (o._resourceid.Contains("com.lci1.one"))
                        _panic = false;
                }
                catch { }
            if (_panic)
                return true;                //no com.lci1.one elements found!!

            foreach (obj o in d.objs)
                try
                {
                    if (o._text.Contains("Unfortunately") && o._text.Contains("has stopped"))
                    {
                        d.statusBlob__lmsg = "device ANRing";
                        d.statusBlob_anrCount++;
                        updateStatusBlob(d);
                        //dismisses dialog and continues loop
                        foreach (obj ob in d.objs)
                            if (ob._text.ToLower() == "ok")
                                ob.click();
                        logCrash(d, $"ANR@{d._last}", $"app stopped responding after {d.statusBlob_cycleCount} cycles ... {DateTime.Now}");
                        return true;
                    }
                }
                catch { }

            foreach (obj o in d.objs)
                try
                {
                    if (o._text.Contains("OFFLINE"))
                    {
                        logCrash(d, $"devOFFLINE@{d._last}", $"a device was found offline after {d.statusBlob_cycleCount} cycles ... {DateTime.Now}");
                        return true;
                    }
                }
                catch { }
            return false;
        }

        public static void recover(device d)
        {
            activateOneControl(d);
            Thread.Sleep(4000);
            if (!d.model.Contains("OCTP_"))
            {
                pin(d);
                Thread.Sleep(4000);
            }
        }


        public static bool suspended = false;
        public static void suspend(device d)
        {
            d.statusBlob__lmsg = "suspended";
            updateStatusBlob(d);
        }
    }
}