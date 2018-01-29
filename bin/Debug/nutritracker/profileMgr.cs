using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Nutritracker
{
    public partial class profileMgr : Form
    {
        public profileMgr()
        {
            InitializeComponent();
        }

        string sl = Path.DirectorySeparatorChar.ToString();

        private void txtNewProfName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtHt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtWt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private bool mH = false;
        private void txtNewProfName_TextChanged(object sender, EventArgs e)
        {
            if (mH)
                return;
            int n = txtNewProfName.SelectionStart;
            string chr = "";
            string rest = "";
            if (txtNewProfName.TextLength == 1)
            {
                mH = true;
                chr = txtNewProfName.Text[0].ToString().ToUpper();
                txtNewProfName.Text = chr;
                txtNewProfName.SelectionStart = n;
                mH = false;
            }
            else if (txtNewProfName.TextLength > 1)
            {
                mH = true;
                rest = txtNewProfName.Text.Substring(1, txtNewProfName.TextLength - 1).ToLower();
                chr = txtNewProfName.Text[0].ToString().ToUpper();
                txtNewProfName.Text = chr + rest;
                txtNewProfName.SelectionStart = n;
                mH = false;
            }

            

            for (int i = 0; i < comboExistingProfs.Items.Count; i++)
                if (txtNewProfName.Text.ToLower() == comboExistingProfs.Items[i].ToString().ToLower())
                {
                    comboExistingProfs.SelectedIndex = i;
                    btnDel.Enabled = true;
                    comboActivity.Enabled = true;
                    comboGoal.Enabled = true;
                    radioFemale.Enabled = true;
                    radioMale.Enabled = true;
                    txtAge.Enabled = true;
                    txtWt.Enabled = true;
                    txtHt.Enabled = true;
                    btnSave.Text = "Save Changes and Load";
                    return;
                }

            if (txtNewProfName.TextLength > 1)
            {
                if ((radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
                btnSave.Text = "Create new Profile";
                mH = true;
                comboExistingProfs.SelectedIndex = -1;
                mH = false;

                comboActivity.Enabled = true;
                comboGoal.Enabled = true;
                comboActivity.SelectedIndex = 1;
                comboGoal.SelectedIndex = 3;

                radioFemale.Enabled = true;
                radioMale.Enabled = true;
                radioFemale.Checked = false;
                radioMale.Checked = false;

                txtAge.Enabled = true;
                txtWt.Enabled = true;
                txtHt.Enabled = true;
                txtAge.Clear();
                txtWt.Clear();
                txtHt.Clear();
                return;
            }

            btnDel.Enabled = false;
            mH = true;
            comboExistingProfs.SelectedIndex = -1;
            mH = false;

            comboActivity.Enabled = false;
            comboGoal.Enabled = false;
            comboActivity.SelectedIndex = 1;
            comboGoal.SelectedIndex = 3;

            radioFemale.Enabled = false;
            radioMale.Enabled = false;
            radioFemale.Checked = false;
            radioMale.Checked = false;

            txtAge.Enabled = false;
            txtWt.Enabled = false;
            txtHt.Enabled = false;
            txtAge.Clear();
            txtWt.Clear();
            txtHt.Clear();
        }


        private int profMax = 0;
        private int profIndex;
        public int defaultIndex;
        private void frmProfile_Load(object sender, EventArgs e)
        {
            comboActivity.SelectedIndex = 1;
            comboGoal.SelectedIndex = 3;
            profIndex = frmMain.currentUser.index;
            string root = $"{Application.StartupPath}{sl}usr";
            try
            {
                defaultIndex = Convert.ToInt32(Directory.GetFiles(root)[0].Replace($"{root}{sl}default", ""));
            }
            catch{
                defaultIndex = 0;
                File.Create($"{root}{sl}default0").Close();
            }

            string[] directs = Directory.GetDirectories(root);
            //MessageBox.Show(string.Join(", ", directs));
            List<string> profs = new List<string>();
            try
            {
                for (int i = 0; i < directs.Length; i++)
                    if (directs[i].EndsWith($"{sl}profile{i}"))
                    {
                        string name = "";
                        foreach (string s in File.ReadAllLines($"{directs[i]}{sl}profile.TXT"))
                            if (s.StartsWith("[Name]"))
                                name = s.Replace("[Name]", "");
                        profs.Add(directs[i]);
                        comboExistingProfs.Items
                            .Add(name); ;
                    }
            }
            catch { }

            if (comboExistingProfs.Items.Count > 0)
                comboExistingProfs.SelectedIndex = profIndex;

            if (defaultIndex > comboExistingProfs.Items.Count - 1)
                defaultIndex -= 1;

            foreach (string s in Directory.GetFiles(root))
                File.Delete(s);
            File.Create($"{root}{sl}default{frmMain.currentUser.index}").Close();
            
            if (comboExistingProfs.SelectedIndex == defaultIndex)
                checkDefaultProf.Enabled = false;
            else
                checkDefaultProf.Enabled = true;
            

            profMax = directs.Length;
        }

        List<string> profs;
        private void btnSave_Click(object sender, EventArgs e)
        {


            if (Convert.ToInt32(txtBodyfat.Text) < 4 || Convert.ToInt32(txtBodyfat.Text) > 80)
            {
                MessageBox.Show("Please enter a body fat value between 4 and 80%.  This will affect calculations.  You can update your bodyfat later.", "Please check body fat %", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> profData = new List<string>();
            int profIndex = 0;
            string root = Application.StartupPath + $"{sl}usr";
            string[] directs = Directory.GetDirectories(root);
            
            //profMax = files.Length;
            profs = new List<string>();
            bool license = false;
            try
            {
                for (int i = 0; i < directs.Length; i++)
                {
                    string name = "";
                    foreach (string s in File.ReadAllLines($"{directs[i]}{sl}profile.TXT"))
                        if (s.StartsWith("[Name]"))
                            name = s.Replace("[Name]", "");
                        else if (s.StartsWith($"[License]StallmanApproves_{name.GetHashCode()}"))
                            license = true;
                    if (directs[i].EndsWith($"{sl}profile{i}")
                        && name.ToLower() == comboExistingProfs.Text.ToLower())
                    {
                        profIndex = i;
                        profs.Add(directs[i]);
                        comboExistingProfs.Items
                            .Add(name); //importArray(root + "profile.txt")[0]);
                        break;
                    }
                    else
                        profIndex = profMax;
                }
            }

            catch { }
            
            string gender = radioMale.Checked ? "male" : "female";
            profData.Add($"[Name]{txtNewProfName.Text}");
            profData.Add($"[Gender]{gender}");
            profData.Add($"[Age]{txtAge.Text}");
            profData.Add($"[Bodyfat]{txtBodyfat.Text}");
            profData.Add($"[Weight]{txtWt.Text}");
            profData.Add($"[Height]{txtHt.Text}");
            profData.Add($"[ActLvl]{comboActivity.SelectedIndex}");
            profData.Add($"[Goal]{comboGoal.SelectedIndex}");
            profData.Add($"[Date]{frmMain.dte}");
            if (license)
                profData.Add($"[License]StallmanApproves_{txtNewProfName.Text.GetHashCode()}");
            
            try { File.WriteAllLines($"{root}{sl}profile{profIndex}{sl}profile.TXT", profData); }
            catch
            {
                Directory.CreateDirectory($"{root}{sl}profile{profIndex}foodlog");
                Directory.CreateDirectory($"{root}{sl}profile{profIndex}exlog");
                Directory.CreateDirectory( $"{root}{sl}profile{profIndex}dtlreports");
                Directory.CreateDirectory($"{root}{sl}profile{profIndex}DBs{sl}_foods");
                Directory.CreateDirectory($"{root}{sl}profile{profIndex}DBs{sl}_recipes");
                File.WriteAllLines($"{root}{sl}profile{profIndex}{sl}profile.TXT", profData);
                comboExistingProfs.Items.Add(txtNewProfName.Text);
                comboExistingProfs.SelectedIndex = comboExistingProfs.Items.Count - 1;
            }

            
            frmMain.currentUser.index = comboExistingProfs.SelectedIndex;
            frmMain.currentUser.root = $"{root}{sl}profile{frmMain.currentUser.index}";
            frmMain.currentUser.name = txtNewProfName.Text;
            frmMain.currentUser.gender = gender;
            frmMain.currentUser.age = Convert.ToInt32(txtAge.Text);
            frmMain.currentUser.bf = Convert.ToInt32(txtBodyfat.Text);
            frmMain.currentUser.wt = Convert.ToInt32(txtWt.Text);
            frmMain.currentUser.ht = Convert.ToInt32(txtHt.Text);
            frmMain.currentUser.actLvl = comboActivity.SelectedIndex;
            frmMain.currentUser.goal = comboGoal.SelectedIndex.ToString();
            
            if (checkDefaultProf.Checked)
            {
                foreach (string s in Directory.GetFiles(root))
                    File.Delete(s);
                File.Create($"{root}{sl}default{frmMain.currentUser.index}").Close();
            }

            
            if (!File.ReadAllLines($"{frmMain.currentUser.root}{sl}profile.TXT").Contains($"[License]StallmanApproves_{txtNewProfName.Text.GetHashCode()}"))
            {
                licenseDialog frmli = new licenseDialog();
                frmli.profData = File.ReadAllLines($"{frmMain.currentUser.root}{sl}profile.TXT").ToList();
                frmli.rt = $"{frmMain.currentUser.root}{sl}";
                frmli.ShowDialog();
                if (!File.ReadAllLines($"{frmMain.currentUser.root}{sl}profile.TXT").ToList().Contains($"[License]StallmanApproves_{txtNewProfName.Text.GetHashCode()}"))
                    Process.GetCurrentProcess().Kill();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                Directory.Delete($"{Application.StartupPath}{sl}usr{sl}profile{comboExistingProfs.SelectedIndex}");
            //File.Delete(Application.StartupPath + $"{slash}usr{slash}profile" + comboExistingProfs.SelectedIndex.ToString() + $"{slash}profile" + comboExistingProfs.SelectedIndex.ToString() + ".txt");
            else
                return;

            //same as form load
            comboExistingProfs.Items.Clear();
            string root = $"{Application.StartupPath}{sl}usr{sl}";
            string[] files = Directory.GetFiles(root);
            //MessageBox.Show(string.Join(", ", files));
            List<string> profs = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (files[i].Contains($"{sl}profile"))
                {
                    profs.Add(files[i]);
                    foreach (string s in File.ReadAllLines(files[i]))
                        if (s.StartsWith("[Name]"))
                            comboExistingProfs.Items.Add(s.Replace("[Name]", "")); 
                }

            if (defaultIndex >= comboExistingProfs.Items.Count)
            defaultIndex -= 1;

            comboExistingProfs.SelectedIndex = defaultIndex;
            foreach (string s in Directory.GetFiles(root))
                File.Delete(s);
            File.Create($"{root}{sl}default{comboExistingProfs.SelectedIndex}").Close();

            profMax = files.Length;
        }

        private void radioMale_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void radioFemale_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0 )
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void txtHt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void txtWt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0 )
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void comboExistingProfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!mH && txtNewProfName.Text.ToLower() != comboExistingProfs.Text.ToLower())
                txtNewProfName.Text = comboExistingProfs.Text;

            string root = Application.StartupPath + $"{sl}usr{sl}";
            string[] directs = Directory.GetDirectories(root);
            profs = new List<string>();
            //MessageBox.Show(string.Join(", ", directs));

            for (int i = 0; i < directs.Length; i++)
            {
                string name = "";
                try
                {
                    foreach (string st in File.ReadAllLines($"{directs[i]}{sl}profile.TXT"))
                        if (st.StartsWith("[Name]"))
                            name = st.Replace("[Name]", "");
                }
                catch { continue; }
                if (directs[i].Contains($"{sl}profile{i}") && name.ToLower() ==
                    comboExistingProfs.Text.ToLower())
                    foreach (string s in File.ReadAllLines($"{directs[i]}{sl}profile.TXT"))                    
                        if (s.StartsWith("[Gender]"))
                            if (s.Contains("female"))
                                radioMale.Checked = false;
                            else
                                radioMale.Checked = true;
                        else if (s.StartsWith("[Name]"))
                            txtNewProfName.Text = s.Replace("[Name]", "");
                        else if (s.StartsWith("[Age]"))
                            txtAge.Text = s.Replace("[Age]", "");
                        else if (s.StartsWith("[Bodyfat]"))
                            txtBodyfat.Text = s.Replace("[Bodyfat]", "");
                        else if (s.StartsWith("[Weight]"))
                            txtWt.Text = s.Replace("[Weight]", "");
                        else if (s.StartsWith("[Height]"))
                            txtHt.Text = s.Replace("[Height]", "");
                        else if (s.StartsWith("[ActLvl]"))
                            comboActivity.SelectedIndex = Convert.ToInt32(s.Replace("[ActLvl]", ""));
                        else if (s.StartsWith("[Goal]"))
                            comboGoal.SelectedIndex = Convert.ToInt32(s.Replace("[Goal]", ""));
                        else if (s.StartsWith("[Date]"))
                            frmMain.dte = s.Replace("[Date]", "");                    
            }
                
            if (comboExistingProfs.SelectedIndex == defaultIndex)
                checkDefaultProf.Checked = true;
            else
                checkDefaultProf.Checked = false;

            if (comboExistingProfs.SelectedIndex == defaultIndex)
                checkDefaultProf.Enabled = false;
            else
                checkDefaultProf.Enabled = true;
        }

        private void comboActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }

        private void comboGoal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNewProfName.TextLength > 1 && (radioMale.Checked || radioFemale.Checked) &&
                    Convert.ToInt32(txtHt.Text) > 9 && Convert.ToInt32(txtWt.Text) > 4 &&
                    Convert.ToInt32(txtAge.Text) > 0)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            catch
            { btnSave.Enabled = false; }
        }
    }
}
