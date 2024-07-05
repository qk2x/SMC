using Framework.EventSystem;
using UnityEngine;

namespace Framework.Misc
{
    public class SingletonUnity<T> : MonoBehaviour where T : MonoBehaviour, IRestartComponent
    {
        private static int _uid;
        private static bool _registerRestart;

        #region Variables
        public bool openLogs = false;
        private static T _instance = null;

        // protected static bool isQuiting = false;
        #endregion
	
        #region Unity
    
        void OnApplicationQuit()
        {
        }
        
        #endregion
    
        #region Interface

        public static T Instance
        {
            get
            {
                Init();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!_registerRestart)
            {
                _registerRestart = true;
                
                GameEventDispatcher.Instance.Register(EventName.GameRestart, OnRestart);
            }
        }

        public static void Init()
        {
            if (_instance == null || _instance.gameObject == null)
            {
                _instance = FindObjectOfType<T>();

                // if (isQuiting)
                // {
                //     DLog.Error($"Application is quiting, disable instance creation.");
                //     return null;
                // }

                if (_instance == null)
                {
                    // Debug.Log($"Instancing MonoBehaviour: <color=#00ff00>{typeof(T)}</color>");
                    GameObject go = new GameObject("SingleMono");
                    _instance = go.AddComponent<T>();
                }
#if UNITY_EDITOR
                var type = _instance.GetType();
                _instance.gameObject.name = _instance.GetType().FullName + $"#{_uid++}";
#endif

            }

            if (!_registerRestart)
            {
                _registerRestart = true;
                
                GameEventDispatcher.Instance.Register(EventName.GameRestart, OnRestart);
            }
        }

        private static void OnRestart(EventParam obj)
        {
            if (_instance != null)
            {
                DLog.Error($"SingleUnity Restart : {typeof(T).ToString()}");
                _instance.OnGameRestart();
            }
            else
            {
                DLog.Error($"---> SingleUnity Restart Null: {typeof(T).ToString()}");
            }

            _registerRestart = false;
        }

        public virtual void OnGameRestart()
        {
            Destroy(_instance.gameObject);
        }

        /// <summary>
        /// 获取Instance而不创建
        /// </summary>
        public static T GetInstance()
        {
            return _instance;
        }

        public static bool HaveInstance()
        {
            return _instance != null;
        }

        public static void ClearSingleton()
        {
            _instance = null;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        #endregion
    
        #region Debug
        protected void Log(string content)
        {
            if(openLogs)
                Debug.Log($"{content}");
        }
        #endregion
    }
}