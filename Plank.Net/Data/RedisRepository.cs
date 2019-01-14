using PagedList;
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

        public override async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            var client = GetClient();

            //TODO: For now search will always go to next but will store any result from the search into redis
            var result = await Next.SearchAsync(expression, pageNumber, pageSize);
            if(result != null)
            {
                await client.AddAllAsync(result.Select(e => new Tuple<string, TEntity>(GetKey($"{e.Id}"), e)).ToList());
            }
            return result;
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
