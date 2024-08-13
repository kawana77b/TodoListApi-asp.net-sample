using TodoApi.Data;
using TodoApi.Models.TODO;
using TodoApi.Repository.Base;
using TodoApi.Repository.Contracts;

namespace TodoApi.Repository
{
    public class TodoRepository(AppDbContext context) : AppDbRepository(context), IRepository<Todo>
    {
        public Todo? FetchById(int id)
        {
            return Context.Todos.Find(id);
        }

        public IEnumerable<Todo> FindByUserId(string userId)
        {
            return [.. Context.Todos.Where(x => x.UserId == userId)];
        }

        public void Add(Todo todo)
        {
            todo.CreatedAt = DateTime.Now;
            todo.UpdatedAt = DateTime.Now;
            WithTransaction(() =>
            {
                Context.Todos.Add(todo);
                Context.SaveChanges();
            });
        }

        public void Remove(Todo todo)
        {
            WithTransaction(() =>
            {
                Context.Todos.Remove(todo);
                Context.SaveChanges();
            });
        }

        public void RemoveRange(IEnumerable<Todo> todos)
        {
            WithTransaction(() =>
            {
                Context.Todos.RemoveRange(todos);
                Context.SaveChanges();
            });
        }

        public void Update(Todo todo)
        {
            todo.UpdatedAt = DateTime.Now;
            WithTransaction(() =>
            {
                Context.Todos.Update(todo);
                Context.SaveChanges();
            });
        }

        public void UpdateRange(IEnumerable<Todo> todos)
        {
            foreach (var todo in todos)
                todo.UpdatedAt = DateTime.Now;
            WithTransaction(() =>
            {
                Context.Todos.UpdateRange(todos);
                Context.SaveChanges();
            });
        }
    }
}