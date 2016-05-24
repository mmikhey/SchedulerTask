
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

        public Calendar(List<Interval> intervallist)
        {
            calendar = new List<Interval>(intervallist);
            calendar.Sort();
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
        /// flag  = true, если попали в интервал false - иначе
        /// </summary>       
        public int FindInterval(DateTime T, out bool flag)
        {
            for (int j = 0; j < calendar.Count; j++)
            {
                DateTime starttime = calendar[j].GetStartTime();
                DateTime endtime = calendar[j].GetEndTime();
                if ((T >= starttime) && (T <= endtime)) { flag = true; return j; }
            }

            for (int i = 0; i < calendar.Count; i++)
                if ((T > calendar[i].GetEndTime()) && (T < calendar[i + 1].GetStartTime())) { flag = false; return i + 1; }

            flag = false;
            return -1;
        }

        /// <summary>
        /// вернуть время, в которое закончится выполнение операции;
        /// T - время начала операции o;
        /// t - длительность операции;
        /// intervalindex - индекс интервала в календаре
        /// </summary>   
        public DateTime GetTimeofRelease(DateTime T, TimeSpan t, int intervalindex)
        {
            TimeSpan intervallasting = calendar[intervalindex].GetEndTime() - T;
            TimeSpan tmptime = t;

            if (t <= intervallasting) return T + t;

            while (tmptime > intervallasting)
            {
                tmptime = tmptime - intervallasting;
                //if (intervalindex == calendar.Count - 1) intervalindex = 0;
                intervalindex += 1;
                intervallasting = calendar[intervalindex].GetEndTime() - calendar[intervalindex].GetStartTime();
            }

            return calendar[intervalindex].GetStartTime() + tmptime;
        }

        /// <summary>
        /// вернуть время ближайшего возможного времени начала выполнения операции
        /// T - время начала операции o
        /// </summary> 
        public DateTime GetNearestStart(DateTime T)
        {
            bool flag;
            int index = FindInterval(T, out flag);
            if (flag == false)
                return calendar[index].GetStartTime();
            else return T;
        }
        
        public Interval GetInterval(int index)
        {
            return calendar[index];
        }
    }
}
