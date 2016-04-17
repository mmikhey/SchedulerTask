    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace SchedulerTask
{


    /// <summary>
    /// оборудование
    /// </summary>
    public class Equipment
    {
        Calendar ca;
        int eqid;
        List<int> eqids;
        bool individflag; //флаг единичного оборудования (false - оборудование атамарно)
        public bool freeflag = true;
        DateTime starttime, endtime; //начало и конец выполнения 


        public Equipment(Calendar ca, int id, bool flag)
        {
            this.ca = ca;
            eqid = id;
            individflag = flag;
        }

        public Equipment(Calendar ca)
        {
            this.ca = ca;
        }

        public bool freeflag = true;
        int starttime, endtime; //начало и конец выполнения 

        public int StartTime
        {
            // get { return starttime; }
            set { starttime = value; }
        }

        public int EndTime
        {
            // get { return starttime; }
            set { endtime = value; }
        }

        //получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        public int GetID()
        {
            return eqid;
        }

        //узнать id конкретного оборудования в группе
        public int GetID(int individpos)
        {
            int id = eqids[individpos];
            return id;
        }

        //узнать, единичное или атамарное оборудование
        public bool GetFlag()
        {
            return individflag;
        }

        //назначить работу o с началом работы time; если работа назначилась - флаг true
        public bool SetWork(DateTime time, AOperation o)
        {
            bool flag;

            DateTime endtime = time.Add(o.GetDuration()); //время окончания операции

        //назначить работу o с началом работы time 
        public void SetWork(int time, AOperation o)
        {
            int endtime = o.GetDuration() + time; //время окончания операции

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

        //проверить доступность оборудования в указанный промежуток времени; true - оборудование доступно; false - занято
        public bool IsFree(DateTime time, DateTime endtime)
        {
            if (ca.EqIsFree(time, endtime)) return true;
            else return false;
        }

        public List<Equipment> GetIndEquipment()
        {
            List<Equipment> eqlist = new List<Equipment>();
            if (individflag == true) eqlist.Add(this);
            //else eqlist.Add
            return eqlist;
            }

        }
    }
}
