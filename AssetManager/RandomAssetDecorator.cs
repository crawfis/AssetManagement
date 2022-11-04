using CrawfisSoftware.AssetManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Provide a key->list of asset names mapping and select a random asset name from the list.
    /// This will then call the underlying IAssetManagerAsync to create the chosen asset.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/RandomDecorator", order = 5)]
    public class RandomAssetDecorator : DecoratorAssetProviderBase
    {
        private List<string> _assetPrefabs = new List<string>();

        public System.Random RandomGenerator { get; set; } = new System.Random();

        //private void Start()
        //{
        //    RandomGenerator = _randomProvider.RandomGenerator;
        //}

        public override Task<GameObject> GetAsync(string name)
        {
            if (_assetPrefabs.Count > 0)
            {
                int assetIndex = RandomGenerator.Next(_assetPrefabs.Count);
                return _assetProvider.GetAsync(_assetPrefabs[assetIndex]);
            }
            return Task.FromResult<GameObject>(null);
        }

        public override async Task Initialize()
        {
            await _assetProvider.Initialize();
            _assetPrefabs = _assetProvider.AvailableAssets().ToList();
        }
    }
}