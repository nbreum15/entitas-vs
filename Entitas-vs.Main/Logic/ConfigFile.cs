using System;
using System.Xml;
using System.Linq;
using EntitasVSGenerator.Extensions;
using System.IO;
using System.Collections.Generic;

namespace EntitasVSGenerator.Logic
{
    class ConfigFile
    {
        private const string TriggerElement = "Trigger";
        private const string PathAttribute = "Path";
        private const string ProjectElement = "Project";
        private const string NameAttribute = "Name";
        private const string GeneratorPathAttribute = "GeneratorPath";
        private const string RootElement = "Settings";
        private readonly string _defaultContent = 
$@"<{RootElement}>
</{RootElement}>
";
        private readonly XmlDocument _document;
        public XmlNode SettingsNode => _document.FirstChild; // root node

        public ConfigFile(string directory)
        {
            SettingsPath = PathUtil.GetSettingsPath(directory);
            _document = new XmlDocument();
        }
        
        public string SettingsPath { get; }
        public string GeneratorPath
        {
            get => SettingsNode.Attributes[GeneratorPathAttribute]?.Value;
            set
            {
                if (SettingsNode.Attributes[GeneratorPathAttribute] == null)
                {
                    XmlAttribute genPathAttribute = _document.CreateAttribute(GeneratorPathAttribute);
                    SettingsNode.Attributes.Append(genPathAttribute);
                }
                SettingsNode.Attributes[GeneratorPathAttribute].Value = value;
            }
        }

        public void Refresh(string projectName, string[] triggers = null) 
        {
            string xpathProjectNode = $"{RootElement}/{ProjectElement}[@{NameAttribute}=\"{projectName}\"]";
            XmlNode projectElement = _document.SelectSingleNode(xpathProjectNode);

            if(projectElement == null) // not found in xml i.e. new project created in settings ui
            {
                projectElement = _document.CreateElement(ProjectElement);
                SettingsNode.AppendChild(projectElement);
                XmlAttribute nameAttribute = _document.CreateAttribute(NameAttribute);
                projectElement.Attributes.Append(nameAttribute);
            }

            projectElement.Attributes[NameAttribute].Value = projectName;
            projectElement.InnerText = "";

            if (triggers != null)
            {
                foreach (string trigger in triggers)
                {
                    XmlElement triggerElement = _document.CreateElement(TriggerElement);
                    XmlAttribute pathAttribute = _document.CreateAttribute(PathAttribute);
                    pathAttribute.Value = trigger;
                    triggerElement.Attributes.Append(pathAttribute);
                    projectElement.AppendChild(triggerElement);
                }
            }
        }

        public string[] GetTriggers(string projectName)
        {
            string xpath = $"{RootElement}/{ProjectElement}[@{NameAttribute}=\"{projectName}\"]/{TriggerElement}";
            IEnumerable<string> triggers = _document.SelectNodes(xpath)
                .Cast<XmlNode>()
                .Select(trigger => trigger.Attributes[PathAttribute].Value);
            return triggers.ToArray();
        }

        public string[] GetProjectNames()
        {
            string xpath = $"{RootElement}/{ProjectElement}";
            string[] names = _document.SelectNodes(xpath)
                .Cast<XmlNode>()
                .Select(node => node.Attributes[NameAttribute].Value)
                .ToArray();
            return names;
        }

        public void Load()
        {
            LoadXmlFile();
        }

        private void LoadXmlFile()
        {
            if (!File.Exists(SettingsPath))
            {
                _document.LoadXml(_defaultContent);
            }
            else
            {
                _document.Load(SettingsPath);
            }
        }

        public void Save()
        {
            _document.Save(SettingsPath);
        }
    }
}
