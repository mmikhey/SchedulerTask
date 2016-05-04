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
        DateTime GetTimeMin();
        DateTime GetTimeMax();
        bool IsInterrupted();
        bool IsEnd(DateTime time_);
        bool IsEnabled();
        bool PreviousOperationIsEnd(DateTime time_);
        Equipment GetEquipment();
    }

    class Operation : AOperation
    {
        private TimeSpan duration;
        private DateTime time_min;
        private DateTime time_max;
        private bool interrupted;
        private List<AOperation> PreviousOperations;
        private bool enable;
        private Equipment equipment;
        Decision decision = null;

        public Operation(int duration_, int time_min_, int time_max_, bool interrupted_, List<AOperation> Prev,Equipment equipment_)
        {
            duration = new  TimeSpan(duration_);
            time_min = new DateTime(time_min_);
            time_max = new DateTime(time_max_);
            interrupted = interrupted_;
            PreviousOperations = new List<AOperation>();
            foreach (Operation prev in Prev)
            {
                PreviousOperations.Add(prev);
            }
            enable = false;
            equipment = equipment_;
        }

        public TimeSpan GetDuration()
        {
            return duration;
        }

        public DateTime GetTimeMin()
        {
            return time_min;
        }

        public DateTime GetTimeMax()
        {
            return time_max;
        }

        public bool IsInterrupted()
        {
            return interrupted;
        }
        public void SetOperationInPlan(DateTime real_start_time, DateTime real_end_time, Equipment real_equipment_id)
        {
            enable = true;
            decision= new Decision (real_start_time, real_end_time, real_equipment_id, this);
        }
        public bool IsEnabled()
        {
            return enable;
        }
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
        public Equipment GetEquipment()
        {
            return equipment;
        }
    } 
}
