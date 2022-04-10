using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Process_Manager
{
    public partial class Form2 : Form
    {
        int _processID;
        ILogger _logger;
        ProcessManager _processMng;
        private Worker _worker;

        public Form2(int processID, ILogger log)
        {
            InitializeComponent();

            _logger = log;
            _processID = processID;

            FormClosing += Form2_FormClosing;
            _logger.Info("Открытие Form2");
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Info("Закрытие Form2");
            _worker.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            _processMng = new ProcessManager();
            _processMng.ChangeProcessInfoEvent += _processMng_ChangeProcessInfoEvent;

            _worker = new Worker(() =>
            {
                _processMng.GetProcessById(_processID);
            }, _logger);

            _worker.Run();
        }

        private void _processMng_ChangeProcessInfoEvent(ProcessProperty prop)
        {
            lblProcessName.Invoke(new Action(() =>
            {
                lblProcessName.Text = prop.ProcessName;
            }));

            dataGridView1.Invoke(new Action(() =>
            {
                var height = 30;
                var fontSize = 9;
                var font = FontFamily.GenericSansSerif;
                
                dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
               // dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                
                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[0].Cells[0].Value = "Process name";
                dataGridView1.Rows[1].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[1].Height = height;
                dataGridView1.Rows[1].Cells[0].Value = "Имя процесса";
                dataGridView1.Rows[0].Cells[1].Value = prop.ProcessName;
                dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[0].Cells[1].Style.BackColor = Color.Aqua;

                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[2].Cells[0].Value = "Id";
                dataGridView1.Rows[3].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[3].Height = height;
                dataGridView1.Rows[3].Cells[0].Value = "Идентификатор процесса (PID) процесса";
                dataGridView1.Rows[2].Cells[1].Value = prop.Id.ToString();
                dataGridView1.Rows[2].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[2].Cells[1].Style.BackColor = Color.Aqua;

                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[4].Cells[0].Value = "NPM";
                dataGridView1.Rows[5].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[5].Height = height;
                dataGridView1.Rows[5].Cells[0].Value = "Объем невыгружаемой памяти, используемой процессом, в килобайтах";
                dataGridView1.Rows[4].Cells[1].Value = prop.NPM.ToString();
                dataGridView1.Rows[4].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[4].Cells[1].Style.BackColor = Color.Aqua;

                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[6].Cells[0].Value = "PM";
                dataGridView1.Rows[7].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[7].Height = height;
                dataGridView1.Rows[7].Cells[0].Value = "Объем выгружаемой памяти, используемой процессом, в килобайтах.";
                dataGridView1.Rows[6].Cells[1].Value = prop.PM.ToString();
                dataGridView1.Rows[6].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[6].Cells[1].Style.BackColor = Color.Aqua;

                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[8].Cells[0].Value = "WS";
                dataGridView1.Rows[9].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[9].Height = height;
                dataGridView1.Rows[9].Cells[0].Value = "Размер рабочего набора процесса в килобайтах. Рабочий набор состоит из страниц памяти, на которые недавно ссылался процесс.";
                dataGridView1.Rows[8].Cells[1].Value = prop.WS.ToString();
                dataGridView1.Rows[8].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[8].Cells[1].Style.BackColor = Color.Aqua;

                dataGridView1.Rows.Add(2);
                dataGridView1.Rows[10].Cells[0].Value = "VM";
                dataGridView1.Rows[11].Cells[0].Style.Font = new Font(font, fontSize);
                dataGridView1.Rows[11].Height = height;
                dataGridView1.Rows[11].Cells[0].Value = "Объем виртуальной памяти, используемой процессом, в мегабайтах. Виртуальная память включает в себя хранение файлов подкачки на диске.";
                dataGridView1.Rows[10].Cells[1].Value = prop.VM.ToString();
                dataGridView1.Rows[10].Cells[0].Style.BackColor = Color.Aqua;
                dataGridView1.Rows[10].Cells[1].Style.BackColor = Color.Aqua;
            }));


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
    }
}
