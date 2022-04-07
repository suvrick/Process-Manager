using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process_Manager
{
    public class ProcessManager
    {
        ILogger _logger;
        public ProcessManager(ILogger log = null)
        {
            _logger = log;
        }

        List<SimpleProcess> Processes { get; set; } = new List<SimpleProcess>();
        
        public void UpdateProcessesList()
        {
            var processes = Process.GetProcesses();

            Processes.Clear();

            for (int i = 0; i < processes.Length; i++)
            {
                var process = processes[i];
               
                int id = process.Id;
                string name = process.ProcessName;

                int ram = 0, cru = 0;

                try
                {
                    ram = getRAM(name);
                }
                catch (Exception ex)
                {
                    _logger.Write($"Исключение при получении память процесса {name}.{ex.Message}");
                }

                try
                {
                    ram = getCRU(name);
                }
                catch (Exception ex)
                {
                    _logger.Write($"Исключение при получении времени процесса {name}.{ex.Message}");
                }

                Processes.Add(new SimpleProcess() {
                    Id = id,
                    CRU = cru,
                    ProccesPath = "",
                    ProcessName = name,
                    RAM = ram
                });


            }

            UpdateDataEvent?.Invoke(Processes);
        }

        public void GetProcessById(int processID) {
            ChangeProcessInfoEvent?.Invoke(new ProcessInfo() { 
                Id=processID,
            });
        }

        int getRAM(string name)
        {
            int ram = 0;
            using (var PC = new PerformanceCounter())
            {
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = name;
                ram = Convert.ToInt32(PC.NextValue()) / (int)(1024);
            }
            return ram;
        }

        int getCRU(string name)
        {
            int cru = 0;
            using (var PC = new PerformanceCounter("Process", "% Processor Time", name, true))
            {
                for(int i = 0; i < 3; i++)
                {
                    cru = (int)PC.NextValue();
                }
            }
            return cru;
        }


        public delegate void UpdateData(List<SimpleProcess> data);
        public delegate void ChangeProcessInfo(ProcessInfo processInfo);
        public event UpdateData UpdateDataEvent;
        public event ChangeProcessInfo ChangeProcessInfoEvent;
    }
}
