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
        bool _exit;
        int _delay = 100;
        Action _action;

        public Worker(Action action, ILogger logger = null)
        {
            _action = action;
            _logger = logger;
        }

        public void Run()
        {
            try
            {
                _logger?.Write("Запуск воркера");
                _thread = new Thread(Do);
                _thread.Start();
            }
            catch (Exception ex)
            {
                _logger?.Write($"Не удалось запустить поток.{ex.Message}");
            }
        }

        public void Close()
        {
            _logger?.Write("Закрытие потока");
            try
            {
                _exit = true;
                _thread.Abort();
            }
            catch (Exception ex)
            {
                _logger?.Write($"Ошибка при закрытие потока.{ex.Message}");
            }
        }

        public void Resume()
        {
            _logger?.Write("Запуск потока");
            _pause = false;
        }

        public void Pause()
        {
            _logger?.Write("Пауза");
            _pause = true;
        }

        void Do()
        {
            while (!_exit)
            {
                Thread.Sleep(_delay);

                if (_pause)
                    continue;

                _action();
            }
        }
    }
}
