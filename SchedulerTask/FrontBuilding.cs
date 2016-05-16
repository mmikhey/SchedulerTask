using System;
using System.Collections.Generic;

namespace SchedulerTask
{
    /// <summary>
    /// Формирование фронта
    /// </summary>
    class FrontBuilding
    {
        private List<Party> party;
        private List<IOperation> operations;
        private EquipmentManager equipmentManager;

        public FrontBuilding(List<Party> party, EquipmentManager equipmentManager)
        {
            this.party = party;

            // Получение операций из партий
            operations = new List<IOperation>();
            foreach (Party i in party)
            {
                TreeIterator partyIterator = i.getIterator();
                while (partyIterator.hasNext())
                {
                    operations.AddRange(partyIterator.Current.getPartyOperations());
                    partyIterator.next();
                }
            }
            
            this.equipmentManager = equipmentManager;
        }

        public void Build()
        {
            List<Event> events = new List<Event>();
            foreach (Party i in party)
            {
                events.Add(new Event(i.getStartTimeParty()));
            }
            events.Sort();

            while (events.Count != 0)
            {
                List<IOperation> front = new List<IOperation>();

                // Формирование фронта
                foreach (IOperation operation in operations)
                {
                    if (!operation.IsEnabled() && operation.PreviousOperationIsEnd(events[0].Time) && operation.GetParty().getStartTimeParty() >= events[0].Time)
                    {
                        DateTime operationTime;
                        int equipmentID;
                        if (equipmentManager.IsFree(events[0].Time, operation, out operationTime, out equipmentID))
                        {
                            front.Add(operation);
                        }
                        else
                        {
                            if (!events.Contains(new Event(operationTime)))
                            {
                                events.Add(new Event(operationTime));
                            }
                        }
                    }
                }

                // Сортировка фронта
                FrontSort.sortFront(front);

                // Назначение операций
                foreach (IOperation operation in front)
                {
                    DateTime operationTime;
                    int equipmentID;

                    if (equipmentManager.IsFree(events[0].Time, operation, out operationTime, out equipmentID))
                    {
                        bool flafEq;
                        IEquipment equipment = equipmentManager.GetEquipByID(equipmentID, out flafEq);
                        operation.SetOperationInPlan(events[0].Time, operationTime, equipment);
                        equipment.OccupyEquip(events[0].Time, operationTime);
                    }

                    if (!events.Contains(new Event(operationTime)))
                    {
                        events.Add(new Event(operationTime));
                    }
                }

                // Удаление текущего события
                events.RemoveAt(0);

                // Сортировка events
                events.Sort();
            }
        }
    }
}
