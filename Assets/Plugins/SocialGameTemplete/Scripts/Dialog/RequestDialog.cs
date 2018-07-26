
namespace SocialGame.Dialog
{
    public class RequestDialog
    {
        public readonly DialogType Type;

        public readonly object Param;

        public RequestDialog(DialogType type, object param)
        {
            Type = type;
            Param = param;
        }
    }
}
