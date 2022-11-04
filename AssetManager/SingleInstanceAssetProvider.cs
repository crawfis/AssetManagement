using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    public class SingleInstanceAssetProvider : MonoBehaviour, IAssetManagerAsync<GameObject>
    {
        [SerializeField] private GameObject _asset;

        public IEnumerable<string> AvailableAssets()
        {
            yield return _asset.name;
        }

        public Task<GameObject> GetAsync(string name)
        {
            return Task.FromResult(_asset);
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task ReleaseAllAsync()
        {
            return Task.CompletedTask;
        }

        public Task ReleaseAsync(GameObject asset)
        {
            return Task.CompletedTask;
        }
    }
}