using CrawfisSoftware.AssetManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSofware.AssetManagement
{
    /// <summary>
    /// Given the underlying asset names, selects them in sequental order.
    /// This will then call the underlying IAssetManagerAsync to create the chosen asset.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/SequentialDecorator", order = 6)]
    public class SequentialAssetDecorator : DecoratorAssetProviderBase
    {
        private List<string> _assetPrefabs = new List<string>();
        private int _currentIndex = 0;

        public override async Task<GameObject> GetAsync(string name)
        {
            if (_currentIndex >= _assetPrefabs.Count) _currentIndex = 0;
            var asset = await _assetProvider.GetAsync(_assetPrefabs[_currentIndex++]);
            return asset;
        }

        public override async Task Initialize()
        {
            await _assetProvider.Initialize();
            _assetPrefabs = _assetProvider.AvailableAssets().ToList();
        }
    }
}