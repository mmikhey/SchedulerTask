

namespace SchedulerTask
{
    using System;

    /// <summary>
    /// оборудование
    /// </summary>
    public class Equipment
    {
        Calendar ca;

        public Equipment(Calendar ca)
        {
            this.ca = ca;
        }

        public bool freeflag = true;
        int starttime, endtime; //начало и конец выполнения 

        public int StartTime
        {
            // get { return starttime; }
            set { starttime = value; }
        }

        public int EndTime
        {
            // get { return starttime; }
            set { endtime = value; }
        }

        //назначить работу o с началом работы time 
        public void SetWork(int time, AOperation o)
        {
            int endtime = o.GetDuration() + time; //время окончания операции
            if (ca.EqIsFree(time, endtime))
            {
                StartTime = time;
                EndTime = endtime;
                ca.SetCalendar(time, o);
            }

        }

        //проверить доступность оборудования в указанный промежуток времени; true - оборудование доступно; false - занято
        public bool IsFree(int time, int endtime)
        {
            if (ca.EqIsFree(time, endtime)) return true;
            else return false;
        }
    }
}
