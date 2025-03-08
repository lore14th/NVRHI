// NVRHI Tinfoil build script

using Sharpmake;
using System;
using System.IO;
using System.Collections.Generic;

[Sharpmake.Generate]
public class NVRHIProject : Sharpmake.Project
{
	public NVRHIProject()
	{
		Name = "NVRHI";
		IsFileNameToLower = false;
		IsTargetFileNameToLower = false;

		SourceFiles.Add("NVRHI.Build.cs");
		
		AddTargets(new Target(
			Platform.win64,
			DevEnv.vs2022,
			Optimization.Debug | Optimization.Release)
		);
	}

	[Sharpmake.Configure]
	public void ConfigureAll(Project.Configuration Config, Target Target)
	{
		Config.Output = Configuration.OutputType.Lib;
		Config.ProjectPath = TinfoilSolution.GetProjectFileFolder();
        Config.TargetPath = TinfoilSolution.GetBinaryFolder(Name, Target);
		Config.IntermediatePath = TinfoilSolution.GetIntermediateFolder(Name, Target);

		Config.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);
		Config.Options.Add(Options.Vc.Compiler.Exceptions.EnableWithSEH);
		Config.Options.Add(Options.Vc.Compiler.RTTI.Enable);
        Config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		Config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);

        Config.IncludePaths.Add(@"/include");
        Config.IncludePaths.Add(@"/rtxmu/include");
        Config.IncludePaths.Add(@"/thirdparty/Vulkan-Headers/include");

		Config.Defines.Add("NOMINMAX");

        // Exclude files
        List<string> ExcludedFolders = new List<string>();
		ExcludedFolders.Add("tools/shaderCompiler");
        ExcludedFolders.Add("Vulkan-Headers/tests");

		if (Target.Platform == Platform.win64)
		{
			Config.Defines.Add("VK_USE_PLATFORM_WIN32_KHR");
		}
		else
		{
			ExcludedFolders.Add("d3d11");
			ExcludedFolders.Add("d3d12");
		}

        Config.SourceFilesBuildExcludeRegex.Add(@"\.*\/(" + string.Join("|", ExcludedFolders.ToArray()) + @")\/");
	}
}
