namespace SchedulerTask
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections;

    /// <summary>
    /// оборудование
    /// </summary>
    public class SingleEquipment : IEquipment
    {
        int index;
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
        /// проверка доступности оборудования в такт времени T
        /// true - оборудование доступно; false - занято
        /// </summary>        
        public bool IsOccupied(DateTime T)
        {
            //return GetCalendar().IsFree(T);
            return OccupyT2 < T;

        }

        /// <summary>
        /// Занять оборудование c t1 до t2
        /// </summary>  
        public void OccupyEquip(DateTime t1, DateTime t2)
        {
            //GetCalendar().OccupyHours(t1, t2);
            OccupyT1 = t1;
            OccupyT2 = t2;

        }

        DateTime OccupyT1 = DateTime.MinValue;
        DateTime OccupyT2 = DateTime.MinValue;
        

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (index == 1)
            {
                return false;
            }

            index++;
            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get
            {
                return this;
            }
        }

    }
}
