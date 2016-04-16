    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace SchedulerTask
{

/// <summary>
/// реализация календаря
/// </summary>
public class Calendar
{
    Interval calendar;
    
    public Calendar(List<Interval> calendar, Equipment eq)
        {
            
            this.calendar = calendar;
            this.eq = eq;

            for (int i = 0; i < calendar.Count; i++)
                if (calendar[i].GetEndTime() >= calendar[i + 1].GetStartTime())
                { calendar[i].SetEndTime(calendar[i + 1].GetEndTime()); calendar.RemoveAt(i + 1); }
        }

        //узнать, свободно ли оборудование в промежуток времени
        //?? дополнительно на вход оборудование ??

        public bool EqIsFree(DateTime time1, DateTime time2)
        {
            int index = FindInterval(time1);
            if ((time1 >= calendar[index].GetStartTime()) & (calendar[index].GetEndTime() <= time2) & (calendar[index].GetOccupiedFlag() == true)) return true;
            else return false;
        }

        // "установить" календарь : время начала выполнения, операция
        public void SetCalendar(DateTime time, AOperation o)
        {
            int index = FindInterval(time);
            DateTime endtime = time.Add(o.GetDuration());
            if (EqIsFree(time, endtime))
            {
                calendar[index].SetFlag(false);
            }
        }

        //узнать, свободно ли оборудование на данный момент. да - выдать момен окончания времени, нет - выдать ближ.доступное время для занятия
        //bool occflag; флаг занятости оборудования: false - свободно, true - занято
        //DateTime operationtime;ближайшее время начала операции (для первого случая)
        // или время окончания операции (для второго случая)
        public void GetTimeofRelease(DateTime T, AOperation o, out bool occflag, out DateTime operationtime)
        {
            int index;      // индекс интервала, в который можно поставить операцию (либо ближайший интервал)
            TimeSpan lasting; //"длительность" интервала
            TimeSpan t = o.GetDuration();//длительность операции
            TimeSpan hours = o.GetDuration();
            if (EqIsFree(T, T.Add(hours)))
            {
                occflag = false;
                index = FindInterval(T);
                DateTime endtime = calendar[index].GetEndTime();
                DateTime startime = calendar[index].GetStartTime();
                lasting = endtime.Subtract(startime);

                if (t <= lasting) operationtime = startime + t;
                else
                {
                    TimeSpan tmptime = t.Subtract(lasting);
                    //остаток для другого интервала                   
                    if (index == calendar.Count - 1) index = 0;
                    else index += index;

                    /* t = calendar[index].GetStartTime() + tmptime;
                     operationtime = t;*/
                    operationtime = calendar[index].GetStartTime() + tmptime;
                }
            }

            else
            {
                index = FindInterval(T);
                occflag = true;
                operationtime = calendar[index].GetStartTime();
            }

        }

        //вернуть индекс интервала в календаре, в который попадает заданный такт времени T
        //если не попадает ни в один из интервалов - найти индекс ближайшего возможного
        public int FindInterval(DateTime T)
        {
            int index = -1;
            for (int j = 0; j < calendar.Count; j++)
            {
                DateTime starttime = calendar[j].GetStartTime();
                DateTime endtime = calendar[j].GetEndTime();
                if ((DateTime.Compare(T, starttime) >= 0) && (DateTime.Compare(T, endtime) <= 0)) { index = j; break; }
            }

            if (index == -1)
            {
                for (int i = 0; i < calendar.Count; i++)
                    if ((T > calendar[i].GetEndTime()) && (T < calendar[i + 1].GetStartTime())) { index = i; break; }
            }

            return index;
        }
    }

        public bool EqIsFree(int time1, int time2)
        {
            if ((time1 >= calendar.starttime) & (calendar.endtime <= time2)&(calendar.occflag==true)) return true;
            else return false;
        }
    
    // "установить" календарь : время начала выполнения, операция
        public void SetCalendar(int time, AOperation o)
        {
            int endtime = time + o.GetDuration();
            if (EqIsFree(time, endtime))
            {
                calendar.occflag = false;
            }
        }
}
}
