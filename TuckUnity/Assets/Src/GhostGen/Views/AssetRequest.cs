using UnityEngine;

namespace GhostGen
{
    public class AssetRequest
    {
        public string path;
        public Transform parent;

        public AssetRequest(string pPath, Transform pParent = null)
        {
            path = pPath;
            parent = pParent;
        }
    }
}
