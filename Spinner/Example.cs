using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace MBSpinner
{
    public partial class Example : Form
    {
        public Example()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            spinner.Start();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            spinner.Stop();
        }

        private void LoadData_Click(object sender, EventArgs e)
        {
            spinner.Start(LoadData, ProgressInfo, ConsumeData);
        }

        private void LoadData(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(5000);
            spinner.ReportProgress(50);
            Thread.Sleep(5000);
            e.Result = "Loaded data!";
        }

        private void ConsumeData(object sender, RunWorkerCompletedEventArgs e)
        {
            string data = (string)e.Result;
            spinner.Stop();
            MessageBox.Show(data);
        }

        private void ProgressInfo(object sender, ProgressChangedEventArgs e)
        {
            MessageBox.Show(e.ProgressPercentage.ToString() + "% complete");
        }
    }
}