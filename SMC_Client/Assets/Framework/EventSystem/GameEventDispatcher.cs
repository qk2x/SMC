using System;
using System.Collections.Generic;
using Framework.Misc;
using UnityEngine;

namespace Framework.EventSystem
{
    public class EnumEventNameComparer : IEqualityComparer<EventName>
    {
        public bool Equals(EventName x, EventName y)
        {
            return (int)x == (int)y;
        }

        public int GetHashCode(EventName obj)
        {
            return (int)obj;
        }
    }
    public class GameEventDispatcher : Singleton<GameEventDispatcher>, IRestartComponent
    {
        private Dictionary<EventName, List<EventListener>> listenerDic = new(50, new EnumEventNameComparer());

        private bool log = false;
        public override void OnGameRestart()
        {
        }

        public void Clean()
        {
            listenerDic.Clear();
        }

        public EventListener Register(EventName eventName, Action<EventParam> callback)
        {
            var listener = EventListener.GetEventListener(eventName, callback);
            Register(eventName, listener);
            return listener;
        }

        public EventListener RegisterAndAutoRemoveWhileDestroy(EventName eventName, Action<EventParam> callback, GameObject gameObject)
        {
            var listener = EventListener.GetEventListener(eventName, callback);
            Register(eventName, listener);
            
            gameObject.GetOrAddComponent<EventRemoveWhileDestroy>().Add(listener);

            return listener;
        }

        public EventListener RegisterAndRemoveWhileEnableAndDisable(EventName eventName, Action<EventParam> callback,
            GameObject gameObject)
        {
            var listener = EventListener.GetEventListener(eventName, callback);
            gameObject.GetOrAddComponent<AutoRegisterAndRemoveWhileEnableAndDisable>().Add(listener);
            return listener;
        }

        public void Register(EventName eventName, EventListener listener)
        {
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                listenersList.Add(listener);
            }
            else
            {
                listenersList = new List<EventListener>(5) {listener};
                listenerDic[eventName] = listenersList;
            }
        }

        public void Unregister(EventName eventName, EventListener listener)
        {
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                listenersList.Remove(listener);
            }
            else
            {
                DLog.Error("RemoveListener eventName -> list is nil ");
            }
            
            EventListener.ReturnEventListener(listener);
        }

        private Action<EventParam> target;
        private bool Find(EventListener e)
        {
            return e.callback == target;
        }
        public void Unregister(EventName eventName, Action<EventParam> action)
        {
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                target = action;
                var e = listenersList.Find(Find);
                if (e != null)
                {
                    listenersList.Remove(e);
                    EventListener.ReturnEventListener(e);
                }
                else
                {
                    DLog.Warning($"[GameEventDispatcher] RemoveListener eventName:{eventName.ToString()} -> can't find it");
                }
            }
            else
            {
                DLog.Warning($"[GameEventDispatcher] RemoveListener eventName:{eventName.ToString()}  -> list is nil");
            }
        }

        public void DispatchEvent(EventName eventName)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(EventParam.GetEmpty());
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, bool b)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(b);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, double d)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(d);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, float f)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get((double) f);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, long v1, long v2 = 0L, long v3 = 0, object o = null)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(v1, v2, v3, o);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, object o)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(o);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, object o, bool b)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(o, b);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, long v1, object o, bool b)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(v1, o, b);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, int o, bool b)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(o, b);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, object o, long v)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(v, 0, 0, o);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, long l, bool b)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get( l, b);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
        
        public void DispatchEvent(EventName eventName, int it)
        {
#if UNITY_EDITOR
            if (log)
            {
                DLog.Log(($"[GameEventDispatcher] 转发消息Event: {eventName}"));
            }
#endif
            if (listenerDic.TryGetValue(eventName, out var listenersList))
            {
                using var e = EventParam.Get(it);
                for (int i = listenersList.Count - 1; i >= 0; --i)
                {
                    listenersList[i].callback(e);
                }
            }
        }
    }
} 