using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[GitHubActions("release", GitHubActionsImage.WindowsLatest, AutoGenerate = true,
    ImportSecrets = new[] { nameof(NugetApiKey) },
    OnPushBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(Publish) }, PublishArtifacts = true)]
class Build : NukeBuild
{
    public const string Version = "1.1.0";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitRepository] readonly GitRepository GitRepository;

    [Solution] readonly Solution Solution;
    [Parameter] [Secret] string NugetApiKey;

    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Parameter] string NugetSymbolsUrl = "https://nuget.smbsrc.net/";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var project in Solution.AllProjects.Where(_ =>
                !_.Name.Contains("Example") && !_.Name.Contains("Build")))
                DotNetPack(_ => _.SetProject(project)
                    .SetAssemblyVersion(Version)
                    .SetFileVersion(Version)
                    .SetVersion(Version)
                    .SetInformationalVersion(Version)
                    .SetNoBuild(true)
                    .SetOutputDirectory(ArtifactsDirectory)
                    .SetIncludeSymbols(true)
                    .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                    .SetAuthors("Mikhail Kozlov")
                    .SetDescription(
                        "Commandify is a lightweight and easy-to-use library for creating and matching commands for C#")
                    .SetPackageProjectUrl("https://github.com/lentrodev/Commandify")
                    .SetRepositoryUrl("https://github.com/lentrodev/Commandify")
                    .SetRepositoryType("git")
                    .SetPackageTags("C#", "command", "processing", "library")
                );
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            ArtifactsDirectory.GlobFiles("*.nupkg")
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSymbolSource(NugetSymbolsUrl)
                        .SetSkipDuplicate(true)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });

    public static int Main() => Execute<Build>(x => x.Compile);
}