using EntitasVSGenerator.Logic;
using System;
using System.Windows.Controls;

namespace EntitasVSGenerator.ViewViewLogic
{
    /// <summary>
    /// Interaction logic for ConfigureTab.xaml
    /// </summary>
    public partial class ConfigureTab : UserControl
    {
        public ConfigureTabModel Model { get; set; }

        public ConfigureTab(ConfigureTabModel model)
        {
            InitializeComponent();
            Model = model;
            CreateProjectTabs();
        }

        private void CreateProjectTabs()
        {
            foreach (ProjectItem item in Model.ProjectItems)
            {
                var projectWindow = new ProjectWindow(item);
                DockPanel.SetDock(projectWindow, Dock.Top);
                ProjectTabContainer.Children.Add(projectWindow);
            }
        }
    }
}
