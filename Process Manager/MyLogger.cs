using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process_Manager
{
    public class MyLogger : ILogger
    {
        string _path = "./log.txt";
        StreamWriter writer;

        private MyLogger()
        {
           
        }

        static MyLogger _instance;

        public static MyLogger GetInstance()
        {

            if (_instance == null)
            {
                try
                {
                    _instance = new MyLogger();
                    _instance.writer = new StreamWriter(_instance._path, true);
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return _instance;
        }

        public void SetPath(string path)
        {
            _path = path;
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
            //writer.WriteLine($"[{DateTime.Now.ToLocalTime()}]: {message}");  
        }
    }
}
