using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Nutritracker
{
    public partial class frmDetailReport : Form
    {
        public frmDetailReport()
        {
            InitializeComponent();
        }

		static string slash = Path.DirectorySeparatorChar.ToString();
        string userRoot;
		string file = "";
        string profileRoot = Application.StartupPath + $"{slash}usr{slash}profile{frmMain.currentUser.index}";
        List<string> activeFields;                
        //frmMain.logItem litm;
        //List<frmMain.logItem> bLog;
        //List<frmMain.logItem> lLog;
        //List<frmMain.logItem> dLog;

        private void frmDetailReport_Load(object sender, EventArgs e)
        {
            string dte = DateTime.Now.ToString().Split(' ')[0].Replace("/", "-");
            userRoot = $"{Application.StartupPath}{slash}usr{slash}profile{frmMain.currentUser.index}";
            string[] logFiles = Directory.GetFiles($"{userRoot}{slash}foodlog");

            //string[] todaysLog = File.ReadAllText($"{userRoot}{slash}foodlog{slash}{dt}.TXT").Replace("\r", "").Split(new string[] { "===========\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> dates = new List<string>();
			foreach (string s in logFiles)			
                dates.Add(s.Split(Path.DirectorySeparatorChar)[s.Split(Path.DirectorySeparatorChar).Length - 1].Replace(".TXT", ""));
            foreach (string s in dates)
                chkLstBoxDays.Items.Add(s);
            //bLog = new List<frmMain.logItem>();
            //lLog = new List<frmMain.logItem>();
            //dLog = new List<frmMain.logItem>();
            //foreach (string s in daysLog)
            //{
            //    string[] lines = s.Split(new string[] { "--Breakfast--" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "--Lunch--" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('\n');
            //    for (int i = 0; i < lines.Length; i++)
            //    {
            //        string st = lines[i];
            //        if (st == "")
            //            continue;
            //        litm = new frmMain.logItem();
            //        litm._db = st.Split('|')[0];
            //        litm.primKeyNo = st.Split('|')[1];
            //        litm.grams = Convert.ToDouble(st.Split('|')[2]);
            //        bLog.Add(litm);
            //    }
            //    lines = s.Split(new string[] { "--Lunch--" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "--Dinner--" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('\n');
            //    for (int i = 0; i < lines.Length; i++)
            //    {
            //        string st = lines[i];
            //        if (st == "")
            //            continue;
            //        litm = new frmMain.logItem();
            //        litm._db = st.Split('|')[0];
            //        litm.primKeyNo = st.Split('|')[1];
            //        litm.grams = Convert.ToDouble(st.Split('|')[2]);
            //        lLog.Add(litm);
            //    }
            //    lines = s.Split(new string[] { "--Dinner--" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('\n');
            //    for (int i = 0; i < lines.Length; i++)
            //    {
            //        string st = lines[i];
            //        if (st == "")
            //            continue;
            //        litm = new frmMain.logItem();
            //        litm._db = st.Split('|')[0];
            //        litm.primKeyNo = st.Split('|')[1];
            //        litm.grams = Convert.ToDouble(st.Split('|')[2]);
            //        dLog.Add(litm);
            //    }
            //}

            chkLstBoxDays.Items.Add("All");
            chkLstBoxDays.SetItemChecked(chkLstBoxDays.Items.Count -1, true);
            int n = 0;
            while (File.Exists(file = $"{profileRoot}{slash}dtlreports{slash}{dte}_{n}.TXT"))
                n++;
            txtOutput.Text = file.Replace(Application.StartupPath, "");
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            if (chkLstBoxDays.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select something!!");
                return;
            }

			activeFields = new List<string>();
            List<string> days = new List<string>();
            for (int i = 0; i < chkLstBoxDays.Items.Count; i++)
                if (chkLstBoxDays.GetItemChecked(i) && chkLstBoxDays.Items[i].ToString() != "All")
                    days.Add(chkLstBoxDays.Items[i].ToString());

            //foreach (frmMain.colObj c in frmMain.activeFieldsWithUnits)
                //activeFields.Add(c.header);
            
            string[] activeNutsLines = File.ReadAllLines($"{profileRoot}{slash}activeFields.TXT");
            foreach (string s in activeNutsLines)
                if (s.Split('#')[0] != "") // && frmMain.activeFields.Contains(leading))
                    activeFields.Add(s.Split('#')[0]);
            


            ProcessStartInfo ps = new ProcessStartInfo($"{Application.StartupPath}{slash}logRunner.exe");

            // arg1   = profile #
            // arg2   = unique log output *.TXT, full file name
            // arg4[] = dates
            ps.Arguments = $"{frmMain.currentUser.index} {file} {string.Join(" ", days)}";
            Process.Start(ps).Close();
            
            //MessageBox.Show($"Log performed over {days.Count} days with {activeFields.Count} active fields\n\nWait for console to finish, it will save a log to\n{file}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void chkLstBoxDays_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (chkLstBoxDays.Items[e.Index].ToString() == "All" && e.NewValue == System.Windows.Forms.CheckState.Checked)
                for (int i = 0; i < chkLstBoxDays.Items.Count - 1; i++)
                    chkLstBoxDays.SetItemChecked(i, true);
             else if (chkLstBoxDays.Items[e.Index].ToString() == "All" && e.NewValue == System.Windows.Forms.CheckState.Unchecked)
                for (int i = 0; i < chkLstBoxDays.Items.Count - 1; i++)
                    chkLstBoxDays.SetItemChecked(i, false);
        }
    }
}