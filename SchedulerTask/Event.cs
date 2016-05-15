using System;

namespace SchedulerTask
{
    /// <summary>
    /// Событие
    /// </summary>
    class Event
    {
        private DateTime time;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">Время возникновения события</param>
        public Event(DateTime time)
        {
            this.time = time;
        }

        public DateTime Time
        {
            get { return time; }
            set { }
        }
    }
}
