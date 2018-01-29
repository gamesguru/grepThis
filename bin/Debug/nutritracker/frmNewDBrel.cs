using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Nutritracker
{
    public partial class frmNewDBrel : Form
    {
        public frmNewDBrel()
        {
            InitializeComponent();
        }

        string slash = Path.DirectorySeparatorChar.ToString();
        string shareDBdir;
        string activeDBdir;
        private void frmNewDBrel_Load(object sender, EventArgs e)
        {
            shareDBdir = $"{Application.StartupPath}{slash}usr{slash}share{slash}DBs";
            txtLoc.Text = $"{slash}usr{slash}share{slash}DBs{slash}rel{slash}multi{slash}";

            foreach (string s in Directory.GetDirectories(shareDBdir))
                comboBox1.Items.Add(s.Replace(shareDBdir + slash, ""));

            try {
                comboBox1.SelectedIndex = Convert.ToInt32(File.ReadAllText($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}Default.TXT"));
            }
            catch { }       
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (radioMultiKey.Checked)
                txtLoc.Text = $"{slash}usr{slash}share{slash}DBs{slash}rel{slash}multi{slash}{txtName.Text}";
            else
                txtLoc.Text = $"{slash}usr{slash}share{slash}DBs{slash}rel{slash}single{slash}{txtName.Text}";                        

            if (txtName.TextLength > 2)
                btnCreate.Enabled = true;
            if (txtName.TextLength < 2)
                btnCreate.Enabled = false;
        }       

        private void txtOutput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private class relFile
        {
            public string file;
            public string[] lines;
            public string[] headers;
            public List<column> columns = new List<column>();
            public string unit = ""; //for multi
        }
        private class Header
        {
            public string header;
            public string unit; //for single
            public int occcurance = 0;
        }
        private class column
        {
            public string header;
            public List<string> rows = new List<string>();
        }

        List<relFile> relFiles;
        relFile relf;
        List<string> headers;
        List<Header> Headers;
        List<string> nutrNos;
        string flavVal;
        string[] files;

        private void txtOutput_DragDrop(object sender, DragEventArgs e)
        {
            relFiles = new List<relFile>();
            relf = new relFile();
            headers = new List<string>();
            Headers = new List<Header>();
            nutrNos = new List<string>();
            txtOutput.Clear();
            txtName.Clear();
            txtNutrDesc.Clear();
            txtFlavVal.Clear();
            txtNdb.Clear();
            txtNutrNo.Clear();
            listBox1.Items.Clear();

            if (radioMultiKey.Checked)
            {
                string folder = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                if (!Directory.Exists(folder))
                {
                    MessageBox.Show("Is not a valid directory\n" + folder);
                    txtOutput.Text = "<Drag and drop>\r\n<the folder>\r\n<contains *.TXT files>\r\n<more than one!!>";
                    return;
                }
                files = Directory.GetFiles(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                folder = folder.Split(new char[] { '/', '\\' })[folder.Split(new char[] { '/', '\\' }).Length - 1];
                txtName.Text = folder.ToUpper();
                foreach (string s in files)
                {
                    relFile r = new relFile();
                    r.file = s;
                    r.lines = File.ReadAllLines(s);
                    r.headers = r.lines[0].Split('\t');

                    for (int i = 0; i < r.headers.Length; i++)
                    {
                        if (r.unit == "")
                        {
                            try
                            {
                                r.unit = r.headers[i].Split('(')[1].Split(')')[0];
                                r.headers[i] = r.headers[i].Split('(')[0].Trim();
                            }
                            catch { }
                        }
                        if (!headers.Contains(r.headers[i]))
                            headers.Add(r.headers[i]);
                    }
                    column c = new column();

                    string f = s.Split(Path.DirectorySeparatorChar)[s.Split(Path.DirectorySeparatorChar).Length - 1];
                    txtOutput.Text += f + "\r\n";
                    txtOutput.Text += $"{string.Join("\r\n", r.headers)}\r\n\r\n";
                    relFiles.Add(r);
                }

                foreach (string s in headers)
                {
                    Header h = new Header();
                    h.header = s;
                    Headers.Add(h);
                }

                foreach (Header h in Headers)
                    foreach (relFile r in relFiles)
                        if (r.headers.Contains(h.header))
                            h.occcurance++;

                int hMax = 0;
                foreach (Header h in Headers)
                    if (h.occcurance > hMax)
                        hMax = h.occcurance;

                List<string> suspKeys = new List<string>();
                for (int i = hMax; i > 1; i--)
                    foreach (Header h in Headers)
                        if (h.occcurance == i)
                            suspKeys.Add(h.header);
                txtOutput.Text = $"Recurrent keys:\r\n{string.Join("\r\n", suspKeys)}\r\n\r\n" + txtOutput.Text;


                string[] parentHeaders = File.ReadAllLines(activeDBdir + slash + "_nameKeyPairs.TXT");
                for (int i = 0; i < parentHeaders.Length; i++)
                    parentHeaders[i] = parentHeaders[i].Split('|')[1];

                foreach (string s in parentHeaders)
                    foreach (string st in headers)
                        if (s == st)
                            listBox1.Items.Add(s);

                AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                foreach (string s in headers)
                {
                    source.Add(s);
                    if (s == "NDB_No")
                        txtNdb.Text = s;
                    else if (s == "NutrDesc")
                        txtNutrDesc.Text = s;
                    else if (s == "Nutr_No")
                        txtNutrNo.Text = s;
                    else if (s == "Flav_Val" || s == "Isfl_Val")
                        txtFlavVal.Text = s;
                }

                txtNutrDesc.AutoCompleteCustomSource = source;
                txtNutrDesc.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNutrDesc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                txtNutrNo.AutoCompleteCustomSource = source;
                txtNutrNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNutrNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                txtFlavVal.AutoCompleteCustomSource = source;
                txtFlavVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtFlavVal.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                txtNdb.AutoCompleteCustomSource = source;
                txtNdb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNdb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            else
            {
                string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                if (!File.Exists(file) || !file.EndsWith(".TXT"))
                {
                    MessageBox.Show("Is not a valid .TXT file\n" + file);
                    txtOutput.Text = "<Drag and drop>\r\n<a single .TXT file>";
                    return;
                }

                relFile r = new relFile();
                r.file = file;
                r.lines = File.ReadAllLines(file);
                r.headers = r.lines[0].Split('\t');
                relf = r;
                foreach (string s in r.headers)
                {
                    Header H = new Header();
                    H.header = s.Split('(')[0].Trim();
                    try {
                        H.unit = s.Split('(')[1].Split(')')[0];
                    }
                    catch { }
                    if (!headers.Contains(s.Split('(')[0].Trim()))
                        headers.Add(s.Split('(')[0].Trim());
                    Headers.Add(H);
                }


                file = file.Split(Path.DirectorySeparatorChar)[file.Split(Path.DirectorySeparatorChar).Length - 1].Replace(".TXT", "");
                txtName.Text = file;


                txtOutput.Text += file + ".TXT\r\n";
                foreach (Header H in Headers)
                    txtOutput.Text += H.header + "\r\n";

                txtOutput.Text += "\r\nUnits detected:\r\n";
                foreach (Header H in Headers)
                    if (H.unit != null && H.unit != "")
                        txtOutput.Text += H.unit + "\r\n";

                string[] parentHeaders = File.ReadAllLines(activeDBdir + slash + "_nameKeyPairs.TXT");
                for (int i = 0; i < parentHeaders.Length; i++)
                    parentHeaders[i] = parentHeaders[i].Split('|')[1];

                foreach (string s in parentHeaders)
                    foreach (string st in headers)
                        if (s == st)
                            listBox1.Items.Add(s);

                AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                foreach (Header H in Headers)
                {
                    string s = H.header;
                    source.Add(s);
                    if (s == "NDB_No")
                        txtNdb.Text = s;
                }
                txtFlavVal.AutoCompleteCustomSource = source;
                txtFlavVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtFlavVal.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                txtNdb.AutoCompleteCustomSource = source;
                txtNdb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNdb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDBdir = shareDBdir + slash + comboBox1.Text;
            listBox1.Items.Clear();
            btnCreate.Enabled = false;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (radioMultiKey.Checked)
            {
                if (!headers.Contains(txtNutrNo.Text) || !headers.Contains(txtNutrDesc.Text) || !headers.Contains(txtNdb.Text) || !headers.Contains(txtFlavVal.Text))
                {
                    MessageBox.Show("Please fill everything out.  Not all the fields match.");
                    return;
                }

                string newRelDir = $"{Application.StartupPath}{slash}usr{slash}share{slash}rel{slash}multi{slash}{txtName.Text}";
                Directory.CreateDirectory(newRelDir);

                string unit = "";
                string ndb = "";
                string nutrNo = "";
                foreach (relFile r in relFiles)
                {
                    string f = r.file.Split(new char[] { '/', '\\' })[r.file.Split(new char[] { '/', '\\' }).Length - 1].Replace(".TXT", "");
                    Directory.CreateDirectory(newRelDir + slash + f);
                    for (int i = 0; i < r.headers.Length; i++)
                    {
                        column c = new column();
                        c.header = r.headers[i];
                        for (int j = 1; j < r.lines.Length; j++)
                            c.rows.Add(r.lines[j].Split('\t')[i]);
                        r.columns.Add(c);
                    }
                    foreach (column c in r.columns)
                    {
                        File.WriteAllLines(newRelDir + slash + f + slash + c.header + ".TXT", c.rows);
                        if (c.header == txtNutrNo.Text)
                            nutrNos = c.rows;
                        else if (c.header == txtFlavVal.Text)
                            flavVal = $"{slash}{f}{slash}{c.header}.TXT";
                        if (r.unit != "")
                            unit = r.unit;
                    }
                    nutrNos = nutrNos.ToArray().Distinct().ToList();
                    nutrNos.Sort();
                    File.WriteAllText(newRelDir + slash + "_dbInit.TXT", $"[ParentDB]{comboBox1.Text}\r\n[Values]{txtFlavVal.Text}.TXT\r\n[Units]{unit}\r\n[ParentKey]$NDBNo\r\n[ChildKey]$NutrNo\r\n[Fields]{txtNutrDesc.Text}.TXT");
                    txtOutput.Text = "Units: " + unit + txtOutput.Text;
                }

                files = Directory.GetFiles(newRelDir, "*.TXT", SearchOption.AllDirectories);
                foreach (string s in files)
                {
                    if (s.EndsWith(txtFlavVal.Text + ".TXT"))
                        ndb = s.Replace(txtFlavVal.Text + ".TXT", txtNdb.Text + ".TXT");
                    else if (s.EndsWith(txtNutrDesc.Text + ".TXT"))
                        nutrNo = s.Replace(txtNutrDesc.Text + ".TXT", txtNutrNo.Text + ".TXT");
                }
                ndb = ndb.Replace(newRelDir, "").Replace(slash, "/");
                nutrNo = nutrNo.Replace(newRelDir, "").Replace(slash, "/");
                string text = File.ReadAllText(newRelDir + slash + "_dbInit.TXT");
                text = text.Replace("$NDBNo", ndb).Replace("$NutrNo", nutrNo);
                File.WriteAllText(newRelDir + slash + "_dbInit.TXT", text);
                MessageBox.Show($"Database created successfully! Go to the extended tab of the {comboBox1.Text} database to configure it");
                //this.Close();
            }
            else
            {
                if (!headers.Contains(txtNdb.Text) || !headers.Contains(txtFlavVal.Text))
                {
                    MessageBox.Show("Please fill everything out.  Not all the fields match.");
                    return;
                }

                string newRelDir = $"{Application.StartupPath}{slash}usr{slash}share{slash}rel{slash}single{slash}{txtName.Text}";

                Directory.CreateDirectory(newRelDir);

                for (int i=0;i<relf.headers.Length;i++){
                    List<string> c = new List<string>();
                    for (int j=1;j<relf.lines.Length;j++)
                        c.Add(relf.lines[j].Split('\t')[i]);                    
                    File.WriteAllLines($"{newRelDir}{slash}{headers[i]}.TXT" , c);
                }
            }
        }

        private void radioMultiKey_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMultiKey.Checked)
            {
                txtOutput.Text = "<Drag and drop>\r\n<the folder>\r\n<contains *.TXT files>\r\n<more than one!!>";
                lblNutrNo.Visible = true;
                lblNutrDesc.Visible = true;
                txtNutrNo.Visible = true;
                txtNutrDesc.Visible = true;
                lblFLavVal.Text = "Flav_Val:";
                txtLoc.Text = $"{slash}usr{slash}share{slash}DBs{slash}rel{slash}multi{slash}";
            }
            else
            {
                txtOutput.Text = "<Drag and drop>\r\n<a single .TXT file>";
                lblNutrNo.Visible = false;
                lblNutrDesc.Visible = false;
                txtNutrNo.Visible = false;
                txtNutrDesc.Visible = false;
                lblFLavVal.Text = "Value:";
                txtLoc.Text = $"{slash}usr{slash}share{slash}DBs{slash}rel{slash}single{slash}";
            }
        }
    }
}
