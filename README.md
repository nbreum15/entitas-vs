# What it is
Entitas-vs is an extension for Visual Studio that makes all code generation in [Entitas-CSharp](https://github.com/sschmid/Entitas-CSharp) purely automatic without any manual intervention from the user.

## Motivation
The current workflow with Visual Studio and Entitas is: 
1. Write component(s); save file(s).
1. Generate files with Entitas CLI (either through CMD or keybind in Visual Studio).
1. Stay or go back to Visual Studio; Press "Reload Solution" (this is the annoying part).

This extension removes the last two steps, leaving only:
1. Write component(s); save file(s); stay in Visual Studio.

Nice right?

# Important: Requirements
The [paid Roslyn Generator](https://www.assetstore.unity3d.com/en/#!/content/87638) is strongly recommand when using this extension because it is able to generate with compiler errors. The Normal Generator from [Entitas](https://github.com/sschmid/Entitas-CSharp/releases) will also work, but because the Normal Generator is reflection-based, compiler errors will block it from generating (which makes it very cumbersome to work with).

# Setup
To use Entitas-vs you need to:
1. Setup Entitas as you normally would (follow one of these guides by the author of Entitas: [video guide](https://www.youtube.com/watch?v=LGKsqSg5FHg), [text guide](https://github.com/sschmid/Entitas-CSharp/issues/476))
1. Make sure that the commandline generator works correctly. Try generating some files before using Entitas-vs.
1. Specify the folders/files that, when saved, triggers the code generation (the solution has to be open).
    1. Open Entitas-vs in View -> Other Windows -> Entitas - Visual Studio
    
    ![](https://i.imgur.com/uzo4Ibn.png)
    
    2. Choose Configure from the dropbox menu
    
    ![](https://i.imgur.com/70sbW1w.png)
    
    3. All projects in the solution will be shown, this allows for generation with multiple projects with different Entitas.properties and User.properties files. For example, here is shown two projects:
    
    ![](https://i.imgur.com/n2P24ad.png)
    
    4. Add the folders that trigger the code generation when files in the folder are saved. Or add files that trigger the code generation when the files are saved. Here the code generator is triggered when changes occur in the Sources\Assets\Components folder (recommended is to use relative paths from where the .csproj rests).
    
    ![](https://i.imgur.com/ABgIh5y.png)
    
    5. That is all. Create a new component, save the file, and the generation will run automatically. All these settings are saved in entitas-vs.cfg, in the solution directory.
