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
        // List<Equipment> eq;

        public EquipmentManager(List<Interval> calendarintervals)
        {
            this.calendarintervals = calendarintervals;


            for (int i = 0; i < calendarintervals.Count; i++)
                if (calendarintervals[i].GetEndTime() >= calendarintervals[i + 1].GetStartTime())
                { calendarintervals[i].SetEndTime(calendarintervals[i + 1].GetEndTime()); calendarintervals.RemoveAt(i + 1); }

            /* for (int i = 0; i < eq.Count; i++)
                 for (int j = 0; j < eq.Count; j++)
                 {
                     if (eq[j].GetID() > eq[j + 1].GetID())
                     {
                         Equipment etpm = eq[j];
                         eq[j] = eq[j + 1];
                         eq[j + 1] = etpm;
                     }
                 }*/
        }

        //вернуть индекс инервала в календаре, в который попадает заданный такт времени T
        //если не попадает ни в один из инервалов - найти индекс ближайшего возможного
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


        /* //вернуть id всех свободных оборудований в указанный промежуток времени
         public List<int> FindVacantEquipment(List<Equipment> e, DateTime starttime, DateTime endtime)
         {
             List<int> indexeslist = new List<int>();

             foreach (Equipment equipment in e)
             {
                 if (equipment.IsFree(starttime, endtime)) indexeslist.Add(equipment.GetID());
             }
             return indexeslist;
         }*/

        //public List<Equipment> GetEquipments()
        //{
        //    return eq;
        //}



        /// <summary>
        /// Найти подходящее оборудование из списка по ID;
        /// !!! Перед работой с методом проверить выходной флаг; флаг = true, если оборудование нашлось по ID
        /// </summary>
        public AEquipment GetEquipByID(int id, out bool flag)
        {
            foreach (AEquipment e in eq)
                if (e.GetID() == id) { flag = true; return e; }

            flag = false;
            return new SingleEquipment(new Calendar(new List<Interval>()), -1, "lala");
        }


        /// <summary>
        /// вернем true, если оборудование групповое
        /// false - атамарно
        /// </summary>
        public bool IsGroup(AEquipment e)
        {
            if (e is GroupEquipment) return true;
            else return false;
        }

        /// <summary>
        /// Поиск свободного оборудования в списке; (возвращаем true, если находим свободное оборудование, false - иначе);
        /// Доп. выходные параметры:
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая); 
        /// </summary>
        public bool IsFree(DateTime T, IOperation o, out DateTime operationtime, out int equipID)
        {
            TimeSpan lasting;
            DateTime startime;
            DateTime endtime;
            TimeSpan t = o.GetDuration();
            int intervalindex;


            //if (IsGroup(o.GetEquipment()))

            foreach (AEquipment e in o.GetEquipment())
            {
                if ((e.IsFree(T)) && (e.GetCalendar().IsInterval(T, out intervalindex)))
                {
                    equipID = e.GetID();
                    endtime = e.GetCalendar().GetInterval(intervalindex).GetEndTime();
                    startime = e.GetCalendar().GetInterval(intervalindex).GetStartTime();
                    lasting = endtime.Subtract(startime);

                    if (t <= lasting) operationtime = startime + t;
                    else operationtime = e.GetCalendar().GetTimeofRelease(T, o);
                    return true;
                }
            }

            //else

            equipID = -1;
            DateTime mintime = new DateTime();
            foreach (AEquipment equip in o.GetEquipment())
                if (equip.GetCalendar().GetNearestStart(T) <= mintime) mintime = equip.GetCalendar().GetNearestStart(T);
            operationtime = mintime;

            return false;
        }
    }
}
