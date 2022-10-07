using UnityEngine;

namespace CrawfisSofware.AssetManagement
{
    public class AutoRelease : MonoBehaviour
    {
        private IAssetManagerAsync<GameObject> _assetProvider;

        public void SetAssetManager(IAssetManagerAsync<GameObject> assetProvider)
        {
            this._assetProvider = assetProvider;
        }
        private void OnDestroy()
        {
            _assetProvider.ReleaseAsync(this.gameObject);
        }
    }
}