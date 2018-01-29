using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;

namespace Nutritracker
{
    public partial class frmDecomposeRecipe : Form
    {
        public frmDecomposeRecipe()
        {
            InitializeComponent();
        }
        static string nl;
		static string sl;


        private static class DB
        {
            public static string[] ndbs;
            public static string[] names;
            public static string[] cals;
            public static int[] wMatch;
            public static string[] joinedMatches;
        }
        char[] _delims = new char[] { '/', ',', ' ', '-', ';', '(', ')' };
        private class _dbObj
        {
            public string db = "USDAstock";
            public string ndbno;
            public string name;
            public double percent = 1;
            public double weight = 100;
        }
        List<_dbObj> dbobjs;
        int curDbIndex;


        private void frmDecomposeRecipe_Load(object sender, EventArgs e)
        {
            updatePer();
            nl = Environment.NewLine;
            sl = Path.DirectorySeparatorChar.ToString();
            string[] nutKeyLines = File.ReadAllLines($"{Application.StartupPath}{sl}usr{sl}share{sl}DBs{sl}USDAstock{sl}_nutKeyPairs.TXT");
            foreach (string s in nutKeyLines)
                if (s.Split('|')[1] == "NDBNo")
                    DB.ndbs = File.ReadAllLines($"{Application.StartupPath}{sl}usr{sl}share{sl}DBs{sl}USDAstock{sl}{s.Split('|')[0]}");
                else if (s.Split('|')[1] == "FoodName")
                    DB.names = File.ReadAllLines($"{Application.StartupPath}{sl}usr{sl}share{sl}DBs{sl}USDAstock{sl}{s.Split('|')[0]}");
                else if (s.Split('|')[1] == "Cals")
                    DB.cals = File.ReadAllLines($"{Application.StartupPath}{sl}usr{sl}share{sl}DBs{sl}USDAstock{sl}{s.Split('|')[0]}");
        }
        
        private void txtIngrieds_TextChanged(object sender, EventArgs e)
        {
            string input = txtIngrieds.Text;
            lstBoxIngrieds.Items.Clear();
            List<string> output = input.Split(new string[] { nl }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i=0;i<output.Count;i++)
                if (output[i].Length < 3)
                    output.RemoveAt(i);
            string[] ingrieds = output.ToArray();
            dbobjs = new List<_dbObj>();
            foreach (string s in ingrieds)
            {
                _dbObj d = new _dbObj();
                d.name = s;
                dbobjs.Add(d);
            }
            updatePer();
            updateList();
        }


        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void lstBoxIngrieds_MouseUp(object sender, MouseEventArgs e) => assignCurObj();
        private void lstBoxIngrieds_SelectedIndexChanged(object sender, EventArgs e) => assignCurObj();

        private void lstViewDBresults_MouseUp(object sender, MouseEventArgs e) => assignNdb();
        private void lstViewDBresults_SelectedIndexChanged(object sender, EventArgs e) => assignNdb();


        private void txtTweakName_TextChanged(object sender, EventArgs e) => search();
        private void txtTweakWeight_TextChanged(object sender, EventArgs e) { 
            try { dbobjs[curDbIndex].weight = Convert.ToDouble(txtTweakWeight.Text);
             } catch { }
            updateList();
        }
        
        private void assignNdb()
        {
            try { dbobjs[curDbIndex].ndbno = lstViewDBresults.SelectedItems[0].SubItems[0].Text;
                lblNDBNo.Text = $"USDA NDB#: {dbobjs[curDbIndex].ndbno}";
            }
            catch { }
        }

        private void assignCurObj()
        {
            try{curDbIndex = lstBoxIngrieds.SelectedIndex;
                txtTweakName.Text = dbobjs[curDbIndex].name;
                txtTweakWeight.Text = Convert.ToString(dbobjs[curDbIndex].weight);
                lblNDBNo.Text = $"USDA NDB#: {dbobjs[curDbIndex].ndbno}";
            }
            catch{}
        }

        double x = 0.4;
        private void trackGeo_Scroll(object sender, EventArgs e)
        {
            updatePer();
            updateList();
        }

