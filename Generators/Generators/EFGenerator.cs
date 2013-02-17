using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using ObliteracyDotNet.Generators.Structures;
using ObliteracyDotNet.Generators.Templates.ConfigFiles;

namespace ObliteracyDotNet.Generators
{
    public class EFGenerator
    {
        public void Exeucte(PatternBuilderConfig options)
        {
            EdmGenCommandLine cmdline = new EdmGenCommandLine();
            cmdline.ContainerName = options.ContextContainerName;
            cmdline.DatabaseName = options.DBInstance;
            cmdline.DatabaseServer = options.DBServer;
            cmdline.NameSpace = options.ProjectNamespace;
            cmdline.ProjectName = options.ProjectName;
            // Test the call
            Environment.CurrentDirectory = options.SaveDirectory;
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) +
                @"\Microsoft.NET\framework\v4.0.30319\EdmGen.exe";
            info.Arguments = cmdline.TransformText();
            info.RedirectStandardOutput = true;
            //info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            //info.CreateNoWindow = true;
            Process.Start(info);

            EdmxFile edmx = ParseEdmx(cmdline);
            WriteEdmxFile(cmdline, edmx);
            WriteAppConfig(cmdline);
        }

        private EdmxFile ParseEdmx(EdmGenCommandLine cmdline)
        {
            
            EdmxFile edmx = new EdmxFile();
            XmlDocument doc = new XmlDocument();

            PollForCsdl(cmdline);

            doc.Load(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".csdl");
            edmx.CSDLText = doc.SelectNodes("/")[0].ChildNodes[1].OuterXml;
            doc.Load(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".ssdl");
            edmx.SSDLText = doc.SelectNodes("/")[0].ChildNodes[1].OuterXml;
            doc.Load(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".msl");
            // Strip out the attributes that the designer gets all pissed about
            XmlNodeList list = doc.SelectNodes("/");
            foreach (XmlNode node in list[0].ChildNodes[1].ChildNodes[0].ChildNodes)
            {
                XmlAttribute es = node.Attributes["StoreEntitySet"];
                XmlAttribute name = node.Attributes["Name"];
                XmlAttribute type = node.Attributes["TypeName"];

                XmlNode typeNode = doc.CreateNode(XmlNodeType.Element, "EntityTypeMapping", "");
                XmlNode mapnode = doc.CreateNode(XmlNodeType.Element, "MappingFragment", "");
                mapnode.Attributes.Append(es);
                mapnode.InnerXml = node.InnerXml;
                foreach (XmlNode cNode in mapnode.ChildNodes)
                {
                    cNode.Attributes.Remove(cNode.Attributes["xmlns"]);
                }
                type.Value = string.Format("{0}.{1}", cmdline.ProjectName, es.Value);
                node.RemoveAll();
                node.Attributes.Append(name);
                typeNode.RemoveAll();
                typeNode.Attributes.Append(type);
                typeNode.AppendChild(mapnode);
                node.AppendChild(typeNode);
            }

            edmx.MSLText = doc.SelectNodes("/")[0].ChildNodes[1].OuterXml.Replace(" xmlns=\"\"","");
            return edmx;
        }

        private static void PollForCsdl(EdmGenCommandLine cmdline)
        {
            int timeout = 100000;
            int ticker = 0;
            do
            {
                if (File.Exists(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".csdl"))
                    break;
                else
                {
                    Thread.Sleep(1000);
                    ticker += 1000;
                }
            } while (ticker < timeout);

            if (!(File.Exists(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".csdl")))
                throw new InvalidOperationException("The CSDL file could not be located.  EDMX generation cannot continue.");
        }

        private void WriteAppConfig(EdmGenCommandLine cmdline)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\app.config", false))
            {
                EdmxAppConfig config = new EdmxAppConfig();
                config.DatabaseName = cmdline.DatabaseName;
                config.DatabaseServer = cmdline.DatabaseServer;
                config.ProjectName = cmdline.ProjectName;
                sw.Write(config.TransformText());
                sw.Close();
            }
        }

        private void WriteEdmxFile(EdmGenCommandLine cmdline, EdmxFile edmx)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\" + cmdline.ProjectName + ".edmx", false))
            {
                sw.Write(edmx.TransformText());
                sw.Close();
            }
        }
    }
}
