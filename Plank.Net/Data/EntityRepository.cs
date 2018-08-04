using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

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

        public Guid Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public Guid Delete(Guid id)
        {
            var item = _context.Set<TEntity>().SingleOrDefault(i => i.Id == id);
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            _context.SaveChanges();

            return id;
        }

        public IPagedList<TEntity> Search(Expression<Func<TEntity, bool>> query, int pageNumber = 1, int pageSize = 10)
        {
            return _context.Set<TEntity>().Where(query).OrderBy(e => e.Id).ToPagedList(pageNumber, pageSize);
        }

        public TEntity Get(Guid id)
        {
            return _context.Set<TEntity>().SingleOrDefault(i => i.Id == id);
        }

        public Guid Update(TEntity entity)
        {
            var existing = Get(entity.Id);
            _context.Entry(existing).CurrentValues.SetValues(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public Guid Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var existing = Get(entity.Id);

            foreach (var p in properties)
            {
                var property = _context.Entry(existing).Property(p);
                property.CurrentValue = entity.GetType().GetProperty(property.Name).GetValue(entity);
            }

            _context.SaveChanges();

            return entity.Id;
        }

        #endregion
    }
}
