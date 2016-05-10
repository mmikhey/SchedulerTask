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
    public class SingleEquipment : AEquipment
    {
        Calendar ca; //календарь для текущего оборудования
        int eqid; //id оборудования
        bool occflag; //флаг занятости оборудования; true - свободно; false - занято;
        string individualname;
        //Dictionary<int, bool> eqtacts; //словарь часовых тактов; int - значение часа; bool - значение занятости оборудования в течение часа 
        //(true - свободно, false - занято); по умолчанию весь интервал, состоящий из тактов времени считается свободным

        public SingleEquipment(Calendar ca, int id, string individualname)
        {
            this.ca = ca;
            eqid = id;
            this.individualname = individualname;
        }


        /// <summary>
        /// получить календарь оборудования 
        /// </summary>   
        public override Calendar GetCalendar()
        {
            return ca;
        }

        /// <summary>
        /// получить id оборудования (если оборудование групповое, то возвращается id группы оборудования)
        /// </summary>      
        public override int GetID()
        {
            return eqid;
        }


        public override bool GetOcFlag()
        {
            return occflag;
        }

        public override void SetOcFlag(bool val)
        {
            occflag = val;
        }


        /// <summary>
        /// можно ли назначить работу o с началом работы starttime? 
        /// да - вернем true
        /// нет - вернем false
        /// </summary> 
        public override bool CanSetWork(DateTime starttime, IOperation o)
        {
            int index;

            if (ca.IsInterval(starttime, out index))
            {
                return true;
            }

            else return false;
        }

        /// <summary>
        /// проверка доступности оборудования в такт времени T
        /// true - оборудование доступно; false - занято
        /// </summary>        
        public override bool IsFree(DateTime T)
        {
            if (GetCalendar().IsFree(T)) return true;
            else return false;
        }

        /// <summary>
        /// Занять оборудование c t1 до t2
        /// </summary>  
        public override void OccupyEquip(DateTime t1, DateTime t2)
        {
            GetCalendar().OccupyHours(t1, t2);
        }

        /// <summary>
        /// узнать, есть ли свободные интервалы времени;
        /// выходные параметры:
        /// bool occflag - флаг занятости (true - свободно, false - занято);
        /// operationtime - время окончания операции (для первого случая) или  ближайшее время начала операции (для второго случая), является списком, для единичного оборудования брать 1ый элемент листа;
        /// List<int> equiplist - список ID оборудования, календари которых имеют интервалы, в которые попадает T, если таких нет, возвращаем список, содержащий -1
        /// </summary>
        /*
        public void IsCalendarFree(DateTime T, AOperation o, EquipmentManager EM, int id, out bool occflag, out List<DateTime> operationtime, out List<int> equipIDlist)
        {
            int index;      // индекс интервала, в который можно поставить операцию (либо ближайший интервал)
            TimeSpan lasting; //"длительность" интервала
            TimeSpan t = o.GetDuration();//длительность операции
            DateTime endtime;
            DateTime startime;
            operationtime = new List<DateTime>();
            equipIDlist = new List<int>();

            //если оборудование атамарно
            if (!(EM.IsGroup(id)))
            {
                if (IsInterval(T, out index))
                {
                    occflag = true;
                    equipIDlist.Add(id);
                    endtime = ca[index].GetEndTime();
                    startime = ca[index].GetStartTime();
                    lasting = endtime.Subtract(startime);

                    if (t <= lasting) operationtime.Add(startime + t);
                    else operationtime.Add(GetTimeofRelease(T, o));
                }

                else
                {
                    occflag = false;
                    equipIDlist.Add(-1);
                    operationtime.Add(GetNearestStart(T));
                }
            }

                //если оборудование групповое
            else
            {
                List<Equipment> elist = new List<Equipment>(EM.GetEquipments());
                bool eoccflag = false;

                foreach (Equipment e in elist)
                {
                    if (e.GetCalendar().IsInterval(T, out index)) eoccflag = true;
                }

                if (eoccflag == false)
                {
                    equipIDlist.Add(-1);
                    occflag = false;
                    operationtime.Add(GetMinNearestStart(T, elist));
                }

                else
                {
                    occflag = true;
                    foreach (Equipment e in elist)
                        if (e.GetCalendar().IsInterval(T, out index))
                        {
                            operationtime.Add(e.GetCalendar().GetTimeofRelease(T, o));
                            equipIDlist.Add(e.GetID());
                        }
                }
            }

        }*/

    }
}
