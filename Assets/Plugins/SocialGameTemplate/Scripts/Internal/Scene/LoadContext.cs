using System;
using System.Linq;
using System.Collections.Generic;
using UnityExtensions;
using SocialGame.Scene;
using SocialGame.Transition;
using UniRx;
using UniRx.Async;

namespace SocialGame.Internal.Scene
{
    internal sealed class LoadContext
    {
        public class Scene
        {
            public readonly string Name;
            public IEnumerable<ISceneLifecycle> Lifecycles;

            public Scene(string name)
            {
                Name = name;
            }
        }

        public readonly Scene NextScene;
        public readonly List<Scene> AdditiveScenes;
        public readonly object TransData;
        public readonly TransMode TransMode;

        public LoadContext(string nextScene, object transData, TransMode transMode)
        {
            NextScene = new Scene(nextScene);
            AdditiveScenes = new List<Scene>();
            TransData = transData;
            TransMode = transMode;
        }

        public void AddAdditiveScene(string name)
        {
            AdditiveScenes.Add(new Scene(name));
        }

        public async UniTask Load()
        {
            await NextScene.Lifecycles
                .Select(x => x.OnLoad(TransData))
                .Concat(AdditiveScenes
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnLoad(TransData)));
        }

        public async UniTask Load(LoadContext prev)
        {
            await NextScene.Lifecycles
                .Select(x => x.OnLoad(TransData))
                .Concat(AdditiveScenes
                    .Where(x => prev == null || !prev.AdditiveScenes.Any(y => x.Name == y.Name))
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnLoad(TransData)));
        }

        public async UniTask TransIn(LoadContext prev)
        {
            await NextScene.Lifecycles
                .Select(x => x.OnTransIn())
                .Concat(AdditiveScenes
                    .Where(x => prev == null || !prev.AdditiveScenes.Any(y => x.Name == y.Name))
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnTransIn()));
        }

        public async UniTask TransIn()
        {
            await NextScene.Lifecycles
                .Select(x => x.OnTransIn())
                .Concat(AdditiveScenes
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnTransIn()));
        }

        public void TransComplete()
        {
            NextScene.Lifecycles.ForEach(x => x.OnTransComplete());
        }

        public async UniTask TransOut()
        {
            await AdditiveScenes
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnTransOut())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnTransOut()));
        }

        public async UniTask TransOut(LoadContext next)
        {
             await AdditiveScenes
                .Where(x => next == null || !next.AdditiveScenes.Any(y => x.Name == y.Name))
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnTransOut())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnTransOut()));
        }

        public async UniTask Unload()
        {
            await AdditiveScenes
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnUnload())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnUnload()));
        }

        public async UniTask Unload(LoadContext next)
        {
            await AdditiveScenes
                .Where(x => next == null || !next.AdditiveScenes.Any(y => x.Name == y.Name))
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnUnload())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnUnload()));
        }
    }
}
