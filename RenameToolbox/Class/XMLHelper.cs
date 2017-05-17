using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RenameToolbox.Class
{
    public class XMLHelper
    {
        public XMLHelper()
        {

        }
        public bool SaveProfileXML(List<Rule> list, string filename)
        {
            try
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Rule");
                    foreach (Rule ru in list)
                    {
                        writer.WriteStartElement("Rule");
                        writer.WriteAttributeString("Enable", ru.Enable ? "True" : "False");
                        writer.WriteAttributeString("Target", string.IsNullOrEmpty(ru.Target) ? "" : ru.Target);
                        writer.WriteAttributeString("Mode", string.IsNullOrEmpty(ru.Mode) ? "" : ru.Mode);
                        writer.WriteAttributeString("p1", string.IsNullOrEmpty(ru.p1) ? "" : ru.p1);
                        writer.WriteAttributeString("p2", string.IsNullOrEmpty(ru.p2) ? "" : ru.p2);
                        writer.WriteAttributeString("Sub", string.IsNullOrEmpty(ru.Sub) ? "" : ru.Sub);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ObservableCollection<Rule> LoadProfileXML(string filename)
        {
            try
            {
                XDocument xdoc = XDocument.Load(filename);
                ObservableCollection<Rule> rulist = new ObservableCollection<Rule>((
                from e in xdoc.Root.Descendants("Rule")
                select new Rule
                {
                    Enable = Convert.ToBoolean(e.Attributes("Enable").Single().Value),
                    Target = e.Attributes("Target").Single().Value,
                    Mode = e.Attributes("Mode").Single().Value,
                    p1 = e.Attributes("p1").Single().Value,
                    p2 = e.Attributes("p2").Single().Value,
                    Sub = e.Attributes("Sub").Single().Value
                }).ToList());
                return rulist;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
