using System;
using System.Windows.Forms;
using System.IO;

namespace Nutritracker
{
    public partial class lmCalc : Form
    {
        public lmCalc()
        {
            InitializeComponent();
        }
        public static int height;
        public static int weight;
        public static int bodyfat;

        string slash = Path.DirectorySeparatorChar.ToString();
        private void frmLeanPotentialCalc_Load(object sender, EventArgs e)
        {
            this.Text = $"Lean Mass Limits for {File.ReadAllLines($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}profile.TXT")[0]}";
            numUpDownEH.Value = bodyfat;
            numUpDownCB.Value = bodyfat;
            lblHt.Text = "Height:   " + height.ToString() +" in";
            lblWt.Text = "Weight: " + weight.ToString() + " lbs";
            lblBF.Text = "Bodyfat: " + bodyfat.ToString() + "%";

            lblMBresult.Text = "Contest shape (5-6%) @ " + Math.Round((height * 2.54 - 102) * 2.205, 0).ToString() + " to " + Math.Round((height * 2.54 - 98) * 2.205, 0).ToString() + " lbs";
            lblEHresult.Text = "Your natural limit: " + Math.Round(2.205 * 2200.0 * height * .0254 * height * .0254 / (100.0 - bodyfat), 0).ToString() + " to " + Math.Round(2.205 * 2450.0 * height * .0254 * height * .0254 / (100.0 - bodyfat), 0).ToString() + " lbs";
        }


        //Eric Helms
        private void numUpDownEH_ValueChanged(object sender, EventArgs e)
        {
            int bf = Convert.ToInt32(numUpDownEH.Value);
            lblEHresult.Text = "Your natural limit: " + Math.Round(2.205 * 2200.0 * height * .0254 * height * .0254 / (100.0 - bf), 0).ToString() + " lbs to " + Math.Round(2.205 * 2450.0 * height * .0254 * height * .0254 / (100.0 - bf), 0).ToString() + " lbs";
        }

        private void numUpDownEH_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


        //Casey Butt
        private void updateCBvalues()
        {
            if (txtWrist.TextLength == 0 || txtAnkle.TextLength == 0)
                return;
            double wrist = Convert.ToDouble(txtWrist.Text);
            double ankle = Convert.ToDouble(txtAnkle.Text);
            int bf = (int)numUpDownCB.Value;
            double result = Math.Sqrt(height) * (height * Math.Sqrt(wrist) / 24.667 + height * Math.Sqrt(ankle) / 19.0104) * (1 + bf / 224);

            lblCBNatLeanLim.Text = "Your lean mass:   " + Math.Round(result, 0).ToString() + " lbs";
            result /= (1 - 0.01 * bf);
            lblCBNatLim.Text = "Your bulked limit: " + Math.Round(result, 0).ToString() + " lbs";

            lblCBchest.Text = "Chest: ".PadRight(12) + Math.Round(1.475 * wrist + 1.1832 * ankle + .2962 * height, 1).ToString() + " in";
            lblCBarm.Text = "Arm: ".PadRight(13) + Math.Round(1.1709 * wrist + .1100 * height, 1).ToString() + " in";
            lblCBforearm.Text = "Forearm: ".PadRight(9) + Math.Round(.95 * wrist + .0991 * height, 1).ToString() + " in";
            lblCBneck.Text = "Neck: ".PadRight(12) + Math.Round(1.1875 * wrist + .1101 * height, 1).ToString() + " in";
            lblCBthigh.Text = "Thigh: ".PadRight(13) + Math.Round(1.3737 * ankle + .1518 * height, 1).ToString() + " in";
            lblCBcalf.Text = "Calf: ".PadRight(14) + Math.Round(.8812 * ankle + .1050 * height, 1).ToString() + " in";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.' || (e.KeyChar == '.' && txtWrist.Text.Contains(".") && txtWrist.SelectionLength == 0))
                e.Handled = true;
            if (txtWrist.TextLength > 1 && !txtWrist.Text.Contains(".") && e.KeyChar != '.' && !Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                e.Handled = true;

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.' || (e.KeyChar == '.' && txtAnkle.Text.Contains(".") && txtAnkle.SelectionLength == 0))
                e.Handled = true;
            if (txtAnkle.TextLength > 1 && !txtAnkle.Text.Contains(".") && e.KeyChar != '.' && !Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtWrist_TextChanged(object sender, EventArgs e)
        {
            updateCBvalues();
        }

        private void txtAnkle_TextChanged(object sender, EventArgs e)
        {
            updateCBvalues();
        }

        private void numUpDownCB_ValueChanged(object sender, EventArgs e)
        {
            updateCBvalues();
        }

        private void txtWrist_MouseUp(object sender, MouseEventArgs e)
        {
            txtWrist.SelectAll();
        }

        private void txtWrist_MouseDown(object sender, MouseEventArgs e)
        {
            txtWrist.SelectAll();
        }

        private void txtAnkle_MouseUp(object sender, MouseEventArgs e)
        {

            txtAnkle.SelectAll();
        }

        private void txtAnkle_MouseDown(object sender, MouseEventArgs e)
        {
            txtAnkle.SelectAll();
        }

        private void txtWrist_Enter(object sender, EventArgs e)
        {
            txtWrist.SelectAll();
        }

        private void txtAnkle_Enter(object sender, EventArgs e)
        {
            txtAnkle.SelectAll();
        }

        private void txtWrist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
                e.SuppressKeyPress = true;
        }

        private void txtAnkle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
                e.SuppressKeyPress = true;
        }
    }
}
