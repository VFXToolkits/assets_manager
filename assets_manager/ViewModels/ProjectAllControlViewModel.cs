using assets_manager.Models;
using System.Collections.ObjectModel;


namespace assets_manager.ViewModels
{
    public class ProjectAllControlViewModel
    {
        public ObservableCollection<ProjectItemModel> ProjectModelItemList { get; } = new();

        public ProjectAllControlViewModel() {
            ProjectModelItemList.Add(new ProjectItemModel("aaa", "test", "aaabbb"));
        }
    }
}
