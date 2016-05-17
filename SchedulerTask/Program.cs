using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Reader reader = new Reader();
            
            Dictionary<int, IEquipment> eqdic;
            reader.ReadSystemData(out eqdic);

            List<Party> partlist;
            Dictionary<int, IOperation> opdic;
            reader.ReadTechData(out partlist, out opdic);

           // eqdic.Values;

            EquipmentManager em = new EquipmentManager();

            FrontBuilding fb = new FrontBuilding(partlist, em);
            fb.Build();

            writer w = new writer();
            w.WriteData(opdic);
        }
    }
}
