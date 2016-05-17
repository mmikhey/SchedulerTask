using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SchedulerTask
{
    public class GroupEquipment : IEquipment
    {

        int index;
        List<IEquipment> equiplist;
        int eqid; //id оборудования
        string name;

        public GroupEquipment(Calendar ca, int id, string name)
        {
            eqid = id;
            this.name = name;
        }

        public void AddEquipment(IEquipment e)
        {
            equiplist.Add(e);
        }

        public Calendar GetCalendar()
        {
            return equiplist[index].GetCalendar();
        }

        /// <summary>
        /// Занять оборудование c t1 до t2
        /// </summary>  
        public void OccupyEquip(DateTime t1, DateTime t2)
        {
            equiplist[index].GetCalendar().OccupyHours(t1, t2);
        }

        /// <summary>
        /// получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        /// </summary>      
        public int GetID()
        {
            return eqid;
        }


        /// <summary>
        /// проверка доступности оборудования в такт времени T
        /// true - оборудование доступно; false - занято
        /// </summary>        
        public bool IsOccupied(DateTime T)
        {
            foreach (IEquipment e in this)
            {
                if (e.GetCalendar().IsFree(T)) return true;
            }

            return false;
        }


        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            bool res = equiplist[index].MoveNext();
            if (!res)
            {
                index++;

                if (index == equiplist.Count)
                {
                    return false;
                }

                equiplist[index].MoveNext();
            }

            return true;
        }

        public void Reset()
        {
            index = 0;
            foreach (IEquipment e in equiplist)
                e.Reset();
        }

        public object Current
        {
            get
            {
                return equiplist[index].Current;
            }
        }
    }
}
