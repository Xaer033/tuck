using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GhostGen
{
    public class ViewFactory : IViewFactory<UIView>
    {
        public Canvas canvas { get; set; }
        
        public class AsyncViewResult : MultiMap<string, UIView>
        {
        }
        
        internal class AsyncBlock
        {
            public static AsyncBlock Create<T>(string name, ResourceRequest p_request, Action<T> p_callback, Transform p_parent) where T : UIView
            {
                AsyncBlock block = new AsyncBlock();
                block.name = name;
                block.request = p_request;
                block.parent = p_parent;
                block.callback = (view) =>
                {
                    if(p_callback != null)
                    {
                        p_callback((T)view);
                    }
                };
                return block;
            }

            public ResourceRequest request;
            public Action<UIView> callback;
            public Transform parent;
            public string name;

        }
        
        private List<AsyncBlock> _asyncList = new List<AsyncBlock>();
        
        public ViewFactory(Canvas guiCanvas)
        {
            canvas = guiCanvas;
        }   
   
        public void Step()
        {
            int asyncLength = _asyncList.Count;
            for(int i = 0; i < asyncLength; ++i)
            { 
                var block = _asyncList[i];
                Assert.IsNotNull(block);
                Assert.IsNotNull(block.request);
            
                if(!block.request.isDone) { continue; }

                Assert.IsNotNull(block.request.asset, "Asset: " + block.name + " couldn't be loaded!");

                UIView view = _createView((UIView)block.request.asset, block.parent);
                Assert.IsNotNull(view);
            
                if(block.callback != null)
                {
                    block.callback(view);
                }

                _asyncList[i] = null;        
            }

            for(int i = asyncLength-1; i >=0; --i)
            {
                if(_asyncList[i] == null)
                {
                    _asyncList.RemoveAt(i);
                }
            }
        }

        public T GetPrefab<T>(string viewPath) where T : UIView
        {
            return (T)Resources.Load<UIView>(viewPath);
        }

        public T Create<T>(string viewPath, Transform parent = null) where T : UIView
        {
            UIView viewBase = Resources.Load<UIView>(viewPath);
            Assert.IsNotNull(viewBase);
        
            return (T)_createView(viewBase, parent);
        }

        public T Create<T>(UIView prefab, Transform parent = null) where T : UIView
        {
            Assert.IsNotNull(prefab);
            return (T)_createView(prefab, parent);
        }

        public bool CreateAsync<T>(string viewPath, Action<T> callback, Transform parent = null) where T : UIView
        {
            ResourceRequest request = Resources.LoadAsync<T>(viewPath);

            if (request == null) { return false; }
            AsyncBlock block = AsyncBlock.Create<T>(viewPath, request, callback, parent);
            _asyncList.Add(block);

            return true;
        }

        public bool CreateAsyncList(List<AssetRequest> requestList, Action<AsyncViewResult> callback)
        {
            Assert.IsNotNull(requestList);
            bool result = true;

            AsyncViewResult resultMap = new AsyncViewResult();
            int viewsToLoadCount = requestList.Count;

            for(int i = 0; i < requestList.Count; ++i)
            {
                Assert.IsNotNull(requestList[i], "Request List item is null at index: " + i);

                string path = requestList[i].path;
                Transform parent = requestList[i].parent;

                result = CreateAsync<UIView>(path, (view) =>
                {
                    viewsToLoadCount--;

                    resultMap.Add(path, view);

                    if(viewsToLoadCount <= 0 && callback != null)
                    {
                        callback(resultMap);
                    }

                }, parent);

                if(!result)
                {
                    Debug.LogError("Error trying to create view: " + path);
                    break;
                }
            }

            return result;
        }

        public void RemoveView(UIView view)
        {
            Assert.IsNotNull(view);
            _removeView(view);
        }

        private void _removeView(UIView view)
        {
            if(view != null)
            {
                view.RemoveAllListeners();
                view.OnViewDispose();
                GameObject.Destroy(view.gameObject);
                view = null;
            }
        }

        private UIView _createView(UIView viewBase, Transform parent)
        {
            Transform viewParent = (parent != null) ? parent : canvas.transform;
            Assert.IsNotNull(viewBase);
            return GameObject.Instantiate<UIView>(viewBase, viewParent, false);
        }
    }
}
