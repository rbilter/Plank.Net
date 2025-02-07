﻿using Plank.Net.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public sealed class PlankRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly PlankDbContext _context;

        #endregion

        #region CONSTRUCTORS

        public PlankRepository(PlankDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region PROPERTIES

        public IRepository<TEntity> NextRepository { get; private set; }

        #endregion

        #region METHODS

        public async Task AddAsync(TEntity entity)
        {
            await NextRepository.AddAsync(entity).ConfigureAwait(false);

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _context.DetachAllEntitiesAsync().ConfigureAwait(false);
        }

        public async Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            await NextRepository.BulkAddAsync(entities).ConfigureAwait(false);

            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _context.DetachAllEntitiesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await NextRepository.DeleteAsync(id).ConfigureAwait(false);

            var item = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _context.DetachAllEntitiesAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var result = await _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if(result != null)
            {
                return result;
            }

            return await NextRepository.GetAsync(id).ConfigureAwait(false);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> repository)
        {
            NextRepository = repository;
            return NextRepository;
        }

        public async Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> includes = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(expression);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.OrderBy(e => e.Id).ToPagedListAsync(pageNumber, pageSize).ConfigureAwait(false);
            if(result != null)
            {
                return result;
            }

            return await NextRepository.SearchAsync(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await NextRepository.UpdateAsync(entity).ConfigureAwait(false);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _context.DetachAllEntitiesAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
