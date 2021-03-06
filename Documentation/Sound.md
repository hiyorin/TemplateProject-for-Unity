# Sound
Sound Module.  
Supports AudioSource and [ADX2](https://game.criware.jp/products/adx2-smartphone/) or [ADX2LE](https://www.adx2le.com/)


## SoundSettings
Resources/SoundSettings.asset

### General

### Unity (AudioSource)

### ADX2 (CRIWare)

### Generate Enum


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
  // Get
  Debug.Log(_soundVolumeController.Get());

  // Set
  _soundVolumeController.Put(new SoundVolume() {
      Master  = 1.0f,
      BGM     = 0.8f,
      SE      = 0.6f,
      Voice   = 0.4f,
    });

  // Save
  _soundVolumeController.Save()
    .Subscribe()
    .AddTo(this);
}
```
