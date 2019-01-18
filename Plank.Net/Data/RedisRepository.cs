using PagedList;
using Plank.Net.Profiles;
using Serialize.Linq.Serializers;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public sealed class RedisRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private static readonly string _connectionString;
        private static readonly object _lock;
        private static readonly string _prefix;
        private static List<ICacheClient> _clients;

        #endregion

        #region CONSTRUCTORS

        static RedisRepository()
        {
            _clients          = new List<ICacheClient>();
            _connectionString = ConfigurationManager.AppSettings["RedisRepository.ConnectionString"];
            _lock             = new object();

            _prefix = ConfigurationManager.AppSettings["RedisRepository.Prefix"];
            if(!string.IsNullOrEmpty(_prefix))
            {
                _prefix = $"{_prefix}:";
            }
        }

        #endregion

        #region METHODS

        public override async Task<TEntity> CreateAsync(TEntity entity)
        {
            var client = GetClient();
            var item   = await Next.CreateAsync(entity);
            var key    = GetKey($"{item.Id}");

            await client.AddAsync(key, entity);
            await FlushSearchCacheAsync();
            
            return entity;
        }

        public override async Task<int> DeleteAsync(int id)
        {
            id         = await Next.DeleteAsync(id);
            var client = GetClient();
            var key    = GetKey($"{id}");

            await client.RemoveAsync(key);
            await FlushSearchCacheAsync();

            return id;
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            var client = GetClient();
            var key    = GetKey($"{id}");

            var result = await client.GetAsync<TEntity>(key);
            if(result == null)
            {
                result = await Next.GetAsync(id);
                if (result != null)
                {
                    await client.AddAsync(key, result);
                }
            }
            return result;
        }

        public override async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber = 1, int pageSize = 10)
        {
            var client     = GetClient();
            var serializer = new ExpressionSerializer(new JsonSerializer());
            var key        = GetKey($"Search:{($"{serializer.SerializeText(expression)}-{pageNumber}-{pageSize}").GetHashCode()}");

            // Search cache
            var cached = await client.GetAsync<PagedListCache>(key);
            if (cached != null)
            {
                var keys   = cached.Ids.Select(i => GetKey($"{i}")).ToList();
                var items  = await client.GetAllAsync<TEntity>(keys);

                var tmpresult = new TEntity[cached.TotalItemCount];
                var index = (pageNumber - 1) * pageSize;

                foreach (var item in items)
                {
                    tmpresult[index] = item.Value;
                    index++;
                }
                return tmpresult.ToPagedList(pageNumber, pageSize);
            }

            // Get from Next
            var result = await Next.SearchAsync(expression, pageNumber, pageSize);
            if(result != null)
            {
                await client.AddAllAsync(result.Select(e => new Tuple<string, TEntity>(GetKey($"{e.Id}"), e)).ToList());
                await client.AddAsync(key, Mapping<TEntity>.Mapper.Map<PagedListCache>(result));
            }
            return result;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var client = GetClient();
            var item   = await Next.UpdateAsync(entity);
            var key    = GetKey($"{item.Id}");

            await client.AddAsync(key, entity);

            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var client = GetClient();
            var item   = await Next.UpdateAsync(entity, properties);
            var key    = GetKey($"{item.Id}");

            await client.AddAsync(key, entity);

            return entity;
        }

        #endregion

        #region PRIVATE METHODS

        private async Task FlushSearchCacheAsync()
        {
            var client     = GetClient();
            var searchKeys = await client.SearchKeysAsync(GetKey("Search:*"));

            await client.RemoveAllAsync(searchKeys);
        }

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
                        client = new StackExchangeRedisCacheClient(new NewtonsoftSerializer(), _connectionString, _prefix);
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
