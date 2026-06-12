using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;
using RoomBooking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomBooking.Infrastructure.Persistence.Repositories {
    public class Repository<T> : IRepository<T> where T : class {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity) {
            var result = await _dbSet.AddAsync(entity);
            return result.State == EntityState.Added;
        }

        public async Task<bool> DeleteAsync(T entity) {
            var result = _dbSet.Remove(entity);
            return result.State == EntityState.Deleted;
        }

        public async Task<IEnumerable<T>> GetAllAsync() {
            var result = await _dbSet.ToListAsync();
            return result;
        }

        public async Task<T?> GetByIdAsync(int id) {
            var result = await _dbSet.FindAsync(id);
            return result;
        }

        public async Task<bool> UpdateAsync(int id, T entity) {
            var result = _dbSet.Update(entity);
            return result.State == EntityState.Modified;
        }
    }
}
