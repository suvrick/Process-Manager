namespace Process_Manager
{
    public class ProcessProperty
    {
        /// <summary>
        /// Идентификатор процесса (PID) процесса.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя процесса.
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// Объем невыгружаемой памяти, используемой процессом, в килобайтах.
        /// </summary>
        public float NPM { get; set; }
        /// <summary>
        /// Объем выгружаемой памяти, используемой процессом, в килобайтах.
        /// </summary>
        public float PM { get; set; }
        /// <summary>
        /// Размер рабочего набора процесса в килобайтах. Рабочий набор состоит из страниц памяти, на которые недавно ссылался процесс.
        /// </summary>
        public float WS { get; set; }

        /// <summary>
        /// Объем виртуальной памяти, используемой процессом, в мегабайтах. Виртуальная память включает в себя хранение файлов подкачки на диске.
        /// </summary>
        public float VM { get; set; }
    }
}
