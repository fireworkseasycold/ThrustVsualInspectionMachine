using MahApps.Metro.Controls;

using Prism.Regions;

using ThrustVsualInspectionMachineProject.Constants;

namespace ThrustVsualInspectionMachineProject.Views;

public partial class ShellWindow : MetroWindow
{
    public ShellWindow(IRegionManager regionManager)
    {
        InitializeComponent();
        RegionManager.SetRegionName(shellContentControl, Regions.Main);
        RegionManager.SetRegionManager(shellContentControl, regionManager);
    }
}
