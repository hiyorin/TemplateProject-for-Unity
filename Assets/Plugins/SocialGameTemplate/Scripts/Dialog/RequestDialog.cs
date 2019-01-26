
namespace SocialGame.Dialog
{
    public sealed class RequestDialog
    {
        public readonly string Name;

        public readonly object Param;

        public readonly bool Primary;
        
        public RequestDialog(DialogType type, object param) : this(type.ToString(), param, false)
        {
            
        }

        public RequestDialog(DialogType type, object param, bool primary) : this(type.ToString(), param, primary)
        {
            
        }

        public RequestDialog(string name, object param) : this(name, param, false)
        {
            
        }
        
        public RequestDialog(string name, object param, bool primary)
        {
            Name = name;
            Param = param;
            Primary = primary;
        }
    }
}
