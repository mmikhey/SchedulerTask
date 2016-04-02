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
        public void SetWork(int time, Operation o)
        {
            int endtime = o.GetLasting() + time; //время окончания операции
            if (ca.EqIsFree(time, endtime))
            {
                StartTime = time;
                EndTime = endtime;
                ca.SetCalendar(time,o);
            }
           
        }
}
