namespace TodoApi.Repository.Contracts
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Add an entity and write
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Update an entity and write
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Remove an entity and write
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);
    }
}