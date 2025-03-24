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
	public void ConfigureAll(Project.Configuration config, TinfoilTarget target)
	{
		config.Output = Configuration.OutputType.Lib;
		config.Defines.Add("NVRHI_SHARED_LIBRARY_BUILD=0");

		config.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);
		config.Options.Add(Options.Vc.Compiler.Exceptions.EnableWithSEH);
		config.Options.Add(Options.Vc.Compiler.RTTI.Enable);
		config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);

		config.IncludePaths.Add(@"/include");
		config.IncludePaths.Add(@"/rtxmu/include");
		config.IncludePaths.Add(@"/thirdparty/Vulkan-Headers/include");

		config.Defines.Add("NOMINMAX"); // windows only
		//config.Defines.Add("NVRHI_WITH_AFTERMATH"); // TODO: 

		// Exclude files
		ExcludeFolder(config, target, "shaderCompiler");
		ExcludeFolder(config, target, "tests");


		List<ERenderingAPI> availableAPIs = base.GetAvailableRenderingAPIs(target);
		if (!availableAPIs.Contains(ERenderingAPI.NV_D3D11))
		{
			ExcludeFolder(config, target, "d3d11");
		}
		if (!availableAPIs.Contains(ERenderingAPI.NV_D3D12))
		{
			ExcludeFolder(config, target, "d3d12");
		}
		if (availableAPIs.Contains(ERenderingAPI.NV_Vulkan))
		{
			if (target.Platform == Platform.win64)
			{
				config.Defines.Add("VK_USE_PLATFORM_WIN32_KHR");
			}

			config.LibraryPaths.Add(Environment.GetEnvironmentVariable("VULKAN_SDK") + @"/Lib");
			config.LibraryFiles.Add("vulkan-1.lib");
		}
		else
		{
			ExcludeFolder(config, target, "vulkan");
		}
	}
}
