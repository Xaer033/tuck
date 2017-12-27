using System;
using System.Collections;

namespace GhostGen
{
    public interface IEventDispatcher
    {
        void AddListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        void RemoveListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        bool HasListener(string eventKey);
        void RemoveAllListeners(string eventKey);
        void DispatchEvent(string eventKey, Hashtable eventData = null);
    }
}