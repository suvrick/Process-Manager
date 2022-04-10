using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Process_Manager
{
    /// <summary>
    /// Обвертка над системным классом Thread.Запускает переданный метод в отдельном потоке и вызывает его в цикле с заданой задержкой.
    /// Если задержка равна 0 - метод выполнится только единожды. 
    /// </summary>

    internal class Worker
    {
        ILogger _logger;
        Thread _thread;
        bool _pause;
        bool _exit;
        int _delay;
        Action _action;

        /// <summary>
        /// Обвертка над системным классом Thread.Запускает переданный метод в отдельном потоке и вызывает его в цикле с заданой задержкой.
        /// Если задержка равна 0 - метод выполнится только единожды. 
        /// </summary>
        /// <param name="action">Метод для вызова в отдельном потоке</param>
        /// <param name="logger">Объект реализующий интерфейс ILogger</param>
        /// <param name="delay">Интервал задержки вызова action метода</param>
        public Worker(Action action, ILogger logger = null, int delay = 0)
        {
            _action = action;
            _delay = delay;
            _logger = logger;
        }

        /// <summary>
        /// Запуск воркера.
        /// </summary>
        public void Run()
        {
            try
            {
                _thread = new Thread(Do);
                _logger?.Info($"Запуск воркера.ThreadId: {_thread.ManagedThreadId}");
                _thread.Start();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Не удалось запустить поток.{ex.Message}");
            }
        }

        /// <summary>
        /// Останавливает вызов переданного метода.Закрывает поток.
        /// </summary>
        public void Close()
        {
            _logger?.Info($"Закрытие воркера.ThreadId: {_thread.ManagedThreadId}");
            try
            {
                _exit = true;
                _thread.Abort();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Ошибка при закрытие потока.{ex.Message}");
            }
        }

        /// <summary>
        /// Возобновляет работу воркера.
        /// </summary>
        public void Resume()
        {
            _logger?.Info($"Возобновление работы воркера.ThreadId: {_thread.ManagedThreadId}");
            _pause = false;
        }

        /// <summary>
        /// Приостановливает работу воркера.
        /// </summary>
        public void Pause()
        {
            _logger?.Info($"Воркер приостановлен.ThreadId: {_thread.ManagedThreadId}");
            _pause = true;
        }

        void Do()
        {

            do
            {
                Thread.Sleep(_delay);

                if (_pause)
                    continue;

                _action();


            } while (_exit != true && _delay != 0);

            _logger?.Info($"Do successed! ThreadId: {_thread.ManagedThreadId}");
        }
    }
}
