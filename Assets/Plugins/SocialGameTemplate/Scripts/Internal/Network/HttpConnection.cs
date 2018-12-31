using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using UniRx;
using SocialGame.Network;
using SocialGame.Internal.Network.FormatContent;
using Env = SocialGame.Internal.Network.Environment;

namespace SocialGame.Internal.Network
{
    internal sealed class HttpConnection : IInitializable, IDisposable, IHttpConnection
    {
        [Inject] private GeneralSettings _generalSettings;
        
        [Inject] private HttpSettings _settings;

        private string _domain;

        private IFormatContent _formatContent;
        
        void IInitializable.Initialize()
        {
            // environment setting
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
                    Debug.unityLogger.LogError(GetType().Name, $"Not supported {_generalSettings.Environment}");
                    break;
            }
            _domain = Path.Combine(_settings.Domain, env);
            
            // data format setting
            switch (_settings.DataFormat)
            {
                case HttpSettings.Format.JSON:
                    _formatContent = new JsonContent();
                    break;
                case HttpSettings.Format.MsgPack:
                    _formatContent = new MsgPackContent();
                    break;
                case HttpSettings.Format.XML:
                    _formatContent = new XmlContent();
                    break;
                default:
                    Debug.unityLogger.LogError(typeof(HttpConnection).Name, $"Not supported {_settings.DataFormat}");
                    break;
            }
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
                if (request.isNetworkError || request.isHttpError || responseCode != HttpStatusCode.OK)
                {
                    observer.OnError(new HttpException(responseCode, request.error));
                    yield break;
                }

                observer.OnNext(request.downloadHandler.data);
            }
        }
        
        #region IHttpConnection implementation
        IObservable<TResponse> IHttpConnection.Get<TRequest, TResponse>(string path, TRequest data)
        {
            string url = Path.Combine(_domain, path);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            request.chunkedTransfer = _settings.UseChunkedTransfer;
            request.uploadHandler = new UploadHandlerRaw(_formatContent.Serialize(data));
            _formatContent.SetRequestHeader(request);
            if (_generalSettings.DebugMode)
            {
                Debug.unityLogger.Log(GetType().Name, $"request : {url}\ndata : {data}");
            }
            
            return Observable
                .FromCoroutine<byte[]>((observer, cancel) => Fetch(request, observer, cancel, _settings.TimeOutSeconds))
                .SelectMany(x => _formatContent.Deserialize<TResponse>(x))
                .ObserveOnMainThread()
                .Do(x =>
                {
                    if (_generalSettings.DebugMode)
                    {
                        Debug.unityLogger.Log(GetType().Name, $"response : {url}\ndata : {x}");
                    }
                })
                .OnErrorRetry((HttpException ex) => { }, _settings.RetryCount);
        }
        
        IObservable<TResponse> IHttpConnection.Post<TRequest, TResponse>(string path, TRequest data)
        {
            string url = Path.Combine(_domain, path);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            request.chunkedTransfer = _settings.UseChunkedTransfer;
            request.uploadHandler = new UploadHandlerRaw(_formatContent.Serialize(data));
            _formatContent.SetRequestHeader(request);
            if (_generalSettings.DebugMode)
            {
                Debug.unityLogger.Log(GetType().Name, $"request : {url}\ndata : {data}");
            }
            
            return Observable
                .FromCoroutine<byte[]>((observer, cancel) => Fetch(request, observer, cancel, _settings.TimeOutSeconds))
                .SelectMany(x => _formatContent.Deserialize<TResponse>(x))
                .ObserveOnMainThread()
                .Do(x =>
                {
                    if (_generalSettings.DebugMode)
                    {
                        Debug.unityLogger.Log(GetType().Name, $"response : {url}\ndata : {x}");
                    }
                })
                .OnErrorRetry((HttpException ex) => { }, _settings.RetryCount);
        }
        #endregion
    }
}
