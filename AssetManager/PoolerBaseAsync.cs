using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawfisSofware.AssetManagement
{
    /// <summary>
    /// Abstract base class for some asynchronous implementations of IPooler
    /// </summary>
    /// <typeparam name="T">The type of instances in the pool.</typeparam>
    public abstract class PoolerBaseAsync<T> : IAssetManagerAsync<T> where T : class
    {
        private readonly Dictionary<string, Queue<T>> _pools = new Dictionary<string, Queue<T>>();
        private readonly Dictionary<T,string> _allocatedAssets = new Dictionary<T,string>();

        public async Task<T> GetAsync(string name)
        {
            T instance;
            if (!_pools.TryGetValue(name, out Queue<T> pool))
            {
                pool = new Queue<T>();
                _pools[name] = pool;
            }
            if (pool.Count == 0)
            {
                instance = await CreateNewPoolInstanceAsync(name);
            }
            else
            {
                instance = pool.Dequeue();
            }
            _allocatedAssets.Add(instance, name);
            ReinitializePoolInstance(instance);
            return instance;
        }

        public Task ReleaseAsync(T poolObject)
        {
            if (_allocatedAssets.TryGetValue(poolObject, out string poolName))
            {
                _pools[poolName].Enqueue(poolObject);
                _allocatedAssets.Remove(poolObject);
                ReturnPoolInstance(poolObject);
            }
            return Task.CompletedTask;
        }

        public Task ReleaseAllAsync()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    T poolObject = pool.Dequeue();
                    ReturnPoolInstance(poolObject);
                }
                pool.Clear();
            }
            _pools.Clear();
            _allocatedAssets.Clear();
            return Task.CompletedTask;
        }

        public abstract IEnumerable<string> AvailableAssets();

        public PoolerBaseAsync()
        {
            //InitPool(initialSize, maxPersistentSize, collectionChecks);
        }

        //protected void InitPool(int initial = 10, int maxPersistentSize = 20, bool collectionChecks = false)
        //{
        //}

        protected abstract Task<T> CreateNewPoolInstanceAsync(string name);
        protected abstract void ReinitializePoolInstance(T poolObject);
        protected abstract void ReturnPoolInstance(T poolObject);
        protected abstract Task DestroyPoolInstanceAsync(T poolObject);
        public abstract Task Initialize();
    }
}