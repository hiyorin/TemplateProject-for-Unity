using System;
using System.IO;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Sound.ADX2
{
    public static class ADX2Utility
    {
        public static IObservable<CriAtomCueSheet> AddCueSheet(CriAtomCueSheet cueSheet)
        {
            if (cueSheet == null)
                throw new ArgumentException();

            if (string.IsNullOrEmpty(cueSheet.name))
                Observable.Return(cueSheet);
            
            return Observable
                .Return(CriAtom.AddCueSheet(cueSheet.name, cueSheet.acbFile, cueSheet.awbFile))
                .SelectMany(x => Observable
                    .EveryUpdate()
                    .Where(_ => !x.IsLoading)
                    .Select(_ => x))
                .First();
        }
        
        public static bool RemoveCueSheet(CriAtomCueSheet cueSheet)
        {
            if (cueSheet == null || string.IsNullOrEmpty(cueSheet.name))
                return false;

            CriAtom.RemoveCueSheet(cueSheet.name);
            return true;
        }

        public static IObservable<byte[]> LoadAcfFile(string fileName)
        {
            string filePath = string.Format("{0}/{1}", Application.streamingAssetsPath, fileName);
            IObservable<byte[]> result = null;
#if UNITY_ANDROID && !UNITY_EDITOR
            var www = new WWW(filePath);
            return www.ToObservable().Select(_ => www.bytes);
#else
            result = Observable
                .Start(() => File.ReadAllBytes(filePath))
                .ObserveOnMainThread();
#endif
            return result;
        }
    }
}