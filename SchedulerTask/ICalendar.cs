using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public interface ICalendar
    {
        int FindInterval(DateTime T);
    }
}
