using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process_Manager
{
    public interface ILogger
    {
        void Info(string message);
        void Warnning(string message);
        void Error(string message);
    }
}
