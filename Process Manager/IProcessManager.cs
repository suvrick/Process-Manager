using System.Collections.Generic;

namespace Process_Manager
{
    public interface IProcessManager
    {
        void UpdateProcessesList();
        void GetInfoByID(int id);
    }
}
