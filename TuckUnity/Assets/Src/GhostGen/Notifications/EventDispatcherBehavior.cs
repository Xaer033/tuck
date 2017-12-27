using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhostGen
{
    public class EventDispatcherBehavior : MonoBehaviour, IEventDispatcher
    {
        private NotificationDispatcher _dispatcher = new NotificationDispatcher();


        public void AddListener(string eventKey, Action<GeneralEvent> callback)
        {
            _dispatcher.AddListener(eventKey, callback);
        }
        public void RemoveListener(string eventKey, Action<GeneralEvent> callback)
        {
            _dispatcher.RemoveListener(eventKey, callback);
        }
        public bool HasListener(string eventKey)
        {
            return _dispatcher.HasListener(eventKey);
        }
        public void RemoveAllListeners(string eventKey)
        {
            _dispatcher.RemoveAllListeners(eventKey);
        }
        public void DispatchEvent(string eventKey, Hashtable eventData = null)
        {
            _dispatcher.DispatchEvent(eventKey, eventData);
        }     
    }
}
