using System;
using SocialGame.Data;
using MessagePack;

namespace SocialGame.Internal.Data.Entity
{
    [Serializable]
    [MessagePackObject]
    internal sealed class Resolution
    {
        [Key(0)] public Quality Quality;
    }
}
