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
        public List<SimpleProcess> Processes { get; set; } = new List<SimpleProcess>();
        
        public void GetProcesses()
        {
            var processes = Process.GetProcesses();

            Processes.Clear();

            for (int i = 0; i < processes.Length; i++)
            {
                var process = processes[i];

                

                int id = process.Id;
                string name = process.ProcessName;
                int ram = getRAM(name);
                int cru = getCRU(name);

                Processes.Add(new SimpleProcess() {
                    Id = id,
                    CRU = cru,
                    ProccesPath = "",
                    ProcessName = name,
                    RAM = ram
                });
            }
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
    }
}
