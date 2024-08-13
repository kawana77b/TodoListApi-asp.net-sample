#if DEBUG

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApi.Models.Attributes;

namespace TodoApi.Models.Helper
{
    /// <summary>
    /// Utility class for checking if all entities are attributed with <see cref="RecordAttribute"/>
    /// </summary>
    public static class RecordAttributeUtil
    {
        /// <summary>
        /// Checks if all entities in the context are attributed with <see cref="RecordAttribute"/>. <br/>
        /// If not, logs a message for each entity that is not attributed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IEnumerable<string> CheckRecordAttributeAll<TDbContext>() where TDbContext : DbContext
        {
            var attrs = new List<string>();

            // Properties defined in a non inherited DbSet of this type
            Func<PropertyInfo, Type, bool> getDbSetProps = (p, context)
                => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) && p.DeclaringType == context;

            var dbContext = typeof(TDbContext);
            var dbSetProperties = dbContext
                .GetProperties()
                .Where((p) => getDbSetProps(p, dbContext));

            foreach (var dbSetProperty in dbSetProperties)
            {
                var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];
                var recordAttribute = entityType.GetCustomAttribute<RecordAttribute>();
                if (recordAttribute == null)
                    attrs.Add(dbSetProperty.Name);
            }
            return [.. attrs];
        }
    }
}

#endif