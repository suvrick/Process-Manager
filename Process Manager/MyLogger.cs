using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process_Manager
{
    /// <summary>
    /// Простой логгер с возможностью задавать кодировку и пути до лог файла.Имеет три уровня логирования [INFO, WARRNING, ERROR].
    /// </summary>
    public class MyLogger : ILogger
    {
        string _dir;
        string _logName;
        string _path;

        Encoding _encoding;
        bool _append = true;
        string _dataFormat = "";

        object _locker = new object();

        /// <summary>
        /// Простой логгер.
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="dir">Каталог до логфайла</param>
        /// <param name="dataFormat">Формат даты</param>
        /// <param name="encode">Кодировка для записи файла</param>
        public MyLogger(string fileName = "log.txt", string dir = null, string dataFormat = null, Encoding encode = null)
        {
            FileName = fileName;
            Dir = dir;
            DataFormat = dataFormat;
            Encoding = encode;
        }

        /// <summary>
        /// Свойство для установки или получения имени лог файла
        /// </summary>
        public string FileName
        {
            get
            {
                return _logName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _logName = "log.txt";
                }
                else
                {
                    _logName = value;
                }

                // Сбрасываем внутреннюю переменную - признак что свойство _logName или _dir изменилось и требуется заново проинициализировать _path.
                _path = String.Empty;
            }
        }

        /// <summary>
        /// Свойство для установки или получения текущии директории для логфайла
        /// </summary>
        public string Dir
        {
            get
            {
                return _dir;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _dir = Directory.GetCurrentDirectory();
                }
                else
                {
                    _dir = Directory.Exists(value) ? value : _dir;
                }

                // Сбрасываем внутреннюю переменную - признак что свойство _logName или _dir изменилось и требуется заново проинициализировать _path.
                _path = String.Empty;
            }
        }

        /// <summary>
        /// Свойство для установки или получения формата даты
        /// </summary>
        public string DataFormat
        {
            get
            {
                return _dataFormat;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _dataFormat = "";
                }
                else
                {
                    _dataFormat = value;
                }
            }
        }

        /// <summary>
        /// Свойство для установки или получения текущии кодировку для записи в файле
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                if (value == null)
                {
                    _encoding = Encoding.UTF8;
                    return;
                }

                _encoding = value;
            }
        }


        /// <summary>
        /// Добавляем запись в лог с уровнем INFO.
        /// 
        /// example: "05.17.2022 12:33:10 [INFO] Create file."
        /// </summary>
        /// <param name="message">Сообщение для логирования</param>
        public void Info(string message)
        {
            write(LogType.INFO, message);
        }

        /// <summary>
        /// Добавляем запись в лог с уровнем WARRNING
        /// example: "05.17.2022 12:33:10 [WARRNING] File not found."
        /// </summary>
        /// <param name="message">Сообщение для логирования</param>
        public void Warnning(string message)
        {
            write(LogType.WARRNING, message);
        }

        /// <summary>
        /// Добавляем запись в лог с уровнем ERROR
        /// example: "05.17.2022 12:33:10 [ERROR] Do fail."
        /// </summary>
        /// <param name="message">Сообщение для логирования</param>
        public void Error(string message)
        {
            write(LogType.ERROR, message);
        }


        private void write(LogType level, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (string.IsNullOrEmpty(_path))
                сreateFileLog();

            try
            {
                var msg = $"{DateTime.Now.ToString(_dataFormat)} [{level}]: {message}";

                lock (_locker)
                {
                    using (var sw = new StreamWriter(_path, _append, _encoding))
                    {
                        sw.WriteLine(msg);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Склеиваем путь до файла лога.Если файл уже существует удаляем его.
        /// </summary>
        private void сreateFileLog()
        {
            try
            {
                _path = Path.Combine(_dir, _logName);
                if (File.Exists(_path))
                {
                    lock (_locker)
                    {
                        File.Delete(_path);
                    }
                }

                Info("Create file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private enum LogType
        {
            INFO,
            WARRNING,
            ERROR
        }
    }
}
