using Plank.Net.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public sealed class EntityRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly DbContext _context;

        #endregion

        #region CONSTRUCTORS

        public EntityRepository(DbContext context)
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;
        }

        #endregion

        #region METHODS

        public override async Task AddAsync(TEntity entity)
        {
            await Next.AddAsync(entity);

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            await Next.DeleteAsync(id);

            var item = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync();
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            var result = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id);
            if(result != null)
            {
                return result;
            }

            return await Next.GetAsync(id);
        }

        public override async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _context.Set<TEntity>().Where(expression).OrderBy(e => e.Id).ToPagedListAsync(pageNumber, pageSize);
            if(result != null)
            {
                return result;
            }

            return await Next.SearchAsync(expression, pageNumber, pageSize);
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            await Next.UpdateAsync(entity);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
