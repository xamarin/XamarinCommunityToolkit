#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "Default"));
var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

var libraries = new Dictionary<string, string> {
 	{ "./src/Converters.sln", "Any" },
	{ "./src/Controls.sln", "Any" },
	{ "./src/Behaviors.sln", "Any" },
	{ "./src/Effects.sln", "Any" },
};

var nugets = new List<string> {
 	"./nuget/Converters.nuspec",
	 "./nuget/Controls.nuspec",
	 "./nuget/Behaviors.nuspec",
	 "./nuget/Effects.nuspec"
};


var BuildAction = new Action<Dictionary<string, string>> (solutions =>
{

	foreach (var sln in solutions) 
    {

		// If the platform is Any build regardless
		//  If the platform is Win and we are running on windows build
		//  If the platform is Mac and we are running on Mac, build
		if ((sln.Value == "Any")
				|| (sln.Value == "Win" && IsRunningOnWindows ())
				|| (sln.Value == "Mac" && IsRunningOnUnix ())) 
        	{
			
			// Bit of a hack to use nuget3 to restore packages for project.json
			if (IsRunningOnWindows ()) 
			{

				Information ("RunningOn: {0}", "Windows");

				NuGetRestore (sln.Key, new NuGetRestoreSettings
				{
					ToolPath = "./tools/nuget3.exe"
				});

				// Windows Phone / Universal projects require not using the amd64 msbuild
				MSBuild (sln.Key, c => 
				 { 
					c.Configuration = "Release";
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
				});
			} 
            		else 
            		{
                		// Mac is easy ;)
				NuGetRestore (sln.Key);

				DotNetBuild (sln.Key, c => c.Configuration = "Release");
			}
		}
	}
});

Task("Libraries").Does(()=>
{
    BuildAction(libraries);
});

Task ("NuGet")
	.IsDependentOn ("Libraries")
	.Does (() =>
{
		if(!DirectoryExists("./Build/nuget/"))
			CreateDirectory("./Build/nuget");

		foreach(var nuget in nugets)
		{
			NuGetPack (nuget, new NuGetPackSettings { 
					Version = version,
					Verbosity = NuGetVerbosity.Detailed,
					OutputDirectory = "./Build/nuget/",
					BasePath = "./",
				});	
		}
});


//Build nugets and libraries
Task ("Default").IsDependentOn("NuGet");


Task ("Clean").Does (() => 
{
	CleanDirectories ("./Build/");

	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});


RunTarget (TARGET);
