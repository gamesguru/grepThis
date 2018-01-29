using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Nutritracker
{


    public partial class frmManageDB : Form
    {

        public frmManageDB()
        {
            InitializeComponent();
        }
        public List<String> importArray(string filename)
        {
            list.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)                
                    list.Add(line);                
            }
            return list;
        }
        List<string> list = new List<string>();

        string slash = Path.DirectorySeparatorChar.ToString();

        public string nutkeyPath = "";
        string dbDir = "";
        string[] pubDBs;
        string[] userDBs;
        private void frmManageDBfields_Load(object sender, EventArgs e)
        {
            pubDBs = Directory.GetDirectories($"{Application.StartupPath}{slash}usr{slash}share{slash}DBs");
            userDBs = Directory.GetDirectories($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs");

            if (pubDBs.Length == 0 && userDBs.Length == 0)
            {
                MessageBox.Show("No databases found, try going to the spreadsheet wizard or reinstalling the program.");
                this.Close();
            }

            for (int i = 0; i < userDBs.Length; i++)
            {
                userDBs[i] = userDBs[i].Replace($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}", "");
                if (!userDBs[i].StartsWith("f_user") && !userDBs[i].StartsWith("_"))
                    comboBox1.Items.Add(userDBs[i] + " (user)");
            }
            for (int i = 0; i < pubDBs.Length; i++)
            {
                pubDBs[i] = pubDBs[i].Replace($"{Application.StartupPath}{slash}usr{slash}share{slash}DBs{slash}", "");
                if (!pubDBs[i].StartsWith("_"))
                    comboBox1.Items.Add(pubDBs[i] + " (share)");
            }
            
            if (comboBox1.Items.Count > 0 && File.Exists($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}Default.TXT"))
                comboBox1.SelectedIndex = Convert.ToInt32(File.ReadAllLines($"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}Default.TXT")[0]);
            
            //comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void refresh()
        {
            #region resets controls
            list.Clear();
            txtname.Text = "";
            txtNDB.Text = "";
            txtServ1.Text = "";
            txtWt1.Text = "";
            txtServ2.Text = "";
            txtWt2.Text = "";

            txtCals.Text = "";
            txtFat.Text = "";
            txtsatFat.Text = "";
            txtCarbs.Text = "";
            txtFiber.Text = "";
            txtSugar.Text = "";
            txtProtein.Text = "";
            txttransFat.Text = "";
            txtpolyFat.Text = "";
            txtmonoFat.Text = "";
            txtala.Text = "";
            txtepadha.Text = "";
            txtK.Text = "";
            txtMg.Text = "";
            txtCholesterol.Text = "";
            txtSodium.Text = "";
            txtCalcium.Text = "";
            txtIron.Text = "";
            txtVitA.Text = "";
            txtVitC.Text = "";

            txtVitD.Text = "";
            txtVitE.Text = "";
            txtVitK.Text = "";
            txtB1.Text = "";
            txtB2.Text = "";
            txtB3.Text = "";
            txtB5.Text = "";
            txtB6.Text = "";
            txtB7.Text = "";
            txtB9.Text = "";
            txtB12.Text = "";
            txtZn.Text = "";
            txtSe.Text = "";
            txtB.Text = "";
            txtI.Text = "";
            txtP.Text = "";
            txtMn.Text = "";
            txtFl.Text = "";
            txtCu.Text = "";
            txtCr.Text = "";
            txtMo.Text = "";
            txtLyco.Text = "";
            txtLutZea.Text = "";
            txtCho.Text = "";
            txtIno.Text = "";
            txtCarn.Text = "";
            txtLipoic.Text = "";

            txtalan.Text = "";
            txtarg.Text = "";
            txtasparag.Text = "";
            txtaspartic.Text = "";
            txtcys.Text = "";
            txtglutamine.Text = "";
            txtgluacid.Text = "";
            txtgly.Text = "";
            txthis.Text = "";
            txtisoleu.Text = "";
            txtleu.Text = "";
            txtlys.Text = "";
            txtmeth.Text = "";
            txtphen.Text = "";
            txtproline.Text = "";
            txtserine.Text = "";
            txtthreo.Text = "";
            txttrypto.Text = "";
            txttyro.Text = "";
            txtvaline.Text = "";
            #endregion
            AutoCompleteStringCollection source = new AutoCompleteStringCollection();
            //MessageBox.Show(list.Count.ToString());
            if (File.Exists(nutkeyPath))
                list = importArray(nutkeyPath);
            dbDir = nutkeyPath.Replace($"{slash}_nutKeyPairs.TXT", "");
            string[] files = Directory.GetFiles(dbDir);

            for (int i = 0; i < list.Count; i++)
            {
                #region prepopulate
                if (list[i].StartsWith("#"))
                    continue;
                string field = list[i].Split('|')[1];
                string file = list[i].Split('|')[0];
                if (field == "FoodName")
                    txtname.Text = file;
                else if (field == "NDBNo")
                    txtNDB.Text = file;
                else if (field == "Serving")
                    txtServ1.Text = file;
                // else if (field == "Serving2")
                //     txtServ2.Text = file;
                else if (field == "Weight")
                    txtWt1.Text = file;
                // else if (field == "Weight2")
                //     txtWt2.Text = file;
                else if (field == "Cals")
                    txtCals.Text = file;
                else if (field == "FatTot")
                    txtFat.Text = file;
                else if (field == "FatSat")
                    txtsatFat.Text = file;
                else if (field == "Carbs")
                    txtCarbs.Text = file;
                else if (field == "Fiber")
                    txtFiber.Text = file;
                else if (field == "Sugar")
                    txtSugar.Text = file;
                else if (field == "Protein")
                    txtProtein.Text = file;
                else if (field == "Cholest")
                    txtCholesterol.Text = file;
                else if (field == "Na")
                    txtSodium.Text = file;
                else if (field == "Ca")
                    txtCalcium.Text = file;
                else if (field == "Fe")
                    txtIron.Text = file;
                else if (field == "VitA")
                    txtVitA.Text = file;
                else if (field == "VitC")
                    txtVitC.Text = file;
                else if (field == "FatTrans")
                    txttransFat.Text = file;
                else if (field == "FatPoly")
                    txtpolyFat.Text = file;
                else if (field == "FatMono")
                    txtmonoFat.Text = file;
                else if (field == "ALA")
                    txtala.Text = file;
                else if (field == "EpaDha")
                    txtepadha.Text = file;
                else if (field == "K")
                    txtK.Text = file;
                else if (field == "Mg")
                    txtMg.Text = file;

                //tab 2
                else if (field == "VitD")
                    txtVitD.Text = file;
                else if (field == "VitE")
                    txtVitE.Text = file;
                else if (field == "VitK")
                    txtVitK.Text = file;
                else if (field == "B1")
                    txtB1.Text = file;
                else if (field == "B2")
                    txtB2.Text = file;
                else if (field == "B3")
                    txtB3.Text = file;
                else if (field == "B5")
                    txtB5.Text = file;
                else if (field == "B6")
                    txtB6.Text = file;
                else if (field == "B7")
                    txtB7.Text = file;
                else if (field == "B9")
                    txtB9.Text = file;
                else if (field == "B12")
                    txtB12.Text = file;
                else if (field == "Zn")
                    txtZn.Text = file;
                else if (field == "Se")
                    txtSe.Text = file;
                else if (field == "B")
                    txtB.Text = file;
                else if (field == "I")
                    txtI.Text = file;
                else if (field == "P")
                    txtP.Text = file;
                else if (field == "Mn")
                    txtMn.Text = file;
                else if (field == "F")
                    txtFl.Text = file;
                else if (field == "Cu")
                    txtCu.Text = file;
                else if (field == "Cr")
                    txtCr.Text = file;
                else if (field == "Mo")
                    txtMo.Text = file;
                else if (field == "Lycopene")
                    txtLyco.Text = file;
                else if (field == "LutZea")
                    txtLutZea.Text = file;
                else if (field == "Choline")
                    txtCho.Text = file;
                else if (field == "Inositol")
                    txtIno.Text = file;
                else if (field == "Carnitine")
                    txtCarn.Text = file;
                else if (field == "LipoiAcid")
                    txtLipoic.Text = file;

                //tab 3
                else if (field == "Alanine")
                    txtalan.Text = file;
                else if (field == "Arginine")
                    txtarg.Text = file;
                else if (field == "Asparagine")
                    txtasparag.Text = file;
                else if (field == "AsparticAcid")
                    txtaspartic.Text = file;
                else if (field == "Cysteine")
                    txtCals.Text = file;
                else if (field == "Glutamine")
                    txtglutamine.Text = file;
                else if (field == "GlutamicAcid")
                    txtgluacid.Text = file;
                else if (field == "Glycine")
                    txtgly.Text = file;
                else if (field == "Histidine")
                    txthis.Text = file;
                else if (field == "Isoleucine")
                    txtisoleu.Text = file;
                else if (field == "Leucine")
                    txtleu.Text = file;
                else if (field == "Lysine")
                    txtlys.Text = file;
                else if (field == "Methionine")
                    txtmeth.Text = file;
                else if (field == "Phenylalanine")
                    txtphen.Text = file;
                else if (field == "Proline")
                    txtproline.Text = file;
                else if (field == "Serine")
                    txtserine.Text = file;
                else if (field == "Threonine")
                    txtthreo.Text = file;
                else if (field == "Tryptophan")
                    txttrypto.Text = file;
                else if (field == "Tyrosine")
                    txttyro.Text = file;
                else if (field == "Valine")
                    txtvaline.Text = file;
                #endregion
            }

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(dbDir + $"{slash}", "");
                if (!files[i].StartsWith("_"))
                    source.Add(files[i]);
            }
            #region autocompletes
            txtname.AutoCompleteCustomSource = source;
            txtname.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtNDB.AutoCompleteCustomSource = source;
            txtNDB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtNDB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtServ1.AutoCompleteCustomSource = source;
            txtServ1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtServ1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtServ2.AutoCompleteCustomSource = source;
            txtServ2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtServ2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtWt1.AutoCompleteCustomSource = source;
            txtWt1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtWt1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtWt2.AutoCompleteCustomSource = source;
            txtWt2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtWt2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCals.AutoCompleteCustomSource = source;
            txtCals.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCals.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtFat.AutoCompleteCustomSource = source;
            txtFat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtsatFat.AutoCompleteCustomSource = source;
            txtsatFat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtsatFat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCarbs.AutoCompleteCustomSource = source;
            txtCarbs.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCarbs.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtFiber.AutoCompleteCustomSource = source;
            txtFiber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFiber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtSugar.AutoCompleteCustomSource = source;
            txtSugar.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSugar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtProtein.AutoCompleteCustomSource = source;
            txtProtein.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtProtein.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCholesterol.AutoCompleteCustomSource = source;
            txtCholesterol.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCholesterol.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtSodium.AutoCompleteCustomSource = source;
            txtSodium.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSodium.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCalcium.AutoCompleteCustomSource = source;
            txtCalcium.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCalcium.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtIron.AutoCompleteCustomSource = source;
            txtIron.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtIron.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVitA.AutoCompleteCustomSource = source;
            txtVitA.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVitA.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVitC.AutoCompleteCustomSource = source;
            txtVitC.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVitC.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txttransFat.AutoCompleteCustomSource = source;
            txttransFat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txttransFat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtpolyFat.AutoCompleteCustomSource = source;
            txtpolyFat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtpolyFat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtmonoFat.AutoCompleteCustomSource = source;
            txtmonoFat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtmonoFat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtala.AutoCompleteCustomSource = source;
            txtala.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtala.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtepadha.AutoCompleteCustomSource = source;
            txtepadha.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtepadha.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtK.AutoCompleteCustomSource = source;
            txtK.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtK.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtMg.AutoCompleteCustomSource = source;
            txtMg.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtMg.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //tab 2

            txtVitD.AutoCompleteCustomSource = source;
            txtVitD.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVitD.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVitE.AutoCompleteCustomSource = source;
            txtVitE.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVitE.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtVitK.AutoCompleteCustomSource = source;
            txtVitK.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVitK.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB1.AutoCompleteCustomSource = source;
            txtB1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB2.AutoCompleteCustomSource = source;
            txtB2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB3.AutoCompleteCustomSource = source;
            txtB3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB5.AutoCompleteCustomSource = source;
            txtB5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB6.AutoCompleteCustomSource = source;
            txtB6.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB7.AutoCompleteCustomSource = source;
            txtB7.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB9.AutoCompleteCustomSource = source;
            txtB9.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB9.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB12.AutoCompleteCustomSource = source;
            txtB12.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB12.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtZn.AutoCompleteCustomSource = source;
            txtZn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtZn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtSe.AutoCompleteCustomSource = source;
            txtSe.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSe.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtB.AutoCompleteCustomSource = source;
            txtB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtI.AutoCompleteCustomSource = source;
            txtI.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtI.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtP.AutoCompleteCustomSource = source;
            txtP.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtMn.AutoCompleteCustomSource = source;
            txtMn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtMn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtFl.AutoCompleteCustomSource = source;
            txtFl.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCu.AutoCompleteCustomSource = source;
            txtCu.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCr.AutoCompleteCustomSource = source;
            txtCr.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCr.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtMo.AutoCompleteCustomSource = source;
            txtMo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtMo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtLyco.AutoCompleteCustomSource = source;
            txtLyco.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtLyco.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtLutZea.AutoCompleteCustomSource = source;
            txtLutZea.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtLutZea.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCho.AutoCompleteCustomSource = source;
            txtCho.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCho.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtIno.AutoCompleteCustomSource = source;
            txtIno.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtIno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtCarn.AutoCompleteCustomSource = source;
            txtCarn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCarn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtLipoic.AutoCompleteCustomSource = source;
            txtLipoic.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtLipoic.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //tab 3

            txtalan.AutoCompleteCustomSource = source;
            txtalan.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtalan.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtarg.AutoCompleteCustomSource = source;
            txtarg.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtarg.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtasparag.AutoCompleteCustomSource = source;
            txtasparag.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtasparag.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtaspartic.AutoCompleteCustomSource = source;
            txtaspartic.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtaspartic.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtcys.AutoCompleteCustomSource = source;
            txtcys.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtcys.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtglutamine.AutoCompleteCustomSource = source;
            txtglutamine.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtglutamine.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtgluacid.AutoCompleteCustomSource = source;
            txtgluacid.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtgluacid.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtgly.AutoCompleteCustomSource = source;
            txtgly.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtgly.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txthis.AutoCompleteCustomSource = source;
            txthis.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txthis.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtisoleu.AutoCompleteCustomSource = source;
            txtisoleu.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtisoleu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtleu.AutoCompleteCustomSource = source;
            txtleu.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtleu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtlys.AutoCompleteCustomSource = source;
            txtlys.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtlys.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtmeth.AutoCompleteCustomSource = source;
            txtmeth.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtmeth.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtphen.AutoCompleteCustomSource = source;
            txtphen.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtphen.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtproline.AutoCompleteCustomSource = source;
            txtproline.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtproline.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtserine.AutoCompleteCustomSource = source;
            txtserine.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtserine.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtthreo.AutoCompleteCustomSource = source;
            txtthreo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtthreo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txttrypto.AutoCompleteCustomSource = source;
            txttrypto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txttrypto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txttyro.AutoCompleteCustomSource = source;
            txttyro.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txttyro.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtvaline.AutoCompleteCustomSource = source;
            txtvaline.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtvaline.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //user page


            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> text = new List<string>();
            #region write new lines
            if (txtNDB.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtNDB.Text}"))
                text.Add(txtNDB.Text + "|NDBNo");
            if (txtname.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtname.Text}"))
                text.Add(txtname.Text + "|FoodName");
            if (txtCals.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCals.Text}"))
                text.Add(txtCals.Text + "|Cals");
            if (txtProtein.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtProtein.Text}"))
                text.Add(txtProtein.Text + "|Protein");
            if (txtCarbs.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCarbs.Text}"))
                text.Add(txtCarbs.Text + "|Carbs");
            if (txtFiber.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtFiber.Text}"))
                text.Add(txtFiber.Text + "|Fiber");
            if (txtSugar.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtSugar.Text}"))
                text.Add(txtSugar.Text + "|Sugar");
            if (txtFat.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtFat.Text}"))
                text.Add(txtFat.Text + "|FatTot");
            if (txttransFat.TextLength > 0 && File.Exists($"{dbDir}{slash}{txttransFat.Text}"))
                text.Add(txttransFat.Text + "|FatTrans");
            if (txtsatFat.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtsatFat.Text}"))
                text.Add(txtsatFat.Text + "|FatSat");
            if (txtpolyFat.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtpolyFat.Text}"))
                text.Add(txtpolyFat.Text + "|FatPoly");
            if (txtmonoFat.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtmonoFat.Text}"))
                text.Add(txtmonoFat.Text + "|FatMono");
            if (txtala.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtala.Text}"))
                text.Add(txtala.Text + "|ALA");
            if (txtepadha.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtepadha.Text}"))
                text.Add(txtepadha.Text + "|EpaDha");
            if (txtK.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtK.Text}"))
                text.Add(txtK.Text + "|K");
            if (txtMg.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtMg.Text}"))
                text.Add(txtMg.Text + "|Mg");
            if (txtCholesterol.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCholesterol.Text}"))
                text.Add(txtCholesterol.Text + "|Cholest");
            if (txtSodium.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtSodium.Text}"))
                text.Add(txtSodium.Text + "|Na");
            if (txtCalcium.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCalcium.Text}"))
                text.Add(txtCalcium.Text + "|Ca");
            if (txtIron.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtIron.Text}"))
                text.Add(txtIron.Text + "|Fe");
            if (txtVitA.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtVitA.Text}"))
                text.Add(txtVitA.Text + "|VitA");
            if (txtVitC.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtVitC.Text}"))
                text.Add(txtVitC.Text + "|VitC");

            //tab 2
            if (txtVitD.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtVitD.Text}"))
                text.Add(txtVitD.Text + "|VitD");
            if (txtVitE.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtVitE.Text}"))
                text.Add(txtVitE.Text + "|VitE");
            if (txtVitK.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtVitK.Text}"))
                text.Add(txtVitK.Text + "|VitK");
            if (txtB1.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB1.Text}"))
                text.Add(txtB1.Text + "|B1");
            if (txtB2.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB2.Text}"))
                text.Add(txtB2.Text + "|B2");
            if (txtB3.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB3.Text}"))
                text.Add(txtB3.Text + "|B3");
            if (txtB5.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB5.Text}"))
                text.Add(txtB5.Text + "|B5");
            if (txtB6.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB6.Text}"))
                text.Add(txtB6.Text + "|B6");
            if (txtB7.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB7.Text}"))
                text.Add(txtB7.Text + "|B7");
            if (txtB9.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB9.Text}"))
                text.Add(txtB9.Text + "|B9");
            if (txtB12.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB12.Text}"))
                text.Add(txtB12.Text + "|B12");

            if (txtZn.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtZn.Text}"))
                text.Add(txtZn.Text + "|Zn");
            if (txtSe.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtSe.Text}"))
                text.Add(txtSe.Text + "|Se");
            if (txtB.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtB.Text}"))
                text.Add(txtB.Text + "|B");
            if (txtP.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtP.Text}"))
                text.Add(txtP.Text + "|P");
            if (txtMn.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtMn.Text}"))
                text.Add(txtMn.Text + "|Mn");
            if (txtFl.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtFl.Text}"))
                text.Add(txtFl.Text + "|F");
            if (txtCu.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCu.Text}"))
                text.Add(txtCu.Text + "|Cu");
            if (txtCr.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCr.Text}"))
                text.Add(txtCr.Text + "|Cr");
            if (txtMo.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtMo.Text}"))
                text.Add(txtMo.Text + "|Mo");
            if (txtLyco.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtLyco.Text}"))
                text.Add(txtLyco.Text + "|Lycopene");
            if (txtLutZea.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtLutZea.Text}"))
                text.Add(txtLutZea.Text + "|LutZea");
            if (txtCho.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCho.Text}"))
                text.Add(txtCho.Text + "|Choline");
            if (txtIno.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtIno.Text}"))
                text.Add(txtIno.Text + "|Inositol");
            if (txtCarn.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtCarn.Text}"))
                text.Add(txtCarn.Text + "|Carnitine");
            if (txtLipoic.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtLipoic.Text}"))
                text.Add(txtLipoic.Text + "|LipoicAcid");

            //tab 3
            if (txtalan.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtalan.Text}"))
                text.Add(txtalan.Text + "|Alanine");
            if (txtarg.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtarg.Text}"))
                text.Add(txtarg.Text + "|Arginine");
            if (txtasparag.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtasparag.Text}"))
                text.Add(txtasparag.Text + "|Asparagine");
            if (txtaspartic.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtaspartic.Text}"))
                text.Add(txtaspartic.Text + "|AsparticAcid");
            if (txtcys.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtcys.Text}"))
                text.Add(txtcys.Text + "|Cystine");
            if (txtglutamine.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtglutamine.Text}"))
                text.Add(txtglutamine.Text + "|Glutamine");
            if (txtgluacid.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtgluacid.Text}"))
                text.Add(txtgluacid.Text + "|GlutamicAcid");
            if (txtgly.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtgly.Text}"))
                text.Add(txtgly.Text + "|Glycine");
            if (txthis.TextLength > 0 && File.Exists($"{dbDir}{slash}{txthis.Text}"))
                text.Add(txthis.Text + "|Histidine");
            if (txtisoleu.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtisoleu.Text}"))
                text.Add(txtisoleu.Text + "|Isoleucine");
            if (txtleu.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtleu.Text}"))
                text.Add(txtleu.Text + "|Leucine");
            if (txtmeth.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtmeth.Text}"))
                text.Add(txtmeth.Text + "|Methionine");
            if (txtphen.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtphen.Text}")) 
                text.Add(txtphen.Text + "|Phenylalanine");
            if (txtproline.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtproline.Text}") )
                text.Add(txtproline.Text + "|Proline");
            if (txtserine.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtserine.Text}"))
                text.Add(txtserine.Text + "|Serine");
            if (txtthreo.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtthreo.Text}")) 
                text.Add(txtthreo.Text + "|Threonine");
            if (txttrypto.TextLength > 0 && File.Exists($"{dbDir}{slash}{txttrypto.Text}")              )                            
                text.Add(txttrypto.Text + "|Tryptophan");
            if (txttyro.TextLength > 0 && File.Exists($"{dbDir}{slash}{txttyro.Text}") )
                text.Add(txttyro.Text + "|Tyrosine");
            if (txtvaline.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtvaline.Text}"))
                text.Add(txtvaline.Text + "|Valine");

            //last to display
            if (txtServ1.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtServ1.Text}"))
                text.Add(txtServ1.Text + "|Serving");
            if (txtServ2.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtServ2.Text}"))    
                text.Add(txtServ2.Text + "|Serving2");
            if (txtWt1.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtWt1.Text}")  )    
                text.Add(txtWt1.Text + "|Weight");
            if (txtWt2.TextLength > 0 && File.Exists($"{dbDir}{slash}{txtWt2.Text}") )
                text.Add(txtWt2.Text + "|Weight2");
                                  #endregion
            File.WriteAllLines(nutkeyPath, text);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Contains("(share)") )
                nutkeyPath = $"{Application.StartupPath}{slash}usr{slash}share{slash}DBs{slash}{comboBox1.Text.Replace(" (share)", "")}{slash}_nutKeyPairs.TXT";
            else
                nutkeyPath = $"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}{slash}DBs{slash}{comboBox1.Text.Replace(" (user)", "")}{slash}_nutKeyPairs.TXT";            
            refresh();
        }
    }
}
