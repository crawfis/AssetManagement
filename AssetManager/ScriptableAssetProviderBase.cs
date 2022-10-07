using CrawfisSofware.AssetManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    public abstract class ScriptableAssetProviderBase : ScriptableObject, IAssetManagerAsync<GameObject>
    {
        [SerializeField] protected List<string> _assetNames = new List<string>();

        public virtual IEnumerable<string> AvailableAssets()
        {
            foreach (string assetName in _assetNames)
            {
                yield return assetName;
            }
        }

        public abstract Task<GameObject> GetAsync(string name);
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public abstract Task ReleaseAllAsync();
        public abstract Task ReleaseAsync(GameObject instance);

    }
}