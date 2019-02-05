#addin nuget:?package=Cake.VersionReader

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/CleanCode/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/CleanCode.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetBuild("./src/CleanCode.sln", settings => settings.SetConfiguration(configuration));
});

Task("Nuget-Pack")
    .IsDependentOn("Build")
    .Does(() => 
{
    var version = GetFullVersionNumber($"./src/CleanCode/bin/{configuration}/net461/CleanCode.dll");
    NuGetPack("./src/CleanCode/CleanCode.nuspec", new NuGetPackSettings {
        OutputDirectory = "distribution",
        Version = version
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Nuget-Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);