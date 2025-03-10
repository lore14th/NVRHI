// NVRHI Tinfoil build script

using Sharpmake;
using System;
using System.IO;
using System.Collections.Generic;

[Sharpmake.Generate]
public class NVRHI : TinfoilProjectBase
{
	public NVRHI()
	{
		Name = "NVRHI";
		SourceFiles.Add("NVRHI.Build.cs");
	}

	[Sharpmake.Configure]
	public void ConfigureAll(Project.Configuration config, Target target)
	{
		config.Output = Configuration.OutputType.Lib;

		config.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);
		config.Options.Add(Options.Vc.Compiler.Exceptions.EnableWithSEH);
		config.Options.Add(Options.Vc.Compiler.RTTI.Enable);
		config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);

		config.IncludePaths.Add(@"/include");
		config.IncludePaths.Add(@"/rtxmu/include");
		config.IncludePaths.Add(@"/thirdparty/Vulkan-Headers/include");

		config.Defines.Add("NOMINMAX");

		// Exclude files
		ExcludeFolder(config, target, "shaderCompiler");
		ExcludeFolder(config, target, "tests");

		if (target.Platform == Platform.win64)
		{
			config.Defines.Add("VK_USE_PLATFORM_WIN32_KHR");
		}
		else
		{
			ExcludeFolder(config, target, "d3d11");
			ExcludeFolder(config, target, "d3d12");
		}
	}
}
