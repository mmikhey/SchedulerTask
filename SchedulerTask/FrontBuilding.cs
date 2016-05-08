using System;
using System.Collections.Generic;

namespace SchedulerTask
{
    /// <summary>
    /// Формирование фронта
    /// </summary>
    class FrontBuilding
    {
        //private Party party;
        private List<AOperation> operations;
        private EquipmentManager equipmentManager;

        public FrontBuilding(/*Party party, */EquipmentManager equipmentManager)
        {
            //this.party = party;
            //operations = party.getPartyOperations();
            this.equipmentManager = equipmentManager;
        }

        public void Build()
        {
            List<Event> events = new List<Event>();
            //events.Add(new Event(party.getStartTimeParty());

            while (events.Count != 0)
            {
                List<AOperation> front = new List<AOperation>();

                // Формирование фронта
                foreach (AOperation operation in operations)
                {
                    if (!operation.IsEnabled() && operation.PreviousOperationIsEnd(events[0].Time))
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
                //FrontSort.sort(front);

                // Назначение операций
                foreach (Operation operation in front)
                {
                    DateTime operationTime;
                    int equipmentID;

                    if (equipmentManager.IsFree(events[0].Time, operation, out operationTime, out equipmentID))
                    {
                        bool flafEq;
                        Equipment equipment = equipmentManager.GetEquipByID(equipmentID, out flafEq);
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
