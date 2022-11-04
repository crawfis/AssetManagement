using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawfisSoftware.AssetManagement
{
    public interface IAssetManagerAsync<T>
    {
        Task<T> GetAsync(string name);
        Task ReleaseAsync(T instance);
        Task ReleaseAllAsync();
        Task Initialize();
        IEnumerable<string> AvailableAssets();
    }
}