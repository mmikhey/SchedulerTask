using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public interface IOperation
    {
        TimeSpan GetDuration();
        void SetOperationInPlan(DateTime real_start_time, DateTime real_end_time, SingleEquipment real_equipment_id);
        bool IsEnd(DateTime time_);
        bool IsEnabled();
        bool PreviousOperationIsEnd(DateTime time_);
        IEquipment GetEquipment();
        Party GetParty();
        Decision GetDecision();
    }

    /// <summary>
    /// операция
    /// </summary>   
    public class Operation : IOperation
    {
        private int id;//id операции
        private string name;//name операции
        private TimeSpan duration;//длительность операции
        private List<IOperation> PreviousOperations;//список предыдущих операций
        private bool enable;//поставлена ли оперция в расписание
        private IEquipment equipment;//обордование или группа оборудований, на котором может выполняться операция
        private Decision decision = null;//решение,создается,когда операция ставится в расписание
        private Party parent_party;//ссылка на партию,в которой состоит данная операция

        public Operation(int id_,string name_,int duration_, List<IOperation> Prev,IEquipment equipment_,Party party)
        {
            id = id_;
            name = name_;
            duration = new  TimeSpan(duration_);
            PreviousOperations = new List<IOperation>();
            foreach (IOperation prev in Prev)
            {
                PreviousOperations.Add(prev);
            }
            enable = false;
            equipment = equipment_;
            parent_party = party;
        }

        /// <summary>
        /// получить id операции
        /// </summary>   
        public int GetID()
        {
            return id;
        }

        /// <summary>
        /// получить имя операции
        /// </summary>   
        public string GetName()
        {
            return name;
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
        public void SetOperationInPlan(DateTime real_start_time, DateTime real_end_time, SingleEquipment real_equipment_id)
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
            foreach (IOperation prev in PreviousOperations)
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
        public IEquipment GetEquipment()
        {
            return equipment;
        }

        /// <summary>
        /// получить ссылку на партию,в которой состоит данная операция
        /// </summary>   
        public Party GetParty()
        {
            return parent_party;
        }

        /// <summary>
        /// получить ссылку решение для данной операции
        /// </summary>
        public Decision GetDecision()
        {
            return decision;
        }
    } 
}
