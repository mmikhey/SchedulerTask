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
        bool occflag; //флаг занятости оборудования; true - свободно; false - занято;
        DateTime starttime, endtime; //начало и конец выполнения операции на данном оборудовании


        public Equipment(Calendar ca, int id, int num, string groupname, string individualname)
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
        /// получить календарь оборудования 
        /// </summary>   
        public Calendar GetCalendar()
        {
            return ca;
        }

        /// <summary>
        /// получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        /// </summary>      
        public int GetID()
        {
            return eqid;
        }

        /// <summary>
        /// получить номер типа оборудования 
        /// </summary>  
        public int GetNum()
        {
            return num;
        }

        public bool GetOcFlag()
        {
            return occflag;
        }

        public void SetOcFlag(bool val)
        {
            occflag = val;
        }


        /// <summary>
        /// можно ли назначить работу o с началом работы starttime? 
        /// да - вернем true
        /// нет - вернем false
        /// </summary> 
        public bool CanSetWork(DateTime starttime, AOperation o)
        {
            int index;

            if (ca.IsInterval(starttime, out index))
            {
                return true;
            }

            else return false;
        }

        /// <summary>
        /// проверка доступности оборудования;
        /// true - оборудование доступно; false - занято
        /// </summary>        
        public bool IsFree()
        {
            if (occflag == true) return true;
            else return false;
        }

    }
}
