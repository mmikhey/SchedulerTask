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
        public void WriteData(Dictionary<int, IOperation> oplist)
        {
          System.IO.File.Delete("tech+solution.xml");
          System.IO.File.Copy("tech.xml", "tech+solution.xml");
          XDocument document = new XDocument();
          document = XDocument.Load("tech+solution.xml");
          XElement root = document.Root;
          XNamespace df = root.Name.Namespace;
          foreach (KeyValuePair<int, IOperation> o in oplist)
          {
              Decision d = o.Value.GetDecision();
              if (d == null) continue;
              string id = Convert.ToString(d.GetOperation().GetID());
              bool found = false;
              foreach (XElement product in root.Descendants(df+ "Product"))
              {
                  foreach (XElement part in product.Elements(df + "Part"))
                  {
                      foreach (XElement op in part.Elements(df + "Operation"))
                      {
                          if (op.Attribute("id").Value == id)
                          {
                              found = true;
                              op.Add(new XAttribute("equipment", d.GetEquipment().GetID()));
                              op.Add(new XAttribute("date_begin", d.GetStartTime()));
                              op.Add(new XAttribute("date_end", d.GetEndTime()));
                              op.Attribute("state").Value = "SCHEDULED";
                              XAttribute attr = op.Attribute("equipmentgroup");
                              attr.Remove();
                              break;
                          }
                      }
                      if (found) break;
                      foreach (XElement sp in part.Elements(df + "SubPart"))
                      {
                          foreach (XElement op in sp.Elements(df + "Operation"))
                          {
                              if (op.Attribute("id").Value == id)
                              {
                                  found = true;
                                  op.Add(new XAttribute("equipment", d.GetEquipment().GetID()));
                                  op.Add(new XAttribute("date_begin", d.GetStartTime()));
                                  op.Add(new XAttribute("date_end", d.GetEndTime()));
                                  XAttribute attr = op.Attribute("equipmentgroup");
                                  attr.Remove();
                                  op.Attribute("state").Value = "SCHEDULED";
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
