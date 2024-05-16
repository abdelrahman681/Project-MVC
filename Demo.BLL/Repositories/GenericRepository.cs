using Demo.BLL.Interfacies;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext _AppDbContext)
        {
            _dbContext = _AppDbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public T Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            }
            else
            {
                return _dbContext.Set<T>().AsNoTracking().ToList();
            }
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
