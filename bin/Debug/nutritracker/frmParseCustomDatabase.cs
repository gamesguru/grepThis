using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Nutritracker
{
    public partial class frmParseCustomDatabase : Form
    {
        public frmParseCustomDatabase()
        {
            InitializeComponent();
            this.AllowDrop = true;     }

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

        public int getBiggestInt(double d)
        {
            return Convert.ToInt32(d);
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

        public class Column{
            public string header;
            public string[] items;
			public string units;
        }

        public static Column[] columns;

        private void frmParseCustomDatabase_Load(object sender, EventArgs e)
        {
            lstViewResult.FullRowSelect = true;
        }
        

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                    lstViewResult.Items.RemoveAt(lstViewResult.SelectedIndices[0]);
            }
            catch { }


        }

        //public static string parseSource;
        private void removeSelectedColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstViewResult.Columns.Count > 0)
            {
                lstViewResult.Columns.RemoveAt(comboColumns.SelectedIndex);
                comboColumns.Items.RemoveAt(comboColumns.SelectedIndex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<string> knownUnits = new List<string>() { "mg", "g", "iu", "mcg", "%", "μg", "ug", "kj" , "cal" , "kcal" };
        List<string> gatheredUnits;
        //List<string> suspectedUnits;
        private void checkForColumnUnitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboColumns.Items.Count == 0)
                MessageBox.Show("No columns were detected!  Please parse something.", "Nothing found", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            else if (comboColumns.Items.Count == 1)
                if (MessageBox.Show("Only one column detected, are you sure you want to gather units?", "Only one column..", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)                
                    gatherColumnUnits();
                
            else
                    gatherColumnUnits();
            
        }

        private void gatherColumnUnits()
        {
            int sure = 0;
            int susp = 0;
            gatheredUnits = new List<string>();
            for (int i = 0; i < lstViewResult.Columns.Count; i++)
            {
                string colText = lstViewResult.Columns[i].Text.ToLower();

                int n2 = colText.IndexOf(")");
                int n1 = colText.IndexOf("(");

                string suspU = "";
                if (n2 > 0 && n1 > 0)
                    suspU = colText.Substring(n1, n2 - n1 + 1);

                foreach (string s in knownUnits)
                {
                    //MessageBox.Show(s);
                    if (colText.Contains(s) && s.Length > 1 || colText.Contains("(" + s + ")"))
                    {
                        gatheredUnits.Add(s);
                        sure++;
                        //MessageBox.Show(s);
                        goto cont;
                    }
                }

                if (colText.Contains("(") && colText.Contains(")") && (n2 - n1) > 1 && (n2 - n1) < 9 && !gatheredUnits.Contains(suspU.Replace("(", "").Replace(")", "")))
                {

                    if (n2 < 0 || n1 < 0)
                        continue;
                    gatheredUnits.Add(suspU);
                    susp++;
                }
                cont:
                continue;
            }

            
            if (gatheredUnits.Count == 0)
                return;
            string result = string.Join(", ", gatheredUnits);
            MessageBox.Show(sure.ToString() + " units identified and " + susp.ToString() + " suspected:\n" + result, "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void convertToGramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int k = comboColumns.SelectedIndex;
            for (int i = 0; i < lstViewResult.Items.Count; i++)
            {
                string source = lstViewResult.Items[i].SubItems[k].Text.ToLower();
                if (source.Contains("ounces") || source.Contains("ounce"))
                {
                    double oldV = getBiggestDouble(source);
                    double newV = Math.Round(oldV * 28.4, oldV.ToString().Length - oldV.ToString().IndexOf("."));
                    lstViewResult.Items[i].SubItems[k].Text = source.Replace(oldV.ToString(), newV.ToString()).Replace("ounces", "g").Replace("ounce", "g");
                }
            }
        }

        private void selectColumnToMake100gToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int k = comboColumns.SelectedIndex;
        }


        private void btnNewDB_Click(object sender, EventArgs e)
        {
            int n = lstViewResult.Columns.Count;

            if (n > 1)
            {
                if (n == 2 && MessageBox.Show("Only two fields detected.  It looks like you meant to create a field or relational database instead.  Continue?", "Create new Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                                    
                frmNewDB frmNDB = new frmNewDB(this);
                for (int i = 0; i < comboColumns.Items.Count; i++)
                    frmNDB.arr.Add(comboColumns.Items[i].ToString());
                frmNDB.n = lstViewResult.Items.Count;
                frmNDB.ShowDialog();
            }
            else { MessageBox.Show("You haven't transferred more than 1 column to the list!"); }
        }

        private void btnNewField_Click(object sender, EventArgs e)
        {
            int n = lstViewResult.Columns.Count;

            if (n > 1)
            {
                frmNewField frmNDF = new frmNewField(this);
                for (int i = 0; i < comboColumns.Items.Count; i++)
                    frmNDF.arr.Add(comboColumns.Items[i].ToString());
                frmNDF.n = lstViewResult.Items.Count;
                frmNDF.ShowDialog();
            }
            else { MessageBox.Show("You haven't transferred more than 1 column to the list!"); }
        }

        public string getVal(int i, int j)
        {
            return lstViewResult.Items[i].SubItems[j].Text;
        }

        public List<string> getCol(string header)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < lstViewResult.Columns.Count; i++)
                if (lstViewResult.Columns[i].Text == header)
                {
                    for (int j = 0; j < lstViewResult.Items.Count; j++)
                        output.Add(lstViewResult.Items[j].SubItems[i].Text);
                    break;
                }
            return output;
        }

        public List<string> getRow(int x, List<string> headers)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < lstViewResult.Columns.Count; i++)
                if (headers.Contains(lstViewResult.Columns[i].Text))
                    output.Add(lstViewResult.Items[x].SubItems[i].Text);
            return output;
        }

        private void parseInput()
        {
            int colSpan = 0;

            foreach (string s in sourceInput)
                if (s.Split('\t').Length > colSpan)
                    colSpan = s.Split('\t').Length;

            textBox1.Text += "\r\n# of headers:\t" + sourceInput[0].Split('\t').Length.ToString();
            textBox1.Text += "\r\n# of max columns:\t" + colSpan.ToString();
            textBox1.Text += "\r\n# of rows:\t\t" + sourceInput.Length.ToString();

            for (int i = 0; i < sourceInput.Length; i++)
                if (sourceInput[i].Split('\t').Length != colSpan)
                    if (MessageBox.Show($"Error on row #{i}\r\nonly has {sourceInput[i].Split('\t').Length} entries, {colSpan} expected!!\r\n{sourceInput[i]}", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                        break;

            try
            {
                columns = new Column[colSpan];
                for (int i = 0; i < colSpan; i++)
                    columns[i] = new Column();
                for (int i = 0; i < sourceInput[0].Split('\t').Length; i++)
                {
                    columns[i].header = sourceInput[0].Split('\t')[i];
                    columns[i].items = new string[sourceInput.Length];
                    for (int j = 0; j < sourceInput.Length; j++)                    
                        columns[i].items[j] = sourceInput[j].Split('\t')[i];
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parse error.\n" + ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ListViewItem[] itms = new ListViewItem[sourceInput.Length - 1];
            for (int j = 0; j < sourceInput.Length - 1; j++)
                itms[j] = new ListViewItem();
            lstViewResult.BeginUpdate();
            for (int i = 0; i < colSpan; i++)
            {
                lstViewResult.Columns.Add(columns[i].header);
                for (int j = 1; j < sourceInput.Length; j++)
                {
                    if (i == 0)
                        itms[j - 1].Text = columns[0].items[j];
                    else
                        itms[j - 1].SubItems.Add(columns[i].items[j]);
                }
            }

            lstViewResult.Items.AddRange(itms);
            lstViewResult.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.HeaderSize);
            lstViewResult.EndUpdate();
            for (int i = 0; i < lstViewResult.Columns.Count; i++)
                comboColumns.Items.Add(lstViewResult.Columns[i].Text);
            
        }

        string[] sourceInput;

        private void importFromtxtFilequickestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            lstViewResult.Clear();
            comboColumns.Items.Clear();
            openFileDialog1.InitialDirectory = Application.StartupPath + Path.DirectorySeparatorChar.ToString() + "lib";
            openFileDialog1.ShowDialog();
            textBox1.Text += openFileDialog1.FileName + "\r\n";
            if (!File.Exists(openFileDialog1.FileName))
                return;
            sourceInput = File.ReadAllLines(openFileDialog1.FileName);
            parseInput();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            textBox1.Clear();
            lstViewResult.Clear();
            comboColumns.Items.Clear();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length != 1)
            {
                textBox1.Text += "Error: not exactly one file dropped onto pallete.";
                foreach (string s in files)
                    textBox1.Text += "\r\n" + s;
                return;
            }

            textBox1.Text += files[0] + "\r\n";
            sourceInput = File.ReadAllLines(files[0]);
            parseInput();
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Clear();
            lstViewResult.Clear();
            comboColumns.Items.Clear();
            openFileDialog1.InitialDirectory = Application.StartupPath + Path.DirectorySeparatorChar.ToString() + "lib";
            openFileDialog1.ShowDialog();
            textBox1.Text += openFileDialog1.FileName + "\r\n";
            if (!File.Exists(openFileDialog1.FileName))
                return;
            sourceInput = File.ReadAllLines(openFileDialog1.FileName);
            parseInput();
        }
    }
}
