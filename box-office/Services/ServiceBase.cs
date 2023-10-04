

using AutoMapper;
using box_office.DataBase;
using box_office.DataBase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace box_office.Services;

public abstract class ServiceBase<TEntity> where TEntity : class, IBaseEntity
{
    public ServiceBase(IServiceProvider serviceProvider, ILogger logger) 
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    protected ILogger Logger { get; }
    protected IMapper Mapper;
    private IServiceProvider ServiceProvider { get; }

    protected ContextType GetContext<ContextType>()
    where ContextType : DbContext
    {
        return ServiceProvider.GetRequiredService<ContextType>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() 
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<TEntity> dbSet = context.Set<TEntity>();

        return await dbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int entityId) 
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<TEntity> dbSet = context.Set<TEntity>();

        var query = from entity in dbSet
                    where entity.Id == entityId
                    select entity;

        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task DeleteByIdAsync(int entityId)
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<TEntity> dbSet = context.Set<TEntity>();

        var oldEntity = await dbSet.FirstOrDefaultAsync(e => e.Id == entityId);

        if (oldEntity == null) return;

        if (context.Entry(oldEntity).State == EntityState.Detached)
        {
            dbSet.Attach(oldEntity);
        }

        dbSet.Remove(oldEntity);

        await context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<TEntity> dbSet = context.Set<TEntity>();

        dbSet.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<TEntity> dbSet = context.Set<TEntity>();

        if ((await dbSet.AnyAsync(e => e.Id == entity.Id)) == false) return null; // сущности в базе нет (не обновляем)

        dbSet.Update(entity);

        await context.SaveChangesAsync();

        return entity;
    }
}
