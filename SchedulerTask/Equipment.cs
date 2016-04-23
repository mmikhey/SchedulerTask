

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
        bool freeflag = false; //флаг занятости оборудования; по умолчанию оборудование свободно; true - занято
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

        //получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        public int GetID()
        {
            return eqid;
        }

        //получить флаг занятости оборудования
        public bool GetOcFlag()
        {
            return freeflag;
        }

        public void SetOcFlag(bool val)
        {
            freeflag = val;
        }

        ////узнать, единичное или атамарное оборудование
        //public bool GetFlag()
        //{
        //    return individflag;
        //}

        //назначить работу o с началом работы time; если работа назначилась - флаг true

        //назначить работу + вернуть флаг занятости

        public bool SetWork(DateTime time, AOperation o, Equipment e)
        {
            bool flag;

            DateTime endtime = time.Add(o.GetDuration()); //время окончания операции
            if (ca.EqIsFree(time, endtime))
            {
                StartTime = time;
                EndTime = endtime;
                ca.SetCalendar(time, o,e);
                flag = true;
                return flag;
            }
            else { flag = false; return flag; }

        }

        //проверить доступность оборудования в указанный промежуток времени; true - оборудование доступно; false - занято
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
