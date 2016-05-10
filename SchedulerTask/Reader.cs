﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;


namespace SchedulerTask
{
    class Reader
    {
        XDocument sdata;
        XDocument tdata;
        DateTime begin;
        DateTime end;
        string datapattern = "MM.dd.yyyy";
        string dtpattern = "MM.dd.yyy H:mm:ss";
     
        Dictionary<int,Operation> opdic;
        List<Party> partlist;
        public Reader()
        {
            sdata = XDocument.Load("system.xml");
            tdata = XDocument.Load("tech.xml");
        }
        public void ReadSystemData(out List<Equipment> eqlist) //чтение данных по расписанию и станкам
        {
            List<Interval> intlist = new List<Interval>();
             eqlist = new List<Equipment>();

            Calendar calendar = new Calendar(intlist);
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            foreach (XElement elm in sdata.Descendants("InformationModel").Descendants("CalendarInformation"))
            {
                if (elm.Attribute("date_begin") != null)
                {
                    string date = elm.Attribute("date_begin").Value;
                    DateTime.TryParseExact(date, datapattern, null, DateTimeStyles.None, out start);
                }
                if (elm.Attribute("date_end") != null)
                {
                    string date = elm.Attribute("date_end").Value;
                    DateTime.TryParseExact(date, datapattern, null, DateTimeStyles.None, out end);
                }
                foreach (XElement eg in elm.Descendants("EquipmentGroup"))
                {
                    foreach (XElement inc in eg.Descendants("Include"))
                    {
                        DateTime tmpdata = start;
                        while (tmpdata.Day != end.Day)
                        {
                            if ((int)tmpdata.DayOfWeek == int.Parse(inc.Attribute("day_of_week").Value))
                            {
                                int ind = inc.Attribute("time_period").Value.IndexOf("-");
                                int sh = int.Parse(inc.Attribute("time_period").Value.Substring(0, 1));
                                int eh = int.Parse(inc.Attribute("time_period").Value.Substring(ind + 1, 2));

                                intlist.Add(new Interval(new DateTime(tmpdata.Year, tmpdata.Month, tmpdata.Day, sh, 0, 0), new DateTime(tmpdata.Year, tmpdata.Month, tmpdata.Day, eh, 0, 0)));
                            }
                            tmpdata.AddDays(1);
                        }
                    }
                    foreach (XElement exc in eg.Descendants("Exclude"))
                    {

                        foreach (Interval t in intlist)
                        {
                            if ((int)t.GetStartTime().DayOfWeek == int.Parse(exc.Attribute("day_of_week").Value))
                            {
                                int ind = exc.Attribute("time_period").Value.IndexOf("-");
                                int sh = int.Parse(exc.Attribute("time_period").Value.Substring(0, 2));
                                int eh = int.Parse(exc.Attribute("time_period").Value.Substring(ind + 1, 2));

                                DateTime dt = t.GetStartTime().AddHours(-t.GetStartTime().Hour);
                                t.OccupyHours(dt.AddHours(sh), dt.AddHours(eh - sh));
                            }
                        }
                    }
                }

            }
            foreach (XElement elm in sdata.Descendants("InformationModel").Descendants("EquipmentGroup"))
            {
                foreach (XElement eg in elm.Descendants("EquipmentGroup"))
                    foreach (XElement eq in eg.Descendants("Equipment"))
                    {
                        eqlist.Add(new Equipment(calendar, int.Parse(eq.Attribute("id").Value), int.Parse(eg.Attribute("id").Value), eg.Attribute("name").Value, eq.Attribute("name").Value));
                    }
            }

        }
        public void ReadTechData(out List<Party> partlist, out Dictionary<int, Operation> opdic) //чтение данных по деталям и операциям
        {
            partlist = new List<Party>();
            opdic = new Dictionary<int, Operation>();
            List<AOperation> tmpop;
            foreach (XElement product in tdata.Descendants("InformationModel").Descendants("WaresInformation").Descendants("Product"))
            {
                foreach (XElement part in product.Descendants("Part"))
                {
                    DateTime.TryParseExact(part.Attribute("date_begin").Value, datapattern, null, DateTimeStyles.None, out begin);
                    DateTime.TryParseExact(part.Attribute("date_end").Value, datapattern, null, DateTimeStyles.None, out end);
                    Party parent = new Party(begin, end, int.Parse(part.Attribute("priority").Value));
                    tmpop = ReadOperations(part);
                    foreach(AOperation op in tmpop)
                    {
                        parent.addOperationToForParty(op);
                    }
                    foreach (XElement subpart in product.Descendants("Subpart"))
                    {
                        Party sp = new Party(begin, end, int.Parse(part.Attribute("priority").Value));
                        tmpop = ReadOperations(subpart);
                        foreach (AOperation op in tmpop)
                        {
                            sp.addOperationToForParty(op);
                        }
                        parent.addSupParty(sp);
                        partlist.Add(sp);
                    }
                    partlist.Add(parent);
                }

            }
        }

        private List<AOperation> ReadOperations(XElement part)
        {
            List<AOperation> tmpop = new List<AOperation>();
            foreach (XElement oper in part.Descendants("Operation"))
            {
                List<AOperation> pop = new List<AOperation>();
                if (oper.Descendants("Previous") != null)
                {
                    foreach (XElement prop in oper.Descendants("Previous"))
                    {
                        pop.Add(opdic[int.Parse(prop.Attribute("id").Value)]);
                    }
                }
                tmpop.Add(new Operation(int.Parse(oper.Attribute("id").Value), oper.Attribute("name").Value, int.Parse(oper.Attribute("duration").Value), pop, int.Parse(oper.Attribute("equipmentgroup").Value)));
                opdic.Add(int.Parse(oper.Attribute("id").Value), new Operation(int.Parse(oper.Attribute("id").Value), oper.Attribute("name").Value, int.Parse(oper.Attribute("duration").Value), pop, int.Parse(oper.Attribute("equipmentgroup").Value)));
            }
            return tmpop;

        }
    }
}