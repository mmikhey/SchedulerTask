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
                TreeIterator partyIterator = i.getIterator(i);
                while (partyIterator.next())
                {
                    operations.AddRange(partyIterator.Current.getPartyOperations());
                }
            }
            
            this.equipmentManager = equipmentManager;
        }

        public void Build()
        {
            EventList events = new EventList();
            foreach (Party i in party)
            {
                events.Add(new Event(i.getStartTimeParty()));
            }

            while (events.Count != 0)
            {
                List<IOperation> front = new List<IOperation>();

                // Формирование фронта
                foreach (IOperation operation in operations)
                {
                    //if (!operation.IsEnabled() && operation.PreviousOperationIsEnd(events[0].Time) && operation.GetParty().getStartTimeParty() <= events[0].Time)
                    if (!operation.IsEnabled() && operation.PreviousOperationIsEnd(events[0].Time) &&
                        DateTime.Compare(operation.GetParty().getStartTimeParty(), events[0].Time) <= 0)    //operation наступает раньше events[0].Time
                    {
                        DateTime operationTime;
                        SingleEquipment equipment;
                        if (equipmentManager.IsFree(events[0].Time, operation, out operationTime, out equipment))
                        {
                            front.Add(operation);
                        }
                        else
                        {
                            events.Add(new Event(operationTime));
                        }
                    }
                }

                // Сортировка фронта
                FrontSort.sortFront(front);

                // Назначение операций
                foreach (IOperation operation in front)
                {
                    DateTime operationTime;
                    SingleEquipment equipment;

                    if (equipmentManager.IsFree(events[0].Time, operation, out operationTime, out equipment))
                    {
                        operation.SetOperationInPlan(events[0].Time, operationTime, equipment);
                        equipment.OccupyEquip(events[0].Time, operationTime);
                    }

                    events.Add(new Event(operationTime));
                }

                events.RemoveFirst();
            }
        }
    }
}
