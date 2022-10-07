using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    public abstract class DecoratorAssetProviderBase : ScriptableAssetProviderBase
    {
        protected ScriptableAssetProviderBase _assetProvider;

        public void SetRealInstance(ScriptableAssetProviderBase assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public override Task<GameObject> GetAsync(string name)
        {
            return _assetProvider.GetAsync(name);
        }

        public override IEnumerable<string> AvailableAssets()
        {
            return _assetProvider.AvailableAssets();
        }

        public override Task Initialize()
        {
            return _assetProvider.Initialize();
        }

        public override Task ReleaseAllAsync()
        {
            return _assetProvider.ReleaseAllAsync();
        }

        public override Task ReleaseAsync(GameObject asset)
        {
            return _assetProvider.ReleaseAsync(asset);
        }
    }
}