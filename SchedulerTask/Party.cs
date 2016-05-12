using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{   
    public class Party
    {        
        //список операций, необходимых для выполнения партии(заказа)
        private List<AOperation> operationsForParty;
        //ранне время начала
        private DateTime startTimeParty;
        //директивный срок(познее время окончания)
        private DateTime endTimeParty;
        //приоритет
        private int priority;
        //name партии
        private String name;
        //num продукта
        private int num_products;
        //родитель(в случае с подпартиями - деревья)
        private Party parent;
        //подпартии
        private List<Party> subParty;
        public TreeIterator iterator;

        public Party(DateTime startTime, DateTime endTime, int priority, String name, int num_products)
        {
            this.startTimeParty = startTime;
            this.endTimeParty = endTime;
            this.priority = priority;
            this.name = name;
            this.num_products = num_products;
        }
        //конструктор для подпартий
        public Party(String name, int num_products)
        {
            this.name = name;
            this.num_products = num_products;
        }
        public Party()
        {
        }
        //добавлениее операций партии
        public void addOperationToForParty(AOperation operation)
        {
            if (operationsForParty == null)
            {
                operationsForParty = new List<AOperation>();
            }
            operationsForParty.Add(operation);

        }
        //добавление подпартий
        public void addSupParty(Party subPart)
        {
            subParty.Add(subPart);
            if (subPart.getRoot() != subPart && this != subPart && this.parent != subPart)
            {
                if (subParty == null)
                {
                    subParty = new List<Party>();
                }
                subParty.Add(subPart);

                if (subPart.getParent() == null)
                {
                    subPart.setParent(this);
                }
            }

        }
        public Party getParent()
        {
            return parent;
        }
        public void setParent(Party parent)
        {
            this.parent = parent;
        }
        public Party getRoot()
        {
            if (this.parent == null)
            {
                return null;
            }
            else
            {
                Party tmp = this;
                while (true)
                {
                    if (tmp.getParent() == null)
                    {
                        // tmp = this;
                        break;
                    }
                    else
                    {
                        tmp = tmp.getParent();
                    }
                }
                return tmp;
            }
        }
        public TreeIterator getIterator()
        {
            if (iterator == null)
            {
                return new TreeIterator(getRoot());
            }
            return iterator;

        }
        public bool isLeaf()
        {
            return subParty == null || subParty.Count == 0;
        }
        public void setStartTimeParty(DateTime start)
        {
            this.startTimeParty = start;
        }
        public void setEndTimeParty(DateTime end)
        {
            this.endTimeParty = end;
        }
        public void setPriority(int pr)
        {
            this.priority = pr;
        }
        public int getPriority()
        {
            return priority;
        }
        public string getPartyName()
        {
            return name;
        }
        public void setPartyName(string name)
        {
            this.name = name;
        }
        public DateTime getStartTimeParty()
        {
            return startTimeParty;
        }
        public DateTime getEndTimeParty()
        {
            return endTimeParty;
        }
        public List<AOperation> getPartyOperations()
        {
            return operationsForParty;
        }
        public List<Party> getSubParty()
        {
            return subParty;
        }

    }   
}



