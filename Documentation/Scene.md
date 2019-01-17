# Scene
Scene Management.  

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

## Usage
```cs
using SocialGame.Scene;
```
