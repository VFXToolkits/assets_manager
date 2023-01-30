using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace assets_manager.Views.template;

public class ProjectItem : TemplatedControl
{
    public static readonly StyledProperty<string> ProjectImageSourceProperty = AvaloniaProperty.Register<ProjectItem, string>(nameof(ProjectImageSource), "Assets/assets_logo.png");

    public string ProjectImageSource { 
        get => (string)GetValue(ProjectImageSourceProperty);
        set => SetValue(ProjectImageSourceProperty, value);
    }

    public static readonly StyledProperty<string> ProjectNameProperty = AvaloniaProperty.Register<ProjectItem, string>(nameof(ProjectName), "project_name");

    public string ProjectName
    {
        get => (string)GetValue(ProjectNameProperty);
        set => SetValue(ProjectNameProperty, value);
    }

}
