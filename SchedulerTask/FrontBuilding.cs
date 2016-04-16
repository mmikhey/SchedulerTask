using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    /// <summary>
    /// Класс формирования фронта
    /// </summary>
    class FrontBuilding
    {
        private List<AOperation> ops;

        public FrontBuilding(List<AOperation> operations)
        {
            this.ops = operations;
        }

        public List<AOperation> BuildFront()
        {
            List<AOperation> r = new List<AOperation>();

            foreach(AOperation o in ops)
            {
                if (o.PreviousOperationIsEnd() && o.GetEquipment().freeflag) r.Add(o);
            }

            return r;
        }
    }
}
