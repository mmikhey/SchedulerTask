using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SchedulerTask
{
    class writer
    {
        public void WriteData(List<Decision> dlist)
        {

          XmlTextWriter textWritter = new XmlTextWriter("tech+solution.xml", Encoding.UTF8);
          textWritter.WriteStartDocument();
          textWritter.WriteEndElement();
          textWritter.WriteStartElement("InformationModel");
          XmlDocument document = new XmlDocument();
          document.Load("tech+solution.xml");
          foreach (Decision d in dlist)
          {
              XmlNode element = document.CreateElement("Operation");
              document.DocumentElement.AppendChild(element); 
              XmlAttribute attribute = document.CreateAttribute("id");
              attribute.Value = Convert.ToString(d.GetOperation().GetID()); 
              element.Attributes.Append(attribute);
              attribute = document.CreateAttribute("name");
              attribute.Value = Convert.ToString(d.GetOperation().GetName());
              element.Attributes.Append(attribute);
              attribute = document.CreateAttribute("state");
              attribute.Value = "SCHEDULED";
              element.Attributes.Append(attribute);
              attribute = document.CreateAttribute("date_begin");
              attribute.Value = Convert.ToString(d.GetStartTime());
              element.Attributes.Append(attribute);
              attribute = document.CreateAttribute("date_end");
              attribute.Value = Convert.ToString(d.GetEndTime());
              element.Attributes.Append(attribute); 
              attribute = document.CreateAttribute("equipment");
              attribute.Value = Convert.ToString(d.GetEquipment().GetID());
              element.Attributes.Append(attribute); 
              attribute = document.CreateAttribute("duration");
              attribute.Value = Convert.ToString(d.GetOperation().GetDuration());
              element.Attributes.Append(attribute); 
          }
          document.Save("tech+solution.xml");

        }


    }
}
