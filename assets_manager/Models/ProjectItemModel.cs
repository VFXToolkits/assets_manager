using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assets_manager.Models
{
    public class ProjectItemModel: ReactiveObject
    {
        public string ImageUrl { get; set; }
        public string ProjectName { get; set; }
        public string Describe { get; set; }

        public ProjectItemModel(string images_url, string title, string describe)
        {
            ImageUrl = images_url;
            ProjectName = title;
            Describe = describe;
        }
    }
}
