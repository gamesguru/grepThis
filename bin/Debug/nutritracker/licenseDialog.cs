using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace Nutritracker
{
    public partial class licenseDialog : Form
    {
        public licenseDialog()
        {
            InitializeComponent();
        }

        private void btnDecline_Click(object sender, EventArgs e) => Process.GetCurrentProcess().Kill();
        
        string[] apache;
        public List<string> profData;
        public string rt = "";
        string sl = Path.DirectorySeparatorChar.ToString();
        bool license;
        private void btnAccept_Click(object sender, EventArgs e)
        {
            profData.Add($"[License]StallmanApproves_{name.GetHashCode()}");
            File.WriteAllLines($"{rt}profile.TXT", profData);
            license = true;
            this.Close();
        }

        private void licenseDialog_FormClosing(object sender, FormClosingEventArgs e) { if (!license) Process.GetCurrentProcess().Kill(); }
        

        string name;
        private void licenseDialog_Load(object sender, EventArgs e)
        {
            mH = true;
            richTextBox1.Clear();
            richTextBox1.Rtf = Nutritracker.Properties.Resources.apache;
            List<string> tmp = richTextBox1.Lines.ToList();
            tmp.RemoveAt(40);
            richTextBox1.Lines = tmp.ToArray();
            mH = false;
            apache = richTextBox1.Lines;
            foreach (string s in profData)
                if (s.StartsWith("[Name]"))
                {
                    name = s.Replace("[Name]", "");
                    this.Text = $"License Agreement — {name}";
                    break;
                }
        }

        bool mH = false;
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (mH) return;
            btnAccept.Visible = false;
            bool en = true;
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
                if (richTextBox1.Lines[i].Replace($"in acknowledgement: {name}", "in acknowledgement: ") != apache[i])
                    en = false;
            if (!richTextBox1.Lines[richTextBox1.Lines.Length - 1].Contains(name))
                en = false;
            btnAccept.Visible = en;
        }
    }
}