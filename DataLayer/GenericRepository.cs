using DbEntities.Interfaces;
using GPGDesign.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class GenericRepository<T> : IRepository<T> where T : class, IBase, new()
    {
        private readonly GPGContext context;
        private readonly DbSet<T> set;

        public int UserId { get; set; }

        public GenericRepository(GPGContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set.AsQueryable();
        }

        public T FindOrCreate(int id)
        {
            if (id != 0)
                return this.Find(id);
            else
                return new T();
        }

        public T Find(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.set.Add(entity);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }

        public int Save(T entity)
        {
            if (entity.Id != 0)
            {
                this.ChangeState(entity, EntityState.Modified);
            }
            else
            {
                this.set.Add(entity);
            }

            this.context.SaveChanges();
            return entity.Id;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                ChangeState(entity, EntityState.Deleted);
            }
        }
    }
}