        private void updateList()
        {
            int o = lstBoxIngrieds.SelectedIndex;
            lstBoxIngrieds.Items.Clear();
            for (int i = 0; i < dbobjs.Count; i++)
                lstBoxIngrieds.Items.Add($"{dbobjs[i].name} -- {dbobjs[i].percent}% -- {dbobjs[i].weight}g");
            try { lstBoxIngrieds.SelectedIndex = o; }
            catch { lstBoxIngrieds.SelectedIndex = lstBoxIngrieds.Items.Count - 1; }
        }
        private void updatePer()
        {
            if (dbobjs == null || dbobjs.Count == 0) {
            return;
            }

            x = (double)trackGeo.Value / trackGeo.Maximum;
            double q = 0;
            for (int i = 0; i < dbobjs.Count; i++)
                q += Math.Pow(x, i);
            double a = 1 / q;

            for (int i = 0; i < dbobjs.Count; i++)
            {
                double m = dbobjs[i].percent;
                dbobjs[i].percent = Math.Round(a * Math.Pow(x, i), 3) * 100;
                dbobjs[i].weight = dbobjs[i].percent;
                //dbobjs[i].weight *= dbobjs[i].percent / m;
                //dbobjs[i].weight = dbobjs[i].weight * (Math.Round(a * Math.Pow(x, i), 3) * 100 / dbobjs[i].percent);
            }
        }

        private void search()
        {
            lstViewDBresults.Items.Clear();
            DB.wMatch = new int[DB.names.Length];
            DB.joinedMatches = new string[DB.names.Length];


            string[] words;
            if (txtTweakName.TextLength < 3)
                return;
            else if (txtTweakName.Text.Split(' ').Length < 2)   //just 1 word
                words = new string[] { txtTweakName.Text };
            else
                words = txtTweakName.Text.Split(_delims);       //a sentence

            foreach (string s in words)
                for (int i = 0; i < DB.names.Length; i++)
                    if (s.Length > 2 && DB.names[i].ToUpper()/*.Split(_delims)*/.Contains(s.ToUpper()))
                        DB.wMatch[i]++; //usdaDB.joinedMatches[i] += s + ", ";     
            List<ListViewItem> itms = new List<ListViewItem>();
            for (int i = DB.wMatch.Max(); i > 0; i--)
                for (int j = 0; j < DB.names.Length; j++)
                    if (DB.wMatch[j] == i)
                    {
                        ListViewItem itm = new ListViewItem(DB.ndbs[j]);
                        itm.SubItems.Add(DB.names[j]); //$"{DB.names[j]} -- [{DB.wMatch[j]}]");
                        itm.SubItems.Add(DB.cals[j]);
                        itms.Add(itm);
                    }         
            lstViewDBresults.BeginUpdate();
            for (int i = 0; i < itms.Count; i++)
                lstViewDBresults.Items.Add(itms[i]);
            lstViewDBresults.EndUpdate();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtRecipeName.TextLength < 4)
            {
                MessageBox.Show("Error: recipe name must be at least four characters.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (char c in txtRecipeName.Text)
                if (!Char.IsLetterOrDigit(c) && !Char.IsSeparator(c))
                {
                    MessageBox.Show("Error: recipe name contains illegal characters.  Letters, digits and spaces only.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            string fp = $"{Application.StartupPath}{sl}usr{sl}profile{frmMain.currentUser.index}{sl}DBs{sl}_recipes";
            Directory.CreateDirectory(fp);
            fp += $"{sl}{txtRecipeName.Text}.TXT";

            List<string> output = new List<string>();
            foreach (_dbObj d in dbobjs)
                output.Add($"USDAstock|{d.ndbno}|{d.weight}");
            if (File.Exists(fp))
            {
                MessageBox.Show($"File already exists under this name.  Please change the name, archive it or edit it manually in your 'DBs{sl}_recipes' folder.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            File.WriteAllLines(fp, output);
            MessageBox.Show($"Saved to the recipes folder.  You can manually edit entries by opening the .TXT file in your 'DBs{sl}_recipes' folder. and following the formatting instructions.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        bool tweak;
        private void txtIngrieds_Leave(object sender, EventArgs e) => tweak = true;
        private void txtIngrieds_Enter(object sender, EventArgs e)
        {
            if (tweak)
            {
                tweak = false;
                MessageBox.Show("Warning, editing this now will reset any progress you have already made.");
            }
        }
    }
}
