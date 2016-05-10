using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public class GroupEquipment : AEquipment
    {
        List<SingleEquipment> equiplist;
        Calendar ca; //календарь для текущего оборудования
        int eqid; //id оборудования
        bool occflag; //флаг занятости оборудования; true - свободно; false - занято;
        string name;
        //Dictionary<int, bool> eqtacts; //словарь часовых тактов; int - значение часа; bool - значение занятости оборудования в течение часа 
        //(true - свободно, false - занято); по умолчанию весь интервал, состоящий из тактов времени считается свободным

        public GroupEquipment(Calendar ca, int id, string name)
        {
            this.ca = ca;
            eqid = id;
            this.name = name;
        }

        public void AddEquipment(SingleEquipment e)
        {
            equiplist.Add(e);
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
        public override bool CanSetWork(DateTime starttime, AOperation o)
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
    }
}
