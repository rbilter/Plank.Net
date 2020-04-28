using Plank.Net.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public sealed class PlankRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly DbContext _context;

        #endregion

        #region CONSTRUCTORS

        public PlankRepository(DbContext context)
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;
        }

        #endregion

        #region METHODS

        public override async Task AddAsync(TEntity entity)
        {
            await Next.AddAsync(entity).ConfigureAwait(false);

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public override async Task DeleteAsync(int id)
        {
            await Next.DeleteAsync(id).ConfigureAwait(false);

            var item = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            var result = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if(result != null)
            {
                return result;
            }

            return await Next.GetAsync(id).ConfigureAwait(false);
        }

        public override async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _context.Set<TEntity>().Where(expression).OrderBy(e => e.Id).ToPagedListAsync(pageNumber, pageSize).ConfigureAwait(false);
            if(result != null)
            {
                return result;
            }

            return await Next.SearchAsync(expression, pageNumber, pageSize).ConfigureAwait(false);
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            await Next.UpdateAsync(entity).ConfigureAwait(false);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
