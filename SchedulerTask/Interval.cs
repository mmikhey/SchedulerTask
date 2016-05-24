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
        

        public Interval(DateTime starttime, DateTime endtime)
        {
            this.starttime = starttime;
            this.endtime = endtime;
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
       
        public int CompareTo(object obj)
        {
            Interval i2 = obj as Interval;
            if (obj == null) return 0;

            return DateTime.Compare(starttime, i2.starttime);
        }
    }
}