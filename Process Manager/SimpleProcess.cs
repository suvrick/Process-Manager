namespace Process_Manager
{
    public class SimpleProcess
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public int RAM { get; set; }
        public int CPU { get; set; }
        public string ProccesPath { get; set; }
    }

    public class ProcessInfo : SimpleProcess
    {
    }

}
