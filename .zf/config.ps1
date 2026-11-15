<#
Build process for Ais.Net, using the 'ZeroFailed.Build.DotNet' extension to provide the features
needed when building a .NET solution.
#>

$zerofailedExtensions = @(
    @{
        # References the extension from its GitHub repository. The commit installed is recorded in
        # extensions.lock.json; delete the installed copy under .zf/extensions to move to a newer commit.
        Name = "ZeroFailed.Build.DotNet"
        GitRepository = "https://github.com/zerofailed/ZeroFailed.Build.DotNet"
        GitRef = "main"
    }
)

# Load the tasks and process
. ZeroFailed.tasks -ZfPath $here/.zf


#
# Build process control options
#
$SkipInit = $false
$SkipVersion = $false
$SkipBuild = $false
$CleanBuild = $Clean
$SkipTest = $false
$SkipTestReport = $false
$SkipAnalysis = $false
$SkipPackage = $false

#
# Build process configuration
#
$SolutionToBuild = (Resolve-Path (Join-Path $here "./Solutions/Ais.Net.sln")).Path
$ProjectsToPublish = @()
$NuSpecFilesToPackage = @()
$NugetPublishSource = property ZF_NUGET_PUBLISH_SOURCE "$here/_local-nuget-feed"
$IncludeAssembliesInCodeCoverage = "Ais.Net"
$ExcludeAssembliesInCodeCoverage = ""


# Customise the build process

task . FullBuild


#
# Build Process Extensibility Points - uncomment and implement as required
#

# task RunFirst {}
# task PreInit {}
# task PostInit {}
# task PreVersion {}
# task PostVersion {}
# task PreBuild {}
# task PostBuild {}
# task PreTest {}
# task PostTest {}
# task PreTestReport {}
# task PostTestReport {}
# task PreAnalysis {}
# task PostAnalysis {}
# task PrePackage {}
# task PostPackage {}
# task PrePublish {}
# task PostPublish {}
# task RunLast {}
