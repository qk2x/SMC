using System.Collections.Generic;
using UnityEngine;

namespace Framework.EventSystem
{
    public class AutoRegisterAndRemoveWhileEnableAndDisable : MonoBehaviour
    {
        private List<EventListener> eventListeners;

        public void Add(EventListener eventListener)
        {
            eventListeners ??= new List<EventListener>();
            eventListeners.Add(eventListener);

            if (gameObject.activeInHierarchy)
            {
                GameEventDispatcher.Instance.Register(eventListener.eventName, eventListener.callback);
            }
        }

        private void OnEnable()
        {
            if (eventListeners != null)
            {
                foreach (var eventListener in eventListeners)
                {
                    GameEventDispatcher.Instance.Register(eventListener.eventName, eventListener.callback);
                }
            }
        }

        private void OnDisable()
        {
            if (eventListeners != null)
            {
                foreach (var eventListener in eventListeners)
                {
                    GameEventDispatcher.Instance.Unregister(eventListener.eventName, eventListener.callback);
                }
            }
        }

        private void OnDestroy()
        {
            if (eventListeners != null)
            {
                if (gameObject.activeInHierarchy)
                {
                    foreach (var eventListener in eventListeners)
                    {
                        GameEventDispatcher.Instance.Unregister(eventListener.eventName, eventListener.callback);
                    }

                    eventListeners.Clear();
                    eventListeners = null;
                }
            }
        }
    }
}


