using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Nutritracker
{
    public partial class frmCustomFood : Form
    {
        public frmCustomFood()
        {
            InitializeComponent();
        }

        #region funcs
        public int getBiggestInt(string s)
        {
            int j = s.Length;
            int g = 0;
            for (int i = j; i > 0; i--)
            {
                foreach (string st in substrings(s, i))
                {
                    if (int.TryParse(st, out g))
                        return g;
                }
            }
            return 0;
        }

        public double getBiggestDouble(string s)
        {
            s = Regex.Replace(s, "[^0-9.]", "");
            double d;
            try { d = Convert.ToDouble(s); return d; }
            catch { return 0.0; }
        }

        public string[] substrings(string s, int n)
        {
            string[] sr = new string[s.Length - n + 1];

            for (int i = 0; i < s.Length - n + 1; i++)
            {
                sr[i] = s.Substring(i, n).ToLower();
            }

            return sr;
        }

        void changeLine(RichTextBox RTB, int line, string text)
        {
            int s1 = RTB.GetFirstCharIndexFromLine(line);
            int s2 = line < RTB.Lines.Count() - 1 ?
                      RTB.GetFirstCharIndexFromLine(line + 1) - 1 :
                      RTB.Text.Length;
            RTB.Select(s1, s2 - s1);
            RTB.SelectedText = text;
        }
        private string getUnits(string input)
        {
            input = input.ToLower();
            if (input.Contains("mg"))
                return "mg";
            else if (input.Contains("ug"))
                return "ug";
            else if (input.Contains("iu"))
                return "iu";
            else if (input.Contains(" g"))
                return "g";
            else if (input.Contains("cal"))
                return "cal";
            else if (input.Contains("%"))
                return "%";
            else
                return "none";
        }
                
        public List<String> importArray(string filename)
        {
            list.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to list.
                }
            }
            return list;
        }

        List<string> list = new List<string>();
        #endregion

        string slash = Path.DirectorySeparatorChar.ToString();        
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsSeparator(e.KeyChar) && !Char.IsLetterOrDigit(e.KeyChar))
                e.Handled = true;
        }
        
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboNDB.Text) < 94000)
                comboNDB.Text = "94000";
            q = Convert.ToInt32(comboNDB.Text);

            try { dbNum = importArray($"{fr}{slash}dbNum.TXT"); }
            catch
            { return; }
            for (int i = 0; i < dbNum.Count; i++)
                if (i == q - 94000)
                {
                    comboFoodName.Text = importArray($"{fr}{slash}names.TXT").ToArray()[i];
                    txtGrams.Text = importArray($"{fr}{slash}srvngSz.TXT").ToArray()[i];

                    macros1[0] = importArray($"{fr}{slash}cals.TXT").ToArray()[i];
                    macros1[1] = importArray($"{fr}{slash}fat.TXT").ToArray()[i];
                    macros1[2] = importArray($"{fr}{slash}fatsat.TXT").ToArray()[i];
                    macros1[3] = importArray($"{fr}{slash}fattrans.TXT").ToArray()[i];
                    macros1[4] = importArray($"{fr}{slash}fatpoly.TXT").ToArray()[i];
                    macros1[5] = importArray($"{fr}{slash}fatmono.TXT").ToArray()[i];
                    
                    macros2[0] = importArray($"{fr}{slash}cholest.TXT").ToArray()[i];
                    macros2[1] = importArray($"{fr}{slash}sodium.TXT").ToArray()[i];
                    macros2[2] = importArray($"{fr}{slash}potassium.TXT").ToArray()[i];
                    macros2[3] = importArray($"{fr}{slash}carbs.TXT").ToArray()[i];
                    macros2[4] = importArray($"{fr}{slash}fiber.TXT").ToArray()[i];
                    macros2[5] = importArray($"{fr}{slash}sugar.TXT").ToArray()[i];
                    macros2[6] = importArray($"{fr}{slash}protein.TXT").ToArray()[i];

                    vits[0] = importArray($"{fr}{slash}vitA.TXT").ToArray()[i];
                    vits[1] = importArray($"{fr}{slash}vitC.TXT").ToArray()[i];
                    vits[2] = importArray($"{fr}{slash}vitD.TXT").ToArray()[i];
                    vits[3] = importArray($"{fr}{slash}vitE.TXT").ToArray()[i];
                    vits[4] = importArray($"{fr}{slash}vitK.TXT").ToArray()[i];
                    vits[5] = importArray($"{fr}{slash}B1.TXT").ToArray()[i];
                    vits[6] = importArray($"{fr}{slash}B2.TXT").ToArray()[i];
                    vits[7] = importArray($"{fr}{slash}B3.TXT").ToArray()[i];
                    vits[8] = importArray($"{fr}{slash}B5.TXT").ToArray()[i];
                    vits[9] = importArray($"{fr}{slash}B6.TXT").ToArray()[i];
                    vits[10] = importArray($"{fr}{slash}B7.TXT").ToArray()[i];
                    vits[11] = importArray($"{fr}{slash}B9.TXT").ToArray()[i];
                    vits[12] = importArray($"{fr}{slash}B12.TXT").ToArray()[i];

                    mins[0] = importArray($"{fr}{slash}Fe.TXT").ToArray()[i];
                    mins[1] = importArray($"{fr}{slash}Ca.TXT").ToArray()[i];
                    mins[2] = importArray($"{fr}{slash}Mg.TXT").ToArray()[i];
                    mins[3] = importArray($"{fr}{slash}Zn.TXT").ToArray()[i];
                    mins[4] = importArray($"{fr}{slash}Se.TXT").ToArray()[i];
                    mins[5] = importArray($"{fr}{slash}B.TXT").ToArray()[i];
                    mins[6] = importArray($"{fr}{slash}I.TXT").ToArray()[i];
                    mins[7] = importArray($"{fr}{slash}P.TXT").ToArray()[i];
                    mins[8] = importArray($"{fr}{slash}Mn.TXT").ToArray()[i];
                    mins[9] = importArray($"{fr}{slash}Fl.TXT").ToArray()[i];
                    mins[10] = importArray($"{fr}{slash}Cu.TXT").ToArray()[i];
                    mins[11] = importArray($"{fr}{slash}Cr.TXT").ToArray()[i];
                    mins[12] = importArray($"{fr}{slash}Mo.TXT").ToArray()[i];


                    richtextmacros1.Clear();
                    for (int j = 0; j < macros1.Length; j++)
                    {
                        richtextmacros1.Text += macros1[j];
                        if (j == macros1.Length - 1)
                            break;
                        richtextmacros1.Text += '\n';
                    }

                    richtextmacros2.Clear();
                    for (int j = 0; j < macros2.Length; j++)
                    {
                        richtextmacros2.Text += macros2[j];
                        if (j == macros2.Length - 1)
                            break;
                        richtextmacros2.Text += '\n';
                    }

                    richtextvits.Clear();
                    for (int j = 0; j < vits.Length; j++)
                    {
                        richtextvits.Text += vits[j];
                        if (j == vits.Length - 1)
                            break;
                        richtextvits.Text += '\n';
                    }

                    richtextmins.Clear();
                    for (int j = 0; j < mins.Length; j++)
                    {
                        richtextmins.Text += mins[j];
                        if (j == mins.Length - 1)
                            break;
                        richtextmins.Text += '\n';
                    }
                    return;
                }
        }

        #region gramsText, keyhandling
        private void txtGrams_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
            if (txtGrams.Text == "0 g")
            {
                txtGrams.SelectionStart = 0;
                txtGrams.SelectionLength = getBiggestInt(txtGrams.Text).ToString().Length;
            }
        }

        int n = 0;
        bool busy = false;
        private void txtGrams_TextChanged(object sender, EventArgs e)
        {
            if (txtGrams.Text == " g")
            {
                txtGrams.Text = "1 g";
                txtGrams.SelectionStart = 0;
                txtGrams.SelectionLength = 1;
                return;
            }
            if (busy)
                return;
            n = txtGrams.SelectionStart;
            int g = getBiggestInt(txtGrams.Text);
            if (g == 0)
                g++;
            busy = true;
            txtGrams.Text = g.ToString() + " g";
            busy = false;
            txtGrams.SelectionStart = n;
        }

        private void txtGrams_Enter(object sender, EventArgs e)
        {
            txtGrams.SelectionStart = 0;
            txtGrams.SelectionLength = getBiggestInt(txtGrams.Text).ToString().Length;
        }
        
        private void txtGrams_MouseDown(object sender, MouseEventArgs e)
        {
            txtGrams.SelectionStart = 0;
            txtGrams.SelectionLength = getBiggestInt(txtGrams.Text).ToString().Length;
        }

        private void txtGrams_MouseUp(object sender, MouseEventArgs e)
        {
            txtGrams.SelectionStart = 0;
            txtGrams.SelectionLength = getBiggestInt(txtGrams.Text).ToString().Length;
        }
        #endregion


        string startingMacros1;
        string startingMacros2;
        string startingVits;
        string startingMins;

        int q = 94000;
        string fr;
        string fp;

        string[] autocomp;
        private void frmCustomFood_Load(object sender, EventArgs e)
        {
            
            startingMacros1 = richtextmacros1.Text;
            startingMacros2 = richtextmacros2.Text;
            startingVits = richtextvits.Text;
            startingMins = richtextmins.Text;

            fr = Application.StartupPath + $"{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}_foods";

            //q = Convert.ToInt32(comboBox1.Text);

            macros1 = richtextmacros1.Text.Split('\n');
            macros2 = richtextmacros2.Text.Split('\n');
            vits = richtextvits.Text.Split('\n');
            mins = richtextmins.Text.Split('\n');
            fp = $"{fr}{slash}dbNum.TXT";

            if (!File.Exists(fp))
            {
                #region create first files
                File.CreateText($"{fr}{slash}dbNum.TXT").Close();
                File.WriteAllText($"{fr}{slash}dbNum.TXT", q.ToString());

                File.CreateText($"{fr}{slash}names.TXT").Close();
                File.WriteAllText($"{fr}{slash}names.TXT", "UNTITLED" + q.ToString());

                File.CreateText($"{fr}{slash}srvngSz.TXT").Close();
                File.WriteAllText($"{fr}{slash}srvngSz.TXT", txtGrams.Text);

                File.CreateText($"{fr}{slash}cals.TXT").Close();
                File.WriteAllText($"{fr}{slash}cals.TXT", macros1[0]);

                File.CreateText($"{fr}{slash}fat.TXT").Close();
                File.WriteAllText($"{fr}{slash}fat.TXT", macros1[1]);

                File.CreateText($"{fr}{slash}fatsat.TXT").Close();
                File.WriteAllText($"{fr}{slash}fatsat.TXT", macros1[2]);

                File.CreateText($"{fr}{slash}fattrans.TXT").Close();
                File.WriteAllText($"{fr}{slash}fattrans.TXT", macros1[3]);

                File.CreateText($"{fr}{slash}fatpoly.TXT").Close();
                File.WriteAllText($"{fr}{slash}fatpoly.TXT", macros1[4]);

                File.CreateText($"{fr}{slash}fatmono.TXT").Close();
                File.WriteAllText($"{fr}{slash}fatmono.TXT", macros1[5]);

                File.CreateText($"{fr}{slash}cholest.TXT").Close();
                File.WriteAllText($"{fr}{slash}cholest.TXT", macros2[0]);

                File.CreateText($"{fr}{slash}sodium.TXT").Close();
                File.WriteAllText($"{fr}{slash}sodium.TXT", macros2[1]);

                File.CreateText($"{fr}{slash}potassium.TXT").Close();
                File.WriteAllText($"{fr}{slash}potassium.TXT", macros2[2]);

                File.CreateText($"{fr}{slash}carbs.TXT").Close();
                File.WriteAllText($"{fr}{slash}carbs.TXT", macros2[3]);
                                
                File.CreateText($"{fr}{slash}fiber.TXT").Close();
                File.WriteAllText($"{fr}{slash}fiber.TXT", macros2[4]);

                File.CreateText($"{fr}{slash}sugar.TXT").Close();
                File.WriteAllText($"{fr}{slash}sugar.TXT", macros2[5]);

                File.CreateText($"{fr}{slash}protein.TXT").Close();
                File.WriteAllText($"{fr}{slash}protein.TXT", macros1[6]);

                //

                File.CreateText($"{fr}{slash}vitA.TXT").Close();
                File.WriteAllText($"{fr}{slash}vitA.TXT", vits[0]);

                File.CreateText($"{fr}{slash}vitC.TXT").Close();
                File.WriteAllText($"{fr}{slash}vitC.TXT", vits[1]);

                File.CreateText($"{fr}{slash}vitD.TXT").Close();
                File.WriteAllText($"{fr}{slash}vitD.TXT", vits[2]);

                File.CreateText($"{fr}{slash}vitE.TXT").Close();
                File.WriteAllText($"{fr}{slash}vitE.TXT", vits[3]);

                File.CreateText($"{fr}{slash}vitK.TXT").Close();
                File.WriteAllText($"{fr}{slash}vitK.TXT", vits[4]);

                File.CreateText($"{fr}{slash}B1.TXT").Close();
                File.WriteAllText($"{fr}{slash}B1.TXT", vits[5]);

                File.CreateText($"{fr}{slash}B2.TXT").Close();
                File.WriteAllText($"{fr}{slash}B2.TXT", vits[6]);

                File.CreateText($"{fr}{slash}B3.TXT").Close();
                File.WriteAllText($"{fr}{slash}B3.TXT", vits[7]);

                File.CreateText($"{fr}{slash}B5.TXT").Close();
                File.WriteAllText($"{fr}{slash}B5.TXT", vits[8]);

                File.CreateText($"{fr}{slash}B6.TXT").Close();
                File.WriteAllText($"{fr}{slash}B6.TXT", vits[9]);

                File.CreateText($"{fr}{slash}B7.TXT").Close();
                File.WriteAllText($"{fr}{slash}B7.TXT", vits[10]);

                File.CreateText($"{fr}{slash}B9.TXT").Close();
                File.WriteAllText($"{fr}{slash}B9.TXT", vits[11]);

                File.CreateText($"{fr}{slash}B12.TXT").Close();
                File.WriteAllText($"{fr}{slash}B12.TXT", vits[12]);

                //

                File.CreateText($"{fr}{slash}Fe.TXT").Close();
                File.WriteAllText($"{fr}{slash}Fe.TXT", mins[0]);

                File.CreateText($"{fr}{slash}Ca.TXT").Close();
                File.WriteAllText($"{fr}{slash}Ca.TXT", mins[1]);

                File.CreateText($"{fr}{slash}Mg.TXT").Close();
                File.WriteAllText($"{fr}{slash}Mg.TXT", mins[2]);

                File.CreateText($"{fr}{slash}Zn.TXT").Close();
                File.WriteAllText($"{fr}{slash}Zn.TXT", mins[3]);

                File.CreateText($"{fr}{slash}Se.TXT").Close();
                File.WriteAllText($"{fr}{slash}Se.TXT", mins[4]);

                File.CreateText($"{fr}{slash}B.TXT").Close();
                File.WriteAllText($"{fr}{slash}B.TXT", mins[5]);

                File.CreateText($"{fr}{slash}I.TXT").Close();
                File.WriteAllText($"{fr}{slash}I.TXT", mins[6]);

                File.CreateText($"{fr}{slash}P.TXT").Close();
                File.WriteAllText($"{fr}{slash}P.TXT", mins[7]);

                File.CreateText($"{fr}{slash}Mn.TXT").Close();
                File.WriteAllText($"{fr}{slash}Mn.TXT", mins[8]);

                File.CreateText($"{fr}{slash}Fl.TXT").Close();
                File.WriteAllText($"{fr}{slash}Fl.TXT", mins[9]);

                File.CreateText($"{fr}{slash}Cu.TXT").Close();
                File.WriteAllText($"{fr}{slash}Cu.TXT", mins[10]);

                File.CreateText($"{fr}{slash}Cr.TXT").Close();
                File.WriteAllText($"{fr}{slash}Cr.TXT", mins[11]);

                File.CreateText($"{fr}{slash}Mo.TXT").Close();
                File.WriteAllText($"{fr}{slash}Mo.TXT", mins[12]);
                #endregion
                comboNDB.Items.Add("94000");
                comboNDB.Text = "94000";


                autocomp = new string[1];
                autocomp[0] = "UNTITLED94000";
                comboFoodName.Items.Add(autocomp[0]);
                comboFoodName.Text = "UNTITLED94000";

                var source = new AutoCompleteStringCollection();
                source.AddRange(autocomp);
                comboFoodName.AutoCompleteCustomSource = source;
                comboFoodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboFoodName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            else
            {
                //lines = importArray(fp).ToArray();

                comboNDB.Items.Clear();
                dbNum = importArray($"{fr}{slash}dbNum.TXT");
                for (int i = 0; i < dbNum.Count; i++)
                    comboNDB.Items.Add(dbNum[i]);

                //q = comboBox1.Items.Count;
                autocomp = new string[comboNDB.Items.Count];
                for (int i = 0; i < comboNDB.Items.Count; i++)
                {
                    autocomp[i] = importArray($"{fr}{slash}names.TXT").ToArray()[i];
                    comboFoodName.Items.Add(autocomp[i]);
                }


                var source = new AutoCompleteStringCollection();
                source.AddRange(autocomp);
                comboFoodName.AutoCompleteCustomSource = source;
                comboFoodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboFoodName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                q = Convert.ToInt32(comboNDB.Text);


                for (int i = 0; i < dbNum.Count; i++)
                    if (i == q - 94000)
                    {
                        comboFoodName.Text = importArray($"{fr}{slash}names.TXT").ToArray()[i];
                        txtGrams.Text = importArray($"{fr}{slash}srvngSz.TXT").ToArray()[i];

                        macros1[0] = importArray($"{fr}{slash}cals.TXT").ToArray()[i];
                        macros1[1] = importArray($"{fr}{slash}fat.TXT").ToArray()[i];
                        macros1[2] = importArray($"{fr}{slash}fatsat.TXT").ToArray()[i];
                        macros1[3] = importArray($"{fr}{slash}fattrans.TXT").ToArray()[i];
                        macros1[4] = importArray($"{fr}{slash}fatpoly.TXT").ToArray()[i];
                        macros1[5] = importArray($"{fr}{slash}fatmono.TXT").ToArray()[i];

                        macros2[0] = importArray($"{fr}{slash}cholest.TXT").ToArray()[i];
                        macros2[1] = importArray($"{fr}{slash}sodium.TXT").ToArray()[i];
                        macros2[2] = importArray($"{fr}{slash}potassium.TXT").ToArray()[i];
                        macros2[3] = importArray($"{fr}{slash}carbs.TXT").ToArray()[i];
                        macros2[4] = importArray($"{fr}{slash}sugar.TXT").ToArray()[i];
                        macros2[5] = importArray($"{fr}{slash}fiber.TXT").ToArray()[i];
                        macros2[6] = importArray($"{fr}{slash}protein.TXT").ToArray()[i];


                        vits[0] = importArray($"{fr}{slash}vitA.TXT").ToArray()[i];
                        vits[1] = importArray($"{fr}{slash}vitC.TXT").ToArray()[i];
                        vits[2] = importArray($"{fr}{slash}vitD.TXT").ToArray()[i];
                        vits[3] = importArray($"{fr}{slash}vitE.TXT").ToArray()[i];
                        vits[4] = importArray($"{fr}{slash}vitK.TXT").ToArray()[i];
                        vits[5] = importArray($"{fr}{slash}B1.TXT").ToArray()[i];
                        vits[6] = importArray($"{fr}{slash}B2.TXT").ToArray()[i];
                        vits[7] = importArray($"{fr}{slash}B3.TXT").ToArray()[i];
                        vits[8] = importArray($"{fr}{slash}B5.TXT").ToArray()[i];
                        vits[9] = importArray($"{fr}{slash}B6.TXT").ToArray()[i];
                        vits[10] = importArray($"{fr}{slash}B7.TXT").ToArray()[i];
                        vits[11] = importArray($"{fr}{slash}B9.TXT").ToArray()[i];
                        vits[12] = importArray($"{fr}{slash}B12.TXT").ToArray()[i];

                        mins[0] = importArray($"{fr}{slash}Fe.TXT").ToArray()[i];
                        mins[1] = importArray($"{fr}{slash}Ca.TXT").ToArray()[i];
                        mins[2] = importArray($"{fr}{slash}Mg.TXT").ToArray()[i];
                        mins[3] = importArray($"{fr}{slash}Zn.TXT").ToArray()[i];
                        mins[4] = importArray($"{fr}{slash}Se.TXT").ToArray()[i];
                        mins[5] = importArray($"{fr}{slash}B.TXT").ToArray()[i];
                        mins[6] = importArray($"{fr}{slash}I.TXT").ToArray()[i];
                        mins[7] = importArray($"{fr}{slash}P.TXT").ToArray()[i];
                        mins[8] = importArray($"{fr}{slash}Mn.TXT").ToArray()[i];
                        mins[9] = importArray($"{fr}{slash}Fl.TXT").ToArray()[i];
                        mins[10] = importArray($"{fr}{slash}Cu.TXT").ToArray()[i];
                        mins[11] = importArray($"{fr}{slash}Cr.TXT").ToArray()[i];
                        mins[12] = importArray($"{fr}{slash}Mo.TXT").ToArray()[i];


                        richtextmacros1.Clear();
                        for (int j = 0; j < macros1.Length; j++)
                        {
                            richtextmacros1.Text += macros1[j];
                            if (j == macros1.Length - 1)
                                break;
                            richtextmacros1.Text += '\n';
                        }

                        richtextmacros2.Clear();
                        for (int j = 0; j < macros2.Length; j++)
                        {
                            richtextmacros2.Text += macros2[j];
                            if (j == macros2.Length - 1)
                                break;
                            richtextmacros2.Text += '\n';
                        }

                        richtextvits.Clear();
                        for (int j = 0; j < vits.Length; j++)
                        {
                            richtextvits.Text += vits[j];
                            if (j == vits.Length - 1)
                                break;
                            richtextvits.Text += '\n';
                        }

                        richtextmins.Clear();
                        for (int j = 0; j < mins.Length; j++)
                        {
                            richtextmins.Text += mins[j];
                            if (j == mins.Length - 1)
                                break;
                            richtextmins.Text += '\n';
                        }
                        return;
                    }
            }
        }



        List<string> dbNum;
        List<string> names;
        List<string> srvngSz;

        List<string> cals;
        List<string> protein;
        List<string> fat;
        List<string> fatsat;
        List<string> fattrans;
        List<string> fatpoly;
        List<string> fatmono;
        List<string> carbs;
        List<string> sugar;
        List<string> fiber;
        List<string> cholest;
        List<string> sodium;
        List<string> potassium;

        List<string> vitA;
        List<string> vitC;
        List<string> vitD;
        List<string> vitE;
        List<string> vitK;
        List<string> B1;
        List<string> B2;
        List<string> B3;
        List<string> B5;
        List<string> B6;
        List<string> B7;
        List<string> B9;
        List<string> B12;
        
        //List<string> K;
        List<string> Mg;
        List<string> Ca;
        List<string> Zn;
        List<string> Se;
        List<string> B;
        List<string> Fe;
        List<string> I;
        List<string> P;
        List<string> Mn;
        List<string> Fl;
        List<string> Cu;
        List<string> Cr;
        List<string> Mo;

        //
        string[] macros1;
        string[] macros2;
        string[] vits;
        string[] mins;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            q = Convert.ToInt32(comboNDB.Text);

            fp = $"{fr}{slash}dbNum.TXT";
            lines = importArray(fp).ToArray();
            q = Convert.ToInt32(lines[lines.Length - 1]) + 1;

            #region import old lists, append new blank entry, save new lists
            dbNum = importArray(fp);
            dbNum.Add(q.ToString());
            File.WriteAllLines($"{fr}{slash}dbNum.TXT", dbNum);

            names = importArray($"{fr}{slash}names.TXT");
            names.Add("UNTITLED" + q.ToString());
            File.WriteAllLines($"{fr}{slash}names.TXT", names);

            srvngSz = importArray($"{fr}{slash}srvngSz.TXT");
            srvngSz.Add("100 g");            
            File.WriteAllLines($"{fr}{slash}srvngSz.TXT", srvngSz);

            //

            cals = importArray($"{fr}{slash}cals.TXT");
            cals.Add("0 cal");
            File.WriteAllLines($"{fr}{slash}cals.TXT", cals);
            

            fat = importArray($"{fr}{slash}fat.TXT");
            fat.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fat.TXT", fat);

            fatsat = importArray($"{fr}{slash}fatsat.TXT");
            fatsat.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fatsat.TXT", fatsat);

            fattrans = importArray($"{fr}{slash}fattrans.TXT");
            fattrans.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fattrans.TXT", fattrans);

            fatpoly = importArray($"{fr}{slash}fatpoly.TXT");
            fatpoly.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fatpoly.TXT", fatpoly);

            fatmono = importArray($"{fr}{slash}fatmono.TXT");
            fatmono.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fatmono.TXT", fatmono);

            //

            cholest = importArray($"{fr}{slash}cholest.TXT");
            cholest.Add("0 mg");
            File.WriteAllLines($"{fr}{slash}cholest.TXT", cholest);

            sodium = importArray($"{fr}{slash}sodium.TXT");
            sodium.Add("0 mg");
            File.WriteAllLines($"{fr}{slash}sodium.TXT", sodium);

            potassium = importArray($"{fr}{slash}potassium.TXT");
            potassium.Add("0 mg");
            File.WriteAllLines($"{fr}{slash}potassium.TXT", potassium);

            carbs = importArray($"{fr}{slash}carbs.TXT");
            carbs.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}carbs.TXT", carbs);

            fiber = importArray($"{fr}{slash}fiber.TXT");
            fiber.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}fiber.TXT", fiber);

            sugar = importArray($"{fr}{slash}sugar.TXT");
            sugar.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}sugar.TXT", sugar);

            protein = importArray($"{fr}{slash}protein.TXT");
            protein.Add("0.0 g");
            File.WriteAllLines($"{fr}{slash}protein.TXT", protein);

            //                            
            
            vitA = importArray($"{fr}{slash}vitA.TXT");
            vitA.Add("0 %");
            File.WriteAllLines($"{fr}{slash}vitA.TXT", vitA);

            vitC = importArray($"{fr}{slash}vitC.TXT");
            vitC.Add("0 %");
            File.WriteAllLines($"{fr}{slash}vitC.TXT", vitC);

            vitD = importArray($"{fr}{slash}vitD.TXT");
            vitD.Add("0 %");
            File.WriteAllLines($"{fr}{slash}vitD.TXT", vitD);

            vitE = importArray($"{fr}{slash}vitE.TXT");
            vitE.Add("0 %");
            File.WriteAllLines($"{fr}{slash}vitE.TXT", vitE);

            vitK = importArray($"{fr}{slash}vitK.TXT");
            vitK.Add("0 %");
            File.WriteAllLines($"{fr}{slash}vitK.TXT", vitK);

            B1 = importArray($"{fr}{slash}B1.TXT");
            B1.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B1.TXT", B1);

            B2 = importArray($"{fr}{slash}B2.TXT");
            B2.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B2.TXT", B2);

            B3 = importArray($"{fr}{slash}B3.TXT");
            B3.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B3.TXT", B3);

            B5 = importArray($"{fr}{slash}B5.TXT");
            B5.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B5.TXT", B5);

            B6 = importArray($"{fr}{slash}B6.TXT");
            B6.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B6.TXT", B6);

            B7 = importArray($"{fr}{slash}B7.TXT");
            B7.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B7.TXT", B7);

            B9 = importArray($"{fr}{slash}B9.TXT");
            B9.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B9.TXT", B9);

            B12 = importArray($"{fr}{slash}B12.TXT");
            B12.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B12.TXT", B12);

            //

            Fe = importArray($"{fr}{slash}Fe.TXT");
            Fe.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Fe.TXT", Fe);

            Ca = importArray($"{fr}{slash}Ca.TXT");
            Ca.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Ca.TXT", Ca);

            Mg = importArray($"{fr}{slash}Mg.TXT");
            Mg.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Mg.TXT", Mg);

            Zn = importArray($"{fr}{slash}Zn.TXT");
            Zn.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Zn.TXT", Zn);

            Se = importArray($"{fr}{slash}Se.TXT");
            Se.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Se.TXT", Se);

            B = importArray($"{fr}{slash}B.TXT");
            B.Add("0 %");
            File.WriteAllLines($"{fr}{slash}B.TXT", B);

            I = importArray($"{fr}{slash}I.TXT");
            I.Add("0 %");
            File.WriteAllLines($"{fr}{slash}I.TXT", I);

            P = importArray($"{fr}{slash}P.TXT");
            P.Add("0 %");
            File.WriteAllLines($"{fr}{slash}P.TXT", P);

            Mn = importArray($"{fr}{slash}Mn.TXT");
            Mn.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Mn.TXT", Mn);
            
            Fl = importArray($"{fr}{slash}Fl.TXT");
            Fl.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Fl.TXT", Fl);

            Cu = importArray($"{fr}{slash}Cu.TXT");
            Cu.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Cu.TXT", Cu);

            Cr = importArray($"{fr}{slash}Cr.TXT");
            Cr.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Cr.TXT", Cr);

            Mo = importArray($"{fr}{slash}Mo.TXT");
            Mo.Add("0 %");
            File.WriteAllLines($"{fr}{slash}Mo.TXT", Mo);
            #endregion

            richtextmacros1.Clear();
            richtextmacros1.Text = startingMacros1;
            richtextmacros2.Clear();
            richtextmacros2.Text = startingMacros2;

            richtextvits.Clear();
            richtextvits.Text = startingVits;
            richtextmins.Clear();
            richtextmins.Text = startingMins;

            comboNDB.Items.Clear();
            for (int i = 94000; i <= q; i++)            
                comboNDB.Items.Add(i.ToString());

            autocomp = new string[comboNDB.Items.Count];
            names = importArray($"{fr}{slash}names.TXT");
            for (int i = 0; i < comboNDB.Items.Count; i++)
                autocomp[i] = names[i];            
            var source = new AutoCompleteStringCollection();
            
            source.AddRange(autocomp);
            comboFoodName.AutoCompleteCustomSource = source;
            comboFoodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboFoodName.AutoCompleteMode = AutoCompleteMode.Suggest;
            
            comboNDB.Text = q.ToString(); //sets new dbNum   

            comboFoodName.SelectAll();
            comboFoodName.Focus();
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            q = comboNDB.Items.Count;
            int m = comboNDB.SelectedIndex;
            if (q < 2)
            {
                MessageBox.Show("Cannot delete any more..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                comboFoodName.SelectAll();
                comboFoodName.Focus();
                return;
            }
            //int m = q - 94000;
            if (MessageBox.Show("Are you sure?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                #region delete one row, shift back
                dbNum = importArray(fp);
                dbNum.RemoveAt(m);
                for (int i = m; i < dbNum.Count; i++)
                    dbNum[i] = (Convert.ToInt32(dbNum[i]) - 1).ToString();
                File.WriteAllLines($"{fr}{slash}dbNum.TXT", dbNum);
                
                names = importArray($"{fr}{slash}names.TXT");
                names.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}names.TXT", names);

                srvngSz = importArray($"{fr}{slash}srvngSz.TXT");
                srvngSz.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}srvngSz.TXT", srvngSz);

                //

                cals = importArray($"{fr}{slash}cals.TXT");
                cals.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}cals.TXT", cals);



                fat = importArray($"{fr}{slash}fat.TXT");
                fat.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fat.TXT", fat);

                fatsat = importArray($"{fr}{slash}fatsat.TXT");
                fatsat.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fatsat.TXT", fatsat);

                fattrans = importArray($"{fr}{slash}fattrans.TXT");
                fattrans.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fattrans.TXT", fattrans);

                fatpoly = importArray($"{fr}{slash}fatpoly.TXT");
                fatpoly.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fatpoly.TXT", fatpoly);

                fatmono = importArray($"{fr}{slash}fatmono.TXT");
                fatmono.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fatmono.TXT", fatmono);
                
                cholest = importArray($"{fr}{slash}cholest.TXT");
                cholest.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}cholest.TXT", cholest);

                sodium = importArray($"{fr}{slash}sodium.TXT");
                sodium.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}sodium.TXT", sodium);

                potassium = importArray($"{fr}{slash}potassium.TXT");
                potassium.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}potassium.TXT", potassium);

                carbs = importArray($"{fr}{slash}carbs.TXT");
                carbs.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}carbs.TXT", carbs);

                fiber = importArray($"{fr}{slash}fiber.TXT");
                fiber.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}fiber.TXT", fiber);

                sugar = importArray($"{fr}{slash}sugar.TXT");
                sugar.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}sugar.TXT", sugar);

                protein = importArray($"{fr}{slash}protein.TXT");
                protein.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}protein.TXT", protein);

                //                            

                vitA = importArray($"{fr}{slash}vitA.TXT");
                vitA.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}vitA.TXT", vitA);

                vitC = importArray($"{fr}{slash}vitC.TXT");
                vitC.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}vitC.TXT", vitC);

                vitD = importArray($"{fr}{slash}vitD.TXT");
                vitD.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}vitD.TXT", vitD);

                vitE = importArray($"{fr}{slash}vitE.TXT");
                vitE.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}vitE.TXT", vitE);

                vitK = importArray($"{fr}{slash}vitK.TXT");
                vitK.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}vitK.TXT", vitK);

                B1 = importArray($"{fr}{slash}B1.TXT");
                B1.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B1.TXT", B1);

                B2 = importArray($"{fr}{slash}B2.TXT");
                B2.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B2.TXT", B2);

                B3 = importArray($"{fr}{slash}B3.TXT");
                B3.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B3.TXT", B3);

                B5 = importArray($"{fr}{slash}B5.TXT");
                B5.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B5.TXT", B5);

                B6 = importArray($"{fr}{slash}B6.TXT");
                B6.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B6.TXT", B6);

                B7 = importArray($"{fr}{slash}B7.TXT");
                B7.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B7.TXT", B7);

                B9 = importArray($"{fr}{slash}B9.TXT");
                B9.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B9.TXT", B9);

                B12 = importArray($"{fr}{slash}B12.TXT");
                B12.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B12.TXT", B12);

                //
                
                Fe = importArray($"{fr}{slash}Fe.TXT");
                Fe.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Fe.TXT", Fe);

                Ca = importArray($"{fr}{slash}Ca.TXT");
                Ca.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Ca.TXT", Ca);

                Mg = importArray($"{fr}{slash}Mg.TXT");
                Mg.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Mg.TXT", Mg);
                
                Zn = importArray($"{fr}{slash}Zn.TXT");
                Zn.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Zn.TXT", Zn);

                Se = importArray($"{fr}{slash}Se.TXT");
                Se.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Se.TXT", Se);

                B = importArray($"{fr}{slash}B.TXT");
                B.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}B.TXT", B);

                I = importArray($"{fr}{slash}I.TXT");
                I.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}I.TXT", I);

                P = importArray($"{fr}{slash}P.TXT");
                P.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}P.TXT", P);

                Mn = importArray($"{fr}{slash}Mn.TXT");
                Mn.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Mn.TXT", Mn);
                
                Fl = importArray($"{fr}{slash}Fl.TXT");
                Fl.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Fl.TXT", Fl);

                Cu = importArray($"{fr}{slash}Cu.TXT");
                Cu.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Cu.TXT", Cu);

                Cr = importArray($"{fr}{slash}Cr.TXT");
                Cr.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Cr.TXT", Cr);

                Mo = importArray($"{fr}{slash}Mo.TXT");
                Mo.RemoveAt(m);
                File.WriteAllLines($"{fr}{slash}Mo.TXT", Mo);
                #endregion
            }

            comboNDB.Items.Clear();
            q--;
            for (int i = 0; i < q; i++)
                comboNDB.Items.Add((i + 94000).ToString());
            comboNDB.Text = q.ToString();

            autocomp = new string[comboNDB.Items.Count];
            names = importArray($"{fr}{slash}names.TXT");
            for (int i = 0; i < comboNDB.Items.Count; i++)
                autocomp[i] = names[i];            
            var source = new AutoCompleteStringCollection();

            comboFoodName.Items.Clear();
            autocomp = new string[comboNDB.Items.Count];
            for (int i = 0; i < comboNDB.Items.Count; i++)
            {
                autocomp[i] = importArray($"{fr}{slash}names.TXT").ToArray()[i];
                comboFoodName.Items.Add(autocomp[i]);
            }

            source.AddRange(autocomp);
            comboFoodName.AutoCompleteCustomSource = source;
            comboFoodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboFoodName.AutoCompleteMode = AutoCompleteMode.Suggest;

            comboFoodName.SelectAll();
            comboFoodName.Focus();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            macros1 = richtextmacros1.Text.Split('\n');
            macros2 = richtextmacros2.Text.Split('\n');
            vits = richtextvits.Text.Split('\n');
            mins = richtextmins.Text.Split('\n');

            q = Convert.ToInt32(comboNDB.Text);
            
            int m = q - 94000;

            #region modify existing row, rewrite whole stack

            names = importArray($"{fr}{slash}names.TXT");
            names.RemoveAt(m);
            names.Insert(m, comboFoodName.Text);
            File.WriteAllLines($"{fr}{slash}names.TXT", names);

            srvngSz = importArray($"{fr}{slash}srvngSz.TXT");
            srvngSz.RemoveAt(m);
            srvngSz.Insert(m, txtGrams.Text);
            File.WriteAllLines($"{fr}{slash}srvngSz.TXT", srvngSz);

            //

            cals = importArray($"{fr}{slash}cals.TXT");
            cals.RemoveAt(m);
            cals.Insert(m, macros1[0]);
            File.WriteAllLines($"{fr}{slash}cals.TXT", cals);
            
            fat = importArray($"{fr}{slash}fat.TXT");
            fat.RemoveAt(m);
            fat.Insert(m, macros1[1]);
            File.WriteAllLines($"{fr}{slash}fat.TXT", fat);

            fatsat = importArray($"{fr}{slash}fatsat.TXT");
            fatsat.RemoveAt(m);
            fatsat.Insert(m, macros1[2]);
            File.WriteAllLines($"{fr}{slash}fatsat.TXT", fatsat);

            fattrans = importArray($"{fr}{slash}fattrans.TXT");
            fattrans.RemoveAt(m);
            fattrans.Insert(m, macros1[3]);
            File.WriteAllLines($"{fr}{slash}fattrans.TXT", fattrans);

            fatpoly = importArray($"{fr}{slash}fatpoly.TXT");
            fatpoly.RemoveAt(m);
            fatpoly.Insert(m, macros1[4]);
            File.WriteAllLines($"{fr}{slash}fatpoly.TXT", fatpoly);

            fatmono = importArray($"{fr}{slash}fatmono.TXT");
            fatmono.RemoveAt(m);
            fatmono.Insert(m, macros1[5]);
            File.WriteAllLines($"{fr}{slash}fatmono.TXT", fatmono);

            cholest = importArray($"{fr}{slash}cholest.TXT");
            cholest.RemoveAt(m);
            cholest.Insert(m, macros2[0]);
            File.WriteAllLines($"{fr}{slash}cholest.TXT", cholest);
            
            sodium = importArray($"{fr}{slash}sodium.TXT");
            sodium.RemoveAt(m);
            sodium.Insert(m, macros2[1]);
            File.WriteAllLines($"{fr}{slash}sodium.TXT", sodium);
                        
            potassium = importArray($"{fr}{slash}potassium.TXT");
            potassium.RemoveAt(m);
            potassium.Insert(m, macros2[2]);
            File.WriteAllLines($"{fr}{slash}potassium.TXT", potassium);

            carbs = importArray($"{fr}{slash}carbs.TXT");
            carbs.RemoveAt(m);
            carbs.Insert(m, macros2[3]);
            File.WriteAllLines($"{fr}{slash}carbs.TXT", carbs);

            fiber = importArray($"{fr}{slash}fiber.TXT");
            fiber.RemoveAt(m);
            fiber.Insert(m, macros2[4]);
            File.WriteAllLines($"{fr}{slash}fiber.TXT", fiber);

            sugar = importArray($"{fr}{slash}sugar.TXT");
            sugar.RemoveAt(m);
            sugar.Insert(m, macros2[5]);
            File.WriteAllLines($"{fr}{slash}sugar.TXT", sugar);

            protein = importArray($"{fr}{slash}protein.TXT");
            protein.RemoveAt(m);
            protein.Insert(m, macros2[6]);
            File.WriteAllLines($"{fr}{slash}protein.TXT", protein);

            //                            

            vitA = importArray($"{fr}{slash}vitA.TXT");
            vitA.RemoveAt(m);
            vitA.Insert(m, vits[0]);
            File.WriteAllLines($"{fr}{slash}vitA.TXT", vitA);

            vitC = importArray($"{fr}{slash}vitC.TXT");
            vitC.RemoveAt(m);
            vitC.Insert(m, vits[1]);
            File.WriteAllLines($"{fr}{slash}vitC.TXT", vitC);

            vitD = importArray($"{fr}{slash}vitD.TXT");
            vitD.RemoveAt(m);
            vitD.Insert(m, vits[2]);
            File.WriteAllLines($"{fr}{slash}vitD.TXT", vitD);

            vitE = importArray($"{fr}{slash}vitE.TXT");
            vitE.RemoveAt(m);
            vitE.Insert(m, vits[3]);
            File.WriteAllLines($"{fr}{slash}vitE.TXT", vitE);

            vitK = importArray($"{fr}{slash}vitK.TXT");
            vitK.RemoveAt(m);
            vitK.Insert(m, vits[4]);
            File.WriteAllLines($"{fr}{slash}vitK.TXT", vitK);

            B1 = importArray($"{fr}{slash}B1.TXT");
            B1.RemoveAt(m);
            B1.Insert(m, vits[5]);
            File.WriteAllLines($"{fr}{slash}B1.TXT", B1);

            B2 = importArray($"{fr}{slash}B2.TXT");
            B2.RemoveAt(m);
            B2.Insert(m, vits[6]);
            File.WriteAllLines($"{fr}{slash}B2.TXT", B2);

            B3 = importArray($"{fr}{slash}B3.TXT");
            B3.RemoveAt(m);
            B3.Insert(m, vits[7]);
            File.WriteAllLines($"{fr}{slash}B3.TXT", B3);

            B5 = importArray($"{fr}{slash}B5.TXT");
            B5.RemoveAt(m);
            B5.Insert(m, vits[8]);
            File.WriteAllLines($"{fr}{slash}B5.TXT", B5);

            B6 = importArray($"{fr}{slash}B6.TXT");
            B6.RemoveAt(m);
            B6.Insert(m, vits[9]);
            File.WriteAllLines($"{fr}{slash}B6.TXT", B6);

            B7 = importArray($"{fr}{slash}B7.TXT");
            B7.RemoveAt(m);
            B7.Insert(m, vits[10]);
            File.WriteAllLines($"{fr}{slash}B7.TXT", B7);

            B9 = importArray($"{fr}{slash}B9.TXT");
            B9.RemoveAt(m);
            B9.Insert(m, vits[11]);
            File.WriteAllLines($"{fr}{slash}B9.TXT", B9);

            B12 = importArray($"{fr}{slash}B12.TXT");
            B12.RemoveAt(m);
            B12.Insert(m, vits[12]);
            File.WriteAllLines($"{fr}{slash}B12.TXT", B12);

            //

            Fe = importArray($"{fr}{slash}Fe.TXT");
            Fe.RemoveAt(m);
            Fe.Insert(m, mins[0]);
            File.WriteAllLines($"{fr}{slash}Fe.TXT", Fe);

            Ca = importArray($"{fr}{slash}Ca.TXT");
            Ca.RemoveAt(m);
            Ca.Insert(m, mins[1]);
            File.WriteAllLines($"{fr}{slash}Ca.TXT", Ca);

            Mg = importArray($"{fr}{slash}Mg.TXT");
            Mg.RemoveAt(m);
            Mg.Insert(m, mins[2]);
            File.WriteAllLines($"{fr}{slash}Mg.TXT", Mg);
            
            Zn = importArray($"{fr}{slash}Zn.TXT");
            Zn.RemoveAt(m);
            Zn.Insert(m, mins[3]);
            File.WriteAllLines($"{fr}{slash}Zn.TXT", Zn);

            Se = importArray($"{fr}{slash}Se.TXT");
            Se.RemoveAt(m);
            Se.Insert(m, mins[4]);
            File.WriteAllLines($"{fr}{slash}Se.TXT", Se);

            B = importArray($"{fr}{slash}B.TXT");
            B.RemoveAt(m);
            B.Insert(m, mins[5]);
            File.WriteAllLines($"{fr}{slash}B.TXT", B);

            I = importArray($"{fr}{slash}I.TXT");
            I.RemoveAt(m);
            I.Insert(m, mins[6]);
            File.WriteAllLines($"{fr}{slash}I.TXT", I);

            P = importArray($"{fr}{slash}P.TXT");
            P.RemoveAt(m);
            P.Insert(m, mins[7]);
            File.WriteAllLines($"{fr}{slash}P.TXT", P);

            Mn = importArray($"{fr}{slash}Mn.TXT");
            Mn.RemoveAt(m);
            Mn.Insert(m, mins[8]);
            File.WriteAllLines($"{fr}{slash}Mn.TXT", Mn);

            Fl = importArray($"{fr}{slash}Fl.TXT");
            Fl.RemoveAt(m);
            Fl.Insert(m, mins[9]);
            File.WriteAllLines($"{fr}{slash}Fl.TXT", Fl);

            Cu = importArray($"{fr}{slash}Cu.TXT");
            Cu.RemoveAt(m);
            Cu.Insert(m, mins[10]);
            File.WriteAllLines($"{fr}{slash}Cu.TXT", Cu);

            Cr = importArray($"{fr}{slash}Cr.TXT");
            Cr.RemoveAt(m);
            Cr.Insert(m, mins[11]);
            File.WriteAllLines($"{fr}{slash}Cr.TXT", Cr);

            Mo = importArray($"{fr}{slash}Mo.TXT");
            Mo.RemoveAt(m);
            Mo.Insert(m, mins[12]);
            File.WriteAllLines($"{fr}{slash}Mo.TXT", Mo);
            #endregion

            autocomp = new string[comboNDB.Items.Count];
            names = importArray($"{fr}{slash}names.TXT");
            for (int i = 0; i < comboNDB.Items.Count; i++)
                autocomp[i] = names[i];
            
            var source = new AutoCompleteStringCollection();
            
            source.AddRange(autocomp);
            comboFoodName.AutoCompleteCustomSource = source;
            comboFoodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboFoodName.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        #region vitamins, keyhandling

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            //lines = richTextBox2.Text.Split('\n');
            int startIndex = richtextvits.SelectionStart;
            //int charsAdded = 0;
            int s = richtextvits.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextvits.Lines[i].Length + 1;
            richtextvits.SelectionStart = sum;
            richtextvits.SelectionLength = richtextvits.Lines[s].Length;
        }

        private void richTextBox2_MouseUp(object sender, MouseEventArgs e)
        {
            int startIndex = richtextvits.SelectionStart;
            //int charsAdded = 0;

            lines = richtextvits.Text.Split('\n');
            int s = richtextvits.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextvits.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    //MessageBox.Show(i.ToString());
                    lines[i] = lines[i].Replace(" ", "");

                    int oldLength = lines[i].Length;
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    if (s > l1)
                        sum += lines[i].Length - oldLength;

                }


            richtextvits.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextvits.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextvits.Text += "\n";
            }
            //
            richtextvits.SelectionStart = sum;
            richtextvits.SelectionLength = richtextvits.Lines[s].Length;
        }


        private void richTextBox2_MouseDown(object sender, MouseEventArgs e)
        {
            int startIndex = richtextvits.SelectionStart;
            //int charsAdded = 0;

            lines = richtextvits.Text.Split('\n');
            int s = richtextvits.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextvits.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    //MessageBox.Show(i.ToString());
                    lines[i] = lines[i].Replace(" ", "");

                    int oldLength = lines[i].Length;
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    if (s > l1)
                        sum += lines[i].Length - oldLength;

                }


            richtextvits.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextvits.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextvits.Text += "\n";
            }
            //
            richtextvits.SelectionStart = sum;
            richtextvits.SelectionLength = richtextvits.Lines[s].Length;
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            int startIndex = richtextvits.SelectionStart;
            l1 = richtextvits.GetLineFromCharIndex(startIndex);
            lines = richtextvits.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;

            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    sum += 2;
                }

            richtextvits.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextvits.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextvits.Text += "\n";
            }
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            l1 = richtextvits.GetLineFromCharIndex(richtextvits.SelectionStart);

            lines = richtextvits.Text.Split('\n');
            //label1.Text = l1.ToString() +"{"+string.Join(",", lines)+"}";

            int sum2 = 0;
            for (int i = 0; i < l1; i++)
                sum2 += lines[i].Length + 1;

            bool b = lines[l1].Contains(" ");//lines[l1].Contains("mg") || lines[l1].Contains("%");

            if (e.KeyCode.ToString() == "Left" || e.KeyCode.ToString() == "Right")
                e.SuppressKeyPress = true;
            else if (e.KeyCode.ToString() == "Return")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 12)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 0)
                    l1++;
                for (int i = 0; i < l1 - 1; i++)
                    sum += lines[i].Length + 1;
                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 - 1].Length;
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 12)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode == Keys.Delete || (e.KeyCode == Keys.Back && richtextvits.SelectionStart == sum2))
            { e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            { e.SuppressKeyPress = true; }
        }

        int l1 = 0;
        string[] lines;
        private void richTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            int startIndex = richtextvits.SelectionStart;
            l1 = richtextvits.GetLineFromCharIndex(startIndex);
            lines = richtextvits.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;


            if (lines[l1].Contains(max.ToString() + "m"))
            {
                lines[l1] = lines[l1].Replace("m", " mg");
                sum += 2;
                richtextvits.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextvits.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextvits.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextvits.SelectionStart = sum;
                    richtextvits.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1].Contains(max.ToString() + "u"))
            {
                lines[l1] = lines[l1].Replace("u", " ug");
                sum += 2;
                richtextvits.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextvits.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextvits.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextvits.SelectionStart = sum;
                    richtextvits.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1].Contains(max.ToString() + "%"))
            {
                lines[l1] = lines[l1].Replace("%", " %");
                sum++;
                richtextvits.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextvits.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextvits.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextvits.SelectionStart = sum;
                    richtextvits.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 < 4 && lines[l1].Contains(max.ToString() + "i"))
            {
                lines[l1] = lines[l1].Replace("i", " iu");
                sum += 2;
                richtextvits.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextvits.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextvits.Text += "\n";
                }
                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1] == "0")
            {
                lines[l1] = "0 %";
                sum += 2;
                richtextvits.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextvits.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextvits.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextvits.SelectionStart = sum;
                    richtextvits.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextvits.SelectionStart = sum;
                richtextvits.SelectionLength = lines[l1 + 1].Length;
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show(e.KeyChar.ToString());
            l1 = richtextvits.GetLineFromCharIndex(richtextvits.SelectionStart);

            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'm' && e.KeyChar != 'u' && e.KeyChar != 'i' 
                && e.KeyChar != '%' || (l1 > 3 && e.KeyChar == 'i'))
                e.Handled = true;
            if ((lines[l1].Contains("m") || lines[l1].Contains("i") || lines[l1].Contains("u") 
                || lines[l1].Contains("%")) && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
            if (getBiggestInt(lines[l1]) == 0 && (!Char.IsDigit(e.KeyChar) || e.KeyChar == '0') && richtextvits.SelectionLength < lines[l1].Length)
                e.Handled = true;
            if (getBiggestInt(lines[l1]).ToString().Length > 4 && richtextvits.SelectionLength == 0 
                && e.KeyChar != 'm' && e.KeyChar != 'u' && e.KeyChar != 'i' && e.KeyChar != '%')
                e.Handled = true;
        }
        #endregion

        #region macros1, keyhandling
        private void richTextBox3_Enter(object sender, EventArgs e)
        {
            int startIndex = richtextmacros1.SelectionStart;
            //int charsAdded = 0;
            int s = richtextmacros1.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros1.Lines[i].Length + 1;
            richtextmacros1.SelectionStart = sum;
            richtextmacros1.SelectionLength = richtextmacros1.Lines[s].Length;
        }
        private void richTextBox3_MouseUp(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmacros1.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmacros1.Text.Split('\n');
            int s = richtextmacros1.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros1.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)            
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i == 0)
                    {
                        int oldLength = lines[i].Length;
                        //MessageBox.Show(oldLength.ToString());
                        //if (oldLength == 0)
                            //sum++;
                        lines[i] = getBiggestInt(lines[i]) + " cal";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;//4;
                    }
                    else
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        if (s > l1) //WORK HERE ARRGGHH, IT WAS S NOT I!!
                            sum += lines[i].Length - oldLength;
                    }
                }
            

            richtextmacros1.Clear();

            
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros1.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros1.Text += "\n";
                //MessageBox.Show("k");
            }
            //
            richtextmacros1.SelectionStart = sum;
            richtextmacros1.SelectionLength = richtextmacros1.Lines[s].Length;
        }


        private void richTextBox3_MouseDown(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmacros1.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmacros1.Text.Split('\n');
            int s = richtextmacros1.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros1.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i == 0)
                    {
                        int oldLength = lines[i].Length;
                        //MessageBox.Show(oldLength.ToString());
                        //if (oldLength == 0)
                        //sum++;
                        lines[i] = getBiggestInt(lines[i]) + " cal";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;//4;
                    }
                    else
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        if (s > l1) //WORK HERE ARRGGHH, IT WAS S NOT I!!
                            sum += lines[i].Length - oldLength;
                    }
                }


            richtextmacros1.Clear();


            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros1.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros1.Text += "\n";
                //MessageBox.Show("k");
            }
            //
            richtextmacros1.SelectionStart = sum;
            richtextmacros1.SelectionLength = richtextmacros1.Lines[s].Length;
        }

        private void richTextBox3_Leave(object sender, EventArgs e)
        {
            int startIndex = richtextmacros1.SelectionStart;
            l1 = richtextmacros1.GetLineFromCharIndex(startIndex);
            lines = richtextmacros1.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;

            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i == 0)
                    {
                        lines[i] = getBiggestInt(lines[i]) + " cal";
                        sum += 4;
                    }
                    else
                    {
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        sum += 4;
                    }
                }

            richtextmacros1.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros1.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros1.Text += "\n";
            }
        }

        private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            lines = richtextmacros1.Text.Split('\n');
            l1 = richtextmacros1.GetLineFromCharIndex(richtextmacros1.SelectionStart);

            if (lines[l1].Length > 4 && e.KeyChar != 'c' && e.KeyChar != 'g' && richtextmacros1.SelectionLength < lines[l1].Length)
                e.Handled = true;
            if (l1 == 0)
            {
                //MessageBox.Show(getBiggestInt(lines[l1]).ToString());
                if (getBiggestInt(lines[l1]) == 0 && e.KeyChar == '0' && richtextmacros1.SelectionLength == 0 && lines[l1].Length > 0)
                    e.Handled = true;
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'c')
                    e.Handled = true;
                if ((lines[l1].Contains("c") && e.KeyChar == 'c') || (lines[l1].Length == 0 && e.KeyChar == 'c'))
                    e.Handled = true;
            }
            else
            {
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 'g')
                    e.Handled = true;
                if ((lines[l1].Contains("g") && e.KeyChar == 'g') || (lines[l1].Contains(".") 
                    && richtextmacros1.SelectionLength < lines[l1].Length && (e.KeyChar=='.' || e.KeyChar== 'g')))
                    e.Handled = true;
                if (lines[l1].Length == 0 && e.KeyChar == 'g')
                    e.Handled = true;
            }         
        }
       
        private void richTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            { e.SuppressKeyPress = true;
                e.Handled = true;
                return; }

            l1 = richtextmacros1.GetLineFromCharIndex(richtextmacros1.SelectionStart);

            lines = richtextmacros1.Text.Split('\n');
            //label1.Text = l1.ToString() +"{"+string.Join(",", lines)+"}";

            int sum2 = 0;
            for (int i = 0; i < l1; i++)
                sum2 += lines[i].Length + 1;

            bool b = lines[l1].Contains(" ");//lines[l1].Contains("mg") || lines[l1].Contains("%");

            if (e.KeyCode.ToString() == "Left" || e.KeyCode.ToString() == "Right")
                e.SuppressKeyPress = true;
            else if (e.KeyCode.ToString() == "Return")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 5)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 0)
                    l1++;
                for (int i = 0; i < l1 - 1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 - 1].Length;
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 5)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode == Keys.Delete || (e.KeyCode == Keys.Back && richtextmacros1.SelectionStart == sum2))
            { e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            { e.SuppressKeyPress = true; }
        }

        private void richTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            int startIndex = richtextmacros1.SelectionStart;
            l1 = richtextmacros1.GetLineFromCharIndex(startIndex);
            lines = richtextmacros1.Text.Split('\n');
            int max = 0;
            double dMax = 0.0;
            //if (l1 == 0)
            max = getBiggestInt(lines[l1]);
            dMax = Math.Round(getBiggestDouble(lines[l1]));
            /*else
                dMax = Math.Round(getBiggestDouble(lines[l1]), 1);*/

            //label31.Text = max.ToString();

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;


            if (l1 == 0 && lines[l1].Contains(max.ToString() + "c"/*al"*/))
            {
                lines[l1] = lines[l1].Replace("c", " cal");
                sum += 3;
                richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 == 0 && lines[l1] == "0")
            {
                lines[l1] = "0 cal";
                sum += 4;
                richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 0 && lines[l1].Contains(max.ToString() + "g"))
            {
                lines[l1] = lines[l1].Replace("g", ".0 g");
                sum += 3;
                richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }

                if (l1 == 5)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros1.SelectionStart = sum;
                    richtextmacros1.SelectionLength = lines[l1].Length;
                    return;
                }
                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 0 && lines[l1] == "0")
            {
                lines[l1] += ".0 g";
                sum += 4;
                 richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }

                if (l1 == 5)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros1.SelectionStart = sum;
                    richtextmacros1.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 0 && lines[l1].Contains(".") && lines[l1].Length == 3)
            {
                lines[l1] += " g";
                sum += 2;
                richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }

                if (l1 == 5)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros1.SelectionStart = sum;
                    richtextmacros1.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 0 && lines[l1].StartsWith(".") && lines[l1].Length == 2)
            {
                lines[l1] = "0" + lines[l1] + " g";
                sum += 3;
                richtextmacros1.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros1.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros1.Text += "\n";
                }

                if (l1 == 5)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros1.SelectionStart = sum;
                    richtextmacros1.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmacros1.SelectionStart = sum;
                richtextmacros1.SelectionLength = lines[l1 + 1].Length;
            }      
        }
        #endregion

        #region macros2, keyhandling
        private void richTextBox4_Enter(object sender, EventArgs e)
        {
            int startIndex = richtextmacros2.SelectionStart;
            //int charsAdded = 0;
            int s = richtextmacros2.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros2.Lines[i].Length + 1;
            richtextmacros2.SelectionStart = sum;
            richtextmacros2.SelectionLength = richtextmacros2.Lines[s].Length;
        }
        private void richTextBox4_MouseUp(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmacros2.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmacros2.Text.Split('\n');
            int s = richtextmacros2.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros2.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i < 3)
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + " mg";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;//4;
                    }
                    else
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;
                    }
                }
            

            richtextmacros2.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros2.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros2.Text += "\n";
            }
            
            richtextmacros2.SelectionStart = sum;
            richtextmacros2.SelectionLength = richtextmacros2.Lines[s].Length;
        }
        
        private void richTextBox4_MouseDown(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmacros2.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmacros2.Text.Split('\n');
            int s = richtextmacros2.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmacros2.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i < 4)
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + " mg";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;//4;
                    }
                    else
                    {
                        int oldLength = lines[i].Length;
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        if (s > l1)
                            sum += lines[i].Length - oldLength;
                    }
                }


            richtextmacros2.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros2.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros2.Text += "\n";
            }

            richtextmacros2.SelectionStart = sum;
            richtextmacros2.SelectionLength = richtextmacros2.Lines[s].Length;
        }

        private void richTextBox4_Leave(object sender, EventArgs e)
        {
            int startIndex = richtextmacros2.SelectionStart;
            l1 = richtextmacros2.GetLineFromCharIndex(startIndex);
            lines = richtextmacros2.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;

            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    if (i < 3)
                    {
                        lines[i] = getBiggestInt(lines[i]) + " mg";
                        sum += 4;
                    }
                    else
                    {
                        lines[i] = getBiggestInt(lines[i]) + ".0 g";
                        sum += 3;
                    }
                }

            richtextmacros2.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmacros2.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmacros2.Text += "\n";
            }
        }

        private void richTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            lines = richtextmacros2.Text.Split('\n');
            l1 = richtextmacros2.GetLineFromCharIndex(richtextmacros2.SelectionStart);

            if (lines[l1].Length > 4 && e.KeyChar != 'g' && e.KeyChar != 'm' && richtextmacros2.SelectionLength < lines[l1].Length)
                e.Handled = true;
            if (richtextmacros2.SelectionLength == 0 && ((lines[l1].Contains("g") && e.KeyChar == 'g') || (lines[l1].Contains("m") && e.KeyChar == 'm')))
                e.Handled = true;
            if (lines[l1].Contains(".") && e.KeyChar == 'g')
                e.Handled = true;

            if (l1 < 3)
            {

                if (lines[l1].StartsWith("0") && e.KeyChar == '0' && richtextmacros2.SelectionLength == 0)
                    e.Handled = true;
                if (getBiggestInt(lines[l1]) == 0 && (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'm') && e.KeyChar != 'g')
                    e.Handled = true;

                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'm' || (lines[l1].Contains("m") && e.KeyChar == 'm'))
                    e.Handled = true;

            }
            else
            {
                if (getBiggestInt(lines[l1]) == 0 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar) 
                    && richtextmacros2.SelectionLength == 0 && lines[l1].Length > 0)
                    e.Handled = true;

                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'g' && e.KeyChar != '.'
                    || (lines[l1].Contains(".") && e.KeyChar == '.' && richtextmacros2.SelectionLength == 0))
                    e.Handled = true;
                if (richtextmacros2.SelectionLength == 0 && ((lines[l1].Length == 0 && e.KeyChar == 'g') || (lines[l1].Length > 1 && e.KeyChar == '.')))
                    e.Handled = true;
            }
        }

        private void richTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            l1 = richtextmacros2.GetLineFromCharIndex(richtextmacros2.SelectionStart);

            lines = richtextmacros2.Text.Split('\n');
            //label1.Text = l1.ToString() +"{"+string.Join(",", lines)+"}";

            int sum2 = 0;
            for (int i = 0; i < l1; i++)
                sum2 += lines[i].Length + 1;

            bool b = lines[l1].Contains(" ");//lines[l1].Contains("mg") || lines[l1].Contains("%");

            if (e.KeyCode.ToString() == "Left" || e.KeyCode.ToString() == "Right")
                e.SuppressKeyPress = true;
            else if (e.KeyCode.ToString() == "Return")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 6)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 0)
                    l1++;
                for (int i = 0; i < l1 - 1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 - 1].Length;
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 6)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode == Keys.Delete || (e.KeyCode == Keys.Back && richtextmacros2.SelectionStart == sum2))
            { e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            { e.SuppressKeyPress = true; }
        }

        private void richTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            int startIndex = richtextmacros2.SelectionStart;
            l1 = richtextmacros2.GetLineFromCharIndex(startIndex);
            lines = richtextmacros2.Text.Split('\n');
            int max = 0;
            //if (l1 == 0)
            max = getBiggestInt(lines[l1]);
            /*else
                dMax = Math.Round(getBiggestDouble(lines[l1]), 1);*/

            //label31.Text = max.ToString();

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;

            if (l1 < 3 && lines[l1].Contains(max.ToString() + "m"))
            {
                lines[l1] = lines[l1].Replace("m", " mg");
                sum += 2;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 < 3 && lines[l1] == "0")
            {
                lines[l1] = "0 mg";
                sum += 3;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }

            else if (l1 > 2 && lines[l1].Contains(max.ToString() + "g"))
            {
                lines[l1] = lines[l1].Replace("g", ".0 g");
                sum += 3;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                if (l1 == 6)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros2.SelectionStart = sum;
                    richtextmacros2.SelectionLength = lines[l1].Length;
                    return;
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 2 && lines[l1] == "0")
            {
                lines[l1] = lines[l1] + ".0 g";
                sum += 4;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                if (l1 == 6)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros2.SelectionStart = sum;
                    richtextmacros2.SelectionLength = lines[l1].Length;
                    return;
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 2 && lines[l1].Contains(".") && lines[l1].Length == 3)
            {
                lines[l1] += " g";
                sum += 2;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                if (l1 == 6)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros2.SelectionStart = sum;
                    richtextmacros2.SelectionLength = lines[l1].Length;
                    return;
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
            else if (l1 > 2 && lines[l1].StartsWith(".") && lines[l1].Length == 2)
            {
                lines[l1] = "0" + lines[l1] + " g";
                sum += 3;
                richtextmacros2.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmacros2.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmacros2.Text += "\n";
                }
                if (l1 == 6)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmacros2.SelectionStart = sum;
                    richtextmacros2.SelectionLength = lines[l1].Length;
                    return;
                }
                richtextmacros2.SelectionStart = sum;
                richtextmacros2.SelectionLength = lines[l1 + 1].Length;
            }
        }
        #endregion

        #region minerals, keyhandling


        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            int startIndex = richtextmins.SelectionStart;
            int s = richtextmins.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmins.Lines[i].Length + 1;
            richtextmins.SelectionStart = sum;
            richtextmins.SelectionLength = richtextmins.Lines[s].Length;
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            int startIndex = richtextmins.SelectionStart;
            l1 = richtextmins.GetLineFromCharIndex(startIndex);
            lines = richtextmins.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;

            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    lines[i] = lines[i].Replace(" ", "");
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    sum += 2;
                }

            richtextmins.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmins.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmins.Text += "\n";
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmins.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmins.Text.Split('\n');
            int s = richtextmins.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmins.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    //MessageBox.Show(i.ToString());
                    lines[i] = lines[i].Replace(" ", "");

                    int oldLength = lines[i].Length;
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    if (s > l1)
                        sum += lines[i].Length - oldLength;

                }


            richtextmins.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmins.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmins.Text += "\n";
            }
            //
            richtextmins.SelectionStart = sum;
            richtextmins.SelectionLength = richtextmins.Lines[s].Length;
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int startIndex = richtextmins.SelectionStart;
            //int charsAdded = 0;

            lines = richtextmins.Text.Split('\n');
            int s = richtextmins.GetLineFromCharIndex(startIndex);
            int sum = 0;
            for (int i = 0; i < s; i++)
                sum += richtextmins.Lines[i].Length + 1;
            //
            for (int i = 0; i < lines.Length; i++)
                if (getUnits(lines[i]) == "none")
                {
                    //MessageBox.Show(i.ToString());
                    lines[i] = lines[i].Replace(" ", "");

                    int oldLength = lines[i].Length;
                    lines[i] = getBiggestInt(lines[i]) + " %";
                    if (s > l1)
                        sum += lines[i].Length - oldLength;

                }


            richtextmins.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                richtextmins.Text += lines[i];
                if (i == lines.Length - 1)
                    break;
                richtextmins.Text += "\n";
            }
            //
            richtextmins.SelectionStart = sum;
            richtextmins.SelectionLength = richtextmins.Lines[s].Length;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            l1 = richtextmins.GetLineFromCharIndex(richtextmins.SelectionStart);

            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 'm' && e.KeyChar != 'u' && e.KeyChar != '%')
                e.Handled = true;
            if (lines[l1].Length == 0 && (e.KeyChar == 'm' && e.KeyChar == 'u' && e.KeyChar == '%'))
                e.Handled = true;
            if ((lines[l1].Contains("m") || lines[l1].Contains("u") || (lines[l1].Contains("%")))
                && (e.KeyChar == 'm' || e.KeyChar == 'u' || e.KeyChar == '%'))
                e.Handled = true;
            if (getBiggestInt(lines[l1]) == 0 &&  e.KeyChar == '0' && richtextmins.SelectionLength < lines[l1].Length)
                e.Handled = true;
            if (getBiggestInt(lines[l1]).ToString().Length > 4 && richtextmins.SelectionLength == 0 && e.KeyChar != 'm' && e.KeyChar != 'u' && e.KeyChar != '%')
                e.Handled = true;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            l1 = richtextmins.GetLineFromCharIndex(richtextmins.SelectionStart);

            lines = richtextmins.Text.Split('\n');
            //label1.Text = l1.ToString() +"{"+string.Join(",", lines)+"}";

            int sum2 = 0;
            for (int i = 0; i < l1; i++)
                sum2 += lines[i].Length + 1;

            bool b = lines[l1].Contains(" ");//lines[l1].Contains("mg") || lines[l1].Contains("%");

            if (e.KeyCode.ToString() == "Left" || e.KeyCode.ToString() == "Right")
                e.SuppressKeyPress = true;
            else if (e.KeyCode.ToString() == "Return")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 12)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 0)
                    l1++;
                for (int i = 0; i < l1 - 1; i++)
                    sum += lines[i].Length + 1;
                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 - 1].Length;
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                e.SuppressKeyPress = true;
                if (!b)
                    return;
                int sum = 0;
                if (l1 == 12)
                    l1--;
                for (int i = 0; i <= l1; i++)
                    sum += lines[i].Length + 1;
                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
            else if (e.KeyCode == Keys.Delete || (e.KeyCode == Keys.Back && richtextmins.SelectionStart == sum2))
            { e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            { e.SuppressKeyPress = true; }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            int startIndex = richtextmins.SelectionStart;
            l1 = richtextmins.GetLineFromCharIndex(startIndex);
            lines = richtextmins.Text.Split('\n');
            int max = getBiggestInt(lines[l1]);

            int sum = 0;
            for (int i = 0; i <= l1; i++)
                sum += lines[i].Length + 1;


            if (lines[l1].Contains(max.ToString() + "m"))
            {
                lines[l1] = lines[l1].Replace("m", " mg");
                sum += 2;
                richtextmins.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmins.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmins.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmins.SelectionStart = sum;
                    richtextmins.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1].Contains(max.ToString() + "u"))
            {
                lines[l1] = lines[l1].Replace("u", " ug");
                sum += 2;
                richtextmins.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmins.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmins.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmins.SelectionStart = sum;
                    richtextmins.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1].Contains(max.ToString() + "%"))
            {
                lines[l1] = lines[l1].Replace("%", " %");
                sum++;
                richtextmins.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmins.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmins.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmins.SelectionStart = sum;
                    richtextmins.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
            else if (lines[l1] == "0")
            {
                lines[l1] = "0 %";
                sum += 2;
                richtextmins.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    richtextmins.Text += lines[i];
                    if (i == lines.Length - 1)
                        break;
                    richtextmins.Text += "\n";
                }

                if (l1 == 12)
                {
                    sum = 0;
                    for (int i = 0; i < l1; i++)
                        sum += lines[i].Length + 1;
                    richtextmins.SelectionStart = sum;
                    richtextmins.SelectionLength = lines[l1].Length;
                    return;
                }

                richtextmins.SelectionStart = sum;
                richtextmins.SelectionLength = lines[l1 + 1].Length;
            }
        }
        #endregion

        private void comboFoodName_TextChanged(object sender, EventArgs e)
        {
            if (comboNDB.Items.Count == 1)
                return;
            for (int i = 0; i < comboNDB.Items.Count; i++)
            {
                if (comboFoodName.Text == autocomp[i])
                {
                    comboNDB.SelectedIndex = i;
                    break;
                }
            }
        }

        private void comboFoodName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsSeparator(e.KeyChar) && !Char.IsLetterOrDigit(e.KeyChar))
                e.Handled = true;
            if (Char.IsLetter(e.KeyChar))
                e.KeyChar = Char.ToUpper(e.KeyChar);
        }
    }
}
