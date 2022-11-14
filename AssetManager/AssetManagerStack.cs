using System.Collections.Generic;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Simple stack-based static class to push and pop the current IAssetManagerAsync
    /// </summary>
    /// <typeparam name="T">The Type of asset this stack supports.</typeparam>
    public static class AssetManagerStack<T>
    {
        private static readonly Stack<IAssetManagerAsync<T>> _managerStack = new Stack<IAssetManagerAsync<T>>();
        public static IAssetManagerAsync<T> Instance { get { return _managerStack.Peek(); } }

        public static void PushInstance(IAssetManagerAsync<T> assetManager)
        {
            _managerStack.Push(assetManager);
        }

        public static IAssetManagerAsync<T> PopInstance()
        {
            return _managerStack.Count > 0 ? _managerStack.Pop() : null;
        }
    }
}