# ThrustVsualInspectionMachine

超声波推力机视觉检测系统（WPF 桌面应用）

本项目是基于 .NET 8 的 WPF 应用程序，采用 Prism (Unity) 实现 MVVM 架构与模块化导航，UI 使用 MahApps.Metro 主题。项目内置应用配置（Microsoft.Extensions.Configuration）、日志（NLog 预置包）、数据访问（SqlSugar 预置包）等基础能力，适合进一步扩展为生产级的视觉检测/工控上位机软件。

## 功能特性
- 基于 Prism 的 MVVM 架构与依赖注入（Unity 容器）
- 区域（Region）与页面导航（RequestNavigate）
- MahApps.Metro 样式与主题（默认 Light.Blue）
- appsettings.json 配置加载与强类型绑定
- 预置 SqlSugar 数据访问与 NLog 日志依赖（可按需启用）
- 模块清晰、可扩展、符合 SOLID 原则

## 架构设计
解决方案包含两部分：
- ThrustVsualInspectionMachineProject（WPF UI，net8.0-windows10.0.19041.0）
- ThrustVsualInspectionMachineProject.Core（领域模型，netstandard2.0）

核心架构要点：
- 启动与 IOC：App 继承 PrismApplication，在 RegisterTypes 中完成依赖注册、导航注册及配置绑定。
- 视图-视模绑定：ShellWindow、MainPage 分别绑定 ShellViewModel、MainViewModel。
- 区域与导航：ShellWindow 内容区注册为 MainRegion，ShellViewModel 在 Loaded 时导航到 Main 页面。

## 目录结构（节选）

```
ThrustVsualInspectionMachineProject/
├── Constants/                  # 常量定义
│   ├── PageKeys.cs            # 页面键常量
│   └── Regions.cs             # 区域常量
├── Models/                    # 数据模型
│   ├── AppConfig.cs           # 应用配置模型
│   └── SampleEntity.cs        # 示例实体模型
├── Services/                  # 服务层
│   ├── ISampleRepository.cs   # 示例仓储接口
│   └── SampleRepository.cs    # 示例仓储实现
├── ViewModels/                # 视图模型
│   ├── MainViewModel.cs      # 主页面视图模型
│   └── ShellViewModel.cs     # 主窗口视图模型
├── Views/                    # 视图
│   ├── MainPage.xaml         # 主页面
│   ├── MainPage.xaml.cs      # 主页面代码隐藏
│   ├── ShellWindow.xaml      # 主窗口
│   └── ShellWindow.xaml.cs   # 主窗口代码隐藏
├── Styles/                   # 样式资源
│   ├── _FontSizes.xaml       # 字体大小定义
│   ├── _Thickness.xaml       # 边距定义
│   ├── MetroWindow.xaml      # 窗口样式
│   ├── TextBlock.xaml        # 文本块样式
│   └── UserControl.xaml      # 用户控件样式
├── Properties/               # 资源文件
│   ├── Resources.Designer.cs # 资源设计器
│   └── Resources.resx        # 资源文件
├── Helpers/                  # 辅助类
│   └── IRegionNavigationServiceExtensions.cs # 区域导航扩展
├── 设计/                     # 设计文档和参考图片
│   ├── BusinessAnalysis.md    # 业务需求分析文档
│   ├── DesignProject.md      # 系统设计说明文档
│   └── 参考pic/              # 参考图片
├── appsettings.json          # 应用配置
├── App.xaml                  # 应用程序资源
├── App.xaml.cs               # 应用程序入口
└── ThrustVsualInspectionMachineProject.csproj # 项目文件

ThrustVsualInspectionMachineProject.Core/
├── Models/                   # 核心数据模型（空目录）
├── readme.txt                # 说明文件
└── ThrustVsualInspectionMachineProject.Core.csproj # 核心项目文件
```

## 技术栈与依赖
- .NET 8 WPF（TargetFramework: net8.0-windows10.0.19041.0）
- Prism.Unity 9.x（MVVM、DI、导航）
- MahApps.Metro 2.x（现代化 WPF UI 主题）
- Microsoft.Extensions.Configuration 9.x（Json/CommandLine/Binder）
- SqlSugarCore 5.x（ORM，预置依赖）
- NLog 6.x（日志，预置依赖）

见 ThrustVsualInspectionMachineProject.csproj 中的 PackageReference。

## 运行环境
- 操作系统：Windows 10 2004 (19041) 或更高版本
- 开发工具：Visual Studio 2022 17.6+ 或 JetBrains Rider（建议 VS 17.11+）
- .NET SDK：8.0+

