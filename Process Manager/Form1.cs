using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Process_Manager
{
    public partial class Form1 : Form
    {
       
        MyLogger log;
        Worker worker;
        public Form1()
        {
            InitializeComponent();
        }

        private void worker_ChangeStatusEvent(string status)
        {
            statusStrip1.Invoke(new Action(() => { 
                toolStripStatusLabel1.Text = status;
            }));
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            worker.Close();
        }

        private void worker_UpdateDataEvent(List<SimpleProcess> data)
        {
            if (data.Count == 0)
                return;

            dataGridView1.Invoke(new Action(() =>
            {
                UpdateRows(data);
            }));
        }

        public void UpdateRows(List<SimpleProcess> processes)
        {
            if (dataGridView1 == null)
                return;

            dataGridView1.Rows.Clear();
            for (var i = 0; i < processes.Count; i++)
            {
                var item = processes[i];

                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = item.Id;
                dataGridView1.Rows[i].Cells[1].Value = item.ProcessName;
                dataGridView1.Rows[i].Cells[2].Value = item.RAM;
                dataGridView1.Rows[i].Cells[3].Value = item.CRU;
                dataGridView1.Rows[i].Cells[4].Value = item.ProccesPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log = new MyLogger();

            worker = new Worker(log);
            worker.UpdateDataEvent += worker_UpdateDataEvent;
            worker.ChangeStatusEvent += worker_ChangeStatusEvent;
            worker.Run();

            Application.ApplicationExit += Application_ApplicationExit;

            toolStripMenuItem4.Enabled = false;

        }

        // Start
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripMenuItem4.Enabled = false;
            toolStripMenuItem5.Enabled = true;
            worker.Start();
        }

        // Stop
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripMenuItem5.Enabled = false;
            toolStripMenuItem4.Enabled = true;
            worker.Stop();
        }

        // Exit
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
