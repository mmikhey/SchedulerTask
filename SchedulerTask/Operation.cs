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
        DateTime GetTimeExecution();
        void SetTimeExecution(TimeSpan time_);
        bool IsEnd(DateTime time_);
        bool IsEnabled();
        //void SetEnd(bool end_);
        bool PreviousOperationIsEnd(DateTime time_);
        Equipment GetEquipment();
        bool MayOperationPerformed(DateTime time_);
    }

    class Operation : AOperation//add equipment???????? and MayOperationPerformed
    {
        private TimeSpan duration;
        private DateTime time_min;
        private DateTime time_max;
        private bool interrupted;
        private DateTime time_execution;//execution time
        private List<AOperation> PreviousOperations;
        //private bool end;//operation is over
        private Equipment equipment;
        //private EquipmentManager manager;
        private List<Interval> intervals;

        public Operation(int duration_, int time_min_, int time_max_, bool interrupted_, List<AOperation> Prev,Equipment equipment_)
        {
            duration = new  TimeSpan(duration_);
            time_min = new DateTime(time_min_);
            time_max = new DateTime(time_max_);
            interrupted = interrupted_;
            PreviousOperations = new List<AOperation>();
            time_execution = new DateTime(0);
            foreach (Operation prev in Prev)
            {
                PreviousOperations.Add(prev);
            }
            //end = false;
            equipment = equipment_;

        }

        public void AddInterval(Interval interval)
        {
            intervals.Add(interval);
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

        public DateTime GetTimeExecution()
        {
            return time_execution;
        }

        public void SetTimeExecution(TimeSpan time_)
        {
            time_execution.Add(time_);
        }

        //если поставлена,может быть запоминать интервал? и атомарное оборудование
        //или просто id интервала,а там хранить операцию и оборудование?
        public bool IsEnabled()
        {
            bool flag = false;
            if (intervals.Count() == 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        //если операция поставлена в расписание и момент времени запроса
        //больше или равен моменту окончания операции(время начала + время длительности

        public bool IsEnd(DateTime time_)
        {
            //return end;
            bool end=false;
            if (this.IsEnabled())
            {
                DateTime tmp = intervals[0].GetEndTime();
                for (int i=0;i<intervals.Count()-1;i++)
                { 
                    if (intervals[i].GetEndTime().CompareTo(intervals[i+1])>0)
                    {
                        tmp = intervals[i].GetEndTime();
                    }
                }
                if ((time_.CompareTo(tmp) > 0)||(time_.CompareTo(tmp) == 0))
                {
                    end = true;
                }
            }
            return end;

        }
          
        //public void SetEnd(bool end_)
        //{
        //    end = end_;
        //}

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

        public bool MayOperationPerformed(DateTime time_)
        {
            bool flag = false;
            if ((this.PreviousOperationIsEnd(time_)) && (equipment.IsFree(time_,time_.Add(this.GetDuration()))))
            {
                flag = true;
            }
            return flag;
        }
    }
    
}
