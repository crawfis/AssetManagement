using CrawfisSoftware.AssetManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSofware.AssetManagement
{
    /// <summary>
    /// Instantiates and destroys a given prefab using the name of the prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AssetProvider", order = 1)]
    public class PrefabAssetProvider : ScriptableAssetProviderBase
    {
        [SerializeField] private List<GameObject> _assetPrefabs = new List<GameObject>();

        private readonly List<GameObject> _allocatedAssets = new List<GameObject>();
        private readonly Dictionary<string, GameObject> _assetMapping = new Dictionary<string, GameObject>();

        private void Awake()
        {
            Initialize();
        }

        public override Task<GameObject> GetAsync(string name)
        {
            if (_assetMapping.TryGetValue(name, out GameObject prefab))
            {
                var asset = Instantiate(prefab);
                _allocatedAssets.Add(asset);
                return Task.FromResult(asset);
            }
            return null;
        }

        public override Task ReleaseAllAsync()
        {
            for (int i = 0; i < _allocatedAssets.Count; i++)
            {
                var asset = _allocatedAssets[i];
                _allocatedAssets[i] = null;
                Destroy(asset);
            }
            _allocatedAssets.Clear();
            return Task.CompletedTask;
        }

        public override Task ReleaseAsync(GameObject asset)
        {
            if (_allocatedAssets.Remove(asset))
            {
                Destroy(asset);
            }
            return Task.CompletedTask;
        }

        public override async Task Initialize()
        {
            foreach (var asset in _assetPrefabs)
            {
                _assetMapping[asset.name] = asset;
                _assetNames.Add(asset.name);
            }
        }
    }
}