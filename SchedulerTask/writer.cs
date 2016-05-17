using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SchedulerTask
{
    class writer
    {
        public void WriteData(List<Decision> dlist)
        {

          System.IO.File.Copy("tech.xml", "tech+solution.xml");
          XDocument document = new XDocument();
          document = XDocument.Load("tech+solution.xml");
          foreach (Decision d in dlist)
          {
              string id = Convert.ToString(d.GetOperation().GetID());
              bool found = false;
              foreach (XElement product in document.Descendants("WaresInformation").Descendants("Product"))
              {
                  foreach (XElement part in product.Descendants("Part"))
                  {
                      foreach (XElement op in part.Descendants("Operation"))
                      {
                          if (op.Attribute("id").Value == id)
                          {
                              found = true;
                              op.Add(new XAttribute("equipment", d.GetEquipment().GetID()));
                              op.Add(new XAttribute("date_begin", d.GetStartTime()));
                              op.Add(new XAttribute("date_end", d.GetEndTime()));
                              XAttribute attr = op.Attribute("equipmentgroup");
                              attr.Remove();
                              break;
                          }
                      }
                      if (found) break;
                      foreach (XElement sp in part.Descendants("SubPart"))
                      {
                          foreach (XElement op in sp.Descendants("Operation"))
                          {
                              if (op.Attribute("id").Value == id)
                              {
                                  found = true;
                                  op.Add(new XAttribute("equipment", d.GetEquipment().GetID()));
                                  op.Add(new XAttribute("date_begin", d.GetStartTime()));
                                  op.Add(new XAttribute("date_end", d.GetEndTime()));
                                  XAttribute attr = op.Attribute("equipmentgroup");
                                  attr.Remove();
                                  break;
                              }
                          }
                          if (found) break;
                      }
                      if (found) break;
                  }
                  if (found) break;
              }
          }
          document.Save("tech+solution.xml");

        }


    }
}
