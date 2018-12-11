using System;
using System.Text;
using UnityEngine;
using UnityExtensions;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SocialGame.Data
{
    /// <summary>
    /// ローカル保存するファイルクラス
    /// 保存するファイル単位で実装してください
    /// </summary>
    /// <typeparam name="T">シリアライズするクラス</typeparam>
    public abstract class LocalStorageBase<T> : MonoBehaviour
    {
        private const string EncryptKey = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";
        private const int EncryptKeyCount = 16;

        [SerializeField]
        private T _model;

        /// <summary>
        /// 保存するデータ
        /// </summary>
        public T Model => _model;

        /// <summary>
        /// ファイル名
        /// </summary>
        protected abstract string FileName { get; }

        private string FilePath
        {
            get
            {
    #if DEBUG
                return FileName;
    #else
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(FileName));
    #endif
            }
        }

        private void Start()
        {
            if (FileUtility.Exists(FilePath))
            {
                try
                {
                    Load();
                    OnInitialize();
                    return;
                }
                catch (Exception e)
                {
                    Debug.unityLogger.LogWarning(GetType().Name, $"Crash save file. {FileName}\n{e.Message}");
                }
            }
            
            _model = OnCreate();
            Save();
            OnInitialize();
        }

        private void OnDestroy()
        {
            Save();
        }

        /// <summary>
        /// It is called when a file is newly created.
        /// Prease setting initial value.
        /// </summary>
        protected abstract T OnCreate();

        protected abstract void OnInitialize();

        public void Save()
        {
            // Serialize and encrypt
            byte[] pack = Encoding.UTF8.GetBytes(JsonUtility.ToJson(Model));;
            byte[] iv = null;
            byte[] data = null;
            CryptUtility.EncryptAESWithCBC(pack, Encoding.UTF8.GetBytes(EncryptKey), EncryptKeyCount, out iv, out data);

            // Save
            FileUtility.Save(FilePath, writer => {
                writer.Write(iv.Length);
                writer.Write(iv);
                
                writer.Write(data.Length);
                writer.Write(data);
            });
        }

        public IObservable<Unit> SaveAsync()
        {
            return Observable.Start(() => Save())
                .ObserveOnMainThread();
        }

        public void Load()
        {
            // Load
            byte[] iv = null;
            byte[] data = null;
            FileUtility.Load(FilePath, reader => {
                int length = reader.ReadInt32();
                iv = reader.ReadBytes(length);
            
                length = reader.ReadInt32();
                data = reader.ReadBytes(length);
            });

            // Decrypt and Deserialize
            byte[] pack = null;
            CryptUtility.DecryptAESWithCBC(data, Encoding.UTF8.GetBytes(EncryptKey), iv, out pack);
            _model = JsonUtility.FromJson<T>(Encoding.UTF8.GetString(pack));
        }

        public IObservable<Unit> LoadAsync()
        {
            return Observable.Start(() => Load())
                .ObserveOnMainThread();
        }

        #if UNITY_EDITOR
        protected abstract class CustomInspectorBase<U, V> : Editor where U:LocalStorageBase<V>
        {
            private U _owner;

            private void OnEnable()
            {
                _owner = target as U;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("Save"))
                {
                    _owner.Save();
                }
                if (GUILayout.Button("Load"))
                {
                    _owner.Load();
                }
            }
        }
        #endif
    }
}
