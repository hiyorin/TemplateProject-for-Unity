
namespace SocialGame.TapEffect
{
    public interface ITapEffectController
    {
        void Start(TapEffectType type);

        void Start(string name);
        
        void Stop();
    }
}
