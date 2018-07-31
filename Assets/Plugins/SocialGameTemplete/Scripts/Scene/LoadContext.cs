﻿using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Transition;
using UniRx;

namespace SocialGame.Scene
{
    public sealed class LoadContext
    {
        public class Scene
        {
            public readonly string Name;
            public List<ISceneLifecycle> Lifecycles;

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

        public IObservable<Unit> Load()
        {
            return NextScene.Lifecycles
                .Select(x => x.OnLoad(TransData))
                .Concat(AdditiveScenes
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnLoad(TransData)))
                .WhenAll();
        }

        public IObservable<Unit> TransIn()
        {
            return NextScene.Lifecycles
                .Select(x => x.OnTransIn())
                .Concat(AdditiveScenes
                    .SelectMany(x => x.Lifecycles)
                    .Select(x => x.OnTransIn()))
                .WhenAll();
        }

        public void TransComplate()
        {
            NextScene.Lifecycles.ForEach(x => x.OnTransComplete());
        }

        public IObservable<Unit> TransOut()
        {
            return AdditiveScenes
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnTransOut())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnTransOut()))
                .WhenAll();
        }

        public IObservable<Unit> Unload()
        {
            return AdditiveScenes
                .SelectMany(x => x.Lifecycles)
                .Select(x => x.OnUnload())
                .Concat(NextScene.Lifecycles
                    .Select(x => x.OnUnload()))
                .WhenAll();
        }
    }
}
