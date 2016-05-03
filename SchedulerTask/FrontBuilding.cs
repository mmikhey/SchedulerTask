using System;
using System.Collections.Generic;

namespace SchedulerTask
{
    /// <summary>
    /// Формирование фронта
    /// </summary>
    class FrontBuilding
    {
        private Calendar calendar;
        private List<Operation> operations;
        private EquipmentManager equipmentManager;

        public FrontBuilding(Calendar calendar, List<Operation> operations, EquipmentManager equipmentManager)
        {
            this.calendar = calendar;
            this.operations = operations;
            this.equipmentManager = equipmentManager;
        }

        public void Build()
        {
            List<Event> events = new List<Event>();

            // Находим событие с самым ранним временем начала
            DateTime minTime = DateTime.MaxValue;
            foreach (Operation operation in operations)
            {
                if (operation.GetTimeMin() < minTime)
                {
                    minTime = operation.GetTimeMin();
                }
            }
            events.Add(new Event(minTime));


            while (events.Count != 0)
            {
                List<Operation> front = new List<Operation>();

                // Формирование фронта
                foreach (Operation operation in operations)
                {
                    if (!operation.IsEnabled() && operation.PreviousOperationIsEnd(events[0].Time))
                    {
                        bool occupation;
                        DateTime operationTime;
                        int equipmentID;
                        equipmentManager.IsFree(events[0].Time, operation, calendar, operation.GetEquipment(), out occupation, out operationTime, out equipmentID);
                        if(occupation) front.Add(operation);
                    }
                }

                // Сортировка фронта
                // ...

                // Назначение операций
                foreach (Operation operation in front)
                {
                    bool occupation;
                    DateTime operationTime;
                    int equipmentID;
                    equipmentManager.IsFree(events[0].Time, operation, calendar, operation.GetEquipment(), out occupation, out operationTime, out equipmentID);

                    if (occupation)
                    {
                        bool flafEq;
                        operation.SetOperationInPlan(events[0].Time, operationTime, equipmentManager.GetEquipByID(equipmentID, out flafEq));
                    }

                    if (!events.Contains(new Event(operationTime))) events.Add(new Event(operationTime));
                }

                // Удаление текущего события
                events.RemoveAt(0);
            }
        }
    }
}
