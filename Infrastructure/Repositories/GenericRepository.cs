using Azure;
using Common.Pagination;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class, IEntity<TKey>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _dbSet;

            // Include navigation property n?u có
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }


        public async Task<PageResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate = null,
            PageRequest? pageRequest = null,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _dbSet;

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply filters
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Total count before pagination
            var totalItems = await query.CountAsync();

            // Apply sorting and pagination
            if (pageRequest != null)
            {
                // Apply sorting
                if (!string.IsNullOrWhiteSpace(pageRequest.OrderBy))
                {
                    // Xác d?nh chi?u s?p x?p (asc/desc)
                    var sortOrder = pageRequest.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                        ? "descending"
                        : "ascending";

                    // S? d?ng System.Linq.Dynamic.Core d? OrderBy d?ng
                    query = query.OrderBy($"{pageRequest.OrderBy} {sortOrder}");
                }

                // Apply pagination
                int skip = (pageRequest.Page - 1) * pageRequest.Size;
                query = query.Skip(skip).Take(pageRequest.Size);

                var items = await query.ToListAsync();

                return new PageResult<T>
                {
                    Items = items,
                    Page = pageRequest.Page,
                    Size = pageRequest.Size,
                    TotalItems = totalItems
                };
            }

            // Không có phân trang thì tr? t?t c?
            var allItems = await query.ToListAsync();
            return new PageResult<T>
            {
                Items = allItems,
                Page = 1,
                Size = allItems.Count,
                TotalItems = allItems.Count
            };
        }


        public async Task<T?> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
                params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _dbSet;

            // Include navigation property n?u có
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetAllByIdsAsync(IEnumerable<TKey> ids, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            // Ch? h? tr? property tên "Id"
            query = query.Where(e => ids.Contains(EF.Property<TKey>(e, "Id")));
            return await query.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.SingleOrDefaultAsync(e => e.Id.Equals(id));

            return entity;
        }
        public IQueryable<T> Query() => _dbSet.AsQueryable();

    }
}
