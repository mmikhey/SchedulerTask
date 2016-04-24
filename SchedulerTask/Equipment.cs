

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
        Calendar ca;
        int eqid; //id оборудования
        int num; //номер группы (для группового оборудования)
        //bool individflag; //флаг единичного оборудования (false - оборудование атамарно)
        bool freeflag = true;
        DateTime starttime, endtime; //начало и конец выполнения 


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
        /// получить флаг занятости
        /// true - свободно
        /// false - занято
        /// </summary> 
        public bool GetOcFlag()
        {
            return freeflag;
        }

        //назначить работу o с началом работы time; если работа назначилась - флаг true
        public bool SetWork(DateTime time, AOperation o)
        {
            bool flag;

            DateTime endtime = time.Add(o.GetDuration()); //время окончания операции
            if (ca.EqIsFree(time, endtime))
            {
                StartTime = time;
                EndTime = endtime;
                ca.SetCalendar(time, o);
                flag = true;
                return flag;
            }
            else { flag = false; return flag; }

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
