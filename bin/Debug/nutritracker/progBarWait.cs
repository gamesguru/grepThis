using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nutritracker
{
    public partial class progBarWait : Form
    {
        public progBarWait()
        {
            InitializeComponent();
        }

        public void setLblCurObj(string s) => this.Invoke((MethodInvoker)delegate { lblProgress.Text = $"Current object: {s}"; }); //lblProgress.Text = $"Current object: {s}";
        public void _setProgMax (int m) => this.Invoke((MethodInvoker)delegate { progBar.Maximum = m; });
        public void setProgVal (int m) => this.Invoke((MethodInvoker)delegate { progBar.Value = m; });
        public void selfDispose() => this.Close();

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //...
                this.Close();
            }
        }
    }
}
