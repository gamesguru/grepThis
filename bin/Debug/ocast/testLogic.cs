﻿using System;
using System.Threading;
using System.Collections.Generic;
using static ocast.uiViewer;
using static ocast.pReader;
using static ocast.Program;
using static ocast.cPrinter;

namespace ocast
{
    public class testLogic
    {
        public static string[] grabInitialGridItems(device d)
        {
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "counting initial items";
            updateStatusBlob(d);
            List<string> tmp = new List<string>();
            swipeUp(d);

            while (true)
            {
                int oldLength = tmp.Count;
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._resourceid.ToLower().Contains("grid_item_text") && !tmp.Contains(o._text))
                            tmp.Add(o._text);
                    }
                    catch { }

                if (tmp.Count == oldLength) //no new items detected
                    break;
                swipeDown(d);
            }
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d._last = tmp[tmp.Count - 1];
            d.statusBlob__lmsg = $"found {tmp.Count} elements";
            updateStatusBlob(d);
            Thread.Sleep(1250);
            return tmp.ToArray();
        }

        public static void runTest(string test, device d) => genericGridItemTest(test, d);

        public static void goIntoThisFromMain(string _this, device d)
        {
            bool neut = string.Compare(d._last, _this) == 0 ? true : false;
            bool down = string.Compare(d._last, _this) < 0 ? true : false;
            if (_this == "MyRV")
                down = false;

            //dbg
            //string downStr = down ?"down":"up";
            //d.statusBlob__lmsg = $"cmp:{d._last},{_this}={downStr}";
            //updateStatusBlob(d);
            //Thread.Sleep(4000);

            int n = 0;
            while (n++ < 4)
            {
                d.statusBlob__lmsg = $"seeking '{_this}'";
                updateStatusBlob(d);
                uiautomatorDump(d);
                d.objs = _objs(d);
                if (panic(d))
                    recover(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._text == _this)
                        {
                            validateObjCenterFallsOnScreen(d, o);
                            o.click();
                            d.statusBlob__lmsg = $"-->clicking '{_this}'";
                            updateStatusBlob(d);
                            d._last = _this;
                            return;
                        }
                    }
                    catch { }

                if (down && !neut)             ///TODO: flip directions and try again
                    swipeDown(d);
                else if (!down && !neut)
                    swipeUp(d);
            }
            if (--n == 4)
            {
                d.statusBlob__lmsg = $"not found: {_this}";
                updateStatusBlob(d);
                //TODO: reporting...
            }
        }



        public static void genericGridItemTest(string test, device d)
        {
            goIntoThisFromMain(test, d);
            Thread.Sleep(1850);
            backArrow(d);
        }



        #region supporting tests
        public static void pin(device d)
        {
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "initial UI dump";
            updateStatusBlob(d);
            uiautomatorDump(d);
            d.objs = _objs(d); //refreshes objs

            string q = "1";
            bool demo = false;
            if (!demo && !d.model.Contains("OCTP_"))
            {
                if (suspended)
                    suspend(d);
                while (suspended)
                    Thread.Sleep(200);
                d.statusBlob__lmsg = $"click '{q}'";
                updateStatusBlob(d);
                foreach (obj o in d.objs)
                    if (o._text == q)
                    {
                        for (int i = 0; i < 4; i++)
                            adb($"shell input tap {(o._x1 + o._x2) / 2} {(o._y1 + o._y2) / 2}", d);
                        Thread.Sleep(100);
                    }

                q = "Submit";
                if (suspended)
                    suspend(d);
                while (suspended)
                    Thread.Sleep(200);
                d.statusBlob__lmsg = $"click '{q}'";
                updateStatusBlob(d);
                foreach (obj o in d.objs)
                    if (o._text == q)
                        adb($"shell input tap {(o._x1 + o._x2) / 2} {(o._y1 + o._y2) / 2}", d);
            }
        }

        private static void backArrow(device d)
        {
            if (suspended)
                suspend(d);
            while (suspended)
                Thread.Sleep(200);
            d.statusBlob__lmsg = "<--navigate up";
            updateStatusBlob(d);
            if (d._last == "MyRV")
            {
                adb($"shell input keyevent 4", d);
                return;
            }
            if (d._last == "Leveling")
            {
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._resourceid.ToUpper().Contains("ACTION_HOME"))
                        {
                            o.click();
                            Thread.Sleep(1000);
                            return;
                        }
                    }
                    catch { }
            }
            if (d.backCoordX == 0 && d.backCoordY == 0)
            {
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._contdesc.ToUpper() == "NAVIGATE UP")
                        {
                            d.backCoordX = (o._x1 + o._x2) / 2;
                            d.backCoordY = (o._y1 + o._y2) / 2;
                            o.click();
                        }
                    }
                    catch { }
            }
            else
                adb($"shell input tap {d.backCoordX} {d.backCoordY}", d);
        }

        private static void checkIfMainMenuChanged()
        {
            ///TODO: check for duplicates, omissions, and other anomalies
        }

        private static void validateObjCenterFallsOnScreen(device d, obj o)
        {
            if ((o._y1 + o._y2) / 2 > d.Sy)     //swipes down if below horizon
                swipeDown(d);
            else if ((o._y1 + o._y2) / 2 < 0)   //swipes up if above origin
                swipeUp(d);
            //dbg
            //if (o._text == "Lighting" || o._text == "Monitor Panel" || o._text == "Slides" || o._text == "Stabilizers")
            //{
            //d.statusBlob__lmsg = $"cnt:{(o._x1 + o._x2) / 2}, {(o._y1 + o._y2) / 2}pb:{d.Sx}, {d.Sy}";
            //updateStatusBlob(d);
            //Thread.Sleep(4000);
            //}
        }

        public static void firstTimeSetup(device d)
        {
            uiautomatorDump(d);
            d.objs = _objs(d);
            foreach (obj o in d.objs)
                try
                {
                    if (o._text.ToUpper() == "ALLOW")
                    {
                        o.click();
                        Thread.Sleep(2000);
                    }
                }
                catch { }

            uiautomatorDump(d);
            d.objs = _objs(d);
            foreach (obj o in d.objs)
                try
                {
                    if (o._text.ToUpper() == "I ACCEPT")
                    {
                        o.click();
                        Thread.Sleep(2000);
                    }
                }
                catch { }

            if (demo)
            {
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._resourceid.Contains("titleView"))
                        {
                            for (int i = 0; i < 7; i++)
                                o.click();
                            Thread.Sleep(800);
                            uiautomatorDump(d);
                            d.objs = _objs(d);
                            foreach (obj ob in d.objs)
                                try
                                {
                                    if (ob._text.ToUpper() == "CREATE A DEMO COACH")
                                    {
                                        ob.click();
                                        Thread.Sleep(950);
                                        return;
                                    }
                                }
                                catch { }
                        }
                    }
                    catch { }
            }
            else
            {
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._text.ToUpper() == "GET STARTED")
                        {
                            o.click();
                            Thread.Sleep(2000);
                        }
                    }
                    catch { }
                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._text.ToUpper() == "MYRV")
                        {
                            o.click();
                            Thread.Sleep(2000);
                        }
                    }
                    catch { }

                uiautomatorDump(d);
                d.objs = _objs(d);
                foreach (obj o in d.objs)
                    try
                    {
                        if (o._text.ToUpper() == "SETTINGS")
                        {
                            o.click();
                            uiautomatorDump(d);
                            d.objs = _objs(d);
                            foreach (obj ob in d.objs)
                                try
                                {
                                    if (ob._class == "android.widget.Switch")
                                    {
                                        ob.click();
                                        Thread.Sleep(800);
                                        adb("shell input keyevent 4", d);   //back button
                                        Thread.Sleep(1200);
                                        uiautomatorDump(d);
                                        d.objs = _objs(d);
                                        foreach (obj ob2 in d.objs)
                                            try
                                            {
                                                if (ob2._text.ToUpper() == "GET STARTED")
                                                {
                                                    o.click();
                                                    Thread.Sleep(2000);
                                                }
                                            }
                                            catch { }

                                        uiautomatorDump(d);
                                        d.objs = _objs(d);
                                        foreach (obj ob2 in d.objs)
                                            try
                                            {
                                                if (ob2._text.ToUpper() == "MYRV")
                                                {
                                                    o.click();
                                                    Thread.Sleep(2000);
                                                }
                                            }
                                            catch { }

                                        uiautomatorDump(d);
                                        d.objs = _objs(d);
                                        foreach (obj ob2 in d.objs)
                                            try
                                            {
                                                if (ob2._text.ToUpper().Contains(wifi.ToUpper()))
                                                {
                                                    ob2.click();
                                                    Thread.Sleep(2000);
                                                }
                                            }
                                            catch { }

                                        //...
                                        //...
                                        //...
                                    }
                                }
                                catch { }
                        }
                    }
                    catch { }
                Thread.Sleep(2000);
            }
        }
        #endregion
    }
}