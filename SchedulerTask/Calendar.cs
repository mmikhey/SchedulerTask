
namespace SchedulerTask
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// реализация календаря
    /// </summary>
    public class Calendar
    {
        List<Interval> calendar;
        //Equipment eq;

        public Calendar(List<Interval> calendar)
        {
            this.calendar = calendar;

            for (int i = 0; i < calendar.Count; i++)
                if (calendar[i].GetEndTime() >= calendar[i + 1].GetStartTime())
                { calendar[i].SetEndTime(calendar[i + 1].GetEndTime()); calendar.RemoveAt(i + 1); }
        }

        /// <summary>
        /// возвращаем true, если попали хоть в один из интервалов календаря;
        /// false - иначе;
        /// дополнительно возвращаем индекс интервала (-1 означает, что ни в какой из интервалов не попали)
        /// </summary>
        public bool IsInterval(DateTime T, out int intervalindex)
        {
            for (int i = 0; i < calendar.Count; i++)
            {
                DateTime starttime = calendar[i].GetStartTime();
                DateTime endtime = calendar[i].GetEndTime();
                if ((T >= starttime) && (T <= endtime))
                { intervalindex = i; return true; }
            }

            intervalindex = -1;
            return false;
        }

        /// <summary>
        /// вернуть индекс интервала в календаре, в который попадает заданное время T
        /// если не попадает ни в один из интервалов - найти индекс ближайшего возможного
        /// </summary>       
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
                    if ((T > calendar[i].GetEndTime()) && (T < calendar[i + 1].GetStartTime())) { index = i + 1; break; }
            }

            return index;
        }

        /// <summary>
        /// вернуть время, в которое закончится выполнение операции;
        /// T - время начала операции o
        /// </summary>   
        public DateTime GetTimeofRelease(DateTime T, AOperation o)
        {
            DateTime operationtime;
            TimeSpan t = o.GetDuration();
            int index = FindInterval(T);
            DateTime endtime = calendar[index].GetEndTime();
            DateTime startime = calendar[index].GetStartTime();
            TimeSpan lasting = endtime.Subtract(startime);
            TimeSpan tmptime = t;

            while (tmptime.CompareTo(lasting) == 1)
            {
                tmptime = tmptime.Subtract(lasting);
                if (index == calendar.Count - 1) index = 0;
                else index += index;
                endtime = calendar[index].GetEndTime();
                startime = calendar[index].GetStartTime();
                lasting = endtime.Subtract(startime);
            }
            operationtime = startime + tmptime;

            return operationtime;
        }

        /// <summary>
        /// вернуть время ближайшего возможного времени начала выполнения операции
        /// T - время начала операции o
        /// </summary> 
        public DateTime GetNearestStart(DateTime T)
        {
            int index = FindInterval(T);
            DateTime operationtime = calendar[index].GetStartTime();
            return operationtime;
        }

        /// <summary>
        /// вернуть минимальное время ближайшего возможного времени начала выполнения операции
        /// !!!!использовать для группового оборудования!!!!!!
        /// </summary> 
        public DateTime GetMinNearestStart(DateTime T, List<Equipment> elist)
        {
            DateTime min = new DateTime();

            foreach (Equipment e in elist)
                if (e.GetCalendar().GetNearestStart(T) <= min) min = e.GetCalendar().GetNearestStart(T);

            return min;
        }


        public bool IsFree(DateTime T)
        {
            int index = FindInterval(T);
            if (calendar[index].IsFree(T, T.AddHours(1))) return true;
            return false;
        }

        /// <summary>
        /// Занять интервал от времени t1 до t2.
        /// </summary>
        public void OccupyHours(DateTime t1, DateTime t2)
        {
            int index = FindInterval(t1);
            calendar[index].OccupyHours(t1, t2);
        }

        public Interval GetInterval(int index)
        {
            return calendar[index];
        }
    }
}
