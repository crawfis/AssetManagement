using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Uses Addressables to create and release Sprites.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/LabelAddressableSpriteProvider", order = 3)]
    internal class LabelAddressablesSpriteProvider : ScriptableAssetProviderBase<Sprite>
    {
        [SerializeField] private List<string> _labels = new List<string>();
        [SerializeField] private Addressables.MergeMode _mergeMode;

        private List<Sprite> _allocatedAssets = new List<Sprite>();
        //private readonly List<AsyncOperationHandle<Sprite>> _assetHandles = new List<AsyncOperationHandle<Sprite>>();
        //private readonly Dictionary<string, IResourceLocation> _assetMapping = new Dictionary<string, IResourceLocation>();
        private readonly Dictionary<string, Sprite> _assetMapping = new Dictionary<string, Sprite>();

        private async void Awake()
        {
            await Initialize();
        }

        /// <inheritdoc/>
        public override async Task<Sprite> GetAsync(string name)
        {
            if (_assetMapping.TryGetValue(name, out Sprite prefab))
            {
                //var handle = Addressables.LoadAssetAsync<Sprite>(prefab);
                ////_assetHandles.Add(handle);
                //var task = handle.Task;
                //await task;
                //Sprite asset = task.Result;
                //_allocatedAssets.Add(asset);
                var asset = Instantiate<Sprite>(prefab);
                return await Task.FromResult(asset);
            }
            return null;
        }

        /// <inheritdoc/>
        public override Task ReleaseAllAsync()
        {
            for (int i = 0; i < _allocatedAssets.Count; i++)
            {
                var asset = _allocatedAssets[i];
                _allocatedAssets[i] = null;
                Addressables.Release<Sprite>(asset);
            }
            _allocatedAssets.Clear();
            //foreach (var handle in this._assetHandles)
            //{
            //    Addressables.Release<Sprite>(handle);
            //}

            //_assetHandles.Clear();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override Task ReleaseAsync(Sprite asset)
        {
            if (_allocatedAssets.Remove(asset))
            {
                Addressables.Release<Sprite>(asset);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override async Task Initialize()
        {
            //await _addressableCatalog.WaitForCatalogToBeLoaded();
            //Hack: Might be a better way to accomplish this by cloning the instance. We could have use the same "instance"
            // with 2 Catalog's.
            _assetNames.Clear();

            // Bug: Unity was changing the enum to the wrong value. Hacking this to Intersection for now.
            //var handle = Addressables.LoadResourceLocationsAsync(_labels, _mergeMode, typeof(Sprite));
            //var handle = Addressables.LoadResourceLocationsAsync(_labels, Addressables.MergeMode.Intersection, typeof(Sprite));
            var handle = Addressables.LoadAssetsAsync<Sprite>(_labels, (_) => { }, _mergeMode); // Addressables.MergeMode.Intersection);
            var resourceLocation = await handle.Task;
            if (resourceLocation != null)
            {
                foreach (var location in resourceLocation)
                {
                    _assetMapping[location.name] = location;
                    _assetNames.Add(location.name);
                }
            }
        }
    }
}