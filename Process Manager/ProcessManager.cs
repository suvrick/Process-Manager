using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Process_Manager
{
    public class ProcessManager
    {
        ILogger _logger;
        List<ProcessProperty> Processes { get; set; } = new List<ProcessProperty>();

        public ProcessManager(ILogger log = null)
        {
            _logger = log;
        }

        public void UpdateProcessesList()
        {
            Processes.Clear();

            using (var r = RunspaceFactory.CreateRunspace())
            {
                r.Open();

                using (var ps = PowerShell.Create())
                {
                    ps.Commands.AddCommand("Get-Process");
                    var results = ps.Invoke();

    

                    foreach (PSObject obj in results)
                    {
                        var prop = new ProcessProperty();
                        prop.Id = Convert.ToInt32(obj.Members["Id"].Value);
                        prop.ProcessName = obj.Members["Name"].Value.ToString();
                        prop.NPM = Convert.ToInt64(obj.Members["NPM"].Value.ToString()) / (1024 * 10);
                        prop.PM = Convert.ToInt64(obj.Members["PM"].Value.ToString()) / (1024 * 10);
                        prop.WS = Convert.ToInt64(obj.Members["WS"].Value.ToString()) / (1024 * 10);
                        prop.VM = Convert.ToInt64(obj.Members["VM"].Value.ToString()) / (1024 * 1024 * 1024);
                        Processes.Add(prop);
                    }

                    UpdateDataEvent?.Invoke(Processes);     
                }
            }
        }

        public void GetProcessById(int processID)
        {
            var prop = new ProcessProperty();

            using (var r = RunspaceFactory.CreateRunspace())
            {
                r.Open();

                using (var ps = PowerShell.Create())
                {
                    ps.Commands.AddCommand("Get-Process");
                    ps.Commands.AddParameter("Id", processID);

                    var results = ps.Invoke();


                    foreach (PSObject obj in results)
                    {
                        prop.Id = Convert.ToInt32(obj.Members["Id"].Value);
                        prop.ProcessName = obj.Members["Name"].Value.ToString();
                        prop.NPM = Convert.ToInt64(obj.Members["NPM"].Value.ToString()) / (1024 * 10);
                        prop.PM = Convert.ToInt64(obj.Members["PM"].Value.ToString()) / (1024 * 10);
                        prop.WS = Convert.ToInt64(obj.Members["WS"].Value.ToString()) / (1024 * 10);
                        prop.VM = Convert.ToInt64(obj.Members["VM"].Value.ToString()) / (1024 * 1024 * 1024);
                    }
                }
            }

            ChangeProcessInfoEvent?.Invoke(prop);
        }

        int getRAM(PSObject obj)
        {
            int ram = 0;
            try
            {
                ram = Convert.ToInt32(obj.Members["NPM"].Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ram;
        }

        int getCPU(PSObject obj)
        {
            int cpu = 0;
            try
            {
                //this.TotalProcessorTime.TotalSeconds
                //var o = obj.Members["CPU"];
                var m = obj.Properties["CPU"];
                
                cpu = Convert.ToInt32(m.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cpu;
        }

        public delegate void UpdateData(List<ProcessProperty> data);
        public delegate void ChangeProcessInfo(ProcessProperty processInfo);
        public event UpdateData UpdateDataEvent;
        public event ChangeProcessInfo ChangeProcessInfoEvent;
    }
}
