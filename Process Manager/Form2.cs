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

        public Form2(int processID)
        {
            InitializeComponent();

            _logger = MyLogger.GetInstance();
            _processID = processID;

            FormClosing += Form2_FormClosing;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _worker.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            lblProcessName.Text = _processID.ToString();

            _processMng = new ProcessManager();
            _processMng.ChangeProcessInfoEvent += _processMng_ChangeProcessInfoEvent;

            _worker = new Worker(() => {
                _processMng.GetProcessById(_processID);
            }, _logger);

            _worker.Run();
        }

        private void _processMng_ChangeProcessInfoEvent(ProcessInfo processInfo)
        {
            
        }
    }
}
