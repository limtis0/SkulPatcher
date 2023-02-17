# SkulPatcher
An all-in-one mod for Skul: The Hero Slayer
<img width="960" alt="Untitled" src="https://user-images.githubusercontent.com/45824078/219701252-5056858f-4361-4e11-8a62-a9317a71830c.png">


# Setup
- Extract **with replacement** [unstripped Unity 2020.3.34f1 files](https://unity.bepinex.dev/libraries/2020.3.34.zip) and [unstripped CoreLibs files](https://unity.bepinex.dev/corlibs/2020.3.34.zip) into `$(SkulFolder)/SkulData/Managed/`, where `$(SkulFolder)` is a location of the game. (If you still are getting "Injection failed: mono_object_get_class() returned NULL" error, then try [these files](https://mega.nz/folder/hnICRaYb#hZKCIBME5rKMemDnHmWd4g) instead)
- Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) or [build it yourself](#build-it-yourself)
- Open the game and load into your save
- Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar

The information for injection is provided below:
| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |


# Build it yourself
Assuming you have .NET Framework 4.8.1 and Visual Studio installed
- Complete the first step of a [setup instruction](#setup)
- Clone the project into Visual Studio
- Install "Lib.Harmony", "ILMerge", "ILMerge.MSBuild.Task" packages via NuGet
- Add "Assembly-CSharp" and all "Plugins..." "Unity..." "UnityEngine..." .dll files from `$(SkulFolder)/SkulData/Managed/` to the references
- For each reference, except "0Harmony" set the "Copy Local" property to "False"
- For "0Harmony" reference set the "Copy Local" property to "True"
- Build the solution


# Compatibility
- [Skul: The Hero Slayer v1.7.0+](https://store.steampowered.com/news/app/1147560/view/5283318909430116714) (Built on [Unity 2020.3.34f1](https://unity.com/releases/editor/whats-new/2020.3.34); Should work for future releases if nothing drastically changes)
- [Harmony v2.2.2](https://github.com/pardeike/Harmony/releases/tag/v2.2.2.0)
