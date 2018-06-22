using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Plank.Net.Data
{
    public class EntityRepository<T> : IRepository<T> where T : Entity
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

        public Guid Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public Guid Delete(Guid id)
        {
            var item = _context.Set<T>().SingleOrDefault(i => i.Id == id);
            _context.Set<T>().Attach(item);
            _context.Set<T>().Remove(item);
            _context.SaveChanges();

            return id;
        }

        public IPagedList<T> Search(Expression<Func<T, bool>> query, int pageNumber = 1, int pageSize = 10)
        {
            return _context.Set<T>().Where(query).OrderBy(e => e.Id).ToPagedList(pageNumber, pageSize);
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().SingleOrDefault(i => i.Id == id);
        }

        public Guid Update(T entity)
        {
            var existing = Get(entity.Id);
            _context.Entry(existing).CurrentValues.SetValues(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        #endregion
    }
}
