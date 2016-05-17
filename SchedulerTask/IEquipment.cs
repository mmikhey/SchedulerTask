using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SchedulerTask
{
    public interface IEquipment : IEnumerable, IEnumerator
    {
        int GetID();
        bool IsOccupied(DateTime T);
        //void OccupyEquip(DateTime t1, DateTime t2);
    }
}
