using System;
using System.Windows.Forms;

namespace Nutritracker
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.Text = $"Settings — {frmMain.currentUser.name}";
        }
    }
}
