using System.Collections.Generic;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    public static class AssetManagerStack
    {
        private static readonly Stack<IAssetManagerAsync<GameObject>> _managerStack = new Stack<IAssetManagerAsync<GameObject>>();
        public static IAssetManagerAsync<GameObject> Instance { get { return _managerStack.Peek(); } }

        public static void PushInstance(IAssetManagerAsync<GameObject> assetManager)
        {
            _managerStack.Push(assetManager);
        }

        public static IAssetManagerAsync<GameObject> PopInstance()
        {
            return _managerStack.Count > 0 ? _managerStack.Pop() : null;
        }
    }
}
