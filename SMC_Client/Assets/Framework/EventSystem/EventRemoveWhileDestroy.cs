using System.Collections.Generic;
using UnityEngine;

namespace Framework.EventSystem
{
    public class EventRemoveWhileDestroy : MonoBehaviour
    {
        private List<EventListener> eventListeners;

        public void Add(EventListener eventListener)
        {
            eventListeners ??= new List<EventListener>();
            eventListeners.Add(eventListener);
        }

        private void OnDestroy()
        {
            if (eventListeners != null)
            {
                foreach (var eventListener in eventListeners)
                {
                    GameEventDispatcher.Instance.Unregister(eventListener.eventName, eventListener);
                }

                eventListeners.Clear();
                eventListeners = null;
            }
        }
    }
}
