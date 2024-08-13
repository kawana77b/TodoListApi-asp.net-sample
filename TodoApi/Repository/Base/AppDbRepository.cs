using TodoApi.Data;

namespace TodoApi.Repository.Base
{
    public abstract class AppDbRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// The database context.
        /// </summary>
        protected AppDbContext Context => _context;

        /// <summary>
        /// Executes the given action within a transaction.
        /// </summary>
        /// <param name="action"></param>
        public void WithTransaction(Action action)
        {
            if (Context.Database.CurrentTransaction != null)
                throw new InvalidOperationException("Cannot start a transaction within a transaction.");

            using var transaction = Context.Database.BeginTransaction();
            try
            {
                action();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Executes the given function within a transaction.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult? WithTransaction<TResult>(Func<TResult> action)
        {
            var result = default(TResult);
            if (Context.Database.CurrentTransaction != null)
                throw new InvalidOperationException("Cannot start a transaction within a transaction.");

            using var transaction = Context.Database.BeginTransaction();
            try
            {
                result = action();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return result;
            }
            return result;
        }
    }
}