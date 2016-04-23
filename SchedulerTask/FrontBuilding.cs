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

        public FrontBuilding(Calendar calendar)
        {
            this.calendar = calendar;
        }

        /// <summary>
        /// Построить фронт
        /// </summary>
        /// <param name="operations">Невыполненные операции</param>
        /// <param name="currenTime">Текущее время</param>
        /// <param name="events">Список событий (будут добавлены новые события)</param>
        public void BuildFront(List<Operation> operations, DateTime currenTime, List<Event> events)
        {
            List<Operation> front = new List<Operation>();

            // Формирование фронта
            foreach (Operation operation in operations)
            {
                DateTime startTime = currenTime;
                DateTime endTime = currenTime.Add(operation.GetDuration());

                if (operation.PreviousOperationIsEnd(currenTime) && operation.GetEquipment().GetOcFlag() &&
                    calendar.EqIsFree(startTime, endTime))
                {
                    front.Add(operation);
                }
            }

            // Упорядочение фронта
            //SortFront.Sort(front);

            // Назначение операций
            foreach (Operation operation in front)
            {
                bool occflag;
                DateTime operationtime;
                calendar.GetTimeofRelease(currenTime, operation, out occflag, out operationtime);
                if (!occflag)
                {
                    // Событие на момент завершения выполнения операции
                    events.Add(new Event(operationtime.Add(operation.GetDuration())));
                }
                else
                {
                    // Событие на момент появления возможности выполнения операции
                    events.Add(new Event(operationtime));
                }
            }
        }
    }
}
