using System;
using UnityEngine;

namespace GhostGen
{
    public interface IViewFactory<B>
    {
        T GetPrefab<T>(string viewPath) where T : B;
        T Create<T>(string viewPath, Transform parent = null) where T : B;
        T Create<T>(B prefab, Transform parent = null) where T : B;
        bool CreateAsync<T>(string viewPath, Action<T> callback, Transform parent = null) where T : B;

        void Step();
        void RemoveView(B view);
    }
}
