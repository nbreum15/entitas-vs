using System.Collections.Generic;
using System.Linq;

namespace EntitasVSGenerator.Logic
{
    public class ProjectViewModel
    {
        public delegate void ChangedEvent(ProjectViewModel viewModel);

        public event ChangedEvent Changed;

        private string _projectName;

        public ProjectViewModel(string projectName, IEnumerable<string> triggers, string directory)
        {
            ProjectName = projectName;
            Directory = directory;
            TriggersInternal = triggers.ToList();
        }

        public IEnumerable<string> Triggers => TriggersInternal;
        public bool IsDeleted { get; set; }
        private List<string> TriggersInternal { get; }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnChanged();
            }
        }

        public string Directory { get; }

        public void AddTrigger(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            TriggersInternal.Add(path.ToLower());
            OnChanged();
        }

        public void RemoveTrigger(int index)
        {
            TriggersInternal.RemoveAt(index);
            OnChanged();
        }

        protected void OnChanged()
        {
            Changed?.Invoke(this);
        }
    }
}
