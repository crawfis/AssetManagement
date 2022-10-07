using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CrawfisSofware.AssetManagement
{
    internal class PreLoadAddressables : MonoBehaviour
    {
        [SerializeField] private string _catalogURL = "Place URL.json here";
        [SerializeField] private List<string> _preloadedAssetNames = new List<string>();

        private readonly List<Task> _loadingTasks = new List<Task>();

        private async void Awake()
        {
            AsyncOperationHandle<IResourceLocator> loadContentCatalogAsync = Addressables.LoadContentCatalogAsync(_catalogURL);
            var loadingTask = loadContentCatalogAsync.Task;
            _loadingTasks.Add(loadingTask);
            await loadingTask;
            foreach (string assetName in _preloadedAssetNames)
            {
                var downloadTask = Addressables.DownloadDependenciesAsync(assetName, true);
                _loadingTasks.Add(downloadTask.Task);
            }
            await Task.WhenAll(_loadingTasks);
        }

        public async Task WaitForCatalogToBeLoaded()
        {
            await Task.WhenAll(_loadingTasks);
        }
    }
}