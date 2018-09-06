using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public sealed class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        #region MEMBERS

        private DbContext _context;

        #endregion

        #region CONSTRUCTORS

        public EntityRepository(DbContext context)
        {
            _context = context;
        }

        #endregion

        #region METHODS

        public async Task<int> CreateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var item = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber = 1, int pageSize = 10)
        {
            return await Task.Run(() => _context.Set<TEntity>().Where(expression).OrderBy(e => e.Id).ToPagedList(pageNumber, pageSize));
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            var existing = await GetAsync(entity.Id);
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var existing = await GetAsync(entity.Id);

            foreach (var p in properties)
            {
                var property = _context.Entry(existing).Property(p);
                property.CurrentValue = entity.GetType().GetProperty(property.Name).GetValue(entity);
            }

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        #endregion
    }
}
