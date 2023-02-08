# SkulPatcher
A small mod for Skul: The Hero Slayer

<img width="960" alt="screenshot" src="https://user-images.githubusercontent.com/45824078/217654642-33a6e442-727a-4e2d-a72b-83f3d4f9c224.png">

# Setup
- Extract [unstripped Unity 2020.3.34f1 files](https://unity.bepinex.dev/libraries/2020.3.34.zip) and [unstripped CoreLibs](https://unity.bepinex.dev/corlibs/2020.3.34.zip) into `$(SkulFolder)/SkulData/Managed/`, where `$(SkulFolder)` is a folder where the game is located
- Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) or build it yourself
- Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar. The information for injector is provided below

| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |
