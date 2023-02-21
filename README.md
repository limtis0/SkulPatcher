# SkulPatcher
An all-in-one mod for Skul: The Hero Slayer
<img width="960" alt="Untitled" src="https://user-images.githubusercontent.com/45824078/219701252-5056858f-4361-4e11-8a62-a9317a71830c.png">

### Setup premise
`$(SkulFolder)` is a location of the game

# Setup (v2.0f+)
1. Extract into `$(SkulFolder)/SkulData/Managed/2020.3.34`
    * [unstripped Unity 2020.3.34 files](https://unity.bepinex.dev/libraries/2020.3.34.zip)
    * [unstripped CoreLibs 2020.3.34 files](https://unity.bepinex.dev/corlibs/2020.3.34.zip)
2. Extract into `$(SkulFolder)`
    * [BepInEx v5.4.21](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21)
3. Run the game once, so all of the BepInEx files will appear
4. In `$(SkulFolder)/doorstop.ini`
    * Set `dllSearchPathOverride=` to `dllSearchPathOverride=2020.3.34`
5. In `$(SkulFolder)/BepInEx/config/BepInEx.cfg`
    * Set `Assembly =` to `Assembly = Assembly-CSharp.dll`
    * Set `Type =` to `Type = Main`
    * Set `Method =` to `Method = StartGame`
6. Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) and place it to `$(SkulFolder)/BepInEx/plugins`

# Setup (<v2.0f)
1. Extract **with replacement** into `$(SkulFolder)/Managed`
    * [unstripped Unity 2020.3.34 files](https://unity.bepinex.dev/libraries/2020.3.34.zip)
    * [unstripped CoreLibs files 2020.3.34](https://unity.bepinex.dev/corlibs/2020.3.34.zip)
2. Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) or [build it yourself](#build-it-yourself)
3. Open the game and load into your save
4. Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar

The information for injection is provided below:
| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |


# Build it yourself (<v2.0f)
Assuming you have .NET Framework 4.7.2 and Visual Studio installed
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
