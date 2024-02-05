using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CSharpProjekt
{
    public partial class Form1 : Form
    {
        private Timer refreshTimer;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("ProcessName", "Process Name");
            dataGridView1.Columns.Add("Id", "Process ID");
            dataGridView1.Columns.Add("Memory", "Memory (MB)");
            dataGridView1.Columns.Add("CpuUsage", "CPU Usage (%)");

            refreshTimer = new Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += RefreshTimer;
            refreshTimer.Start();
        }

        private void RefreshTimer(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                int memoryUsageMB = (int)(process.WorkingSet64 / (1024 * 1024));
                float cpuUsage = GetCpuUsage(process);
                dataGridView1.Rows.Add(process.ProcessName, process.Id, memoryUsageMB);
            }
        }
        private float GetCpuUsage(Process process)
        {
            try
            {
                TimeSpan totalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
                TimeSpan processCpuTime = process.TotalProcessorTime;
                float cpuUsage = (float)(processCpuTime.TotalMilliseconds / totalProcessorTime.TotalMilliseconds) * 100;
                return cpuUsage;
            }
            catch(System.ComponentModel.Win32Exception)
            {
                return 0.0f;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshTimer.Stop();
        }

    }
}
