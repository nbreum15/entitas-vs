using System.Xml;
using System.Linq;
using EntitasVSGenerator.Extensions;
using System.IO;

namespace EntitasVSGenerator.Logic
{
    public class ConfigFile
    {
        private const string TriggerElement = "Trigger";
        private const string PathAttribute = "Path";
        private const string ProjectElement = "Project";
        private const string NameAttribute = "Name";
        private const string RootElement = "Settings";
        private string DefaultContent = 
$@"
<{RootElement}>
</{RootElement}>
";
        private XmlDocument _document;

        public ConfigFile(string projectDirectory) : this(projectDirectory, PathUtil.GetSettingsPath(projectDirectory))
        { }

        public ConfigFile(string projectDirectory, string settingsPath)
        {
            ProjectDirectory = projectDirectory;
            SettingsPath = settingsPath;
            _document = new XmlDocument();
            LoadXmlFile();
        }
        
        public string ProjectDirectory { get; }
        public string SettingsPath { get; }

        public void AddTrigger(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            XmlElement item = _document.CreateElement(TriggerElement);
            item.SetAttribute(PathAttribute, path);
            XmlNode triggers = _document.SelectSingleNode($"{RootElement}/{ProjectElement}");
            triggers.AppendChild(item);
            Save();
        }

        public void ChangeProject(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            XmlElement item = _document.CreateElement(ProjectElement);
            item.SetAttribute(NameAttribute, name);
            XmlNode project = _document.SelectSingleNode(RootElement);
            project.AppendChild(item);
            Save();
        }

        public string[] GetTriggers(string projectName)
        {
            string xpath = $"{RootElement}/{ProjectElement}[@{NameAttribute}=\"{projectName}\"]/{TriggerElement}";
            string[] triggers = _document.SelectNodes(xpath)
                .Cast<XmlNode>()
                .Select(trigger => trigger.Attributes[PathAttribute].Value)
                .ToArray();
            return triggers;
        }

        private void LoadXmlFile()
        {
            if (!File.Exists(SettingsPath))
                File.WriteAllText(SettingsPath, DefaultContent);
            else
                _document.Load(SettingsPath);
        }

        private void Save()
        {
            _document.Save(SettingsPath);
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
    }
}
