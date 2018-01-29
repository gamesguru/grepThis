using System;
using System.Windows.Forms;

namespace Nutritracker
{
    public partial class sfCalc : Form
    {
        public sfCalc()
        {
            InitializeComponent();
        }
        public static string currentName = "";
        public int age = 0;
        public int wt = 0;
        public int ht = 0;
        public string gender = "";

        private void frmBodyfatCalc_Load(object sender, EventArgs e)
        {
            this.Text = "Bodyfat Estimator — " + currentName;
            height = ht * 2.54;
            lblAge.Text = "Age: " + age.ToString();
            lblGender.Text = "Gender: " + gender;
            lblHt.Text = "Height: " + ht.ToString() + " in";
            lblWt.Text = "Weight: " + wt.ToString() + " lbs";
            if (gender == "female")
            {
                lblHips.Visible = true;
                lblHipInch.Visible = true;
                txtHip.Visible = true;

                lbl3Chest.Text = "Tricep: ";
                lbl3Ab.Text = "Suprailiac: ";
            }
            button1.Text = "Save Measurement";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        #region Navy test
        private void txtNeck_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtWaist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtHip_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        public double neck = 1;
        public double waist = 1;
        public double hip = 1;
        public double height = 1;

        private void txtNeck_TextChanged(object sender, EventArgs e)
        {
            if (txtNeck.TextLength == 0)
                return;
            neck = Convert.ToInt32(txtNeck.Text);
            neck *= 2.54;
            if (neck > 0 && waist > 0 && hip > 0)
                navyUpdate();
        }

        private void txtWaist_TextChanged(object sender, EventArgs e)
        {
            if (txtWaist.TextLength == 0)
                return;
            waist = Convert.ToInt32(txtWaist.Text);
            waist *= 2.54;
            if (neck > 0 && waist > 0 && hip > 0)
                navyUpdate();
        }

        private void txtHip_TextChanged(object sender, EventArgs e)
        {
            if (txtHip.TextLength == 0)
                return;
            hip = Convert.ToInt32(txtHip.Text);
            hip *= 2.54;
            if (neck > 0 && waist > 0 && hip > 0)
                navyUpdate();
        }
        private void navyUpdate()
        {            
            double result = 0.0;
            if (gender == "male")
                result = 495 / (1.0324 - 0.19077 * (Math.Log10(waist - neck)) + 0.15456 * (Math.Log10(height))) - 450;
            else
                result = 495 / (1.29579 - 0.35004 * (Math.Log10(waist + hip - neck)) + 0.22100 * (Math.Log10(height))) - 450;
            lblNavyResult.Text = "Your bodyfat: " + Math.Round(result, 1).ToString() + "%";
        }
#endregion

        #region three site test
        private void txt3Chest_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt3Ab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt3Thigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }


        private void txt3Chest_TextChanged(object sender, EventArgs e)
        {
            threeSiteUpdate();
        }

        private void txt3Ab_TextChanged(object sender, EventArgs e)
        {
            threeSiteUpdate();
        }

        private void txt3Thigh_TextChanged(object sender, EventArgs e)
        {
            threeSiteUpdate();
        }

        private void threeSiteUpdate()
        {
            if (txt3Thigh.TextLength == 0 || txt3Ab.TextLength == 0 || txt3Chest.TextLength == 0)
                return;
            int s = Convert.ToInt32(txt3Chest.Text) + Convert.ToInt32(txt3Ab.Text) + Convert.ToInt32(txt3Thigh.Text);
            double result = 0.0;
            if (gender == "male")
                result = 495 / (1.10938 - (0.0008267 * s) + (0.0000016 * s * s) - (0.0002574 * age)) - 450;
            else
                result = 495 / (1.089733 - (0.0009245 * s) + (0.0000025 * s * s) - (0.0000979 * age)) - 450;
            lbl3Result.Text = "Your bodyfat: " + Math.Round(result, 1).ToString() + "%";
        }
        #endregion

        #region seven site test
        #endregion
        private void txt7Chest_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Ab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Thigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Tricep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Sub_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Sup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Mid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt7Chest_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Ab_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Thigh_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Tricep_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Sub_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Sup_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void txt7Mid_TextChanged(object sender, EventArgs e)
        {
            sevenSitesUpdate();
        }

        private void sevenSitesUpdate()
        {
            if (txt7Chest.TextLength == 0 || txt7Ab.TextLength == 0 || txt7Thigh.TextLength == 0 || txt7Tricep.TextLength == 0 || txt7Sub.TextLength == 0 || txt7Sup.TextLength == 0 || txt7Mid.TextLength == 0)
                return;
            int s = Convert.ToInt32(txt7Chest.Text) + Convert.ToInt32(txt7Ab.Text) + Convert.ToInt32(txt7Thigh.Text) + Convert.ToInt32(txt7Tricep.Text) + Convert.ToInt32(txt7Sub.Text) + Convert.ToInt32(txt7Sup.Text) + Convert.ToInt32(txt7Mid.Text);
            double result = 0.0;
            if (gender == "male")
                result = 495 / (1.112 - (0.00043499 * s) + (0.00000055 * s * s) - (0.00028826 * age)) - 450;
            else
                result = 495 / (1.097 - (0.00046971 * s) + (0.00000056 * s * s) - (0.00012828 * age)) - 450;
            lbl7Result.Text = "Your bodyfat: " + Math.Round(result, 1).ToString() + "%";
        }
    }
}
