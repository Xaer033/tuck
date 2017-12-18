using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GhostGen
{
    public interface IEventDispatcher
    {
        void AddListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        void RemoveListener(string eventKey, Action<GhostGen.GeneralEvent> callback);
        void RemoveAllListeners(string eventKey);
        void DispatchEvent(string eventKey, Hashtable eventData = null);
    }
}