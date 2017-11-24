using System;
using System.Collections.Generic;
using System.Linq;

namespace EntitasVSGenerator.Logic
{
    public class ProjectItem
    {
        public delegate void ChangedEvent(ProjectItem item, string oldProjectName);

        public event ChangedEvent Changed;

        private string _projectName;
        private string _previousProjectName;

        public ProjectItem(string projectName, IEnumerable<string> triggers)
        {
            ProjectName = projectName;
            TriggersInternal = triggers.ToList();
            _previousProjectName = ProjectName;
        }

        public IEnumerable<string> Triggers => TriggersInternal;
        public bool IsDeleted { get; set; }
        private List<string> TriggersInternal { get; }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _previousProjectName = _projectName;
                OnChanged();
                _projectName = value;
            }
        }

        public void AddTrigger(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            TriggersInternal.Add(path);
            OnChanged();
        }

        public void RemoveTrigger(int index)
        {
            TriggersInternal.RemoveAt(index);
            OnChanged();
        }

        protected void OnChanged()
        {
            Changed?.Invoke(this, _previousProjectName);
        }
    }
}
