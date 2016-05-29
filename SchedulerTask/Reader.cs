using System;
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
        string datapattern = "dd.MM.yyyy";
        string dtpattern = "MM.dd.yyy H:mm:ss";
        Dictionary<int, IEquipment> eqdic;
        Dictionary<int,Operation> opdic;
        List<Party> partlist;
        XNamespace df;
        public Reader()
        {
            sdata = XDocument.Load("system.xml");
            tdata = XDocument.Load("tech.xml");
        }
        public Dictionary<int, IEquipment> ReadSystemData() //чтение данных по расписанию и станкам
        {
            List<Interval> intlist = new List<Interval>();
            List<Interval> doneintlist = new List<Interval>();
            eqdic = new Dictionary<int, IEquipment>();
            
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            XElement root = sdata.Root;
            df = sdata.Root.Name.Namespace;

            foreach (XElement elm in root.Descendants(df + "CalendarInformation"))
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
                foreach (XElement eg in elm.Elements(df + "EquipmentGroup"))
                {
                    foreach (XElement inc in eg.Elements(df + "Include"))
                    {
                        DateTime tmpdata = start;
                        while (tmpdata != end)
                        {
                            if ((int)tmpdata.DayOfWeek == int.Parse(inc.Attribute("day_of_week").Value))
                            {
                                int ind = inc.Attribute("time_period").Value.IndexOf("-");
                                int sh = int.Parse(inc.Attribute("time_period").Value.Substring(0, 1));
                                int eh = int.Parse(inc.Attribute("time_period").Value.Substring(ind + 1, 2));

                                intlist.Add(new Interval(new DateTime(tmpdata.Year, tmpdata.Month, tmpdata.Day, sh, 0, 0), new DateTime(tmpdata.Year, tmpdata.Month, tmpdata.Day, eh, 0, 0)));
                            }
                            tmpdata = tmpdata.AddDays(1);
                        }
                    }
                    foreach (XElement exc in eg.Elements(df + "Exclude"))
                    {

                        foreach (Interval t in intlist)
                        {
                            if ((int)t.GetStartTime().DayOfWeek == int.Parse(exc.Attribute("day_of_week").Value))
                            {
                                int ind = exc.Attribute("time_period").Value.IndexOf("-");
                                int sh = int.Parse(exc.Attribute("time_period").Value.Substring(0, 2));
                                int eh = int.Parse(exc.Attribute("time_period").Value.Substring(ind + 1, 2));

                                DateTime dt = t.GetStartTime().AddHours(-t.GetStartTime().Hour);
                                Interval tmpint;
                                doneintlist.Add(SeparateInterval(t, dt.AddHours(sh), dt.AddHours(eh), out tmpint));
                                doneintlist.Add(tmpint);

                            }
                        }
                    }
                }
              

            }

            Calendar calendar = new Calendar(doneintlist);

            foreach (XElement elm in root.Descendants(df + "EquipmentInformation").Elements(df + "EquipmentGroup"))
            {
                GroupEquipment tmp = new GroupEquipment(calendar, int.Parse(elm.Attribute("id").Value), elm.Attribute("name").Value);
                foreach (XElement eg in elm.Elements(df + "EquipmentGroup"))
                {
                    GroupEquipment gtmp = new GroupEquipment(calendar, int.Parse(eg.Attribute("id").Value), eg.Attribute("name").Value);
                    foreach (XElement eq in eg.Elements(df + "Equipment"))
                    {
                        SingleEquipment stmp = new SingleEquipment(calendar, int.Parse(eq.Attribute("id").Value), eq.Attribute("name").Value);
                        eqdic.Add(stmp.GetID(), stmp);
                        gtmp.AddEquipment(stmp);
                    }
                    tmp.AddEquipment(gtmp);
                    eqdic.Add(gtmp.GetID(), gtmp);
                }
                eqdic.Add(tmp.GetID(), tmp);
            }

            

            return eqdic;
        }
        public void ReadTechData(out List<Party> partlist, out Dictionary<int, IOperation> opdic) //чтение данных по деталям и операциям
        {
            XElement root = tdata.Root;
            df = root.Name.Namespace;
            partlist = new List<Party>();
            opdic = new Dictionary<int, IOperation>();
            List<IOperation> tmpop;
            foreach (XElement product in root.Descendants(df + "Product"))
            {
                foreach (XElement part in product.Elements(df + "Part"))
                {
                    DateTime.TryParseExact(part.Attribute("date_begin").Value, datapattern, null, DateTimeStyles.None, out begin);
                    DateTime.TryParseExact(part.Attribute("date_end").Value, datapattern, null, DateTimeStyles.None, out end);
                    Party parent = new Party(begin, end, int.Parse(part.Attribute("priority").Value), part.Attribute("name").Value, int.Parse(part.Attribute("num_products").Value));
                    tmpop = ReadOperations(part , parent, opdic);
                    foreach(IOperation op in tmpop)
                    {
                        parent.addOperationToForParty(op);
                    }
                    foreach (XElement subpart in part.Elements(df + "SubPart"))
                    {
                        Party sp = new Party(subpart.Attribute("name").Value, int.Parse(subpart.Attribute("num_products").Value));
                        tmpop = ReadOperations(subpart, parent, opdic);
                        foreach (IOperation op in tmpop)
                        {
                            sp.addOperationToForParty(op);
                        }
                        parent.addSupParty(sp);
                        //partlist.Add(sp);
                    }
                    partlist.Add(parent);
                }

            }
        }

        private List<IOperation> ReadOperations(XElement part, Party parent, Dictionary<int, IOperation> opdic)
        {
            List<IOperation> tmpop = new List<IOperation>();
            foreach (XElement oper in part.Elements(df + "Operation"))
            {
                List<IOperation> pop = new List<IOperation>();
                if (oper.Elements(df + "Previous") != null)
                {
                    foreach (XElement prop in oper.Elements(df + "Previous"))
                    {
                        pop.Add(opdic[int.Parse(prop.Attribute("id").Value)]);
                    }
                }
                int id = int.Parse(oper.Attribute("id").Value);
                int duration = int.Parse(oper.Attribute("duration").Value);
                int group = int.Parse(oper.Attribute("equipmentgroup").Value);
                tmpop.Add(new Operation(id, oper.Attribute("name").Value,new TimeSpan(duration, 0, 0), pop, eqdic[group], parent));
                opdic.Add(id, new Operation(id, oper.Attribute("name").Value, new TimeSpan(duration, 0, 0), pop, eqdic[group], parent));
            }
            return tmpop;

        }

        private Interval SeparateInterval(Interval ii, DateTime start, DateTime end, out Interval oi)
        {
            oi = new Interval(end, ii.GetEndTime());
            return new Interval(ii.GetStartTime(), start);
        }
    }
}
