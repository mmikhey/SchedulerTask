using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public class Decision
    {
        private DateTime start_time;//реальное время начала операции в расписании
        private DateTime end_time;// реальное время окончания операции в расписании
        private SingleEquipment equipment_id;//id реального атомарного оборудования,
                                        //на котором операция будет выполнена в расписании
        private Operation op;//операция, для которой созданно данное решение

        public Decision(DateTime start_time_, DateTime end_time_, SingleEquipment equipment_id_, Operation op_)
        {
            start_time = start_time_;
            end_time = end_time_;
            equipment_id = equipment_id_;
            op = op_;
        }

        /// <summary>
        /// получить время начала операции в расписании
        /// </summary>   
        public DateTime GetStartTime()
        {
            return start_time;
        }

        /// <summary>
        /// получить время окончания операции в расписании
        /// </summary>   
        public DateTime GetEndTime()
        {
            return end_time;
        }

        /// <summary>
        /// получить id реального атомарного оборудования,
        /// на котором операция будет выполнена в расписании
        /// </summary>   
        public SingleEquipment GetEquipment()
        {
            return equipment_id;
        }

        /// <summary>
        /// получить операцию, для которой созданно данное решение 
        /// </summary>   
        public Operation GetOperation()
        {
            return op;
        }
    }
}
