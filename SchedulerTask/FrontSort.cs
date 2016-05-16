using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    class FrontSort
    {
        private static int comparision(IOperation a, IOperation b)
        {
            int res = 0;
            if (a.GetParty().getPriority() > b.GetParty().getPriority())
                res = 1;
            if (a.GetParty().getPriority() == b.GetParty().getPriority())
                res = 0;
            if (a.GetParty().getPriority() < b.GetParty().getPriority())
                res = -1;
            
            return res;
        }

        public static void sortFront(List<IOperation> front)
        {
            front.Sort(comparision);	
		    
		}

        
		
    }
}
