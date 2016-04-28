using System.Collections.Generic;

namespace SchedulerTask
{

    using System;

    /// <summary>
    /// класс промежутков тактов (целые числа)
    /// </summary>
    /// 


    public class Interval
    {
        DateTime starttime;
        DateTime endtime;
        Dictionary<int, bool> tacts; //словарь часовых тактов; int - значение часа; bool - значение занятости в течение часа (true - свободно, false - занято)

        public Interval(DateTime starttime, DateTime endtime)
        {
            this.starttime = starttime;
            this.endtime = endtime;
            tacts = new Dictionary<int, bool>();
        }

        public DateTime GetStartTime()
        { return starttime; }

        public DateTime GetEndTime()
        { return endtime; }

        public void SetStartTime(DateTime val)
        {
            starttime = val;
        }

        public void SetEndTime(DateTime val)
        {
            endtime = val;
        }

        /// <summary>
        /// Узнать, свободен ли интервал от времени t1 до t2.
        /// Если хоть 1 такт занят - вернем false. Весь промежуток свободен - вернем true.
        /// </summary>
        public bool IsFree(DateTime t1, DateTime t2)
        {
            int T1 = t1.Hour;
            int T2 = t2.Hour;
            bool occflag;

            for (int i = T1; i < T2; i++)
            {
                tacts.TryGetValue(i, out occflag);
                if (!occflag) return false;
            }

            return true;
        }
    }
}