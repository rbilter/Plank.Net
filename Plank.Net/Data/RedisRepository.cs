using PagedList;
using Plank.Net.Utilities;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public sealed class RedisRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private static readonly object _lock;
        private static List<ICacheClient> _clients;

        #endregion

        #region CONSTRUCTORS

        static RedisRepository()
        {
            _clients = new List<ICacheClient>();
            _lock    = new object();
        }

        #endregion

        #region METHODS

        public override async Task<int> CreateAsync(TEntity entity)
        {
            var client = GetClient();
            var id     = await Next.CreateAsync(entity);
            var key    = GetKey($"{id}");

            await client.AddAsync(key, entity);

            return id;
        }

        public override async Task<int> DeleteAsync(int id)
        {
            id         = await Next.DeleteAsync(id);
            var client = GetClient();
            var key    = GetKey($"{id}");

            await client.RemoveAsync(key);

            return id;
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            var client = GetClient();
            var key    = GetKey($"{id}");

            var result = await client.GetAsync<TEntity>(key);
            if(result != null)
            {
                result = await Next.GetAsync(id);
                if (result != null)
                {
                    await client.AddAsync(key, result);
                }
            }
            return result;
        }

        public override async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            //TODO: Need to figure out a  way to update the cached search result so for now not going to store
            return await Next.SearchAsync(expression, pageNumber, pageSize);
        }

        public override async Task<int> UpdateAsync(TEntity entity)
        {
            var client = GetClient();
            var id     = await Next.UpdateAsync(entity);
            var key    = GetKey($"{id}");

            await client.AddAsync(key, entity);

            return id;
        }

        public override async Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var client = GetClient();
            var id     = await Next.UpdateAsync(entity, properties);
            var key    = GetKey($"{id}");

            await client.AddAsync(key, entity);

            return id;
        }

        #endregion

        #region PRIVATE METHODS

        private static ICacheClient GetClient()
        {
            ICacheClient client = null;
            lock (_lock)
            {
                if (_clients.Count < 5)
                {
                    client = _clients.FirstOrDefault(c => c.Database.Multiplexer.GetCounters().TotalOutstanding <= 5);
                    if (client == null)
                    {
                        client = new StackExchangeRedisCacheClient(new NewtonsoftSerializer(), "clustercfg.singlecare-qa.ug7kvy.use1.cache.amazonaws.com:6379,ssl=true", "RichTest:");
                        _clients.Add(client);
                    }
                }
                else
                {
                    client = _clients.OrderBy(c => c.Database.Multiplexer.GetCounters().TotalOutstanding).First();
                }
            }

            return client;
        }

        private string GetKey(string keyPart)
        {
            return $"{typeof(TEntity).Name}:{keyPart}";
        }

        #endregion
    }
}
