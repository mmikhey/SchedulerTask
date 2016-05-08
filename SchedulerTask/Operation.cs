using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public interface AOperation
    {
        TimeSpan GetDuration();
        void SetOperationInPlan(DateTime real_start_time, DateTime real_end_time, Equipment real_equipment_id);
        bool IsEnd(DateTime time_);
        bool IsEnabled();
        bool PreviousOperationIsEnd(DateTime time_);
        Equipment GetEquipment();
    }

    /// <summary>
    /// операция
    /// </summary>   
    class Operation : AOperation
    {
        private TimeSpan duration;//длительность операции
        private List<AOperation> PreviousOperations;//список предыдущих операций
        private bool enable;//поставлена ли оперция в расписание
        private Equipment equipment;//обордование или группа оборудований, на котором может выполняться операция
        Decision decision = null;//решение,создается,когда операция ставится в расписание

        public Operation(int duration_, List<AOperation> Prev,Equipment equipment_)
        {
            duration = new  TimeSpan(duration_);
            PreviousOperations = new List<AOperation>();
            foreach (Operation prev in Prev)
            {
                PreviousOperations.Add(prev);
            }
            enable = false;
            equipment = equipment_;
        }

        /// <summary>
        /// получить длительность операции
        /// </summary>   
        public TimeSpan GetDuration()
        {
            return duration;
        }

        /// <summary>
        /// поставить операцию в расписание и создать решение
        /// </summary>   
        public void SetOperationInPlan(DateTime real_start_time, DateTime real_end_time, Equipment real_equipment_id)
        {
            enable = true;
            decision= new Decision (real_start_time, real_end_time, real_equipment_id, this);
        }

        /// <summary>
        /// поставлена ли операция в расписание
        /// </summary>  
        public bool IsEnabled()
        {
            return enable;
        }

        /// <summary>
        /// выполнилась ли операция к тому времени,которое подано на вход
        /// </summary>  
        public bool IsEnd(DateTime time_)
        {
            bool end = false;
            if (this.IsEnabled())
            {
                if (time_>= decision.GetEndTime())
                {
                    end = true;
                }
            }
            return end;
        }

        /// <summary>
        /// выполнены ли предыдущие операции
        /// </summary>  
        public bool PreviousOperationIsEnd(DateTime time_)
        {
            bool flag = true;
            foreach (AOperation prev in PreviousOperations)
            {
                if (prev.IsEnd(time_) == false)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// получить оборудование или группу оборудований, на котором может выполняться операция
        /// </summary>  
        public Equipment GetEquipment()
        {
            return equipment;
        }
    } 
}
