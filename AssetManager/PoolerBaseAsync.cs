using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Pool;

namespace CrawfisSofware.AssetManagement
{
    /// <summary>
    /// Abstract base class for some asynchronous implementations of IPooler
    /// </summary>
    /// <typeparam name="T">The type of instances in the pool.</typeparam>
    public abstract class PoolerBaseAsync<T> : IAssetManagerAsync<T> where T : class
    {
        protected ObjectPool<T> _pool;
        protected string _currentAssetName;

        public Task<T> GetAsync(string name)
        {
            _currentAssetName = name;
            //T asset = await Task<T>.Run(() => { T result = _pool.Get(); return result; });
            T asset = _pool.Get();
            return Task.FromResult(asset);
        }
        public Task ReleaseAsync(T poolObject)
        {
            _pool.Release(poolObject);
            return Task.CompletedTask;
        }

        public Task ReleaseAllAsync()
        {
            _pool.Clear();
            return Task.CompletedTask;
        }

        public abstract IEnumerable<string> AvailableAssets();

        public PoolerBaseAsync(int initialSize = 100, int maxPersistentSize = 10000, bool collectionChecks = false)
        {
            InitPool(initialSize, maxPersistentSize, collectionChecks);
        }

        protected void InitPool(int initial = 10, int maxPersistentSize = 20, bool collectionChecks = false)
        {
            _pool = new ObjectPool<T>(
                CreateNewPoolInstance,
                GetPoolInstance,
                ReleasePoolInstance,
                DestroyPoolInstance,
                collectionChecks,
                initial,
                maxPersistentSize);
        }

        protected abstract Task<T> CreateNewPoolInstanceAsync(string name);
        protected abstract void ReinitializePoolInstance(T poolObject);
        protected abstract void ReturnPoolInstance(T poolObject);
        protected abstract Task DestroyPoolInstanceAsync(T poolObject);
        public abstract Task Initialize();

        private T CreateNewPoolInstance()
        {
            Task<T> task = CreateNewPoolInstanceAsync(_currentAssetName);
            task.GetAwaiter().GetResult();
            return task.Result;
        }

        private void GetPoolInstance(T poolObject)
        {
            ReinitializePoolInstance(poolObject);
        }

        private void ReleasePoolInstance(T poolObject)
        {
            ReturnPoolInstance(poolObject);
        }
        private void DestroyPoolInstance(T poolObject)
        {
            Task task = DestroyPoolInstanceAsync(poolObject);
            task.Wait();
        }
    }
}