using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class Repository<T>(TicketsContext context) where T : class
{
    public TicketsContext Context { get; set; } = context;

    public virtual async Task<T?> GetById(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> Insert(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }
}
