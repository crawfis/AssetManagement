using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AddDecorators", order = 3)]
    public class AssetProviderWithDecorators : DecoratorAssetProviderBase
    {
        [SerializeField] private ScriptableAssetProviderBase _baseAssetProvider;
        [SerializeField] private List<DecoratorAssetProviderBase> _decorationAssetProviders = new List<DecoratorAssetProviderBase>();
        public override Task<GameObject> GetAsync(string name)
        {
            return _assetProvider.GetAsync(name);
        }

        public override Task Initialize()
        {
            AddDecorators();
            return _assetProvider.Initialize();
        }
        protected void AddDecorators()
        {
            int count = _decorationAssetProviders.Count;
            ScriptableAssetProviderBase lastDecorator = _baseAssetProvider;
            for (int i = 0; i < count; i++)
            {
                DecoratorAssetProviderBase decorator = _decorationAssetProviders[i];
                decorator.SetRealInstance(lastDecorator);
                lastDecorator = decorator;
            }
            _assetProvider = lastDecorator;
        }
    }
}