using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    /// <summary>
    /// Список событий
    /// </summary>
    public class EventList
    {
        List<Event> events;
        public EventList()
        {
            events = new List<Event>();
        }

        /// <summary>
        /// Добавление нового события. Событие будет добавлено, если отсутствует в списке.
        /// </summary>
        /// <param name="item">Новое событие</param>
        public void Add(Event item)
        {
            foreach (Event e in events)
            {
                if (e.Time == item.Time) return;
            }
            events.Add(item);
            Sort();
        }

        /// <summary>
        /// Сортировка списка событий по невозрастанию.
        /// </summary>
        public void Sort()
        {
            events.Sort();
        }

        public int Count
        {
            get { return events.Count; }
        }

        public Event this[int index]
        {
            get { return events[index]; }
        }

        public void RemoveFirst()
        {
            if (events.Count != 0) events.RemoveAt(0);
            Sort();
        }
    }
}
