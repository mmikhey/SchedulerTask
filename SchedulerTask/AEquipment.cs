using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SchedulerTask
{
    public abstract class AEquipment : IEnumerable, IEnumerator
    {
        public abstract Calendar GetCalendar();
        public abstract int GetID();
        public abstract bool GetOcFlag();
        public abstract void SetOcFlag(bool val);
        public abstract bool CanSetWork(DateTime starttime, IOperation o);
        public abstract bool IsFree(DateTime T);
        public abstract void OccupyEquip(DateTime t1, DateTime t2);

        int index;
        List<SingleEquipment> equiplist;

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (index == equiplist.Count - 1)
            {
                Reset();
                return false;
            }

            index++;
            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get
            {
                return equiplist[index];
            }
        }



    }
}
