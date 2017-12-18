using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GhostGen
{
    public class NotificationDispatcher : IEventDispatcher
    {
        private Dictionary<string, List<Action<GhostGen.GeneralEvent>>> _eventDictionary = new Dictionary<string, List<Action<GhostGen.GeneralEvent>>>();
        
        public void AddListener(string eventKey, Action<GhostGen.GeneralEvent> callback)
        {
            Assert.IsNotNull(callback);
            if (callback == null) { return; }

            List<Action<GhostGen.GeneralEvent>> callbackList = null;
            if (!_eventDictionary.TryGetValue(eventKey, out callbackList))
            {
                callbackList = new List<Action<GhostGen.GeneralEvent>>();
                _eventDictionary.Add(eventKey, callbackList);
            }
            callbackList.Add(callback);
        }

        public void RemoveListener(string eventKey, Action<GhostGen.GeneralEvent> callback)
        {
            Assert.IsNotNull(callback);
            if (callback == null) { return; }

            List<Action<GhostGen.GeneralEvent>> callbackList = null;
            if (_eventDictionary.TryGetValue(eventKey, out callbackList))
            {
                int index = callbackList.FindIndex((x) => x == callback);
                callbackList.RemoveAt(index);
            }
        }

        public void RemoveAllListeners(string eventKey)
        {
            List<Action<GhostGen.GeneralEvent>> callbackList = null;
            if (_eventDictionary.TryGetValue(eventKey, out callbackList))
            {
                callbackList.Clear();
            }
        }

        public void DispatchEvent(string eventKey, Hashtable eventData = null)
        {
            List<Action<GhostGen.GeneralEvent>> callbackList = null;
            if (_eventDictionary.TryGetValue(eventKey, out callbackList))
            {
                GhostGen.GeneralEvent e = new GhostGen.GeneralEvent();
                e.type = eventKey;
                e.target = this;
                e.data = eventData;

                int length = callbackList.Count;
                for (int i = 0; i < length; ++i)
                {
                    if (callbackList[i] != null)
                    {
                        callbackList[i].Invoke(e);
                    }
                }
            }
        }
    }
}