# What it is
[Download link.](https://marketplace.visualstudio.com/items?itemName=nbreum.entitas-vs)

An extension for Visual Studio that makes the workflow with [Entitas-CSharp](https://github.com/sschmid/Entitas-CSharp) alot smoother.

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

# Installation Guide
To use Entitas VS you need to:
1. Setup Entitas as you normally would (follow one of these guides by the author of Entitas: [video guide](https://www.youtube.com/watch?v=LGKsqSg5FHg), [text guide](https://github.com/sschmid/Entitas-CSharp/issues/476))
1. Make sure that the commandline generator works correctly. Try generating some files before using Entitas VS.
1. Download the extension here: https://marketplace.visualstudio.com/items?itemName=nbreum.entitas-vs
1. Fill out the generator path and the trigger paths.
    1. Open Tools -> Entitas VS
    
    ![](https://i.imgur.com/Kti0Qrl.png)
    
    3. Set the generator path. 
    
    ![](https://i.imgur.com/aE1czCv.png)
    
    Select the Generator folder by pressing the "..." button. If the Generator folder is in the solution directory the path will automatically be converted to a relative path (which is recommended). 

    2. Click the drop down-menu to add a project.
    
    ![](https://i.imgur.com/EFwGHJq.png)
    
    Note: For each project you want to generate files for, the Entitas.properties and User.properties has to be placed in the project directory (beside the .csproj file). This allows for multiple project generation (though only one generator is supported for now).
    
    4. Add the folders that trigger the code generation when files in the folder are saved. Here the code generator is triggered when changes occur in the Sources\Assets\Components folder. Like the Generator folder, if the Trigger folder is in the project directory, the path will be converted to a relative path.
    
    ![](https://i.imgur.com/jJDcFNY.png)
    
    5. That is all. Create a new component, save the file, and the generation will run automatically. All these settings are saved in entitas-vs.cfg, in the solution directory.
