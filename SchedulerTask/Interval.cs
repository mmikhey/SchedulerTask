using System.Collections.Generic;

namespace SchedulerTask
{

    using System;

    /// <summary>
    /// класс промежутков тактов (целые числа)
    /// </summary>
    /// 


    public class Interval:IComparable
    {
        DateTime starttime;
        DateTime endtime;
        Dictionary<int, bool> tacts; //словарь часовых тактов; int - значение часа; bool - значение занятости в течение часа 
        //(true - свободно, false - занято); по умолчанию весь интервал, состоящий из тактов времени считается свободным

        public Interval(DateTime starttime, DateTime endtime)
        {
            this.starttime = starttime;
            this.endtime = endtime;

            tacts = new Dictionary<int, bool>();
            for (int t = starttime.Hour; t < endtime.Hour; t++)
                tacts.Add(t, true);
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



        /// <summary>
        /// Занять интервал от времени t1 до t2.
        /// </summary>
        public void OccupyHours(DateTime t1, DateTime t2)
        {
            for (int t = t1.Hour; t < t2.Hour; t++)
            {
                tacts[t] = false;
            }

        }

        public int CompareTo(object obj)
        {
            Interval i2 = obj as Interval;
            if (obj == null) return 0;

            return DateTime.Compare(starttime, i2.starttime);
        }
    }
}