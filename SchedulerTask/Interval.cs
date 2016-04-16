
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
        bool occflag;//флаг занятости; true - оборудование свободно, false - занято
        DateTime endtime;

        public DateTime GetStartTime()
        { return starttime; }

        public DateTime GetEndTime()
        { return endtime; }

        public bool GetOccupiedFlag()
        { return occflag; }

        public void SetStartTime(DateTime val)
        {
            starttime = val;
        }

        public void SetEndTime(DateTime val)
        {
            endtime = val;
        }

        public void SetFlag(bool val)
        {
            occflag = val;
        }
    }
}