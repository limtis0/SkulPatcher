# SkulPatcher
A small mod for Skul: The Hero Slayer

<img width="960" alt="Preview" src="https://user-images.githubusercontent.com/45824078/218730883-8b3f610a-b18c-4f58-89ef-800ab9d9a66b.png">

# Setup
- Extract **with replacement** [unstripped Unity 2020.3.34f1 files](https://unity.bepinex.dev/libraries/2020.3.34.zip) and [unstripped CoreLibs](https://unity.bepinex.dev/corlibs/2020.3.34.zip) into `$(SkulFolder)/SkulData/Managed/`, where `$(SkulFolder)` is a location of the game. (If you still are getting "Injection failed: mono_object_get_class() returned NULL" error, then try [these files](https://mega.nz/folder/hnICRaYb#hZKCIBME5rKMemDnHmWd4g) instead)
- Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) or [build it yourself](#build-it-yourself)
- Open the game and load into your save
- Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar

The information for injection is provided below:
| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |


# Build it yourself
Assuming you have .NET runtime and Visual Studio installed
- Complete the first step of a [setup instruction](#setup)
- Clone the project into Visual Studio
- Install "Lib.Harmony", "ILMerge", "ILMerge.MSBuild.Task" packages via NuGet
- Add "Assembly-CSharp" and all "Plugins." "Unity." "UnityEngine." .dll files from `$(SkulFolder)/SkulData/Managed/` to the references
- For each reference, except "0Harmony" set the "Copy Local" property to "False"
- For "0Harmony" reference set the "Copy Local" property to "True"
- Build the solution
