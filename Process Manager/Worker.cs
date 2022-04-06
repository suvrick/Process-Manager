using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Process_Manager
{
    internal class Worker
    {
        ILogger _logger;
        Thread _thread;
        bool _pause;
        int _delay = 100;

        public delegate void UpdateData(List<SimpleProcess> data);
        public delegate void ChangeStatus(string status);
        public event UpdateData UpdateDataEvent;
        public event ChangeStatus ChangeStatusEvent;


        public Worker(ILogger logger)
        {
            _logger = logger;
            _logger.Write("Запуск менеджера процессов");
        }

        public void Run()
        {
            _thread = new Thread(Do);
            _thread.Start();            
            ChangeStatusEvent?.Invoke("Иницилизация...");
        }

        public void Close()
        {
            _logger.Write("Закрытие потока");
            _thread.Abort();
        }

        public void Start()
        {
            _logger.Write("Запуск потока");
            _pause = false;
        }

        public void Stop()
        {        
            _logger.Write("Пауза");
            _pause = true;
        }

        void Do()
        {
            var processManager = new ProcessManager();

            while (_thread != null)
            {

                Thread.Sleep(_delay);

                if (_pause)
                {
                    ChangeStatusEvent?.Invoke("Пауза");
                    continue;
                }

                ChangeStatusEvent?.Invoke("Получение списка процессов");

                processManager.GetProcesses();

                UpdateDataEvent?.Invoke(processManager.Processes);

                ChangeStatusEvent?.Invoke("Обновление данных");
            }
        }


    }
}
