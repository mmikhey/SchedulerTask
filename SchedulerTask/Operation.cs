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
        int GetTimeMin();
        int GetTimeMax();
        bool IsInterrupted();
        int GetTimeExecution();
        void SetTimeExecution(int time_);
        bool IsEnd();
        void SetEnd(bool end_);
        bool PreviousOperationIsEnd();
    }

    class Operation : AOperation//add equipment????????
    {
        private TimeSpan duration;
        private int time_min;
        private int time_max;
        private bool interrupted;
        private int time_execution;//execution time
        private List<AOperation> PreviousOperations;
        private bool end;//operation is over

        public Operation(TimeSpan duration_, int time_min_, int time_max_, bool interrupted_, List<AOperation> Prev)
        {
            duration = duration_;
            time_min = time_min_;
            time_max = time_max_;
            interrupted = interrupted_;
            PreviousOperations = new List<AOperation>();
            time_execution = 0;
            foreach (Operation prev in Prev)
            {
                PreviousOperations.Add(prev);
            }
            end = false;

        }

        public TimeSpan GetDuration()
        {
            return duration;
        }

        public int GetTimeMin()
        {
            return time_min;
        }

        public int GetTimeMax()
        {
            return time_max;
        }

        public bool IsInterrupted()
        {
            return interrupted;
        }

        public int GetTimeExecution()
        {
            return time_execution;
        }

        public void SetTimeExecution(int time_)
        {
            time_execution = time_;
        }

        public bool IsEnd()
        {
            return end;
        }

        public void SetEnd(bool end_)
        {
            end = end_;
        }

        public bool PreviousOperationIsEnd()
        {
            bool flag = true;
            foreach (AOperation prev in PreviousOperations)
            {
                if (prev.IsEnd() == false)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
    }
    
}
