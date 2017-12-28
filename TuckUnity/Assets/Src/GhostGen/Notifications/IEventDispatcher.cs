using System;
using System.Collections;
using UnityEngine;

namespace GhostGen
{
    public interface IEventDispatcher
    {
        void AddListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        void RemoveListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        bool HasListener(string eventKey);
        void RemoveAllListeners(string eventKey);
        bool DispatchEvent(string eventKey, bool bubble = false, Hashtable eventData = null);
        bool DispatchEvent(GeneralEvent e);
    }
}