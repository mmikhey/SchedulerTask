using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    class Decision
    {
        private DateTime start_time;
        private DateTime end_time;
        private Equipment equipment_id;
        private Operation op;

        public Decision(DateTime start_time_, DateTime end_time_, Equipment equipment_id_, Operation op_)
        {
            start_time = start_time_;
            end_time = end_time_;
            equipment_id = equipment_id_;
            op = op_;
        }
        public DateTime GetStartTime()
        {
            return start_time;
        }
        public DateTime GetEndTime()
        {
            return end_time;
        }
        public Equipment GetEquipment()
        {
            return equipment_id;
        }
        public Operation GetOperation()
        {
            return op;
        }
    }
}
