using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Nutritracker
{
    public partial class frmActiveFields : Form
    {
        public frmActiveFields()
        {
            InitializeComponent();
        }

        List<string> availFields = new List<string>();
        List<string> oldInput;
        string sl = Path.DirectorySeparatorChar.ToString();
        string userRoot = "";
        private void frmActiveFields_Load(object sender, EventArgs e)
        {
            userRoot = $"{Application.StartupPath}{sl}usr{sl}profile{frmMain.currentUser.index}";
            this.Text = $"Editing {frmMain.currentUser.name}'s Active Fields";
            oldInput = File.ReadAllLines($"{userRoot}{sl}activeFields.TXT").ToList();
            for (int i = 0; i < oldInput.Count; i++)
                if (oldInput[i] == "")
                    oldInput.RemoveAt(i);
                else
                    oldInput[i] = oldInput[i].Replace("\r", "");

            //primary DBs (USDA, CNV, etc)
            string[] dbs = Directory.GetDirectories($"{Application.StartupPath}{sl}usr{sl}share{sl}DBs");
            foreach (string s in dbs)
            {
                string name = s.Split(Path.DirectorySeparatorChar)[s.Split(Path.DirectorySeparatorChar).Length - 1];
                if (!name.StartsWith("_"))
                {
                    string[] nutLines = File.ReadAllLines($"{s}{sl}_nutKeyPairs.TXT");
                    foreach (string st in nutLines)
                        if (!st.StartsWith("#") && !availFields.Contains(st.Split('|')[1]))
                            availFields.Add(st.Split('|')[1]);
                }
            }
            
            //user fields
            dbs = Directory.GetDirectories($"{Application.StartupPath}{sl}usr{sl}profile{frmMain.currentUser.index}{sl}DBs");
            foreach (string s in dbs)            
                if (s.Contains("f_user_")){
                    string[] configLines = File.ReadAllLines($"{s}{sl}_dbConfig.TXT");
                    foreach (string st in configLines)
                        if (st.StartsWith("[MetricName]") && !availFields.Contains(st.Replace("[MetricName]", "")))
                            availFields.Add(st.Replace("[MetricName]", ""));                                                          
                }
            
            //extended flavonoid DB         
            dbs = Directory.GetDirectories($"{Application.StartupPath}{sl}usr{sl}share{sl}rel{sl}multi");
            foreach (string s in dbs)
            {
                string[] initLines = File.ReadAllLines($"{s}{sl}_dbInit.TXT");
                string nutrDescFile = "";
                foreach (string st in initLines)
                    if (st.StartsWith("[Fields]"))
                        nutrDescFile = st.Replace("[Fields]", "");
                nutrDescFile = Directory.GetFiles(s, nutrDescFile, SearchOption.AllDirectories)[0];

                foreach (string st in File.ReadAllLines(nutrDescFile))
                    if (!availFields.Contains(st)) //shouldn't be necessary.. for good measure
                        availFields.Add(st);
            }
            
		    //availFields.Sort(); //undesirable
            richTxtInput.Text = string.Join("\n", oldInput);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string[] units = {"μg", "ug", "g", "mg", "g"};
        private void richTxtInput_TextChanged(object sender, EventArgs e)
        {
            //richTxtInput.textr
            //TextRange
            int z = richTxtInput.SelectionStart;
            Font font = richTxtInput.Font;
            string[] lines = richTxtInput.Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Replace("\r", " ");
            int m = 0;
            for (int i = 0; i < lines.Length; i++)
            {
				int l = lines[i].Length;
                if (lines[i].Contains("#"))
                {
                    int c = lines[i].IndexOf("#");
                    
                    richTxtInput.Select(m + c, l - c);
                    richTxtInput.SelectionColor = Color.Gray;
                    richTxtInput.SelectionFont = new Font(font, FontStyle.Italic);
                    richTxtInput.Select(z, 0);
                }
                else
                {
                    richTxtInput.Select(m, l);
                    richTxtInput.SelectionColor = Color.Red;
                    richTxtInput.SelectionFont = new Font(font, FontStyle.Regular);
                    richTxtInput.Select(z, 0);
                }

                string[] words = lines[i].Split('#')[0].Split(new char [] {' ', '|'});
                int n = m;
                for (int j = 0; j < words.Length; j++)
                {
                    if (availFields.Contains(words[j]))
                    {
                        richTxtInput.Select(n, words[j].Length);
                        richTxtInput.SelectionColor = Color.Blue;
                        richTxtInput.SelectionFont = new Font(font, FontStyle.Bold);
                        richTxtInput.Select(z, 0);
                    }
                    else if (units.Contains(words[j]))
                    {
                        richTxtInput.Select(n, words[j].Length);
                        richTxtInput.SelectionColor = Color.Green;
                        richTxtInput.SelectionFont = new Font(font, FontStyle.Bold);
                        richTxtInput.Select(z, 0);
                    }
                    n += words[j].Length + 1;
                }
                m += lines[i].Length + 1;
            }
        }
        
        private void btnListFields_Click(object sender, EventArgs e)
        {
            itemListerDialog ild = new itemListerDialog(this);
            ild.items = availFields;
            btnListFields.Enabled = false;
            ild.Show();
            //MessageBox.Show(string.Join("\n", availFields));
        }
        
        public void reEnableButton()
        {
            btnListFields.Enabled = true;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTxtInput.Text = File.ReadAllText($"{userRoot}{sl}activeFields.TXT");
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.Arguments = $"{userRoot}{sl}activeFields.TXT";
            if (sl == "/")
                ps.FileName = "gedit";
            else
                ps.FileName = "notepad";
            Process.Start(ps);
        }
    }
}
