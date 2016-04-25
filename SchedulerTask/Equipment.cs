namespace SchedulerTask
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// оборудование
    /// </summary>
    public class Equipment
    {
        Calendar ca; //календарь для текущего оборудования
        int eqid; //id оборудования
        int num; //номер группы (для группового оборудования)
        DateTime starttime, endtime; //начало и конец выполнения операции на данном оборудовании


        public Equipment(Calendar ca, int id, int num)
        {
            this.ca = ca;
            eqid = id;
            this.num = num;
        }

        public DateTime StartTime
        {
            set { starttime = value; }
        }

        public DateTime EndTime
        {
            set { endtime = value; }
        }

        /// <summary>
        /// получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        /// </summary>      
        public int GetID()
        {
            return eqid;
        }


        /// <summary>
        /// можно ли назначить работу o с началом работы starttime? 
        /// да - вернем true
        /// нет - вернем false
        /// </summary> 
        public bool CanSetWork(DateTime starttime, AOperation o)
        {
            if (ca.SetCalendar(starttime, o))
            {
                return true;
            }

            else return false;
        }

        /// <summary>
        /// проверка доступности оборудования в указанный промежуток времени (от time до endtime); 
        /// true - оборудование доступно; false - занято
        /// </summary>        
        public bool IsFree(DateTime time, DateTime endtime)
        {
            if (ca.EqIsFree(time, endtime)) return true;
            else return false;
        }

        //public List<Equipment> GetIndEquipment()
        //{
        //    List<Equipment> eqlist = new List<Equipment>();
        //    if (individflag == true) eqlist.Add(this);
        //    //else eqlist.Add
        //    return eqlist;
        //}
    }
}
