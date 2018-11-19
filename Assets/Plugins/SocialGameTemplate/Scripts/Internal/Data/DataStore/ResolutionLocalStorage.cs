using SocialGame.Data;
using SocialGame.Internal.Data.Entity;

namespace SocialGame.Internal.Data.DataStore
{
    internal sealed class ResolutionLocalStorage : LocalStorageBase<Resolution>
    {
        protected override string FileName => "Resolution";

        protected override Resolution OnCreate()
        {
            return new Resolution();
        }

        protected override void OnInitialize()
        {

        }
        
        #if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(ResolutionLocalStorage))] 
        private class CustomInspector : CustomInspectorBase<ResolutionLocalStorage, Resolution>
        {
        }
        #endif
    }
}
