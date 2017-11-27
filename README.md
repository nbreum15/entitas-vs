# What it is
[Download link.](https://marketplace.visualstudio.com/items?itemName=nbreum.entitas-vs)

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
1. Download the extension here: https://marketplace.visualstudio.com/items?itemName=nbreum.entitas-vs
1. Open up a solution in Visual Studio (otherwise the extension will not load).
1. Fill out the generator path and the trigger paths.
    1. Open Entitas-vs in View -> Other Windows -> Entitas - Visual Studio
    
    ![](https://i.imgur.com/uzo4Ibn.png)
    
    2. This shows all the settings (if it does not, select "Configure" in the drop down menu, or check "entitas-vs.log" in the solution directory). For example, here is a solution with two projects:
    
    ![](https://i.imgur.com/n2P24ad.png)
    
    Note: For each project you want to generate files for, the Entitas.properties and User.properties has to be placed in the project directory (beside the .csproj file). This allows for multiple project generation.
    
    3. It will tell you that the Generator path has not been set. 
    
    ![](https://i.imgur.com/k8u6XHr.png)
    
    Select the Generator folder by pressing the "..." button. If the Generator folder is in the solution directory the path will automatically be converted to a relative path (which is recommended). 

    After this, it will prompt you asking if the generator should be loaded.
    
    ![](https://i.imgur.com/pfEdCIP.png)
    
    If the folder is correct press "Yes" otherwise set the folder again and press "Yes" when it is correct. 
    
    4. Add the folders that trigger the code generation when files in the folder are saved. Here the code generator is triggered when changes occur in the Sources\Assets\Components folder. Like the Generator folder, if the Trigger folder is in the project directory, the path will be converted to a relative path.
    
    ![](https://i.imgur.com/ABgIh5y.png)
    
    5. That is all. Create a new component, save the file, and the generation will run automatically. All these settings are saved in entitas-vs.cfg, in the solution directory.
