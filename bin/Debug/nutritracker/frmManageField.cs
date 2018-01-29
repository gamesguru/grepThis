using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Nutritracker
{
    public partial class frmManageField : Form
    {
        private frmParseCustomDatabase mainForm = null;
        public frmManageField(Form callingForm)
        {
            mainForm = callingForm as frmParseCustomDatabase;
            InitializeComponent();
        }
        public List<String> importArray(string filename)
        {
            list.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }
        List<string> list = new List<string>();

        public enum Unit
        {
            g, mg, ug, kg, percent, iu, tbsp, tsp, cup, kcal
        }

        public class field
        {
			public string dbInit;
            public string dbConfig;
			public List<dbi> dbInitKeys = new List<dbi>();
			public List<dbc> dbConfigKeys = new List<dbc>();
            public string name;
            public int z;
            public string standardization;
            public string[] nameOfFood;
            public string[] value1;
            public string[] value2;
            public string[] value3;
            public string[] serving;
            public string[] weight;
            public string[] othUnits;
			public string[] valueNames = new string[2];
			public Unit weightUnit;
			public Unit servUnit;
        }

		public class dbi
		{
			public string fileName;
			public string header;
			public string unit;
		}
		public class dbc
		{
			public string fileName;
			public string metricName;
            public string field;
		}

        List<field> Fields = new List<field>();
        string slash = Path.DirectorySeparatorChar.ToString();
        private void frmManageField_Load(object sender, EventArgs e)
        {
            string[] fields = Directory.GetDirectories(Application.StartupPath + $"{slash}usr{slash}profile" +
                                                       frmMain.currentUser.index + $"{slash}DBs");
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Replace(Application.StartupPath + $"{slash}usr{slash}profile" +
                                              frmMain.currentUser.index + $"{slash}DBs{slash}", "");
                if (fields[i].StartsWith("f_user_"))
                {
                    string field = fields[i].Remove(0, 7);

                    field f = new field();
                    f.name = fields[i].Replace("f_user_", "");
                    Fields.Add(f);
                    comboFields.Items.Add(field);
                }
            }
            if (comboFields.Items.Count == 0)
            {
                MessageBox.Show("No Fields detected, please go to the spreadsheet wizard to create some.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].dbInit = File.ReadAllText(Application.StartupPath + $"{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{Fields[i].name}{slash}_dbInit.TXT");
                string[] items = Fields[i].dbInit.Split(new string[] {"[File]"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in items){
                    string[] lines = s.Replace("\r", "").Split('\n');
                    dbi di = new dbi();
                    di.fileName = lines[0];
                    di.header = lines[1].Replace("[Header]", "");
					foreach (string st in lines)					
						if (st.Contains("[Unit]"))
                            di.unit = st.Replace("[Unit]", "");
                    Fields[i].dbInitKeys.Add(di);
                }


				Fields[i].dbConfig = File.ReadAllText($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{Fields[i].name}{slash}_dbConfig.TXT");
				items = Fields[i].dbConfig.Split(new string[] { "[File]" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in items)
				{
					string[] lines = s.Replace("\r", "").Split('\n');
					dbc dc = new dbc();
					dc.fileName = lines[0];
					foreach (string st in lines)
					{
						if (st.Contains("[Field]"))
                            dc.field = st.Replace("[Field]", "");
						else if (st.Contains("[MetricName]"))
							dc.metricName = st.Replace("[MetricName]", "");
					}
                    Fields[i].dbConfigKeys.Add(dc);
				}
			}

            comboFields.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Clear();
            source.Clear();
            textBox1.Clear();
            updateTextboxes();
            string dr = Application.StartupPath + $"{slash}usr{slash}profile" + frmMain.currentUser.index + $"{slash}DBs{slash}f_user_" + comboFields.Text;
            string[] files = Directory.GetFiles(dr);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(dr + $"{slash}", "");
                if (!files[i].StartsWith("_"))
                {
                    source.Add(files[i]);
                    if (textBox1.Text == "" || textBox1.Text.EndsWith("\n"))
                        textBox1.Text += files[i].Replace(dr + slash, "") + "\t";
                    else if (textBox1.Text.EndsWith("\t"))
                        textBox1.Text += files[i].Replace(dr + slash, "") + "\n";
                }
            }
            #region autocompletes
            txtName.AutoCompleteCustomSource = source;
            txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtServ.AutoCompleteCustomSource = source;
            txtServ.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtServ.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtWeight.AutoCompleteCustomSource = source;
            txtWeight.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtWeight.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVal1.AutoCompleteCustomSource = source;
            txtVal1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVal1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVal2.AutoCompleteCustomSource = source;
            txtVal2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVal2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVal3.AutoCompleteCustomSource = source;
            txtVal3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVal3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtOthUn.AutoCompleteCustomSource = source;
            txtOthUn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtOthUn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            #endregion

            //adds columns and reads arrays from disk
            List<ListViewItem> itms = new List<ListViewItem>();
            foreach (field f in Fields)
                if (f.name == comboFields.Text)
                {
                    foreach (dbc k in f.dbConfigKeys)
                    {
                        if (k.field == "FoodName")
                        {
                            listView1.Columns.Add(k.field);
                            f.nameOfFood = File.ReadAllLines(dr + slash + k.fileName);
                            f.z = f.nameOfFood.Length;
                        }
                        else if (k.field == "Value1")
                        {
                            listView1.Columns.Add(k.metricName);
                            f.value1 = File.ReadAllLines(dr + slash + k.fileName);
                        }
                        else if (k.field == "Value2")
                        {
                            listView1.Columns.Add(k.metricName);
                            f.value2 = File.ReadAllLines(dr + slash + k.fileName);
                        }
                        else if (k.field == "Value3")
                        {
                            listView1.Columns.Add(k.metricName);
                            f.value3 = File.ReadAllLines(dr + slash + k.fileName);
                        }
                        else if (k.field == "OtherUnits")
                        {
                            listView1.Columns.Add(k.field);
                            f.othUnits = File.ReadAllLines(dr + slash + k.fileName);
                        }
                        else if (k.field == "Serving")
                        {
                            listView1.Columns.Add(k.field);
                            f.serving = File.ReadAllLines(dr + slash + k.fileName);
                        }
                        else if (k.field == "Weight")
                        {
                            listView1.Columns.Add(k.field);
                            f.weight = File.ReadAllLines(dr + slash + k.fileName);
                        }
                    }
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    
                    
                    for (int i = 0; i < f.z; i++)
                    {
                        ListViewItem itm = new ListViewItem();
                        itm.Text = f.nameOfFood[i];
                        if (f.value1 != null && f.value1.Count() == f.z)
                            itm.SubItems.Add(f.value1[i]);
                        if (f.value2 != null && f.value2.Count() == f.z)
                            itm.SubItems.Add(f.value2[i]);
                        if (f.value3 != null && f.value3.Count() == f.z)
                            itm.SubItems.Add(f.value3[i]);
                        if (f.othUnits != null && f.othUnits.Count() == f.z)
                            itm.SubItems.Add(f.othUnits[i]);
                        if (f.serving != null && f.serving.Count() == f.z)
                            itm.SubItems.Add(f.serving[i]);
                        if (f.weight != null && f.weight.Count() == f.z)
                            itm.SubItems.Add(f.weight[i]);
                        itms.Add(itm);
                    }
                    listView1.BeginUpdate();
                    foreach (ListViewItem itm in itms)
                        listView1.Items.Add(itm);
                    listView1.EndUpdate();
                }
        }


        private void updateTextboxes()
        {
            txtName.Text = "";
            txtVal1.Text = "";
            txtVal2.Text = "";
            txtVal3.Text = "";
            txtValName1.Text = "";
            txtValName2.Text = "";
            txtValName3.Text = "";
            txtOthUn.Text = "";
            txtServ.Text = "";
            txtWeight.Text = "";
            foreach (field f in Fields)
            {
                if (f.name == comboFields.Text)
                    foreach (dbc k in f.dbConfigKeys)
                    {
                        if (k.field == "FoodName")
                            txtName.Text = k.fileName;
                        else if (k.field == "Value1")
                        {
                            txtVal1.Text = k.fileName;
                            txtValName1.Text = k.metricName;
                        }
                        else if (k.field == "Value2")
                        {
                            txtVal2.Text = k.fileName;
                            txtValName2.Text = k.metricName;
                        }
                        else if (k.field == "Value3")
                        {
                            txtVal3.Text = k.fileName;
                            txtValName3.Text = k.metricName;
                        }
                        else if (k.field == "OtherUnits")
                            txtOthUn.Text = k.fileName;
                        else if (k.field == "Serving")
                            txtServ.Text = k.fileName;
                        else if (k.field == "Weight")
                            txtWeight.Text = k.fileName;
                    }
            }

            bool chkCalBool = false;
            bool chkGramsBool = false;

            chkCal.Checked = chkCalBool;
            chkGrams.Checked = chkGramsBool;
        }

        private void button4_Click(object sender, EventArgs e) //reset button
        {
            updateTextboxes();
        }

        //this is causing problems if the user switches values 2 and 3, or something similar.  the [Header] and [Units]
        private void btnSave_Click(object sender, EventArgs e)
        {
            string dr = $"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{comboFields.Text}";
            field f = new field();
            foreach (field F in Fields)
                if (F.name == comboFields.Text)
                    f = F;

			//marks updates to the log
			//if (chkCal.Checked)
			//    text.Add("200 kcal|Standardization");

			//if (chkGrams.Checked)
			//text.Add("100 grams|Standardization");
			foreach (dbc ck in f.dbConfigKeys)
			{
				if (txtName.Text == ck.fileName)				
					ck.field = "FoodName";				
				else if (txtVal1.Text == ck.fileName)
				{
					ck.field = "Value1";
					ck.metricName = txtValName1.Text;
				}
				else if (txtVal2.Text == ck.fileName)
				{
					ck.field = "Value2";
					ck.metricName = txtValName2.Text;
				}
				else if (txtVal3.Text == ck.fileName)
				{
					ck.field = "Value3";
					ck.metricName = txtValName3.Text;
				}
				else if (txtOthUn.Text == ck.fileName)
					ck.field = "OtherUnits";
				else if (txtServ.Text == ck.fileName)
					ck.field = "Serving";
				else if (txtWeight.Text == ck.fileName)
					ck.field = "Weight";
			}

            List<string> activeFiles = new List<string>();
            #region activeTextBoxFiles
            if (File.Exists(dr + slash + txtName.Text))
                activeFiles.Add(txtName.Text);
            if (File.Exists(dr + slash + txtVal1.Text))
                activeFiles.Add(txtVal1.Text);
            if (File.Exists(dr + slash + txtVal2.Text))
                activeFiles.Add(txtVal2.Text);
            if (File.Exists(dr + slash + txtVal3.Text))
                activeFiles.Add(txtVal3.Text);
            if (File.Exists(dr + slash + txtOthUn.Text))
                activeFiles.Add(txtOthUn.Text);
            if (File.Exists(dr + slash + txtServ.Text))
                activeFiles.Add(txtServ.Text);
            if (File.Exists(dr + slash + txtWeight.Text))
                activeFiles.Add(txtWeight.Text);
            #endregion

            //saves to disk
            List<string> output = new List<string>();
            foreach (dbc k in f.dbConfigKeys){
				List<string> temp = new List<string>();
				temp.Add($"[File]{k.fileName}");
                if (activeFiles.Contains(k.fileName)) //removes from database if txtBox is blank
                {
                    if (k.field != null)
                        temp.Add("[Field]" + k.field);
                    if (k.field != null && (k.field == "Value1" || k.field == "Value2" || k.field == "Value3"))
                        temp.Add("[MetricName]" + k.metricName);
                }
				temp.Add("");
				output.Add(string.Join("\r\n", temp));
            }
            File.WriteAllLines(dr + slash+"_dbConfig.TXT", output);
			this.Close();
        }

        private void chkCal_CheckedChanged(object sender, EventArgs e)
        {
            string dr = $"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{comboFields.Text}";
            if (chkCal.Checked)
            {
                if (File.Exists(dr + slash + "SER.TXT") && MessageBox.Show("Are you sure you want to overwrite the old files? \n" + dr + slash + "SER.TXT", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    chkCal.Checked = false;
                    return;
                }


                try
                {
                    int n = 0;
                    foreach (string file in Directory.GetFiles(dr))
                        if (!file.StartsWith("_"))
                        {
                            n = File.ReadAllLines(file).Length;
                            break;
                        }

                    string[] servs = new string[n];
                    for (int i = 0; i < n; i++)
                        servs[i] = "200 kcal";
                    File.WriteAllLines(dr + slash + "SER.TXT", servs);
                    txtServ.Text = "SER.TXT";
                }
                catch
                {

                }
                chkGrams.Checked = false;
            }
            else
            {
                if (MessageBox.Show("Also delete file?\n" + dr + slash + "SER.TXT", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    File.Delete(dr + slash + "SER.TXT");
                txtServ.Text = "";
            }
        }

        private void chkGrams_CheckedChanged(object sender, EventArgs e)
        {
            string dr = $"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}f_user_{comboFields.Text}";
            int n = 0;
            try
            {
                foreach (string file in Directory.GetFiles(dr))
                    if (!file.StartsWith("_"))
                    {
                        n = File.ReadAllLines(file).Length;
                        break;
                    }

            }
            catch
            {

            }
            if (chkGrams.Checked)
            {
                string[] grams = new string[n];
                string[] readIN = new string[n];
                try { readIN = File.ReadAllLines(dr + slash + "WEI.TXT"); }
                catch { }
                for (int i = 0; i < n; i++)
                    grams[i] = "100 g";
                bool fileMatch = true;
                for (int i = 0; i < n; i++)
                    if (grams[i] != readIN[i])
                        fileMatch = false;
                //MessageBox.Show(fileMatch.ToString());
                if (File.Exists(dr + slash + "WEI.TXT") && !fileMatch && MessageBox.Show("Are you sure you want to overwrite the old files with 100g standardization? \n" + dr + slash + "WEI.TXT", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    chkGrams.Checked = false;
                    return;
                }

                File.WriteAllLines(dr + slash + "WEI.TXT", grams);
                txtWeight.Text = "WEI.TXT";

                chkCal.Checked = false;
            }
            else
            {
                string[] grams = new string[n];
                string[] readIN;
                try { readIN = File.ReadAllLines(dr + slash + "WEI.TXT"); }
                catch { return; }
                for (int i = 0; i < n; i++)
                    grams[i] = "100 g";
                bool fileMatch = true;
                for (int i = 0; i < n; i++)
                    if (grams[i] != readIN[i])
                        fileMatch = false;
                if (File.Exists(dr + slash + "WEI.TXT") && fileMatch && MessageBox.Show("Shall we also delete the record of the file?\n" + dr + slash + "WEI.TXT", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    File.Delete(dr + slash + "WEI.TXT");
                txtWeight.Text = "";
                //
                //add code to modify _nutKeyPairs.TXT
                //do the same for 200kcal standardization chunk of code
            }
        }
       
    }
}
