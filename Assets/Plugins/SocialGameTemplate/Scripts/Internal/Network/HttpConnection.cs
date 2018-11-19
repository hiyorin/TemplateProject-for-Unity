using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using MessagePack;
using Zenject;
using UniRx;
using SocialGame.Network;
using Env = SocialGame.Internal.Network.Environment;

namespace SocialGame.Internal.Network
{
    internal sealed class HttpConnection : IInitializable, IDisposable, IHttpConnection
    {
        [Inject] private GeneralSettings _generalSettings;
        
        [Inject] private HttpSettings _settings;

        private string _domain;
        
        void IInitializable.Initialize()
        {
            string env = string.Empty;
            switch (_generalSettings.Environment)
            {
                case Env.Production:
                    env = _settings.ProductionEnvironment;
                    break;
                case Env.Staging:
                    env = _settings.StagingEnvironment;
                    break;
                case Env.Development:
                    env = _settings.DevelopmentEnvironment;
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, string.Format("Not supported {0}", _generalSettings.Environment));
                    break;
            }

            _domain = Path.Combine(_settings.Domain, env);
        }

        void IDisposable.Dispose()
        {
            
        }

        private static IEnumerator Fetch(UnityWebRequest request, IObserver<byte[]> observer, CancellationToken cancel, float timeOutSec)
        {
            var currentDate = DateTime.UtcNow;
            var limitTick = (TimeSpan.FromTicks(currentDate.Ticks) + TimeSpan.FromSeconds(timeOutSec)).Ticks;

            using (request)
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    yield return null;
                    // time out.
                    if (timeOutSec > 0 && limitTick < DateTime.UtcNow.Ticks)
                    {
                        request.Abort();
                        observer.OnError(new HttpException(HttpStatusCode.RequestTimeout, "Time out."));
                        yield break;
                    }
                }

                var responseCode = (HttpStatusCode)request.responseCode;
                if (request.isNetworkError || responseCode != HttpStatusCode.OK)
                {
                    observer.OnError(new HttpException(responseCode, request.error));
                    yield break;
                }

                observer.OnNext(request.downloadHandler.data);
            }
        }

        private static byte[] Serialize<T>(T data, HttpSettings.Format format)
        {
            switch (format)
            {
                case HttpSettings.Format.JSON:
                    return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                case HttpSettings.Format.MessagePack:
                    return MessagePackSerializer.Serialize(data);
                default:
                    Debug.unityLogger.LogError(typeof(HttpConnection).Name, string.Format("Not supported {0}", format));
                    return null;
            }
        }
        
        private static IObservable<T> Deserialize<T>(byte[] data, HttpSettings.Format format)
        {
            return Observable.Start(() =>
            {
                switch (format)
                {
                    case HttpSettings.Format.JSON:
                        return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data));
                    case HttpSettings.Format.MessagePack:
                        return MessagePackSerializer.Deserialize<T>(data);
                    default:
                        Debug.unityLogger.LogError(typeof(HttpConnection).Name, string.Format("Not supported {0}", format));
                        return default(T);
                }
            });
        }

        #region IHttpConnection implementation
        IObservable<TResponse> IHttpConnection.Get<TRequest, TResponse>(string path, TRequest data)
        {
            string url = Path.Combine(_domain, path);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            request.chunkedTransfer = _settings.UseChunkedTransfer;
            request.uploadHandler = new UploadHandlerRaw(Serialize(data, _settings.DataFormat));
            if (_generalSettings.DebugMode)
            {
                Debug.unityLogger.Log(GetType().Name, string.Format("request : {0}\ndata : {1}", url, data));
            }
            
            return Observable
                .FromCoroutine<byte[]>((observer, cancel) => Fetch(request, observer, cancel, _settings.TimeOutSeconds))
                .SelectMany(x => Deserialize<TResponse>(x, _settings.DataFormat))
                .Do(x =>
                {
                    if (_generalSettings.DebugMode)
                    {
                        Debug.unityLogger.Log(GetType().Name, string.Format("response : {0}\ndata : {1}", url, x));
                    }
                })
                .OnErrorRetry((HttpException ex) => { }, _settings.RetryCount);
        }
        
        IObservable<TResponse> IHttpConnection.Post<TRequest, TResponse>(string path, TRequest data)
        {
            string url = Path.Combine(_domain, path);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            request.chunkedTransfer = _settings.UseChunkedTransfer;
            request.uploadHandler = new UploadHandlerRaw(Serialize(data, _settings.DataFormat));
            if (_generalSettings.DebugMode)
            {
                Debug.unityLogger.Log(GetType().Name, string.Format("request : {0}\ndata : {1}", url, data));
            }
            
            return Observable
                .FromCoroutine<byte[]>((observer, cancel) => Fetch(request, observer, cancel, _settings.TimeOutSeconds))
                .SelectMany(x => Deserialize<TResponse>(x, _settings.DataFormat))
                .Do(x =>
                {
                    if (_generalSettings.DebugMode)
                    {
                        Debug.unityLogger.Log(GetType().Name, string.Format("response : {0}\ndata : {1}", url, x));
                    }
                })
                .OnErrorRetry((HttpException ex) => { }, _settings.RetryCount);
        }
        #endregion
    }
}
