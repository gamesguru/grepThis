using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Nutritracker
{
    public partial class frmNewField : Form
    {
        private frmParseCustomDatabase mainForm = null;
        public frmNewField(Form callingForm)
        {
            mainForm = callingForm as frmParseCustomDatabase;
            InitializeComponent();
        }

        string slash = Path.DirectorySeparatorChar.ToString();
        public List<string> arr = new List<string>();
        public int n = 0;
        string searchKey = "";
        string value1Key = "";
        List<string> nameKeyPairs;
        private void frmNewField_Load(object sender, EventArgs e)
        {
            txtLoc.Text = $"{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_";
            lblRowCount.Text = $"Your Field will have {n} entries";
            for (int i = 0; i < arr.Count; i++)
            {
                string s = arr[i];
                listBox1.Items.Add(s);
                string s2 = "";
                try { s2 = s.Substring(0, 3); }
                catch { s2 = s.Substring(0, 2); }

                foreach (char c in new char[] { '/', '\\', ':', '*', '?', '"', '<', '>', '|' , ' '})
                    if (s2.Contains(c))
                        s2 = s2.Replace(c, 'X');
                s2 = s2.Replace("(", i.ToString()).Replace(" ", "X");

                if (!listBox2.Items.Contains(s2.ToUpper()))
                    listBox2.Items.Add(s2.ToUpper());
                else { listBox2.Items.Add(s2.ToUpper() + i.ToString()); }
            }
            nameKeyPairs = new List<string>();
            for (int i = 0; i < listBox1.Items.Count; i++)
                nameKeyPairs.Add($"{listBox1.Items[i]}|{listBox2.Items[i]}");


            lblColumnCount.Text = $"Your {listBox1.Items.Count} columns and their abbreviations";

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.TextLength > 2 && lblSearchField.Text != "N/A" && lblCalories.Text != "N/A")
                btnCreate.Enabled = true;
            if (txtName.TextLength < 2)
                btnCreate.Enabled = false;
            txtLoc.Text = $"{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{txtName.Text}";
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsLetterOrDigit(e.KeyChar) && !Char.IsSeparator(e.KeyChar) &&
                e.KeyChar != '_' && e.KeyChar != '-')
                e.Handled = true;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (lblSearchField.Text == "N/A")
            {
                lblSearchField.Text = listBox1.SelectedItem.ToString();
                searchKey = $"{listBox2.Items[listBox1.SelectedIndex]}.TXT";
                return;
            }
            if (lblCalories.Text == "N/A")
            {
                lblCalories.Text = listBox1.SelectedItem.ToString();
                value1Key = $"{listBox2.Items[listBox1.SelectedIndex]}.TXT";
            }
            if (txtName.TextLength > 2 && lblSearchField.Text != "N/A" && lblCalories.Text != "N/A")
                btnCreate.Enabled = true;
        }

        class dbKey {
            public string fileName;
            public string fields;
            public string metricName;
            public string headers;
            public string units;
        }
        List<dbKey> dbKeys;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string fp = Application.StartupPath + txtLoc.Text;
            List<string> files = new List<string>();
            for (int i = 0; i < listBox2.Items.Count; i++)
                files.Add($"{listBox2.Items[i]}.TXT");
            dbKeys = new List<dbKey>();
            for (int i = 0; i < files.Count(); i++)
            {
                dbKey k = new dbKey();
                k.fileName = files[i];
                k.headers = frmParseCustomDatabase.columns[i].header;
                if (k.fileName == searchKey)
                    k.fields = "FoodName";
                else if (k.fileName == value1Key)
                    k.fields = "Value1";

                string unit = "";
                try
                {
                    unit = k.headers.Split('(')[1].Split(')')[0].Trim();//.ToLower().Replace(" ", "");
					k.headers = k.headers.Split('(')[0].Trim();
                }
                catch
                {
                    try
                    {
						unit = k.headers.Split('[')[1].Split(']')[0].Trim();//.ToLower().Replace(" ", "");
						k.headers = k.headers.Split('[')[0].Trim();
                    }
                    catch
                    {
                        //try { unit = "per " + k.headers.Split(new string[] { " per ", " Per ", " PER " }, StringSplitOptions.None)[1].Trim().ToLower(); }
                        //catch { }
                    }
                }

                if (unit.Length > 0)
                    k.units = unit;

                dbKeys.Add(k);
            }

            foreach (dbKey k in dbKeys){
                try
                {
                    string[] perSplit = k.units.Split(new string[] { "Per ", "per " }, StringSplitOptions.RemoveEmptyEntries); 
                    string perWhat = perSplit[perSplit.Length - 1].Trim();

                    foreach (dbKey ki in dbKeys)
                        if (ki.headers == perWhat)
                            k.units = k.units.Replace(perWhat, "$" + perWhat);
                }
                catch{}
            }

            if (Directory.Exists(fp))
            {
                DialogResult dRG = MessageBox.Show("A directory with this name was already found, are you sure you want to overwrite it?", "Overwrite database?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dRG == DialogResult.No)
                    return;
                string[] st = Directory.GetFiles(fp);
                for (int i = 0; i < st.Length; i++)
                    File.Delete(st[i]);
                goto nxt;
            }
            Directory.CreateDirectory(fp);
            //MessageBox.Show(fp.Split(new string[] { "f_user_" }, StringSplitOptions.None)[0]);
            if (!File.Exists(fp.Split(new string[] { "f_user_" }, StringSplitOptions.None)[0] + "Slots.TXT"))
                File.Create(fp.Split(new string[] { "f_user_" }, StringSplitOptions.None)[0] + "Slots.TXT");
            nxt:
            for (int i = 0; i < files.Count; i++)
            {
                string[] colVal = new string[n];
                for (int j = 0; j < n; j++)
                    colVal[j] = mainForm.getVal(j, i).Replace('\r', '\0').Replace('\n', '\0');
                File.WriteAllLines(fp + $"{slash}" + files[i], colVal);
            }

			List<string> dbInit = new List<string>();
			List<string> dbConfig = new List<string>();
			foreach (dbKey k in dbKeys){
				List<string> temp = new List<string>();
                temp.Add("[File]" + k.fileName);
				temp.Add("[Header]" + k.headers);
				if (k.units != null)
					temp.Add("[Unit]" + k.units);
				temp.Add("");
                dbInit.Add(string.Join("\r\n", temp));
            }
            foreach (dbKey k in dbKeys){
				List<string> temp = new List<string>();
				temp.Add("[File]" + k.fileName);
				if (k.fields != null)
					temp.Add("[Field]" + k.fields);
				if (k.fields != null && (k.fields == "Value1" || k.fields == "Value2" || k.fields == "Value3"))
				{
                    k.metricName = k.fields;
					temp.Add("[MetricName]" + k.metricName);
				}
				temp.Add("");
                dbConfig.Add(string.Join("\r\n", temp));
            }
			File.WriteAllLines(fp + slash + "_dbInit.TXT", dbInit);
			File.WriteAllLines(fp + slash + "_dbConfig.TXT", dbConfig);

            MessageBox.Show("Database created successfully.  Please use the search function on the main page to try it out.  Your first time using it, you will be asked to assign real nutrient names to the imported fields.  The software isn't able to do that yet.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnClearChoices_Click(object sender, EventArgs e)
        {
            lblSearchField.Text = "N/A";
            lblCalories.Text = "N/A";
            searchKey = "";
            value1Key = "";
            btnCreate.Enabled = false;
        }
    }
}
