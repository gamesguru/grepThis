using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using static ocast.Program;
using static ocast.pReader;

namespace ocast
{
    public class uiViewer
    {
        public class obj
        {
            public device d;
            public string _text;
            public string _resourceid;
            public string _class;
            public string _contdesc;
            public bool _checkable;
            public bool _checked;
            public bool _clickable;
            public bool _enabled;
            public bool _focusable;
            public bool _focused;
            public bool _scrollable;
            public bool _longclickable;
            public bool _password;
            public bool _selected;
            public string _bounds;
            public int _x1;
            public int _x2;
            public int _y1;
            public int _y2;
            public void click() => adb($"shell input tap {(_x1 + _x2) / 2} {(_y1 + _y2) / 2}", d);
        };

        public static string[] seekers = {
            "text",
            "resource-id",
            "class",
            "content-desc"
        };
        public static string[] vals = {
            "checkable",
            "checked",
            "\" clickable", //distinguishes from 'long-clickable'
            "enabled",
            "focusable",
            "focused",
            "scrollable",
            "long-clickable",
            "password",
            "selected",
            "bounds"
        };

        public static ConcurrentBag<obj> _objs(device d)
        {
            ConcurrentBag<obj> output;
            List<string> items = new List<string>();
            for (int i = 0; i < seekers.Length; i++)
                items = d.xml.Split(new string[] { $"{seekers[i]}=" }, StringSplitOptions.None).ToList();

            output = new ConcurrentBag<obj>();
            for (int k = 0; k < items.Count; k++)
                output.Add(new obj());

            for (int i = 0; i < seekers.Length; i++)
            {
                items = d.xml.Split(new string[] { $"{seekers[i]}=" }, StringSplitOptions.None).ToList();
                for (int j = 0; j < items.Count; j++)
                    if (!items[j].StartsWith("\""))
                        items.RemoveAt(j);
                for (int j = 0; j < items.Count; j++)
                {
                    string val = items[j].Split('\"')[1];

                    if (i == 0)
                        output.ToArray()[j]._text = val;
                    else if (i == 1)
                        output.ToArray()[j]._resourceid = val;
                    else if (i == 2)
                        output.ToArray()[j]._class = val;
                    else if (i == 3)
                        output.ToArray()[j]._contdesc = val;
                }
            }

            for (int i = 0; i < vals.Length; i++)
            {
                items = d.xml.Split(new string[] { $"{vals[i]}=" }, StringSplitOptions.None).ToList();
                for (int j = 0; j < items.Count; j++)
                    if (items[j].StartsWith("<"))
                        items.RemoveAt(j);
                for (int j = 0; j < items.Count; j++)
                {
                    string val = items[j].Split('\"')[1];

                    bool b = false;
                    if (val == "true")
                        b = true;
                    else if (val == "false")
                        b = false;

                    if (i == 0)
                        output.ToArray()[j]._checkable = b;
                    else if (i == 1)
                        output.ToArray()[j]._checked = b;
                    else if (i == 2)
                        output.ToArray()[j]._clickable = b;
                    else if (i == 3)
                        output.ToArray()[j]._enabled = b;
                    else if (i == 4)
                        output.ToArray()[j]._focusable = b;
                    else if (i == 5)
                        output.ToArray()[j]._focused = b;
                    else if (i == 6)
                        output.ToArray()[j]._scrollable = b;
                    else if (i == 7)
                        output.ToArray()[j]._longclickable = b;
                    else if (i == 8)
                        output.ToArray()[j]._password = b;
                    else if (i == 9)
                        output.ToArray()[j]._selected = b;
                    else if (i == 10)
                    {
                        output.ToArray()[j]._bounds = val;
                        string os = val.Split(new string[] { "][" }, StringSplitOptions.None)[0].Replace("[", "");
                        output.ToArray()[j]._x1 = Convert.ToInt32(os.Split(',')[0]);
                        output.ToArray()[j]._y1 = Convert.ToInt32(os.Split(',')[1]);
                        string ts = val.Split(new string[] { "][" }, StringSplitOptions.None)[1].Replace("]", "");
                        output.ToArray()[j]._x2 = Convert.ToInt32(ts.Split(',')[0]);
                        output.ToArray()[j]._y2 = Convert.ToInt32(ts.Split(',')[1]);
                    }
                }
            }
            return output;
        }
    }
}