## 快速开始
1. 克隆仓库并用 Visual Studio 打开解决方案 ThrustVsualInspectionMachine.sln。
2. 设为启动项目：ThrustVsualInspectionMachineProject。
3. 直接 F5 调试运行，或使用命令：
   ```powershell
   dotnet build ThrustVsualInspectionMachine/ThrustVsualInspectionMachine.sln -c Debug
   dotnet run --project ThrustVsualInspectionMachine/ThrustVsualInspectionMachine/ThrustVsualInspectionMachineProject/ThrustVsualInspectionMachineProject.csproj
   ```

## 配置说明（appsettings.json）
示例结构：
```json
{
  "AppConfig": {
    "PrivacyStatement": "..."
  }
}
```
- App 启动时通过 Microsoft.Extensions.Configuration 加载 appsettings.json 与命令行参数，并绑定到 Models/AppConfig。
- 在 RegisterTypes 中以单例方式注入 IConfiguration 与 AppConfig，便于全局访问。

## MVVM 与导航
- Region 名称：MainRegion（Constants/Regions.cs）
- 页面键：Main（Constants/PageKeys.cs）
- 导航注册：
  - MainPage <-> MainViewModel 以键 Main 注册（App.RegisterTypes）。
  - ShellWindow <-> ShellViewModel 也注册到导航服务（便于扩展）。
- 导航流程：ShellViewModel 在 LoadedCommand 中获取 IRegionNavigationService，并 RequestNavigate 到 Main。
- 返回导航：ShellViewModel 提供 GoBackCommand（基于 NavigationJournal）。

## UI 与主题
- 应用级资源在 App.xaml 中合并，包括 Styles 目录和 MahApps.Metro 主题资源。
- 默认主题使用 Light.Blue，如需切换请替换相应主题资源字典。
- 常用样式集中于 Styles 文件夹（如 MetroWindow.xaml、TextBlock.xaml、UserControl.xaml、_FontSizes.xaml、_Thickness.xaml）。

## 数据访问（SqlSugar）
项目已预置 SqlSugarCore 依赖，可按需初始化与注入：
```csharp
// 示例：创建 SqlSugarClient（请根据实际数据库修改连接字符串与配置）
var db = new SqlSugar.SqlSugarClient(new ConnectionConfig
{
    ConnectionString = "Server=...;Database=...;User Id=...;Password=...;",
    DbType = DbType.SqlServer,
    IsAutoCloseConnection = true
});
```
建议将数据访问封装在独立 Service 层，通过 Prism 容器注入到 ViewModel。

## 日志（NLog）
- 已包含 NLog 包，可在项目根添加 NLog.config 并在启动时进行加载。
- 建议封装 ILoggerService 适配 NLog，并通过依赖注入提供日志能力。

## 国际化（I18N）
- WPF 默认语言为 en-US，项目内使用 Properties/Resources.resx 提供多语言资源键。
- 如需完整本地化，请添加对应的 .resx 资源文件（例如 Resources.zh-CN.resx）并在 UI 绑定 StaticResource。

## 代码规范与最佳实践
- 架构：严格遵循 MVVM，View 不直接持有业务逻辑。
- 异步：使用 async/await，避免阻塞 UI；涉及 UI 更新时使用 Dispatcher。
- 依赖注入：通过 Prism 容器（Unity）注册与解析，降低耦合。
- 可测试性：ViewModel 仅依赖抽象接口；避免在构造函数中做重活。
- 命名：采用 PascalCase/CamelCase，遵循 .NET 命名规范；资源键统一管理。
- 错误处理：捕获并记录异常，在 App.OnDispatcherUnhandledException 做兜底处理。

## 测试与质量保障
- 建议使用 xUnit + Moq 或 NSubstitute 编写 ViewModel/Service 层单元测试，覆盖率≥80%。
- 可扩展 CI：GitHub Actions 或 Azure DevOps 执行构建、单元测试、代码风格检查（如 dotnet format）。

## 常见问题（FAQ）
- 启动空白或未导航：确认 ShellViewModel.Loaded 已触发且 Regions.Main 已正确注册。
- 样式未生效：检查 App.xaml ResourceDictionary 合并顺序与 MahApps 主题字典。
- 运行时异常：查看 NLog 输出或调试窗口；检查 appsettings.json 配置路径。
- DPI/缩放：确保使用矢量图标与自适应布局，必要时启用 Per-Monitor DPI Aware。

## 许可证
- 本仓库包含 LICENSE.txt，请根据其中条款使用与分发。

---
如果你计划将此骨架扩展为完整的超声波推力机视觉检测系统，建议新增模块：
- 设备通讯与实时数据采集（串口/以太网/现场总线）
- 图像处理与检测算法模块（可封装为独立服务）
- 任务流程引擎（配方、工艺参数、工步状态机）
- 数据存储与报表（SqlSugar + SQLite/SQL Server）
- 权限与用户体系、操作日志、系统配置中心