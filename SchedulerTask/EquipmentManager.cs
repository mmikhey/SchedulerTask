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
                    if ((T > calendarintervals[i].GetEndTime()) && (T < calendarintervals[i + 1].GetStartTime())) { index = i; break; }
            }

            return index;
        }


        //вернуть id всех свободных оборудований в указанный промежуток времени
        public List<int> FindVacantEquipment(List<Equipment> e, DateTime starttime, DateTime endtime)
        {
            List<int> indexeslist = new List<int>();

            foreach (Equipment equipment in e)
            {
                if (equipment.GetFlag() == false)
                {
                    List<Equipment> eqlist = equipment.GetIndEquipment();
                    for (int i = 0; i < eqlist.Count; i++)
                        if (eqlist[i].IsFree(starttime, endtime)) indexeslist.Add(equipment.GetID(i));
                }

                else if (equipment.IsFree(starttime, endtime)) indexeslist.Add(equipment.GetID());
            }


            return indexeslist;
        }
    }
}
