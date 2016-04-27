using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{

    public class EquipmentManager : ICalendar
    {
        List<Interval> calendarintervals;
        List<Equipment> eq;

        public EquipmentManager(List<Interval> calendarintervals, List<Equipment> eq)
        {
            this.calendarintervals = calendarintervals;
            this.eq = eq;

            for (int i = 0; i < calendarintervals.Count; i++)
                if (calendarintervals[i].GetEndTime() >= calendarintervals[i + 1].GetStartTime())
                { calendarintervals[i].SetEndTime(calendarintervals[i + 1].GetEndTime()); calendarintervals.RemoveAt(i + 1); }

            for (int i = 0; i < eq.Count; i++)
                for (int j = 0; j < eq.Count; j++)
                {
                    if (eq[j].GetID() > eq[j + 1].GetID())
                    {
                        Equipment etpm = eq[j];
                        eq[j] = eq[j + 1];
                        eq[j + 1] = etpm;
                    }
                }
        }


        /// <summary>
        /// вернуть индекс инервала в календаре, в который попадает заданный такт времени T;
        /// если не попадает ни в один из интервалов - найти индекс ближайшего возможного
        /// </summary>
        public int FindInterval(DateTime T)
        {
            int index = -1;
            for (int j = 0; j < calendarintervals.Count; j++)
            {
                if ((T >= calendarintervals[j].GetStartTime()) && (T <= calendarintervals[j].GetEndTime())) { index = j; break; }
            }

            if (index == -1)
            {
                for (int i = 0; i < calendarintervals.Count; i++)
                    if ((T > calendarintervals[i].GetEndTime()) && (T < calendarintervals[i + 1].GetStartTime())) { index = i + 1; break; }
            }

            return index;
        }


        public List<Equipment> GetEquipments()
        {
            return eq;
        }

        /// <summary>
        /// вернем true, если оборудование групповое
        /// false - атамарно
        /// </summary>
        public bool IsGroup(int id)
        {
            int num = eq[id].GetNum();

            for (int i = 0; i < eq.Count; i++)
            {
                if (num == i) continue;
                if (eq[i].GetNum() == num) return true;
            }

            return false;
        }

        /// <summary>
        /// Найти подходящее оборудование из списка по ID;
        /// !!! Перед работой с методом проверить выходной флаг; флаг = true, если оборудование нашлось по ID
        /// </summary>
        public Equipment GetEquipByID(int id, out bool flag)
        {
            foreach (Equipment e in eq)
                if (e.GetID() == id) { flag = true; return e; }

            flag = false;
            return new Equipment(new Calendar(new List<Interval>()), -1, -1);
        }

        /// <summary>
        /// выходные параметры:
        /// bool occflag - флаг занятости оборудования (true - свободно, false - занято);
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая), является списком, для единичного оборудования брать 1ый элемент листа;
        /// List<int> equiplist - список ID оборудования, календари которых имеют интервалы, в которые попадает T, если таких нет, возвращаем список, содержащий -1
        /// </summary>
        public void IsFree(DateTime T, AOperation o, Calendar ca, Equipment e, out bool occflag, out DateTime operationtime, out int equipID)
        {
            bool flag;
            List<int> IDlist;
            List<DateTime> timelist;
            int index;
            TimeSpan lasting;
            DateTime startime;
            DateTime endtime;
            TimeSpan t = o.GetDuration();

            ca.IsFree(T, o, this, e.GetID(), out flag, out timelist, out IDlist);

            //int n = IDlist.Count;
            equipID = IDlist[0];
            ca.IsInterval(T, out index);

            if (index == -1)
            {
                index = ca.FindInterval(T);
                endtime = calendarintervals[index].GetEndTime();
                startime = calendarintervals[index].GetStartTime();
            }

            else
            {
                endtime = calendarintervals[index].GetEndTime();
                startime = calendarintervals[index].GetStartTime();
            }

            if (ca.IsInterval(T, out index))
            {
                occflag = true;
                lasting = endtime.Subtract(startime);
                if (t <= lasting) operationtime = startime + t;
                else operationtime = ca.GetTimeofRelease(T, o);
            }

            else
            {
                occflag = false;
                operationtime = ca.GetNearestStart(startime);
            }
        }
    }
}
