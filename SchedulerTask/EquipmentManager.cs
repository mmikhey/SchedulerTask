using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{

    public class EquipmentManager
    {
        /// <summary>
        /// Поиск свободного оборудования в списке; (возвращаем true, если находим свободное оборудование, false - иначе);
        /// Доп. выходные параметры:
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая); 
        /// </summary>
        public bool IsFree(DateTime T, IOperation o, out DateTime operationtime, out SingleEquipment equip)
        {
            TimeSpan t = o.GetDuration();
            int intervalindex;

            foreach (SingleEquipment e in o.GetEquipment())
            {
                if ((e.IsNotOccupied(T)) && (e.GetCalendar().IsInterval(T, out intervalindex)))
                {
                    equip = e;
                    operationtime = e.GetCalendar().GetTimeofRelease(T, t, intervalindex);
                    return true;
                }
            }

            equip = null;
            DateTime mintime = DateTime.MaxValue;
            foreach (SingleEquipment e in o.GetEquipment())
                if (e.GetCalendar().GetNearestStart(T) <= mintime) mintime = e.GetCalendar().GetNearestStart(T);
            operationtime = mintime;

            return false;
        }
    }
}
