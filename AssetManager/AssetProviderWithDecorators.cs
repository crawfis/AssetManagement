﻿using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// General IAssetManagerAsync for GameObjects that provides the underlying access (prefabs, addressables, multiple, single, etc.)
    /// with decorators that provide additional functionality (randomly select, sequentially select, use pooling, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AddDecorators", order = 3)]
    public class AssetProviderWithDecorators : DecoratorAssetProviderBase<GameObject>
    {
        [SerializeField] private ScriptableAssetProviderBase<GameObject> _baseAssetProvider;
        [SerializeField] private List<DecoratorAssetProviderBase<GameObject>> _decorationAssetProviders = new List<DecoratorAssetProviderBase<GameObject>>();

        /// <inheritdoc/>
        public override Task<GameObject> GetAsync(string name)
        {
            return _assetProvider.GetAsync(name);
        }

        /// <inheritdoc/>
        public override Task Initialize()
        {
            AttachDecorators();
            return _assetProvider.Initialize();
        }

        public void SetBaseAssetProvider(ScriptableAssetProviderBase<GameObject> baseAssetProvider)
        {
            _baseAssetProvider = baseAssetProvider;
        }

        public void AddDecorator(DecoratorAssetProviderBase<GameObject> decorator)
        {
            _decorationAssetProviders.Add(decorator);
        }
        /// <summary>
        /// Initializes the decorators and produces the correct instance that propagates through the decorators to the actual provider.
        /// </summary>
        protected void AttachDecorators()
        {
            int count = _decorationAssetProviders.Count;
            var lastDecorator = _baseAssetProvider;
            for (int i = 0; i < count; i++)
            {
                var decorator = _decorationAssetProviders[i];
                decorator.SetRealInstance(lastDecorator);
                lastDecorator = decorator;
            }
            _assetProvider = lastDecorator;
        }
    }
}