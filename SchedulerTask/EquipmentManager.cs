using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{

    public class EquipmentManager
    {
        //List<Interval> calendarintervals;
        List<IEquipment> elist;

        public EquipmentManager(/*List<IEquipment> elist*/)
        {
            //this.elist = elist;

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
        //public IEquipment GetEquipByID(int id, out bool flag)
        //{
        //    foreach (IEquipment e in elist)
        //        if (e.GetID() == id) { flag = true; return e; }

        //    flag = false;
        //    return new SingleEquipment(new Calendar(new List<Interval>()), -1, "lala");
        //}



        /// <summary>
        /// Поиск свободного оборудования в списке; (возвращаем true, если находим свободное оборудование, false - иначе);
        /// Доп. выходные параметры:
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая); 
        /// </summary>
        public bool IsFree(DateTime T, IOperation o, out DateTime operationtime, out IEquipment equip)
        {
            TimeSpan lasting;
            DateTime startime;
            DateTime endtime;
            TimeSpan t = o.GetDuration();
            int intervalindex;

            foreach (IEquipment e in o.GetEquipment())
            {
                if ((e.IsOccupied(T)) && (e.GetCalendar().IsInterval(T, out intervalindex)))
                {
                    equip = e;
                    endtime = e.GetCalendar().GetInterval(intervalindex).GetEndTime();
                    startime = e.GetCalendar().GetInterval(intervalindex).GetStartTime();
                    lasting = endtime.Subtract(startime);

                    if (t <= lasting) operationtime = startime + t;
                    else operationtime = e.GetCalendar().GetTimeofRelease(T, o);
                    return true;
                }
            }

            equip = null;
            DateTime mintime = DateTime.MaxValue;
            foreach (IEquipment e in o.GetEquipment())
                if (e.GetCalendar().GetNearestStart(T) <= mintime) mintime = e.GetCalendar().GetNearestStart(T);
            operationtime = mintime;

            return false;
        }
    }
}
