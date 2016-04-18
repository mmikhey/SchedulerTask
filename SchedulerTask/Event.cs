using System;

namespace SchedulerTask
{
    /// <summary>
    /// Событие
    /// </summary>
    class Event
    {
        private DateTime time;
        private Operation operation;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation">Операция, для которой создаётся событие</param>
        /// <param name="time">Время возникновения события</param>
        public Event(Operation operation, DateTime time)
        {
            this.operation = operation;
            this.time = time;
        }

        public Operation Operation
        {
            get { return operation; }
            set { }
        }

        public DateTime Time
        {
            get { return time; }
            set { }
        }
    }
}
