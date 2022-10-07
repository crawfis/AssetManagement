using CrawfisSofware.AssetManagement;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/PooledDecorator", order = 4)]
    public class PooledAssetProviderScriptable : DecoratorAssetProviderBase
    {
        private PooledAssetsManagerDecorator _pooledAssets;

        public override Task<GameObject> GetAsync(string name)
        {
            return _pooledAssets.GetAsync(name);
        }

        public override Task Initialize()
        {
            _pooledAssets = new PooledAssetsManagerDecorator(_assetProvider);
            return _assetProvider.Initialize();
        }

        public override Task ReleaseAllAsync()
        {
            return _pooledAssets.ReleaseAllAsync();
        }

        public override Task ReleaseAsync(GameObject instance)
        {
            return _pooledAssets.ReleaseAsync(instance);
        }
    }
}
