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
        ProcessManager processManager;


        public Form1()
        {
            InitializeComponent();

            log = MyLogger.GetInstance();
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Убиваем поток, при закрытие приложения 
            worker.Close();
        }

        private void processManager_UpdateDataEvent(List<SimpleProcess> data)
        {
            if (data.Count == 0)
                return;

            try
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    UpdateRows(data);
                }));

                if(toolStripMenuItem5.Enabled)
                    toolStripStatusLabel2.Text = "Таблица обновлена";
            }
            catch (Exception ex)
            {
                log.Write($"Ошибка обновление datagridview.{ex.Message}");
            }
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
            Application.ApplicationExit += Application_ApplicationExit;

            processManager = new ProcessManager(log);
            processManager.UpdateDataEvent += processManager_UpdateDataEvent;

            worker = new Worker(processManager.UpdateProcessesList, log);
            worker.Run();


            toolStripMenuItem4.Enabled = false;
            toolStripStatusLabel2.Text = "Инициализация данных";
        }

        // Start
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripMenuItem4.Enabled = false;
            toolStripMenuItem5.Enabled = true;
            toolStripStatusLabel2.Text = "Получение данных";
            worker.Resume();
        }

        // Stop
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripMenuItem5.Enabled = false;
            toolStripMenuItem4.Enabled = true;
            toolStripStatusLabel2.Text = "Обновление данных приостановлено";
            worker.Pause();
        }

        // Exit
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;

            var value = dataGridView1.Rows[index].Cells[0].Value;

            if (value == null)
                return;

            var processID = int.Parse(value.ToString());

            new Form2(processID).ShowDialog();
        }
    }
}
