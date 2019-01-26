# Scene
Scene Management.  

## Setup
* Create
  1. Add SceneContext  
    Add Component implement SceneInstaller class
  1. Add SceneSettings  
    Add SceneSettings to SceneContext GameObject
  1. Add to Unity BuildSettings
    Auto generated Scene.cs
* SceneSettings
  1. Load sub scenes  
    Select sub scene you want to load
  1. Active scenes  
    Select initial active of sub scene

## Usage
```cs
using SocialGame.Scene;
```

### Transition
```cs
[Inject] private ISceneManager _sceneManager;
private void Example() {
  _sceneManager.Next(Scene.Example, "Trans Data object", TransMode.BlackFade));
}
```

### Lifecycle
Please implement ISceneLifecycle and Bind to container.

```cs
public class InstallerExample : SceneInstaller {
  public override void OnInstallBindings() {
    Container.BindInterfaceTo<SceneLifecycleExample>().AsSingle();
  }
}
```

```cs
public class SceneLifecycleExample : ISceneLifecycle {
  public async UniTask OnLoad(object transData) {
    // Load dynamic assets.
    await Resources.LoadAsync("asset name");
  }
  public async UniTask OnTransIn() {
    // Interaction at in-transition
  }
  public void OnTransComplete() {
    // Complete in-transition
  }
  public async UniTask OnTransOut() {
    // Interaction at out-transition
  }
  public async UniTask OnTransOut() {
    // Unload dynamic assets.
    Resources.UnloadAsset();
  }
}
```
