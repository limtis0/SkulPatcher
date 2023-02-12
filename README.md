# SkulPatcher
A small mod for Skul: The Hero Slayer

<img width="960" alt="Untitled" src="https://user-images.githubusercontent.com/45824078/218150426-c41380ac-4882-4484-93e0-894ff66cd94e.png">

# Setup
- Extract **with replacement** [unstripped Unity 2020.3.34f1 files](https://unity.bepinex.dev/libraries/2020.3.34.zip) and [unstripped CoreLibs](https://unity.bepinex.dev/corlibs/2020.3.34.zip) into `$(SkulFolder)/SkulData/Managed/`, where `$(SkulFolder)` is a folder where the game is located (if this didn't worked, try [these files](https://mega.nz/folder/hnICRaYb#hZKCIBME5rKMemDnHmWd4g) instead)
- Download the mod from ["Releases" page](https://github.com/limtis0/SkulPatcher/releases) or build it yourself
- Open the game and load into your save
- Inject the DLL into the game using [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector), [MInjector](https://github.com/EquiFox/MInjector) or anything similar. The information for injector is provided below

| Namespace | Class | Method |
| --------- | ----- | ------ |
| SkulPatcher | Main | Init |
