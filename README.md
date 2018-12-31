# TemplateProject-for-Unity

# Requirement
* [UniRx](https://github.com/neuecc/UniRx)(6.1.1 or higher)
* [Zenject](https://github.com/svermeulen/Zenject)(7.3.0 or higher)
* [DOTween](https://github.com/Demigiant/dotween)
* [Extensions-for-Unity](https://github.com/hiyorin/Extensions-for-Unity)
* [MemoryInfoPlugin-for-Unity](https://github.com/hiyorin/MemoryInfoPlugin-for-Unity)

# Install
SocialGameTemplete.unitypackage

# Setup
  1. Open SetupWindow  
    Menu/Window/Setup SocialGameTemplete
  1. Create!

# Usage
```cs
using SocialGame;
```

## Scene
* Create
  1. Add Component  
    Add SceneSettings to SceneContext GameObject
  1. Add to Unity BuildSettings
    Auto generated Scene.cs
* SceneSettings
  1. Load sub scenes  
    Select sub scene you want to load
  1. Active scenes  
    Select initial active of sub scene
* Transition
  ```cs
  [Inject] private ISceneManager _sceneManager;
  private void Example() {
     _sceneManager.Next(Scene.Example, "Trans Data object", TransMode.BlackFade));
  }
  ```

## Dialog

## Toast

## Loading

## Transition

## TapEffect
