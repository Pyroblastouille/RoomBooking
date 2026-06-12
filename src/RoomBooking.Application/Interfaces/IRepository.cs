using System;
using System.Collections.Generic;
using System.Text;

namespace RoomBooking.Application.Interfaces {
    public interface IRepository<T> where T : class {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(int id,T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
