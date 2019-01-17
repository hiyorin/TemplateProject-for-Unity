# Sound
Sound Module.  
Supports AudioSource and ADX2


## SoundSettings
Resources/SoundSettings.asset

### Unity (AudioSource)

### ADX2 (CRIWare)



## Usage
```cs
using SocialGame.Sound;
```

### Example : BGM
```cs
[Inject] private ISoundController _soundController;
private void Example() {
  _soundController.PlayBGM(BGM.Example);
}
```

### Example : SE
```cs
[Inject] private ISoundController _soundController;
private void Example() {
  _soundController.PlaySE(SE.Example);
}
```

### Example : Voice
```cs
[Inject] private ISoundController _soundController;
private void Example() {
  _soundController.PlayVoice(Voice.Example);
}
```

### Example : Volume
```cs
[Inject] private ISoundVolumeController _soundVolumeController;
private void Example() {
  _soundVolumeController.Get()
    .Subscribe(x => Debug.Log(x))
    .AddTo(this);

  _soundVolumeController.Put(new SoundVolume() {
      Master  = 1.0f,
      BGM     = 0.8f,
      SE      = 0.6f,
      Voice   = 0.4f,
    })
    .Subscribe()
    .AddTo(this);
}
```
