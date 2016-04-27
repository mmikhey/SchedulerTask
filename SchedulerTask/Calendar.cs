
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
        /// узнать, есть ли свободные интервалы времени;
        /// выходные параметры:
        /// bool occflag - флаг занятости (true - свободно, false - занято);
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая), является списком, для единичного оборудования брать 1ый элемент листа;
        /// List<int> equiplist - список ID оборудования, календари которых имеют интервалы, в которые попадает T, если таких нет, возвращаем список, содержащий -1
        /// </summary>
        public void IsFree(DateTime T, AOperation o, EquipmentManager EM, int id, out bool occflag, out List<DateTime> operationtime, out List<int> equipIDlist)
        {
            int index;      // индекс интервала, в который можно поставить операцию (либо ближайший интервал)
            TimeSpan lasting; //"длительность" интервала
            TimeSpan t = o.GetDuration();//длительность операции
            DateTime endtime;
            DateTime startime;
            operationtime = new List<DateTime>();
            equipIDlist = new List<int>();

            //если оборудование атамарно
            if (!(EM.IsGroup(id)))
            {
                if (IsInterval(T, out index))
                {
                    occflag = true;
                    equipIDlist.Add(id);
                    endtime = calendar[index].GetEndTime();
                    startime = calendar[index].GetStartTime();
                    lasting = endtime.Subtract(startime);

                    if (t <= lasting) operationtime.Add(startime + t);
                    else operationtime.Add(GetTimeofRelease(T, o));
                }

                else
                {
                    occflag = false;
                    equipIDlist.Add(-1);
                    operationtime.Add(GetNearestStart(T));
                }
            }

                //если оборудование групповое
            else
            {
                List<Equipment> elist = new List<Equipment>(EM.GetEquipments());
                bool eoccflag = false;

                foreach (Equipment e in elist)
                {
                    if (e.GetCalendar().IsInterval(T, out index)) eoccflag = true;
                }

                if (eoccflag == false)
                {
                    equipIDlist.Add(-1);
                    occflag = false;
                    operationtime.Add(GetMinNearestStart(T, elist));
                }

                else
                {
                    occflag = true;
                    foreach (Equipment e in elist)
                        if (e.GetCalendar().IsInterval(T, out index))
                        {
                            operationtime.Add(e.GetCalendar().GetTimeofRelease(T, o));
                            equipIDlist.Add(e.GetID());
                        }
                }
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

    }
}
