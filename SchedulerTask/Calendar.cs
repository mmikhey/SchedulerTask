using System;

/// <summary>
/// реализация календаря
/// </summary>
public class Calendar
{
    Interval calendar;
    
    public Calendar(Interval calendar)
        {
            this.calendar = calendar;
        }

        //узнать, свободно ли оборудование в промежуток времени
        //?? дополнительно на вход оборудование ??
        public bool EqIsFree(int time1, int time2)
        {
            if ((time1 >= calendar.starttime) & (calendar.endtime <= time2)&(calendar.occflag==true)) return true;
            else return false;
        }




        // "установить" календарь : время начала выполнения, операция
        public void SetCalendar(int time, Operation o)
        {
            int endtime = time + o.GetLasting();
            if (EqIsFree(time, endtime))
            {
                calendar.occflag = false;
            }
        }
}
