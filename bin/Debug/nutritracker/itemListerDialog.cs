using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nutritracker
{
    public partial class itemListerDialog : Form
    {
        private  frmActiveFields mainForm = null;
        public itemListerDialog(Form callingForm)
        {
            mainForm = callingForm as frmActiveFields;
            InitializeComponent();
        }

        public List<string> items;
        private void itemListerDialog_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string s = items[i];
                if (i < items.Count - 1)
                    s += "\r\n";
                textBox1.Text += s;
            }
            textBox1.Select(0, 0);
        }

        private void itemListerDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { mainForm.reEnableButton(); }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
