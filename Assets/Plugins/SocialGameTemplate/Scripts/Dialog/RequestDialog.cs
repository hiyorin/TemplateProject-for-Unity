
namespace SocialGame.Dialog
{
    public class RequestDialog
    {
        public readonly DialogType Type;

        public readonly object Param;

        public readonly bool Primary;
        
        public RequestDialog(DialogType type, object param) : this(type, param, false)
        {
            
        }

        public RequestDialog(DialogType type, object param, bool primary)
        {
            Type = type;
            Param = param;
            Primary = primary;
        }
    }
}
