using PocReportViewer.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocReportViewer.Repository.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        IList<TEntity> FindAll();
        IList<TEntity> FindAll(Func<TEntity, string> order);
        TEntity FindById(long id);
        TEntity Create(TEntity entity);
        IList<TEntity> Create(IList<TEntity> entities);
        TEntity Update(TEntity entity);
        IList<TEntity> Update(IList<TEntity> entities);
        void Remove(int id);
        void Remove(TEntity entity);
        void Remove(IList<TEntity> entities);
    }

    /// <summary>
    /// Base class for all repositories
    /// </summary>
    /// <typeparam name="TEntity">Model entity.</typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> _Repository;

        /// <summary>
        /// Default constructor
        /// </summary>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        protected BaseRepository()
        {
            _Repository = UnitOfWork.Context.Set<TEntity>();
        }

        /// <summary>
        /// Find all instances of a entity.
        /// </summary>
        /// <returns>Entity list.</returns>
        public IList<TEntity> FindAll()
        {
            return _Repository.ToList();
        }

        public IList<TEntity> FindAll(Func<TEntity, string> order)
        {
            return _Repository.OrderBy(order).ToList();
        }

        public IList<TEntity> FindAll(Func<TEntity, bool> condition)
        {
            return _Repository.Where(condition).ToList();
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity instance.</returns>
        public virtual TEntity FindById(long id)
        {
            return _Repository.Find(id);
        }

        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity to be created.</param>
        /// <returns>Created entity</returns>
        public TEntity Create(TEntity entity)
        {
            _Repository.Add(entity);
            return entity;
        }

        /// <summary>
        /// Create a list of entity
        /// </summary>
        /// <param name="entities">The list of entites</param>
        /// <returns>The created list of entities</returns>
        public IList<TEntity> Create(IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _Repository.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// Update the data of an entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        /// <returns>Updated entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            UnitOfWork.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Updates a list of entity
        /// </summary>
        /// <param name="entities">The list of entites</param>
        /// <returns>The updated list of entities</returns>
        public IList<TEntity> Update(IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                UnitOfWork.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
            return entities;
        }

        /// <summary>
        /// Remove an entity by its id.
        /// </summary>
        /// <param name="id">Id of the entity to be removed.</param>
        public void Remove(int id)
        {
            TEntity entity = FindById(id);
            _Repository.Remove(entity);

        }

        /// <summary>
        /// Remove a given entity.
        /// </summary>
        /// <param name="entity">Entity to be removed.</param>
        public void Remove(TEntity entity)
        {
            _Repository.Remove(entity);
        }

        /// <summary>
        /// Remove a list of entities.
        /// </summary>
        /// <param name="entities">List of entities that should be removed.</param>
        public void Remove(IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _Repository.Remove(entity);

            }
        }

        /// <summary>
        /// Get the records of the entity based on some pagination parameters
        /// </summary>
        /// <param name="pageNumber">Number of the page desired.</param>
        /// <param name="pageSize">Number of items that is being considered for each page.</param>
        public List<TEntity> PagedList(int pageNumber, int pageSize)
        {
            return _Repository.Skip(pageNumber * pageSize).Take(pageSize).ToList();
        }

    }
}
