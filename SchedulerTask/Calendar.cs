
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
        Equipment eq;

        public Calendar(List<Interval> calendar, Equipment eq)
        {
            this.calendar = calendar;
            this.eq = eq;

            for (int i = 0; i < calendar.Count; i++)
                if (calendar[i].GetEndTime() >= calendar[i + 1].GetStartTime())
                { calendar[i].SetEndTime(calendar[i + 1].GetEndTime()); calendar.RemoveAt(i + 1); }
        }

        /// <summary>
        /// узнать, свободно ли оборудование в промежуток времени (от time1 до time2)
        /// </summary>
        public bool EqIsFree(DateTime time1, DateTime time2)
        {
            int index = FindInterval(time1);
            if ((time1 >= calendar[index].GetStartTime()) & (calendar[index].GetEndTime() <= time2) & (calendar[index].GetOccupiedFlag() == true)) return true;
            else return false;
        }

        /// <summary>
        /// проверить, свободно ли оборудование для указанных интервалов (по индексу интервала)
        /// возвращаем true, если оборудование свободно для всех интервалов
        /// </summary>
        public bool EqIsFree(List<int> indexlist)
        {
            int n = indexlist.Count;
            int count = 0;
            foreach (int i in indexlist)
            {
                DateTime starttime = calendar[i].GetStartTime();
                DateTime endtime = calendar[i].GetEndTime();
                if (EqIsFree(starttime, endtime)) count++;
            }

            if (count == n) return true;
            else return false;
        }


        /// <summary>
        /// "установить" календарь: время начала выполнения, операция
        /// возвращаем true, если установили операцию, false - иначе
        /// </summary>        
        public bool SetCalendar(DateTime starttime, AOperation o)
        {
            int index = FindInterval(starttime);
            DateTime endtime = starttime.Add(o.GetDuration());

            //можно ли уложить операцию без прерывания
            if (calendar[index].GetEndTime() - calendar[index].GetStartTime() >= o.GetDuration())
            {
                if (EqIsFree(starttime, endtime))
                {
                    calendar[index].SetFlag(false);
                    o.AddInterval(calendar[index]);
                    return true;
                }

                else return false;
            }

                //ищем интервалы
            else
            {
                if (o.IsInterrupted())
                {
                    int ind; // индекс последнего интервала, в который уложится операция
                    ind = index;
                    while (!((calendar[ind].GetStartTime() <= endtime) && (calendar[ind].GetEndTime() >= endtime))) ind++;


                    List<int> indexlist = new List<int>();
                    for (int i = index; i < ind; i++)
                        indexlist.Add(i);

                    if (EqIsFree(indexlist)) 
                    {
                        for (int i = index; i < ind; i++)
                        o.AddInterval(calendar[i]);
                        return true;
                    }
                    else return false;
                }

                else return false;
            }

        }

        /// <summary>
        /// узнать, свободно ли оборудование на данный момент:
        /// да - выдать момент окончания времени работы, иначе - выдать ближ.доступное время для занятия оборудования;
        /// выходные параметры:
        /// bool occflag - флаг занятости оборудования (false - свободно, true - занято)
        /// DateTime operationtime - ближайшее время начала операции (для первого случая) или время окончания операции (для второго случая)
        /// </summary>
        public void GetTimeofRelease(DateTime T, AOperation o, out bool occflag, out DateTime operationtime)
        {
            int index;      // индекс интервала, в который можно поставить операцию (либо ближайший интервал)
            TimeSpan lasting; //"длительность" интервала
            TimeSpan t = o.GetDuration();//длительность операции
            TimeSpan tmptime;   //остаток для другого интервала   
            DateTime endtime;
            DateTime startime;

            if (EqIsFree(T, T.Add(o.GetDuration())))
            {
                occflag = false;
                index = FindInterval(T);
                endtime = calendar[index].GetEndTime();
                startime = calendar[index].GetStartTime();
                lasting = endtime.Subtract(startime);

                if (t <= lasting) operationtime = startime + t;
                else
                {
                    tmptime = t;

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
                }
            }

            else
            {
                index = FindInterval(T);
                occflag = true;
                operationtime = calendar[index].GetStartTime();
            }

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
    }
}
