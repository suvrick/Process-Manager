using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        int delay = 1000;
        ProcessManager processManager;

        public Form1()
        {
            InitializeComponent();

            log = new MyLogger();
            log.FileName = ".log";
            log.Encoding = Encoding.UTF8;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Убиваем поток, при закрытие приложения 
            worker.Close();
            log.Info("Закрытие Form1");
        }

        private void processManager_UpdateDataEvent(List<ProcessProperty> data)
        {
            if (data.Count == 0)
                return;



            try
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    if (!dataGridView1.Visible)
                    {
                        dataGridView1.Visible = true;
                    }
                    UpdateRows(data);
                }));

                if(toolStripMenuItem5.Enabled)
                    toolStripStatusLabel2.Text = "Таблица обновлена";
            }
            catch (Exception ex)
            {
                log.Warnning($"Ошибка обновление datagridview.{ex.Message}");
            }
        }

        public void UpdateRows(List<ProcessProperty> processes)
        {


            if (dataGridView1 == null)
                return;


            for (var i = 0; i < processes.Count; i++)
            {
                var item = processes[i];
                
                bool exists = true;

                for (int x = 0; x < dataGridView1.Rows.Count; x++)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[x].Cells[1].Value);

                    if(id == item.Id)
                    {
                        exists = false;
                        dataGridView1.Rows[x].Cells[0].Value = item.ProcessName;
                        dataGridView1.Rows[x].Cells[2].Value = item.NPM;
                        dataGridView1.Rows[x].Cells[3].Value = item.PM;
                        dataGridView1.Rows[x].Cells[4].Value = item.WS;
                        dataGridView1.Rows[x].Cells[5].Value = item.VM;
                        break;
                    }
                }

                if (exists)
                {
                    var last_index = dataGridView1.Rows.Count;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[last_index].Cells[0].Value = item.ProcessName;
                    dataGridView1.Rows[last_index].Cells[1].Value = item.Id;
                    dataGridView1.Rows[last_index].Cells[2].Value = item.NPM;
                    dataGridView1.Rows[last_index].Cells[3].Value = item.PM;
                    dataGridView1.Rows[last_index].Cells[4].Value = item.WS;
                    dataGridView1.Rows[last_index].Cells[5].Value = item.VM;

                    log.Info($"Добавлен новый процесс ID:{item.Id}, Name: {item.ProcessName}");
                }

            }

            dataGridView1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {     
            Application.ApplicationExit += Application_ApplicationExit;

            processManager = new ProcessManager(log);
            processManager.UpdateDataEvent += processManager_UpdateDataEvent;

            worker = new Worker(processManager.UpdateProcessesList, log, delay);
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

        // Open form2
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;

            var value = dataGridView1?.Rows[index]?.Cells[1]?.Value;

            if (value == null)
                return;

            var processID = int.Parse(value.ToString());

            var f2 = new Form2(processID, log);
            
                f2.ShowDialog();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            var cell = dataGridView1[e.ColumnIndex, e.RowIndex];

            if (cell == null)
                return;

            if (cell.Tag == null)
                cell.Tag = cell.Value;


            if (cell.Tag.ToString() != cell.Value.ToString())
            {
                cell.Style.BackColor = Color.Aqua;
                Console.WriteLine($"Aque Tag: {cell.Tag} Value:{cell.Value}");
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //(sender as DataGridView).Rows[e.RowIndex]
        }
    }
}
