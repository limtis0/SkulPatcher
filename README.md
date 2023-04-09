# SkulPatcher
A mod for "Skul: The Hero Slayer" that lets you create your own playing experience
<img width="960" alt="Untitled" src="https://user-images.githubusercontent.com/45824078/229655741-a45dc6e8-cd80-40c4-976d-8bb4f255c733.png">

### Setup premise
`$(SkulFolder)` is a directory where the game is located on your PC

# New versions (v2.0f+)
### Automatic setup (Not working for now, but will be soon)
Visit [Thunderstore page of this mod](https://thunderstore.io/c/skul-the-hero-slayer/p/Limtis/SkulPatcher/) and install it with their Mod Manager

### Manual setup
1. Extract into `$(SkulFolder)/2020.3.34` (You will need to create a folder)
    - [unstripped Unity 2020.3.34 files](https://unity.bepinex.dev/libraries/2020.3.34.zip)
    - [unstripped CoreLibs 2020.3.34 files](https://unity.bepinex.dev/corlibs/2020.3.34.zip)
2. Extract into `$(SkulFolder)`
    - [BepInEx v5.4.21](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21)
3. In `$(SkulFolder)/doorstop.ini`
    - Set `dllSearchPathOverride=` to `dllSearchPathOverride=2020.3.34`
4. Download (or [build it yourself](#build-it-yourself)) the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) and place it to `$(SkulFolder)/BepInEx/plugins`

### Build it yourself
Assuming you have .NET Framework 4.7.2 and Visual Studio installed
1. Clone the project into Visual Studio
2. NuGet packages should install automatically, if they didn't - do so
3. Add `Assembly-CSharp` and all `Plugins...` `Unity...` `UnityEngine...` .dll files from `$(SkulFolder)/SkulData/Managed/` to the references
4. Build the solution

### Want to contribute?
Check out the ["Issues" page](https://github.com/limtis0/SkulPatcher/issues)

Or, you can create a custom stat for `Stat menu`:
1. Fork the repository (Branch: `BepInEx`)
2. Create a new class that inherits from `SpecialStat` in `SkulPatcher/SkulPatcher/Mods/SpecialStats/` folder
3. Make any feature you want. Many examples can be found in the same folder
4. No need to edit anything else, `SpecialStat` types are dynamically loaded into `Stat menu`
5. Make sure everything works and create a pull request!

<details> 
  <summary>List of contributors</summary>
   No one is here, yet. You can be first!
</details>

# Old versions (<v2.0f)
### Setup
1. Extract **with replacement** into `$(SkulFolder)/Skul_Data/Managed`
    - [unstripped Unity 2020.3.34 files](https://unity.bepinex.dev/libraries/2020.3.34.zip)
    - [unstripped CoreLibs files 2020.3.34](https://unity.bepinex.dev/corlibs/2020.3.34.zip)
2. Download (or [build it yourself](#build-it-yourself-1)) the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases)
3. Open the game and load into your save
4. Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar

The information for injection is provided below:
| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |

### Build it yourself
Assuming you have .NET Framework 4.7.2 and Visual Studio installed
1. Complete the first step of a [setup instruction](#setup-1)
2. Clone the project (Branch: `Old`) into Visual Studio
3. Install `Lib.Harmony`, `ILMerge`, `ILMerge.MSBuild.Task` packages via NuGet
4. Add `Assembly-CSharp` and all `Plugins...` `Unity...` `UnityEngine...` .dll files from `$(SkulFolder)/SkulData/Managed/` to the references
5. For each reference, except "0Harmony" set the `Copy Local` property to `False`
6. For `0Harmony` reference set the `Copy Local` property to `True`
7. Build the solution


# Compatibility
- [Skul: The Hero Slayer v1.7.0+](https://store.steampowered.com/news/app/1147560/view/5283318909430116714) (Built on [Unity 2020.3.34f1](https://unity.com/releases/editor/whats-new/2020.3.34); Should work for future releases if nothing drastically changes)
- [Harmony v2.2.2 (<v2.0f)](https://github.com/pardeike/Harmony/releases/tag/v2.2.2.0)
- [BepInEx v5.4.21 (v2.0f+)](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21)
