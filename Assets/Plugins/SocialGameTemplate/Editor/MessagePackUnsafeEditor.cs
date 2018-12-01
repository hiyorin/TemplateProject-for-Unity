using UnityEditor;
using UnityExtensions.Editor;

namespace SocialGame.Internal
{
    internal static class MessagePackUnsafeEditor
    {
        private const string EnableItemName     = "Tools/MessagePack/Unsafe/Enable";
        private const string DisableItemName    = "Tools/MessagePack/Unsafe/Disable";
        private const string Symbol             = "ENABLE_UNSAFE_MSGPACK";
        
        [MenuItem(EnableItemName)]
        public static void Enable()
        {
            new AssemblyDefinitionBuilder("Plugins/MessagePack/MessagePack")
                .EnableAllowUnsafeCode()
                .Write();
            AssetDatabase.Refresh();
            MenuEditor.AddSymbols(Symbol);
        }

        [MenuItem(EnableItemName, true)]
        private static bool EnableValidate()
        {
            if (EditorApplication.isCompiling)
                return false;
            
#if ENABLE_UNSAFE_MSGPACK
            Menu.SetChecked(EnableItemName, true);
            return false;
#else
            Menu.SetChecked(EnableItemName, false);
            return true;
#endif
        }

        [MenuItem(DisableItemName)]
        public static void Disable()
        {
            new AssemblyDefinitionBuilder("Plugins/MessagePack/MessagePack")
                .DisableAllowUnsafeCode()
                .Write();
            AssetDatabase.Refresh();
            MenuEditor.RemoveSymbols(Symbol);
        }

        [MenuItem(DisableItemName, true)]
        private static bool DisableValidate()
        {
            if (EditorApplication.isCompiling)
                return false;
            
#if ENABLE_UNSAFE_MSGPACK
            Menu.SetChecked(DisableItemName, false);
            return true;
#else
            Menu.SetChecked(DisableItemName, true);
            return false;
#endif
        }
    }
}